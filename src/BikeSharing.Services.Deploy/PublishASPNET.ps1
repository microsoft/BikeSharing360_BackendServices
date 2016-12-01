
param(
	[parameter(Mandatory=$TRUE)]
    [String] $ResourceGroupName,
	[parameter(Mandatory=$TRUE)]
    [String] $webAppName,
	[parameter(Mandatory=$TRUE)]
    [String] $webPackagePath
)

$webApp = Get-AzureRMWebApp -ResourceGroupName $ResourceGroupName -Name $webAppName

$publishProfile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, "profile_.xml"))
$pp = Get-AzureRMWebAppPublishingProfile -WebApp $webApp -OutputFile $publishProfile

[xml]$ppXml = Get-Content $publishProfile
$publishProperties = @{'WebPublishMethod'='MSDeploy';
						'MSDeployServiceUrl'=$ppXml.publishData.FirstChild.publishUrl;
						'DeployIisAppPath'=$webAppName;
						'Username'=$ppXml.publishData.FirstChild.userName;
						'Password'=$ppXml.publishData.FirstChild.userPWD}

$publishScript = "$PSScriptRoot\default-publish.ps1"


. $publishScript -publishProperties $publishProperties  -packOutput $webPackagePath

Remove-Item $publishProfile
Remove-Item $webPackagePath -recurse



