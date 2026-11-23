// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System.Reflection;

namespace NUnit.Agents
{
    public class Net80Agent : NUnitAgent<Net80Agent>
    {
        private static readonly Logger log = InternalTrace.GetLogger(typeof(Net80Agent));

        public static void Main(string[] args)
        {
            log.Debug("Using nunit.agent.core version " +
                typeof(NUnitAgent<Net80Agent>).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version);
            Execute(args);
        }
    }
}
