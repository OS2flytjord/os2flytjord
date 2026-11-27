<#
.SYNOPSIS
    Henter skemaet (uden data) fra FlytJordTest ind i SQL-projektet FlytJord.Database.

.DESCRIPTION
    Bruger SqlPackage /Action:Extract med én .sql-fil pr. objekt (mappe pr. schema/objekttype).
    Brugere, roller, logins og rettigheder tages ikke med - de hører til serveren, ikke til skemaet.
    Eksisterende objektfiler i projektet erstattes; selve .sqlproj og publish-profiler røres ikke.

.EXAMPLE
    .\Hent-Skema.ps1
#>
[CmdletBinding()]
param(
    [string] $KildeServer = 'info-udv-db01',
    [string] $KildeDatabase = 'FlytJordTest',
    [pscredential] $Credential
)

$ErrorActionPreference = 'Stop'
$projektMappe = $PSScriptRoot

function Find-SqlPackage {
    $cmd = Get-Command SqlPackage -ErrorAction SilentlyContinue
    if ($cmd) { return $cmd.Source }
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (Test-Path $vswhere) {
        foreach ($vs in & $vswhere -prerelease -products * -sort -property installationPath) {
            $fundet = Join-Path $vs 'Common7\IDE\Extensions\Microsoft\SQLDB\DAC\SqlPackage.exe'
            if (Test-Path $fundet) { return $fundet }
        }
    }
    throw 'SqlPackage blev ikke fundet. Installér Visual Studio med "Data storage and processing" eller kør: dotnet tool install -g microsoft.sqlpackage'
}

$sqlPackage = Find-SqlPackage
if (-not $Credential) {
    $Credential = Get-Credential -Message "SQL-login til $KildeServer/$KildeDatabase" -UserName 'scripter'
}

$tmp = Join-Path ([IO.Path]::GetTempPath()) ("FlytJordSkema_" + [guid]::NewGuid().ToString('N'))
try {
    Write-Host "Henter skema fra $KildeServer/$KildeDatabase..."
    & $sqlPackage '/Action:Extract' `
        "/SourceServerName:$KildeServer" "/SourceDatabaseName:$KildeDatabase" `
        "/SourceUser:$($Credential.UserName)" "/SourcePassword:$($Credential.GetNetworkCredential().Password)" `
        '/SourceEncryptConnection:True' '/SourceTrustServerCertificate:True' `
        "/TargetFile:$tmp" `
        '/p:ExtractTarget=SchemaObjectType' `
        '/p:ExtractAllTableData=False' `
        '/p:ExtractApplicationScopedObjectsOnly=True' `
        '/p:IgnorePermissions=True' `
        '/p:IgnoreUserLoginMappings=True' `
        '/p:VerifyExtraction=False'
    if ($LASTEXITCODE -ne 0) { throw 'Extract fejlede.' }

    # Brugere, roller og rettigheder er server-/miljøspecifikke.
    Get-ChildItem $tmp -Recurse -Filter *.sql | Where-Object {
        (Get-Content $_.FullName -Raw) -match '(?im)^\s*(CREATE\s+(USER|ROLE|LOGIN)|ALTER\s+ROLE|GRANT|DENY|EXEC(UTE)?\s+sp_addrolemember)\b'
    } | ForEach-Object {
        Write-Host "Udelader $($_.FullName.Substring($tmp.Length + 1))"
        Remove-Item $_.FullName
    }

    # Erstat objektfilerne i projektet.
    Get-ChildItem $projektMappe -Directory | Where-Object { $_.Name -notin 'bin', 'obj', 'Properties', 'PublishProfiles' } |
        Remove-Item -Recurse -Force
    Get-ChildItem $tmp | Where-Object { $_.Name -notlike '*.sqlproj' -and $_.Name -ne 'model.xml' } |
        Copy-Item -Destination $projektMappe -Recurse -Force
    Get-ChildItem $projektMappe -Recurse -Directory | Where-Object { -not (Get-ChildItem $_.FullName -Recurse -File) } |
        Remove-Item -Recurse -Force -ErrorAction SilentlyContinue

    $antal = (Get-ChildItem $projektMappe -Recurse -Filter *.sql | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }).Count
    Write-Host "Færdig: $antal objektfiler i $projektMappe." -ForegroundColor Green
}
finally {
    Remove-Item $tmp -Recurse -Force -ErrorAction SilentlyContinue
}
