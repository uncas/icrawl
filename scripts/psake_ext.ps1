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
  $fullVersion = "$version.$commitCount"
  "Version $fullVersion (commit hash: $commit, commit log count: $commitCount)"
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$fullVersion"")]
[assembly: AssemblyInformationalVersionAttribute(""$fullVersion ($commit)"")]
[assembly: AssemblyFileVersionAttribute(""$fullVersion"")]
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