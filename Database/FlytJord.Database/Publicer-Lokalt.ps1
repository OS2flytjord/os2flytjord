<#
.SYNOPSIS
    Bygger SQL-projektet og publicerer det til den lokale SQL Server med Windows-login.

.DESCRIPTION
    Publicerer til FlytJord_UnitTest og FlytJord_IntegrationTest med profilen PublishProfiles\Lokal.publish.xml.
    Databaserne oprettes, hvis de ikke findes, og opdateres ellers til projektets skema.
    Data lægges ind af testene selv (Mock-klasserne), ikke her.

.EXAMPLE
    .\Publicer-Lokalt.ps1
#>
[CmdletBinding()]
param(
    [string] $Server = 'localhost',
    [string[]] $Databaser = @('FlytJord_UnitTest', 'FlytJord_IntegrationTest')
)

$ErrorActionPreference = 'Stop'
$projekt = Join-Path $PSScriptRoot 'FlytJord.Database.sqlproj'
$profil = Join-Path $PSScriptRoot 'PublishProfiles\Lokal.publish.xml'
$dacpac = Join-Path $PSScriptRoot 'bin\Debug\FlytJord.Database.dacpac'

$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
# Første Visual Studio-installation med MSBuild, SSDT og SqlPackage (SSMS 22 har MSBuild, men ikke SSDT-targets).
$vs = & $vswhere -prerelease -products * -sort -property installationPath | Where-Object {
    (Test-Path (Join-Path $_ 'MSBuild\Current\Bin\MSBuild.exe')) -and
    (Test-Path (Join-Path $_ 'Common7\IDE\Extensions\Microsoft\SQLDB\DAC\SqlPackage.exe')) -and
    (Get-ChildItem (Join-Path $_ 'MSBuild\Microsoft\VisualStudio') -Filter 'Microsoft.Data.Tools.Schema.SqlTasks.targets' -Recurse -ErrorAction SilentlyContinue)
} | Select-Object -First 1
if (-not $vs) { throw 'Fandt ingen Visual Studio med SQL Server Data Tools (SSDT).' }
$msbuild = Join-Path $vs 'MSBuild\Current\Bin\MSBuild.exe'
$sqlPackage = Join-Path $vs 'Common7\IDE\Extensions\Microsoft\SQLDB\DAC\SqlPackage.exe'

Write-Host "Bygger $projekt..."
& $msbuild $projekt /t:Build /p:Configuration=Debug /v:minimal /nologo
if ($LASTEXITCODE -ne 0) { throw 'Build af SQL-projektet fejlede.' }

foreach ($db in $Databaser) {
    Write-Host "Publicerer til $Server/$db..."
    & $sqlPackage '/Action:Publish' "/SourceFile:$dacpac" "/Profile:$profil" `
        "/TargetConnectionString:Data Source=$Server;Initial Catalog=$db;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
    if ($LASTEXITCODE -ne 0) { throw "Publish til $db fejlede." }
}

Write-Host "Færdig." -ForegroundColor Green
