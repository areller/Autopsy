using ICSharpCode.Decompiler.Metadata;
using System.Reflection;

namespace LiveDelegate.ILSpy
{
    interface IAssemblyProvider : IAssemblyResolver
    {
        void Prepare(Assembly assembly);
    }
}