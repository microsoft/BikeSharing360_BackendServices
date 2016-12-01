#Requires -Version 3.0
#Requires -Module AzureRM.Resources
#Requires -Module Azure.Storage

Param(
    [string] [Parameter(Mandatory=$true)] $ResourceGroupLocation,
    [string] $ResourceGroupName = 'BikeSharing.Services.Deploy',
    [switch] $UploadArtifacts,
    [string] $StorageAccountName,
    [string] $StorageContainerName = $ResourceGroupName.ToLowerInvariant() + '-stageartifacts',
    [string] $TemplateFile = 'Templates\azuredeploy.json',
    [string] $TemplateParametersFile = 'Templates\azuredeploy.parameters.json',
    [string] $ArtifactStagingDirectory = '.',
    [string] $DSCSourceFolder = 'DSC',
    [switch] $ValidateOnly
)

Import-Module Azure -ErrorAction SilentlyContinue

try {
    [Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.AddUserAgent("VSAzureTools-$UI$($host.name)".replace(" ","_"), "2.9.5")
} catch { }

Set-StrictMode -Version 3

function Format-ValidationOutput {
    param ($ValidationOutput, [int] $Depth = 0)
    Set-StrictMode -Off
    return @($ValidationOutput | Where-Object { $_ -ne $null } | ForEach-Object { @("  " * $Depth + $_.Code + ": " + $_.Message) + @(Format-ValidationOutput @($_.Details) ($Depth + 1)) })
}

$OptionalParameters = New-Object -TypeName Hashtable
$TemplateFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $TemplateFile))
$TemplateParametersFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $TemplateParametersFile))

