#Requires -Version 7.0

$TestOutput = dotnet test --collect:"XPlat Code Coverage;Format=cobertura"
if ($LastExitCode -eq 0) {
    $CoverageReports = $TestOutput | Select-String coverage.cobertura.xml | ForEach-Object { $_.Line.Trim() } | Join-String -Separator ';'
    dotnet reportgenerator "-reports:$CoverageReports" "-targetdir:./CoverageReport" "-reporttype:Html"
    Start-Process "coveragereport\index.html" -Verb "open"
} else {
    Write-Host "dotnet test failed"
    Write-Host $TestOutput
}