# Original from https://github.com/ayende/rhino-mocks/blob/master/psake_ext.ps1

function Get-Git-Commit
{
    $gitLog = git log --oneline -1
    return $gitLog.Split(' ')[0]
}

function Get-Git-CommitCount
{
    $gitLog = git log --pretty=oneline
    return $gitLog.length
}

function Generate-Assembly-Info
{
param(
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$version,
	[string]$file = $(throw "file is a required parameter.")
)
  $commit = Get-Git-Commit
  $commitCount = Get-Git-CommitCount
  $full_version = "$version.$commitCount"
  $script:full_version = $full_version
  "Version $full_version (commit hash: $commit, commit log count: $commitCount)"
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$full_version"")]
[assembly: AssemblyInformationalVersionAttribute(""$full_version ($commit)"")]
[assembly: AssemblyFileVersionAttribute(""$full_version"")]
[assembly: AssemblyDelaySignAttribute(false)]
"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $file"
	out-file -filePath $file -encoding UTF8 -inputObject $asmInfo
}

function Run-Test
{
    param(
        [string]$test_project_name = $(throw "file is a required parameter."),
        [string]$out_dir = $(throw "out dir is a required parameter.")
    )

    $test_result_file = "$out_dir\$test_project_name.TestResult.xml"
    & $nunit_exe "$base_dir\test\$test_project_name\bin\$configuration\$test_project_name.dll" /xml=$test_result_file

    if ($lastExitCode -ne 0) {
        throw "One or more failures in tests - see details above."
    }
}