if ($UploadArtifacts) {
    # Convert relative paths to absolute paths if needed
    $ArtifactStagingDirectory = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $ArtifactStagingDirectory))
    $DSCSourceFolder = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $DSCSourceFolder))

    Set-Variable ArtifactsLocationName '_artifactsLocation' -Option ReadOnly -Force
    Set-Variable ArtifactsLocationSasTokenName '_artifactsLocationSasToken' -Option ReadOnly -Force

    $OptionalParameters.Add($ArtifactsLocationName, $null)
    $OptionalParameters.Add($ArtifactsLocationSasTokenName, $null)

    # Parse the parameter file and update the values of artifacts location and artifacts location SAS token if they are present
    $JsonContent = Get-Content $TemplateParametersFile -Raw | ConvertFrom-Json
    $JsonParameters = $JsonContent | Get-Member -Type NoteProperty | Where-Object {$_.Name -eq "parameters"}

    if ($JsonParameters -eq $null) {
        $JsonParameters = $JsonContent
    }
    else {
        $JsonParameters = $JsonContent.parameters
    }

    $JsonParameters | Get-Member -Type NoteProperty | ForEach-Object {
        $ParameterValue = $JsonParameters | Select-Object -ExpandProperty $_.Name

        if ($_.Name -eq $ArtifactsLocationName -or $_.Name -eq $ArtifactsLocationSasTokenName) {
            $OptionalParameters[$_.Name] = $ParameterValue.value
        }
    }

    # Create DSC configuration archive
    if (Test-Path $DSCSourceFolder) {
        $DSCSourceFilePaths = @(Get-ChildItem $DSCSourceFolder -File -Filter "*.ps1" | ForEach-Object -Process {$_.FullName})
        foreach ($DSCSourceFilePath in $DSCSourceFilePaths) {
            $DSCArchiveFilePath = $DSCSourceFilePath.Substring(0, $DSCSourceFilePath.Length - 4) + ".zip"
            Publish-AzureRmVMDscConfiguration $DSCSourceFilePath -OutputArchivePath $DSCArchiveFilePath -Force -Verbose
        }
    }

    # Create a storage account name if none was provided
    if($StorageAccountName -eq "") {
        $subscriptionId = ((Get-AzureRmContext).Subscription.SubscriptionId).Replace('-', '').substring(0, 19)
        $StorageAccountName = "stage$subscriptionId"
    }

    $StorageAccount = (Get-AzureRmStorageAccount | Where-Object{$_.StorageAccountName -eq $StorageAccountName})

    # Create the storage account if it doesn't already exist
    if($StorageAccount -eq $null){
        $StorageResourceGroupName = "ARM_Deploy_Staging"
        New-AzureRmResourceGroup -Location "$ResourceGroupLocation" -Name $StorageResourceGroupName -Force
        $StorageAccount = New-AzureRmStorageAccount -StorageAccountName $StorageAccountName -Type 'Standard_LRS' -ResourceGroupName $StorageResourceGroupName -Location "$ResourceGroupLocation"
    }

    $StorageAccountContext = (Get-AzureRmStorageAccount | Where-Object{$_.StorageAccountName -eq $StorageAccountName}).Context

    # Generate the value for artifacts location if it is not provided in the parameter file
    $ArtifactsLocation = $OptionalParameters[$ArtifactsLocationName]
    if ($ArtifactsLocation -eq $null) {
        $ArtifactsLocation = $StorageAccountContext.BlobEndPoint + $StorageContainerName
        $OptionalParameters[$ArtifactsLocationName] = $ArtifactsLocation
    }

    # Copy files from the local storage staging location to the storage account container
    New-AzureStorageContainer -Name $StorageContainerName -Context $StorageAccountContext -Permission Container -ErrorAction SilentlyContinue *>&1

    $ArtifactFilePaths = Get-ChildItem $ArtifactStagingDirectory -Recurse -File | ForEach-Object -Process {$_.FullName}
    foreach ($SourcePath in $ArtifactFilePaths) {
        $BlobName = $SourcePath.Substring($ArtifactStagingDirectory.length + 1)
        Set-AzureStorageBlobContent -File $SourcePath -Blob $BlobName -Container $StorageContainerName -Context $StorageAccountContext -Force -ErrorAction Stop
    }

    # Generate the value for artifacts location SAS token if it is not provided in the parameter file
    $ArtifactsLocationSasToken = $OptionalParameters[$ArtifactsLocationSasTokenName]
    if ($ArtifactsLocationSasToken -eq $null) {
        # Create a SAS token for the storage container - this gives temporary read-only access to the container
        $ArtifactsLocationSasToken = New-AzureStorageContainerSASToken -Container $StorageContainerName -Context $StorageAccountContext -Permission r -ExpiryTime (Get-Date).AddHours(4)
        $ArtifactsLocationSasToken = ConvertTo-SecureString $ArtifactsLocationSasToken -AsPlainText -Force
        $OptionalParameters[$ArtifactsLocationSasTokenName] = $ArtifactsLocationSasToken
    }
}

# Create or update the resource group using the specified template file and template parameters file
New-AzureRmResourceGroup -Name $ResourceGroupName -Location $ResourceGroupLocation -Verbose -Force -ErrorAction Stop

