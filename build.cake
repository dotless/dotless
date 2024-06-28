#tool nuget:?package=NUnit.ConsoleRunner
#tool nuget:?package=vswhere
#tool "nuget:?package=GitVersion.CommandLine&version=5.7.0"

#addin "nuget:?package=Cake.FileHelpers&version=3.1.0"

// ARGUMENTS
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var version = Argument("BuildVersion", "0.0.1");

// PREPARATION
// Define directories.
var outputDir = Directory("./BuildArtifacts");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(outputDir);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./dotless.sln");
});


Task("Version")
	.Description("Retrieves the current version from the git repository")
	.Does(() => {
		
    if (version == "0.0.1")
    {
        var versionInfo = GitVersion(new GitVersionSettings {
            UpdateAssemblyInfo = false
        });
        
        version = versionInfo.AssemblySemVer;

        Information("Version: "+ version);        
    }
});

Task("SetVersion")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .Does(() =>
{
	ReplaceRegexInFiles("./src/dotless.AspNet/Properties/AssemblyInfo.cs", "(?<=AssemblyVersion\\(\")(.+?)(?=\"\\))", version);
	ReplaceRegexInFiles("./src/dotless.AspNet/Properties/AssemblyInfo.cs", "(?<=AssemblyFileVersion\\(\")(.+?)(?=\"\\))", version);
	
	ReplaceRegexInFiles("./src/dotless.Compiler/dotless.Compiler.csproj", "<Version>0.0.0.1</Version>", $"<Version>{version}</Version>");
	ReplaceRegexInFiles("./src/dotless.Core/dotless.Core.csproj", "<Version>0.0.0.1</Version>", $"<Version>{version}</Version>");	
});


Task("Build")	
    .IsDependentOn("Restore")
	.IsDependentOn("SetVersion")
    .Does(() =>
{    
	// get MSBuild 15 location
	var vsLatest  = VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild" });
    
	FilePath msBuildPath = (vsLatest==null)
                            ? null
                            : GetFiles(System.IO.Path.Combine(vsLatest.ToString(), "MSBuild/*/Bin/MSBuild.exe")).Single();

	Information("Using MSBuild "+ msBuildPath);

	MSBuild("./dotless.sln", new MSBuildSettings {
            Verbosity = Verbosity.Minimal,
			ToolPath = msBuildPath,
            Configuration = configuration,
            PlatformTarget = PlatformTarget.MSIL // AnyCPU
        });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
	// run tests for .NET core projects
	DotNetCoreTest("./tests/dotless.Core.Test/dotless.Core.Test.csproj");
	
	// run other tests
    NUnit3("./tests/dotless.AspNet.Test/bin/" + configuration + "/*.Test.dll", 
		new NUnit3Settings { NoResults = true }
		);
		
	NUnit3("./tests/dotless.CompatibilityTests/bin/" + configuration + "/*.Test.dll", 
		new NUnit3Settings { NoResults = true }
		);
});

Task("Publish")
    .IsDependentOn("Test")
    .Does(() =>
{	
	var nuGetPackSettings   = new NuGetPackSettings {                                    
                                     Version                 = version,                                     
                                     NoPackageAnalysis       = false,                                   
                                     BasePath                = "./src/dotless.Core/bin/"+configuration,
                                     OutputDirectory         = outputDir
                                 };

     NuGetPack("./nuspec/dotless.Core.nuspec", nuGetPackSettings);
	 
	 nuGetPackSettings.BasePath = "./src/dotless.AspNet/bin/"+configuration;
	 NuGetPack("./nuspec/dotless.AspNetHandler.nuspec", nuGetPackSettings);
	 
	 nuGetPackSettings.BasePath = "./";
	 NuGetPack("./nuspec/dotless.nuspec", nuGetPackSettings);
	 
	 nuGetPackSettings.BasePath = "./src/dotless.Compiler/bin/"+configuration;
	 NuGetPack("./nuspec/dotless.CLI.nuspec", nuGetPackSettings);
});

// TASK TARGETS
Task("Default")
    .IsDependentOn("Test");

// EXECUTION
RunTarget(target);
