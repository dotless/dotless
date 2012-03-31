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
        -copyright "Copyright © dotless project 2010-2012" `
        -partial $True
    Generate-Assembly-Info `
        -file "$source_dir\dotless.Test\Properties\AssemblyInfo.cs" `
        -title "dotless Tests $version" `
        -description "Dynamic CSS for .net" `
        -company "dotless project" `
        -product "dotless" `
        -version $version `
        -copyright "Copyright © dotless project 2010-2012"
    Generate-Assembly-Info `
        -file "$source_dir\dotless.Compiler\Properties\AssemblyInfo.cs" `
        -title "dotless Compiler $version" `
        -description "Dynamic CSS for .net" `
        -company "dotless project" `
        -product "dotless" `
        -version $version `
        -copyright "Copyright © dotless project 2010-2012"
    Generate-Assembly-Info `
        -file "$source_dir\dotless.AspNet\Properties\AssemblyInfo.cs" `
        -title "dotless Compiler $version" `
        -description "Dynamic CSS for .net" `
        -company "dotless project" `
        -product "dotless" `
        -version $version `
        -copyright "Copyright © dotless project 2010-2012"

    new-item $build_dir -itemType directory
    new-item $release_dir -itemType directory
    
}

task Build -depends Init {
	$buildSettings = (
		($source_dir + '\dotless.Compiler\dotless.Compiler.csproj'),
		('/p:Configuration=' + $config),
		('/p:OutDir=' + $build_dir + '\')
	)
    msbuild $buildSettings
    if ($lastExitCode -ne 0) {
        throw "Error: compile failed"
    }
}

task Test -depends Build {
	$buildSettings = (
		($source_dir + '\dotless.Test\dotless.Test.csproj'),
		('/p:Configuration=' + $config),
		('/p:OutDir=' + $build_dir + '\')
	)
    msbuild $buildSettings
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
        Microsoft.Practices.ServiceLocation.dll `
        /out:$filename `
        /internalize `
        /keyfile:../src/dotless-open-source.snk `
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
        Microsoft.Practices.ServiceLocation.dll `
        dotless.AspNet.dll `
        /out:$filename `
        /internalize `
        /keyfile:../src/dotless-open-source.snk `
        /t:library
    if ($lastExitCode -ne 0) {
        throw "Error: Failed to merge assemblies"
    }
    
    $compilerfilename = "dotless.Compiler.dll"
    write-host "Executing ILMerge"
    & $lib_dir\ilmerge\ILMerge.exe $filename-partial.dll `
        Pandora.dll `
        Microsoft.Practices.ServiceLocation.dll `
        /out:$compilerfilename `
        /internalize `
        /keyfile:../src/dotless-open-source.snk `
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
    $build_dir\dotless.compiler.dll `
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

task Gem -depends Merge {
    $target = "$base_dir\gems"
    remove-item -force -recurse $target\lib -ErrorAction SilentlyContinue 
    new-item $target\lib -itemType directory
    remove-item -force -recurse $target\bin\ -ErrorAction SilentlyContinue 
    new-item $target\bin -itemType directory
    remove-item $target\VERSION -ErrorAction SilentlyContinue
    
    cp $build_dir\dotless.Core.dll $target\lib\dotless.Core.dll
    cp $build_dir\dotless.Compiler.exe $target\bin\dotless.Compiler.exe
    
    $fileContent = "result = system(File.dirname(__FILE__) + ""/dotless.Compiler.exe "" + ARGV.join(' '))
exit 1 unless result"
    out-file -filePath $target\bin\dotless -encoding ascii -inputObject $fileContent
    out-file -filePath $target\VERSION -encoding ascii -inputObject $version
    cd $target
    gem build dotless.gemspec
}

task Release -depends Test, Merge, NuGetPackage, t4css {
    $commit = Get-Git-Commit
    $filename = "dotless.core"
    & $lib_dir\7zip\7za.exe a $release_dir\dotless-$commit.zip `
    $build_dir\$filename.dll `
    $build_dir\$filename.pdb `
    $build_dir\Testresult.xml `
    $build_dir\dotless.compiler.exe `
    acknowledgements.txt `
    license.txt `
    
    Move-Item $build_dir\*.nupkg $release_dir\
    
    
    Write-host "-----------------------------"
    Write-Host "dotless $version was successfully compiled and packaged."
    Write-Host "The release bits can be found in ""$release_dir"""
    Write-Host -ForegroundColor Cyan "Thank you for using dotless!"
}


task NuGetPackage -depends Merge {
    $target = "$build_dir\NuGet\"
    remove-item -force -recurse $target -ErrorAction SilentlyContinue     
    New-Item $target -ItemType directory
    New-Item $target\lib -ItemType directory
    New-Item $target\tool -ItemType directory
    New-Item $target\content -ItemType directory
    
    Copy-Item $source_dir\Dotless.nuspec $target
    Copy-Item $source_dir\web.config.transform $target\content\
    Copy-Item $build_dir\dotless.Core.dll $target\lib\
    Copy-Item $build_dir\dotless.compiler.exe $target\tool\
    Copy-Item acknowledgements.txt $target
    Copy-Item license.txt $target
        
    .\lib\NuGet.exe pack $target\Dotless.nuspec -o $build_dir
}