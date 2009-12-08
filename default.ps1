# This script was derived from the Rhino.Mocks buildscript written by Ayende Rahien.
include .\psake_ext.ps1

properties {
    $config = 'debug'
    $showtestresult = $FALSE
    $base_dir = resolve-path .
    $lib_dir = "$base_dir\lib\"
    $build_dir = "$base_dir\build\" 
    $release_dir = "$base_dir\release\"
    $source_dir = "$base_dir\src"
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
		-file "$source_dir\dotless.Core\Properties\AssemblyInfo.cs" `
		-title "dotless $version" `
		-description "Dynamic CSS for .net" `
		-company "dotless project" `
		-product "dotless" `
		-version $version `
		-copyright "Copyright © dotless project 2009"
    Generate-Assembly-Info `
		-file "$source_dir\dotless.Test\Properties\AssemblyInfo.cs" `
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
    msbuild $source_dir\dotless.Compiler\dotless.Compiler.csproj /p:OutDir=$build_dir /p:Configuration=$config
    if ($lastExitCode -ne 0) {
        throw "Error: compile failed"
    }
}

task Test -depends Build {
    msbuild $source_dir\dotless.Test\dotless.Test.csproj /p:OutDir=$build_dir /p:Configuration=$config
    if ($lastExitCode -ne 0) {
        throw "Error: Test compile failed"
    }
    $old = pwd
    cd $build_dir
    & $lib_dir\NUnit\nunit-console-x86.exe $build_dir\dotless.Test.dll 
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
	
	$filename = "dotless.Compiler.exe"
	Remove-Item $filename-partial.exe -ErrorAction SilentlyContinue
	Rename-Item $filename $filename-partial.exe
	& $lib_dir\ilmerge\ILMerge.exe $filename-partial.exe `
		Pandora.dll `
		dotless.Core.dll `
		$lib_dir\Pandora\Microsoft.Practices.ServiceLocation.dll `
        PegBase.dll `
        /out:$filename `
        /internalize `
        /t:exe
	if ($lastExitCode -ne 0) {
        throw "Error: Failed to merge compiler assemblies"
    }
	Remove-Item $filename-partial.exe -ErrorAction SilentlyContinue
	
    $filename = "dotless.Core.dll"
    Remove-Item $filename-partial.dll -ErrorAction SilentlyContinue
    Rename-Item $filename $filename-partial.dll
    write-host "Executing ILMerge"
    & $lib_dir\ilmerge\ILMerge.exe $filename-partial.dll `
        Pandora.dll `
        $lib_dir\Pandora\Microsoft.Practices.ServiceLocation.dll `
        PegBase.dll `
        /out:$filename `
        /internalize `
        /t:library
    if ($lastExitCode -ne 0) {
        throw "Error: Failed to merge assemblies"
    }
	Remove-Item $filename-partial.dll -ErrorAction SilentlyContinue
    cd $old
}

task Release-NoTest -depends Merge {
    $commit = Get-Git-Commit
    $filename = "dotless.core"
    & $lib_dir\7zip\7za.exe a $release_dir\dotless-$commit.zip `
    $build_dir\$filename.dll `
    $build_dir\$filename.pdb `
    $build_dir\dotless.compiler.exe `
	acknowledgements.txt `
    license.txt `
    #$build_dir\Testresult.xml `
    
    
    Write-Host -ForegroundColor Yellow "Please note that no tests where run during release process!"
    Write-host "-----------------------------"
    Write-Host "dotless $version was successfully compiled and packaged."
    Write-Host "The release bits can be found in ""$release_dir"""
    Write-Host -ForegroundColor Cyan "Thank you for using dotless!"
}

task t4css -depends Merge {
    $commit = Get-Git-Commit
    $dir = pwd
    $target = "$build_dir\t4css"
    echo "bla"
    mkdir $build_dir\t4css -ErrorAction silentlycontinue
    mkdir $build_dir\t4css\T4CssWeb -ErrorAction silentlycontinue
    mkdir $build_dir\t4css\T4CssWeb\Css -ErrorAction silentlycontinue
    cp $build_dir\dotless.Core.dll $target\dotless.Core.dll
    cp $dir\t4less\T4Css.sln $target\T4Css.sln
    cp $dir\t4less\T4CssWeb\T4CssWeb.csproj $target\T4CssWeb\T4CssWeb.csproj
    cp $dir\t4less\T4CssWeb\Css\T4CSS.tt $target\T4CssWeb\Css\T4CSS.tt
    cp $dir\t4less\T4CssWeb\Css\*.less $target\T4CssWeb\Css\
    cp $dir\t4less\T4CssWeb\Css\*.css $target\T4CssWeb\Css\
    cp $dir\t4less\T4CssWeb\Css\*.log $target\T4CssWeb\Css\
    
    
    & $lib_dir\7zip\7za.exe a $release_dir\t4css-$commit.zip `
    $build_dir\t4css\    
}

task Release -depends Test, Merge, t4css {
    $commit = Get-Git-Commit
    $filename = "dotless.core"
    & $lib_dir\7zip\7za.exe a $release_dir\dotless-$commit.zip `
    $build_dir\$filename.dll `
    $build_dir\$filename.pdb `
    $build_dir\Testresult.xml `
    $build_dir\dotless.compiler.exe `
    acknowledgements.txt `
    license.txt `
    
    
    Write-host "-----------------------------"
    Write-Host "dotless $version was successfully compiled and packaged."
    Write-Host "The release bits can be found in ""$release_dir"""
    Write-Host -ForegroundColor Cyan "Thank you for using dotless!"
}
