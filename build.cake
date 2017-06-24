#addin nuget:?package=NuGet.Core&version=2.14.0
#addin "Cake.ExtendedNuGet"

Task("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreRestoreSettings
    {
        Sources = new[] { "https://www.nuget.org/api/v2" }
    };
    DotNetCoreRestore(settings);
});

Task("Build")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = "Release",
        OutputDirectory = "./artifacts/",
        EnvironmentVariables = new Dictionary<string, string> {
            { "ReleaseNotes", "Testing..." },
        },
    };
    DotNetCorePack("./src/CommonBotLibrary/", settings);
    DotNetCoreBuild("./tests/CommonBotLibrary.Tests/");
});

Task("Deploy")
    .WithCriteria(Branch == "dev")
    .Does(() =>
{
    var settings = new NuGetPushSettings
    {
        Source = "https://www.nuget.org/api/v2/package",
        ApiKey = EnvironmentVariable("NUGET_KEY")
    };
    var packages = GetFiles("./artifacts/*.nupkg");
    NuGetPush(packages, settings);
});

Task("Default")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Deploy")
    .Does(() => 
{
    Information("Successfully pushed to NuGet.");
});

RunTarget("Default");
