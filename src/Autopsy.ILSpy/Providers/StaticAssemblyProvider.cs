using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Autopsy.ILSpy.Exceptions;
using ICSharpCode.Decompiler.Metadata;

namespace Autopsy.ILSpy.Providers
{
    class StaticAssemblyProvider : IAssemblyProvider
    {
        private ConcurrentDictionary<string, Assembly> _assembliesByName;
        private ConcurrentDictionary<string, Assembly> _assemblies;

        private bool _addOnPrepare;
        private bool _throwOnMissing;

        public StaticAssemblyProvider(IList<Assembly> assemblies, bool addOnPrepare, bool throwOnMissing)
        {
            _assembliesByName = new ConcurrentDictionary<string, Assembly>(assemblies.ToDictionary(asm => asm.GetName().Name, asm => asm));
            _assemblies = new ConcurrentDictionary<string, Assembly>(assemblies.ToDictionary(asm => asm.FullName, asm => asm));
            _addOnPrepare = addOnPrepare;
            _throwOnMissing = throwOnMissing;
        }

        public void Prepare(Assembly assembly)
        {
            if (!_addOnPrepare && !_assemblies.ContainsKey(assembly.FullName))
            {
                throw new AssemblyNotFoundException(assembly);
            }

            _assemblies.TryAdd(assembly.FullName, assembly);
            _assembliesByName.TryAdd(assembly.GetName().Name, assembly);
        }

        public PEFile Resolve(IAssemblyReference reference)
        {
            if (!_assemblies.TryGetValue(reference.FullName, out Assembly asm))
            {
                if (!_assembliesByName.TryGetValue(reference.Name, out asm))
                {
                    if (_throwOnMissing)
                        throw new AssemblyResolvingException(reference.FullName);
                    else
                        return null;
                }
            }

            var file = asm.Location;
            return new PEFile(file, new FileStream(file, FileMode.Open, FileAccess.Read), PEStreamOptions.Default,
                    MetadataReaderOptions.Default);
        }

        public PEFile ResolveModule(PEFile mainModule, string moduleName)
        {
            var baseDir = Path.GetDirectoryName(mainModule.FileName);
            var moduleFileName = Path.Combine(baseDir, moduleName);
            if (!File.Exists(moduleFileName))
            {
                if (_throwOnMissing)
                    throw new AssemblyResolvingException(moduleName);
                else
                    return null;
            }

            return new PEFile(moduleFileName, new FileStream(moduleFileName, FileMode.Open, FileAccess.Read), PEStreamOptions.Default,
                MetadataReaderOptions.Default);
        }
    }
}