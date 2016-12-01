#
# Deploy DACPAC
#

Param(
    [string] [Parameter(Mandatory=$true)] $dacpacname,
    [string] [Parameter(Mandatory=$true)] $dbServer,
	[string] [Parameter(Mandatory=$true)] $dbname,
	[string] [Parameter(Mandatory=$true)] $dbLogin,
	[string] [Parameter(Mandatory=$true)] $dbPassword
)


$constr = "Data Source=tcp:$dbServer; Initial Catalog=$dbname; User ID=$dbLogin; Password=$dbPassword"

Write-Host "Deploying $dacpacname to $constr"

$dacpacname =  [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $dacpacname))

# Load in Microsoft.SqlServer.Dac.dll (retrieved from nuget)
$localDirectory = (Get-Location).Path
Add-Type -Path "$PSScriptRoot\..\lib\Microsoft.SqlServer.Dac.1.0.3\lib\Microsoft.SqlServer.Dac.dll"

# Create a DacServices object, which needs a connection string 
# The (localdb)\ProjectsV12 instance is created by SQL Server Data Tools (SSDT) and should not be used by applications
# (localdb)\MSSQLLocalDB is the SQL Server 2014 LocalDB default instance name
# (localdb)\v11.0 is the SQL Server 2012 LocalDB default instance name
$dacServices = New-Object Microsoft.SqlServer.Dac.DacServices $constr

# Options
$deployOptions = New-Object Microsoft.SqlServer.Dac.DacDeployOptions
$deployOptions.CreateNewDatabase = $false
$deployOptions.BlockOnPossibleDataLoss = $false
$deployOptions.BlockWhenDriftDetected = $false
# Must specify variables if there are any defined!
$deployOptions.SqlCommandVariableValues.Add("Environment", "dev")

# register event. For info on this cmdlet, see http://technet.microsoft.com/en-us/library/hh849929.aspx 
Register-ObjectEvent -InputObject $dacServices -EventName "Message" -Action { Write-Host $EventArgs.Message.Message } | Out-Null
 
# Load dacpac from file & deploy database
$dp = [Microsoft.SqlServer.Dac.DacPackage]::Load($dacpacname) 
$dacServices.Deploy($dp, $dbname, $true, $deployOptions)

# Print rows in tables
# sql credit http://stackoverflow.com/a/24119273
$sql = "SELECT t.name, s.row_count from sys.tables t JOIN sys.dm_db_partition_stats s ON t.object_id = s.object_id AND t.type_desc = 'USER_TABLE' AND t.name not like '%dss%' AND s.index_id = 1"
$sda = New-Object System.Data.SqlClient.SqlDataAdapter ($sql, $constr)
$sdt = New-Object System.Data.DataTable
$sda.fill($sdt) | Out-Null
$sdt.Rows