try
{
    $maintenanceKey = $Env:MaintenanceKey
    $hostname = $Env:WEBSITE_HOSTNAME
    $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
    $headers.Add("MaintenanceKey", "$($MaintenanceKey)")

    $uri = "https://$hostname/maintenance/RydGamleZipFiler?timer=24";
    Write-Output $uri

    $ProgressPreference = "SilentlyContinue"
    $Response = Invoke-WebRequest -Uri $uri -Headers $headers -Method Post -TimeoutSec 900 -UseBasicParsing
} catch {
	Write-Output $_.Exception
	exit 1
}
exit 0