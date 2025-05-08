// Load the recipe
#load nuget:?package=NUnit.Cake.Recipe&version=1.5.0-alpha.4
// Comment out above line and uncomment below for local tests of recipe changes
//#load ../NUnit.Cake.Recipe/recipe/*.cake

BuildSettings.Initialize
(
    context: Context,
    title: "Net80PluggableAgent",
    solutionFile: "net80-pluggable-agent.sln",
    unitTests: "**/*.tests.exe",
    githubOwner: "NUnit",
    githubRepository: "net80-pluggable-agent"
);

var PackageTests = new PackageTest[] {
    //new PackageTest(1, "NetCore11PackageTest")
    //{
    //    Description = "Run mock-assembly.dll targeting .NET Core 1.1",
    //    Arguments = "tests/netcoreapp1.1/mock-assembly.dll",
    //    ExpectedResult = new ExpectedResult("Failed")
    //    {
    //        Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
    //        Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
    //    }
    //},
    //new PackageTest(1, "NetCore21PackageTest")
    //{
    //    Description = "Run mock-assembly.dll targeting .NET Core 2.1",
    //    Arguments = "tests/netcoreapp2.1/mock-assembly.dll",
    //    ExpectedResult = new ExpectedResult("Failed")
    //    {
    //        Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
    //        Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
    //    }
    //},
    new PackageTest(1, "NetCore31PackageTest")
    {
        Description = "Run mock-assembly.dll targeting .NET Core 3.1",
        Arguments = "testdata/netcoreapp3.1/mock-assembly.dll",
        ExpectedResult = new ExpectedResult("Failed")
        {
            Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
        }
    },
    //new PackageTest(1, "Net50PackageTest")
    //{
    //    Description = "Run mock-assembly.dll targeting .NET 5.0",
    //    Arguments = "testdata/net5.0/mock-assembly.dll",
    //    ExpectedResult = new ExpectedResult("Failed")
    //    {
    //        Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
    //        Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
    //    }
    //},
    new PackageTest(1, "Net60PackageTest")
    {
        Description = "Run mock-assembly.dll targeting .NET 6.0",
        Arguments = "testdata/net6.0/mock-assembly.dll",
        ExpectedResult = new ExpectedResult("Failed")
        {
            Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
        }
    },
    new PackageTest(1, "Net70PackageTest")
    {
        Description = "Run mock-assembly.dll targeting .NET 7.0",
        Arguments = "testdata/net7.0/mock-assembly.dll",
        ExpectedResult = new ExpectedResult("Failed")
        {
            Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
        }
    },
    new PackageTest(1, "Net80PackageTest")
    {
        Description = "Run mock-assembly.dll targeting .NET 8.0",
        Arguments = "testdata/net8.0/mock-assembly.dll",
        ExpectedResult = new ExpectedResult("Failed")
        {
            Total = 35, Passed = 21, Failed = 5, Warnings = 1, Inconclusive = 1, Skipped = 7,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("mock-assembly.dll") }
        }
    },
    new PackageTest(1, "AspNetCore80Test")
    {
        Description = "Run test using AspNetCore targeting .NET 8.0",
        Arguments = "testdata/net8.0/aspnetcore-test.dll",
        ExpectedResult = new ExpectedResult("Passed")
        {
            Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("aspnetcore-test.dll") }
        }
    },
    new PackageTest(1, "Net80WindowsFormsTest")
    {
        Description = "Run test using windows forms under .NET 8.0",
        Arguments = "testdata/net8.0-windows/windows-test.dll",
        ExpectedResult = new ExpectedResult("Passed")
        {
            Total = 2, Passed = 2, Failed = 0, Warnings = 0, Inconclusive = 0, Skipped = 0,
            Assemblies = new ExpectedAssemblyResult[] { new ExpectedAssemblyResult("windows-test.dll") }
        }
    }
};

BuildSettings.Packages.Add(new NuGetPackage(
    id: "NUnit.Extension.Net80PluggableAgent",
    source: BuildSettings.NuGetDirectory + "Net80PluggableAgent.nuspec",
    checks: new PackageCheck[]
    {
        HasFiles("LICENSE.txt", "README.md", "nunit_256.png"),
        HasDirectory("tools").WithFiles(
            "nunit-agent-launcher-net80.dll", "nunit.engine.api.dll", "nunit.agent.core.dll"),
        HasDirectory("tools/agent").WithFiles(
            "nunit-agent-net80.dll", "nunit.engine.api.dll", "nunit.common.dll", 
            "nunit.extensibility.api.dll", "nunit.extensibility.dll", "nunit.agent.core.dll",
            "TestCentric.Metadata.dll", "Microsoft.Extensions.DependencyModel.dll")
    },
    //symbols: new PackageCheck[]
    testRunner: new AgentRunner(BuildSettings.NuGetTestDirectory + "NUnit.Extension.Net80PluggableAgent." + BuildSettings.PackageVersion + "/tools/agent/nunit-agent-net80.dll"),
    tests: PackageTests
    ));

BuildSettings.Packages.Add(new ChocolateyPackage(
    "nunit-extension-net80-pluggable-agent",
    source: BuildSettings.ChocolateyDirectory + "net80-pluggable-agent.nuspec",
    checks: new PackageCheck[]
    {
        HasDirectory("tools").WithFiles(
            "LICENSE.txt", "README.md", "nunit_256.png", "VERIFICATION.txt",
            "nunit-agent-launcher-net80.dll", "nunit.engine.api.dll", "nunit.agent.core.dll"),
        HasDirectory("tools/agent").WithFiles(
            "nunit-agent-net80.dll", "nunit.engine.api.dll", "nunit.common.dll",
            "nunit.extensibility.api.dll", "nunit.extensibility.dll", "nunit.agent.core.dll",
            "TestCentric.Metadata.dll", "Microsoft.Extensions.DependencyModel.dll")
    },
//    packageContent: new PackageContent()
//        .WithRootFiles("../../nunit.png")
//        .WithDirectories(
//            new DirectoryContent("tools").WithFiles(
//                "../../LICENSE.txt", "../../README.md", "../../VERIFICATION.txt",
//                "net80-agent-launcher.dll", "net80-agent-launcher.pdb",
//                "NUnit.Extensibility.Api.dll", "NUnit.Engine.Api.dll"),
//            new DirectoryContent("tools/agent").WithFiles(
//                "agent/net80-agent.dll", "agent/net80-agent.pdb", "agent/net80-agent.dll.config", "agent/NUnit.Agent.Core.dll",
//                "agent/net80-agent.deps.json", $"agent/net80-agent.runtimeconfig.json",
//                "agent/NUnit.InternalTrace.dll", "agent/NUnit.Metadata.dll",
//                "agent/NUnit.Extensibility.dll", "agent/NUnit.Extensibility.Api.dll",
//                "agent/NUnit.Engine.Api.dll", "agent/Microsoft.Extensions.DependencyModel.dll")),
    testRunner: new AgentRunner(BuildSettings.ChocolateyTestDirectory + "nunit-extension-net80-pluggable-agent." + BuildSettings.PackageVersion + "/tools/agent/nunit-agent-net80.dll"),
    tests: PackageTests));

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

Build.Run();
