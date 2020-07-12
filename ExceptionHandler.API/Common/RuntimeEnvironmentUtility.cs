using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ExceptionHandler.API.Common
{
    public static class RuntimeEnvironmentUtility
    {
        public static string GetTypeAssemblyVersion(Type theType)
        {
            return theType.Assembly.GetName().Version.ToString(3);
        }

        public static string[] GetAssemblyTargetFrameworks(Type typeFromAssembly)
        {
            return RuntimeEnvironmentUtility.GetAssemblyTargetFrameworks(typeFromAssembly.Assembly);
        }

        public static string[] GetAssemblyTargetFrameworks(Assembly theAssembly)
        {
            return ((IEnumerable<TargetFrameworkAttribute>)theAssembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), false).Cast<TargetFrameworkAttribute>().ToArray<TargetFrameworkAttribute>()).Select<TargetFrameworkAttribute, string>((Func<TargetFrameworkAttribute, string>)(a => a.FrameworkName)).ToArray<string>();
        }

        public static string GetOSDescription()
        {
            return RuntimeInformation.OSDescription;
        }

        public static string GetCpuArchitecture()
        {
            return RuntimeInformation.OSArchitecture.ToString();
        }
    }
}
