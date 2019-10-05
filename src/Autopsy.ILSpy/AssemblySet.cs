using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace Autopsy.ILSpy
{
    public static class AssemblySet
    {
        public static readonly IList<Assembly> Core = new List<Assembly>()
        {
            typeof(object).Assembly,
            Assembly.Load(Assembly.GetEntryAssembly().GetReferencedAssemblies().FirstOrDefault(asm => asm.Name == "System.Runtime"))
        };
    }
}