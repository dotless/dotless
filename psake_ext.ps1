# This script was derived from the Rhino.Mocks buildscript written by Ayende Rahien.

function Get-Git-Commit
{
    $v = git describe
    trap [Exception]
	{ 
        $currentDir = pwd
        throw "Error: Could not execute git-describe in folder $currentDir. Please make sure that git is installed in your PATH"
    }
    return $v
}

function Get-Git-Version
{
    $v = git describe --abbrev=0
    trap [Exception]
	{ 
        $currentDir = pwd
        throw "Error: Could not execute git-describe in folder $currentDir. Please make sure that git is installed in your PATH"
    }
    return $v -replace "v", ""
}

function Generate-Assembly-Info
{
param(
	[string]$clsCompliant = "true",
	[string]$title, 
	[string]$description, 
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$version,
	[string]$file = $(throw "file is a required parameter."),
	[bool]$partial = $false
)
  $commit = Get-Git-Commit
  $asmInfo = "using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;


[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyTitleAttribute(""$title"")]
[assembly: AssemblyDescriptionAttribute(""$description"")]
[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product $commit"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyInformationalVersionAttribute(""$version"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyDelaySignAttribute(false)]
"
	if($partial) {
        $asmInfo = $asmInfo + "[assembly: AllowPartiallyTrustedCallers]"
    }

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $file"
    out-file -filePath $file -encoding UTF8 -inputObject $asmInfo
}