param(
	[parameter(Mandatory=$TRUE)]
	$svcSource,
	[parameter(Mandatory=$TRUE)]
	[String] $svcName,
	[parameter(Mandatory=$TRUE)]
	[String] $webPackage
)


Write-Host 'Invoking dotnet publish for - ' $svcName
$out = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, "pubs\" + $svcName + "\wwwroot"))
If (Test-Path $out) {
	Remove-Item $out -recurse
}

Set-Location -Path $svcSource
$exe = "dotnet"
&"$exe" publish -o $out
