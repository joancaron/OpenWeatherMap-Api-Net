#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/OpenWeatherMap/bin") + Directory(configuration);
var solutionFile = File("./src/OpenWeatherMap.sln");
var version = "1.1.0";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("CleanPackages")
    .Does(() =>
{
	CleanDirectory("./artifacts/packages");
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./src/OpenWeatherMap.sln", settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild("./src/OpenWeatherMap.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});

Task("Package")
    .Description("Packages all nuspec files into nupkg packages.")
    .IsDependentOn("Build")
    //.IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
	var artifactsPath = Directory("./artifacts/packages");
	var nugetProps = new Dictionary<string, string>() { {"Configuration", configuration} };

	CreateDirectory(artifactsPath);

    var nuspecFileSpec = "./nuspec/OpenWeatherMap.nuspec";
    Information("Nuspec path: " + nuspecFileSpec);
		
    // if (buildSettings.NuGet.UpdateVersion)
    // {
    //     VersionUtils.UpdateNuSpecVersion(Context, buildSettings, versionInfo, nuspecFileSpec.ToString());
	// }

	// VersionUtils.UpdateNuSpecVersionDependency(Context, buildSettings, versionInfo, nuspecFileSpec.ToString());

	NuGetPack(nuspecFileSpec, new NuGetPackSettings {
			Version = version,
			//ReleaseNotes = versionInfo.ReleaseNotes,
			Symbols = true,
			Properties = nugetProps,
			OutputDirectory = artifactsPath,
			ArgumentCustomization = args => args.Append("-NoDefaultExcludes")
	});
});

Task("Publish")
    .Description("Publishes all of the nupkg packages to the nuget server. ")
    .IsDependentOn("Package")
    .Does(() =>
{
	var nupkgFiles = GetFiles("./artifacts/packages/*.nupkg");
	foreach(var pkg in nupkgFiles)
	{
		// Lets skip everything except the current version and we can skip the symbols pkg for now
		if (!pkg.ToString().Contains(version) || pkg.ToString().Contains("symbols")) {
			Information("Skipping {0}", pkg);
			continue;
		}

		Information("Publishing {0}", pkg);

		var nugetSettings = new NuGetPushSettings {
			Source = EnvironmentVariable("NUGET_FEED_URL"),
			ConfigFile = "./src/.nuget/NuGet.config",
			Verbosity = NuGetVerbosity.Detailed
		};

		var feedApiKey = EnvironmentVariable("NUGET_FEED_APIKEY");
		if (!string.IsNullOrEmpty(feedApiKey))
		{
			nugetSettings.ApiKey = feedApiKey;
		}

		NuGetPush(pkg, nugetSettings);
	}
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
