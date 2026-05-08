param(
    [string]$ResultsDirectory = "TestResults",
    [string]$OutputFile = "TestResults/test-results-summary.md"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $ResultsDirectory)) {
    Write-Host "Results directory not found: $ResultsDirectory"
    exit 0
}

$trxFile = Get-ChildItem -LiteralPath $ResultsDirectory -Filter *.trx -File |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if ($null -eq $trxFile) {
    Write-Host "No TRX files found in: $ResultsDirectory"
    exit 0
}

[xml]$trx = Get-Content -LiteralPath $trxFile.FullName

$ns = New-Object System.Xml.XmlNamespaceManager($trx.NameTable)
$ns.AddNamespace("t", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")

$counters = $trx.SelectSingleNode("/t:TestRun/t:ResultSummary/t:Counters", $ns)
$results = $trx.SelectNodes("/t:TestRun/t:Results/t:UnitTestResult", $ns)

$passed = [int]$counters.passed
$failed = [int]$counters.failed
$total = [int]$counters.total
$executed = [int]$counters.executed
$start = $trx.SelectSingleNode("/t:TestRun/t:Times", $ns).start
$finish = $trx.SelectSingleNode("/t:TestRun/t:Times", $ns).finish

$items = foreach ($result in $results) {
    [PSCustomObject]@{
        TestName = [string]$result.testName
        Outcome = [string]$result.outcome
        Duration = [string]$result.duration
    }
}

$ordered = $items | Sort-Object @{ Expression = { $_.Outcome -ne "Failed" } }, TestName

$lines = [System.Collections.Generic.List[string]]::new()
$lines.Add("# Test Results Summary")
$lines.Add("")
$lines.Add("- Source: $($trxFile.Name)")
$lines.Add("- Started: $start")
$lines.Add("- Finished: $finish")
$lines.Add("- Total: $total")
$lines.Add("- Executed: $executed")
$lines.Add("- Passed: $passed")
$lines.Add("- Failed: $failed")
$lines.Add("")
$lines.Add("## Per-Test Details")
$lines.Add("")
$lines.Add("| Outcome | Duration | Test |")
$lines.Add("|---|---:|---|")

foreach ($item in $ordered) {
    $lines.Add("| $($item.Outcome) | $($item.Duration) | $($item.TestName) |")
}

$parent = Split-Path -Parent $OutputFile
if (-not [string]::IsNullOrWhiteSpace($parent) -and -not (Test-Path -LiteralPath $parent)) {
    New-Item -ItemType Directory -Path $parent -Force | Out-Null
}

$lines | Set-Content -LiteralPath $OutputFile -Encoding UTF8
Write-Host "Wrote readable summary: $OutputFile"
