# This script was derived from the Rhino.Mocks buildscript written by Ayende Rahien.
include .\psake_ext.ps1

properties {
    $config = 'debug'
    $showtestresult = $FALSE
    $base_dir = resolve-path .
    $lib_dir = "$base_dir\nLess.lib\"
    $build_dir = "$base_dir\build\" 
    $release_dir = "$base_dir\release\"
    $source_dir = "$base_dir\"
	$version = Get-Git-Version
}

task default -depends Release

task Clean {
    remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue 
    remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
}

task Init -depends Clean {
    Write-Host $version
    Generate-Assembly-Info `
		-file "$source_dir\nless.Core\Properties\AssemblyInfo.cs" `
		-title "dotless $version" `
		-description "Dynamic CSS for .net" `
		-company "dotless project" `
		-product "dotless" `
		-version $version `
		-copyright "Copyright © dotless project 2009"
    Generate-Assembly-Info `
		-file "$source_dir\nless.Test\Properties\AssemblyInfo.cs" `
		-title "dotless Tests $version" `
		-description "Dynamic CSS for .net" `
		-company "dotless project" `
		-product "dotless" `
		-version $version `
		-copyright "Copyright © dotless project 2009"
    Generate-Assembly-Info `
		-file "$source_dir\dotless.Compiler\Properties\AssemblyInfo.cs" `
		-title "dotless Compiler $version" `
		-description "Dynamic CSS for .net" `
		-company "dotless project" `
		-product "dotless" `
		-version $version `
		-copyright "Copyright © dotless project 2009"
        
    new-item $build_dir -itemType directory
    new-item $release_dir -itemType directory
    
}

task Build -depends Init {
    msbuild dotless.Compiler\dotless.Compiler.csproj /p:OutDir=$build_dir /p:Configuration=$config
}

task Test -depends Build {
    msbuild nless.Test\nLess.Test.csproj /p:OutDir=$build_dir /p:Configuration=$config
    
    $old = pwd
    cd $build_dir
    & $lib_dir\NUnit\nunit-console-x86.exe $build_dir\nLess.Test.dll 
    if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute tests"
        if ($showtestresult)
        {
            start $build_dir\TestResult.xml
        }
    }
    cd $old
}

task Merge -depends Build {
    $old = pwd
    cd $build_dir
    $filename = "nLess.Core.dll"
    Remove-Item $filename-partial.dll -ErrorAction SilentlyContinue
    Rename-Item $filename $filename-partial.dll
    write-host "Executing ILMerge"
    & $lib_dir\ilmerge\ILMerge.exe $filename-partial.dll `
        PegBase.dll `
        /out:$filename `
        /internalize `
        /t:library
    if ($lastExitCode -ne 0) {
        throw "Error: Failed to merge assemblies"
    }
    cd $old
}

task Release-NoTest -depends Merge {
    $commit = Get-Git-Commit
    $filename = "nLess.core"
    & $lib_dir\7zip\7za.exe a $release_dir\dotless-$commit.zip `
    $build_dir\$filename.dll `
    $build_dir\$filename.pdb `
    $build_dir\dotless.compiler.exe
    #$build_dir\Testresult.xml `
    #license.txt `
    #acknowledgements.txt `
    
    Write-Host -ForegroundColor Yellow "Please note that no tests where run during release process!"
    Write-host "-----------------------------"
    Write-Host "dotless was successfully compiled and packaged."
    Write-Host "The release bits can be found in ""$release_dir"""
    Write-Host -ForegroundColor Cyan "Thank you for using dotless!"
}

task Release -depends Test, Merge {
    $commit = Get-Git-Commit
    $filename = "nLess.core"
    & $lib_dir\7zip\7za.exe a $release_dir\dotless-$commit.zip `
    $build_dir\$filename.dll `
    $build_dir\$filename.pdb `
    $build_dir\Testresult.xml `
    $build_dir\dotless.compiler.exe
    #license.txt `
    #acknowledgements.txt `
    
    Write-host "-----------------------------"
    Write-Host "dotless was successfully compiled and packaged."
    Write-Host "The release bits can be found in ""$release_dir"""
    Write-Host -ForegroundColor Cyan "Thank you for using dotless!"
}