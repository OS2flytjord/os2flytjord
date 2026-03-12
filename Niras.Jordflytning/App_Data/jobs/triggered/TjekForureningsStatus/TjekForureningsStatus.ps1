try
{
    & .\..\..\..\..\bin\Niras.Jordflytning.BatchJob.exe
} catch {
	Write-Output $_.Exception
	exit 1
}
exit 0