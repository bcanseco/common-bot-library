#addin nuget:?package=NuGet.Core&version=2.14.0
#addin "Cake.ExtendedNuGet"

var version = Argument<string>("r");

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
            { "Version", version }
        },
    };
    DotNetCorePack("./src/CommonBotLibrary/", settings);
    DotNetCoreBuild("./tests/CommonBotLibrary.Tests/");
});

Task("Deploy")
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

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./artifacts");
});

Task("Default")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Deploy")
    .IsDependentOn("Clean")
    .Does(() =>
{
    Information($"Successfully published {version} to NuGet.");
});

RunTarget("Default");
