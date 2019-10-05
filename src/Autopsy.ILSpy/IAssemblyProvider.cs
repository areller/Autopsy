using ICSharpCode.Decompiler.Metadata;
using System.Reflection;

namespace Autopsy.ILSpy
{
    interface IAssemblyProvider : IAssemblyResolver
    {
        void Prepare(Assembly assembly);
    }
}