$PackageCode = "WastedgeQuerier"
$PackageFileName = "Wastedge Querier Setup"
$PackageTitle = "Wastedge Querier"

# Load SharpZipLib

[void][System.Reflection.Assembly]::LoadFile((Split-Path -Path $MyInvocation.MyCommand.Definition -Parent) + "\ICSharpCode.SharpZipLib.dll")

# Create a directory to work in

$TempPath = [System.IO.Path]::GetTempPath() + "\" + [System.Guid]::NewGuid().ToString()

New-Item -ItemType Directory -Path $TempPath | Out-Null

# Get the NuGetUpdate package

$TargetPath = [System.IO.Path]::GetDirectoryName([System.Environment]::CurrentDirectory)

[System.Environment]::CurrentDirectory = $TempPath

$WebClient = New-Object System.Net.WebClient
$WebClient.DownloadFile("https://nuget.org/api/v1/package/NuGetUpdate", $TempPath + "\NuGetUpdate.zip")

(New-Object ICSharpCode.SharpZipLib.Zip.FastZip).ExtractZip($TempPath + "\NuGetUpdate.zip", $TempPath, $Null)

$BootStrapperPath = $TempPath + "\Tools\Bootstrapper"

Write-Host "Building" $PackageCode

$Arguments =
    "-file `"" + $BootStrapperPath + "\MakeSetup.ps1`" " +
    "`"" + $TargetPath + "\" + $PackageFileName + "`" " +
    "`"https://nuget.org/api/v2`" " +
    "`"" + $PackageCode + "`" " +
    "`"" + $PackageTitle + "`""

Start-Process `
    -NoNewWindow `
    -Wait `
    -FilePath ($PSHome + "\powershell.exe") `
    -ArgumentList $Arguments `
    -WorkingDirectory $BootStrapperPath `
    -PassThru | Out-Null

[System.Environment]::CurrentDirectory = [System.IO.Path]::GetTempPath()

Remove-Item -Recurse -Force $TempPath
