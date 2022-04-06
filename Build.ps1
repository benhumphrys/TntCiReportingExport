Clear

$buildDirectoryName = "."
$outputArchiveDirectoryPath = "C:\Release"
$binaryDirectoryPath = ".\TntCiReportingExport\bin\Release\"
$binaryFileName = "TntCiReportingExport.dll"
$binaryFilePath = Join-Path -path $binaryDirectoryPath -ChildPath $binaryFileName
$releasePaths = $binaryDirectoryPath
$mickeyParentDirectoryPath = "\\Mickey\External Data\stu\TNT - TNT\Commerical Invoice OCR Standard\Source\TntCiReportingExport"
#$mickeyParentDirectoryPath = "C:\Misc\TntCiReportingExport"

Write-Host "Starting NuGet server..."
docker start scansationnuget

msbuild .\TntCiReportingExport\TntCiReportingExport.csproj /p:Configuration=Release

Write-Host "Opening VPN..."
rasdial "ScansationVPN" "BenH" '0R2^l$OsqvjK'

Write-Host "Removing non-required files from build..."
$releasePaths | ForEach-Object {"$_\*"} | Get-ChildItem `
	-Include ("ACLog*", "Kofax*", "log4net*", "Polly*", "ScintillaNET.xml") | Remove-Item

Write-Host "Getting version number from $binaryFilePath..."
$versionNumber = (Get-Command $binaryFilePath).FileVersionInfo.FileVersion
Write-Host "Version number: $versionNumber"

Write-Host "Creating Release archive..."
$baseFileName = [IO.Path]::GetFileNameWithoutExtension($binaryFilePath)
$releaseZipFileName = $baseFileName + $versionNumber + ".zip"
$releaseZipFilePath = Join-Path -Path $outputArchiveDirectoryPath -ChildPath $releaseZipFileName
Write-Host "File name: $releaseZipFileName"
$releaseFilePaths = Get-ChildItem -Path $releasePaths | Sort-Object -Property Name | Get-Unique -AsString
$releaseFilePaths | Compress-Archive -DestinationPath $releaseZipFilePath -Force

Write-Host "Copying release binaries to $mickeyParentDirectoryPath"

$latestReleaseBinariesDirectoryPath = Join-Path -Path $mickeyParentDirectoryPath -ChildPath "Latest Release Binaries"
$historicalBinariesDirectoryPath = Join-Path -Path $mickeyParentDirectoryPath -ChildPath "Historical Binaries"
$sourceDirectoryPath = Join-Path -Path $mickeyParentDirectoryPath -ChildPath "Source"

$mickeyParentDirectoryPath, $latestReleaseBinariesDirectoryPath, $historicalBinariesDirectoryPath, $sourceDirectoryPath |
    ForEach-Object -Process {if ((Test-Path -Path $_) -eq $False){mkdir $_}}

$previousLatestVersionFilePath = Join-Path -Path $latestReleaseBinariesDirectoryPath -ChildPath $binaryFileName
if (Test-Path -Path $previousLatestVersionFilePath){
	$prevVersionNumber = (Get-Command $previousLatestVersionFilePath).FileVersionInfo.FileVersion
	$newHistoricalDirectoryPath = Join-Path -Path $historicalBinariesDirectoryPath -ChildPath $prevVersionNumber

	Write-Host "Archiving historical version to $newHistoricalDirectoryPath..."
	mkdir $newHistoricalDirectoryPath
	Move-Item $latestReleaseBinariesDirectoryPath\* $newHistoricalDirectoryPath
}

$releaseFilePaths | Copy-Item -Destination $latestReleaseBinariesDirectoryPath 

$sourceArchiveDirectoryPath = Join-Path -Path $sourceDirectoryPath -ChildPath $versionNumber
Write-Host "Copying source to $sourceArchiveDirectoryPath..."
mkdir $sourceArchiveDirectoryPath\.git\
Copy-Item .git\*  $sourceArchiveDirectoryPath\.git\ -Recurse 
Copy-Item $buildDirectoryName\* $sourceArchiveDirectoryPath -Recurse 