#tool nuget:?package=NUnit.ConsoleRunner
#tool nuget:?package=vswhere

// ARGUMENTS
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// PREPARATION
// Define directories.
var buildDir = Directory("./BuildArtifacts") + Directory(configuration);
var outputDir = Directory("./BuildArtifacts/output");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    //CleanDirectory(buildDir);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./dotless.sln");
});

Task("Build")	
    .IsDependentOn("Restore")
    .Does(() =>
{    
	// get MSBuild 15 location
	var vsLatest  = VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild" });
	FilePath msBuildPath = (vsLatest==null)
                            ? null
                            : vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");

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
   // var packSettings = new DotNetCorePackSettings
	// {
		// OutputDirectory = outputDir,
		// NoBuild = true
	// };

	 // DotNetCorePack(projJson, settings);
});

// TASK TARGETS
Task("Default")
    .IsDependentOn("Test");

// EXECUTION
RunTarget(target);