$ErrorMessages = @()
if ($ValidateOnly) {
    $ErrorMessages = Format-ValidationOutput (Test-AzureRmResourceGroupDeployment -ResourceGroupName $ResourceGroupName `
                                                                                  -TemplateFile $TemplateFile `
                                                                                  -TemplateParameterFile $TemplateParametersFile `
                                                                                  @OptionalParameters `
                                                                                  -Verbose)
}
else {
    $results = New-AzureRmResourceGroupDeployment -Name ((Get-ChildItem $TemplateFile).BaseName + '-' + ((Get-Date).ToUniversalTime()).ToString('MMdd-HHmm')) `
                                       -ResourceGroupName $ResourceGroupName `
                                       -TemplateFile $TemplateFile `
                                       -TemplateParameterFile $TemplateParametersFile `
                                       @OptionalParameters `
                                       -Force -Verbose `
                                       -ErrorVariable ErrorMessages
    $ErrorMessages = $ErrorMessages | ForEach-Object { $_.Exception.Message.TrimEnd("`r`n") }

	if ($results) {
		# Deploy dacpacs
		$pkgScript = "$PSScriptRoot\Data\DeployDacpac.ps1"
		Write-Host 'Deploying DACPACs - Events'
		. $pkgScript -dacpacname ".\dacpac\bikesharing-services-events.dacpac" -dbServer $results.Outputs.dbServer.Value -dbname $results.Outputs.dbEventsName.Value -dbLogin $results.Outputs.dbLogin.Value -dbPassword $results.Outputs.dbPassword.Value
		Write-Host 'Deploying DACPACs - Rides'
		. $pkgScript -dacpacname ".\dacpac\bikesharing-services-rides.dacpac" -dbServer $results.Outputs.dbServer.Value -dbname $results.Outputs.dbRidesName.Value -dbLogin $results.Outputs.dbLogin.Value -dbPassword $results.Outputs.dbPassword.Value
		Write-Host 'Deploying DACPACs - Feedback'
		. $pkgScript -dacpacname ".\dacpac\bikesharing-services-feedback.dacpac" -dbServer $results.Outputs.dbServer.Value -dbname $results.Outputs.dbFeedbackName.Value -dbLogin $results.Outputs.dbLogin.Value -dbPassword $results.Outputs.dbPassword.Value
		Write-Host 'Deploying DACPACs - Events'
		. $pkgScript -dacpacname ".\dacpac\bikesharing-services-profiles.dacpac" -dbServer $results.Outputs.dbServer.Value -dbname $results.Outputs.dbProfilesName.Value -dbLogin $results.Outputs.dbLogin.Value -dbPassword $results.Outputs.dbPassword.Value

		# Deploy webappscode
		$pkgScript = "$PSScriptRoot\Create-NetCore-Package.ps1"
		Write-Host 'Packaging WebApp - Events'
		# When deployinh script runs in bin/<Configuration>/staging. This is why we need all those ..

		#Run dotnet publish on all netcore services
		. $pkgScript -svcSource "$PSScriptRoot\..\..\..\..\..\..\src\BikeSharing.Services.Events" -svcName "events" -webPackage "events.zip"
		Write-Host 'Packaging WebApp - Profiles'
		. $pkgScript -svcSource "$PSScriptRoot\..\..\..\..\..\..\src\BikeSharing.Services.Profiles" -svcName "profiles" -webPackage "profiles.zip"
		Write-Host 'Packaging WebApp - Feedback'
		. $pkgScript -svcSource "$PSScriptRoot\..\..\..\..\..\..\src\BikeSharing.Services.Feedback" -svcName "feedback" -webPackage "feedback.zip"
		Write-Host 'Packaging WebApp - Rides'

		#Deploy netcore services using msdeploy
		$webAppName = $results.Outputs.servicesWebAppName.value
		Write-Host 'Deploy WebApp - Events to ' $webAppName
		$publisScript = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'PublishASPNET.ps1'))
		& $publisScript $ResourceGroupName $webAppName "$PSScriptRoot\pubs\events"
		$webAppName = $results.Outputs.feedbackWebAppName.value
		Write-Host 'Deploy WebApp - Feedback to ' $webAppName
		& $publisScript $ResourceGroupName $webAppName "$PSScriptRoot\pubs\feedback"
		$webAppName = $results.Outputs.profilesWebAppName.value
		Write-Host 'Deploy WebApp - Profiles to ' $webAppName
		& $publisScript $ResourceGroupName $webAppName "$PSScriptRoot\pubs\profiles"

		#rides is a nodejs project... Copy to a temp folder and publish it using msdeploy
		$webAppName = $results.Outputs.ridesWebAppName.value
		Write-Host 'Deploy WebApp - Rides to ' $webAppName
		$ridesTmpOut = "$PSScriptRoot\pubs\rides\wwwroot\" 
		if (Test-Path $ridesTmpOut) {
			Remove-Item $ridesTmpOut -recurse
		}
		Copy-Item "$PSScriptRoot\..\..\..\..\..\..\src\BikeSharing.Services.Rides\" $ridesTmpOut -recurse
		& $publisScript $ResourceGroupName $webAppName "$PSScriptRoot\pubs\rides\"
	}

}
if ($ErrorMessages)
{
    "", ("{0} returned the following errors:" -f ("Template deployment", "Validation")[[bool]$ValidateOnly]), @($ErrorMessages) | ForEach-Object { Write-Output $_ }
}
