// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

#nullable enable

using System;
using System.IO;
using System.Reflection;
using NUnit.Common;
using NUnit.Framework;

namespace NUnit.Engine.Agents
{
    [NonParallelizable]
    public class DotNetExecutableTests
    {
        private string _directory = null!;
        private string? _originalRoot;
        private string? _originalX86Root;
        private MethodInfo _getExecutable = null!;

        [SetUp]
        public void SetUp()
        {
            _directory = Path.Combine(Path.GetTempPath(), "nunit-dotnet-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_directory);
            _originalRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
            _originalX86Root = Environment.GetEnvironmentVariable("DOTNET_ROOT_X86");
            Directory.CreateDirectory(Path.Combine(_directory, "native"));
            Directory.CreateDirectory(Path.Combine(_directory, "x86"));
            Environment.SetEnvironmentVariable("DOTNET_ROOT", Path.Combine(_directory, "native"));
            Environment.SetEnvironmentVariable("DOTNET_ROOT_X86", Path.Combine(_directory, "x86"));

            // Isolate the helper's cached install directories from other launcher tests.
            var assembly = Assembly.Load(File.ReadAllBytes(typeof(DotNet).Assembly.Location));
            _getExecutable = assembly.GetType(typeof(DotNet).FullName!)!
                .GetMethod(nameof(DotNet.GetDotNetExe))!;
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("DOTNET_ROOT", _originalRoot);
            Environment.SetEnvironmentVariable("DOTNET_ROOT_X86", _originalX86Root);
            Directory.Delete(_directory, true);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void FindsPlatformExecutable(bool runAsX86)
        {
            string expected = Path.Combine(_directory, runAsX86 ? "x86" : "native", Platform.IsWindows ? "dotnet.exe" : "dotnet");
            File.WriteAllText(expected, string.Empty);

            Assert.That(_getExecutable.Invoke(null, new object[] { runAsX86 }), Is.EqualTo(expected));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void RejectsExecutableForOtherPlatform(bool runAsX86)
        {
            File.WriteAllText(Path.Combine(_directory, runAsX86 ? "x86" : "native", Platform.IsWindows ? "dotnet" : "dotnet.exe"), string.Empty);

            Assert.That((Action)(() => _getExecutable.Invoke(null, new object[] { runAsX86 })),
                Throws.TypeOf<TargetInvocationException>().With.InnerException.TypeOf<Exception>());
        }

        [Test]
        public void MissingExecutableDiagnosticNamesPlatformExecutable()
        {
            string name = Platform.IsWindows ? "dotnet.exe" : "dotnet";

            Assert.That((Action)(() => _getExecutable.Invoke(null, new object[] { false })),
                Throws.TypeOf<TargetInvocationException>().With.InnerException.Message.EqualTo($"Unable to locate {name}."));
        }
    }
}
