using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Reflection.Metadata;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Metadata;
using LiveDelegate.ILSpy.Providers;

namespace LiveDelegate.ILSpy
{
    public class DelegateReader : IDelegateReader
    {
        private IAssemblyProvider _assemblyProvider;

        #region Constructors

        private DelegateReader() { }

        internal DelegateReader(IAssemblyProvider assemblyProvider)
        {
            _assemblyProvider = assemblyProvider;
        }

        #endregion

        public SyntaxTree Read(Delegate @delegate)
        {
            Contract.Assert(@delegate?.Method?.DeclaringType != null);

            var asm = @delegate.Method.DeclaringType.Assembly;
            _assemblyProvider.Prepare(asm);
            var decompiler = new CSharpDecompiler(asm.Location, _assemblyProvider, new DecompilerSettings()
            {
                ExtensionMethods = false,
                NamedArguments = false
            });
            var token = @delegate.Method.MetadataToken;
            var method = MetadataTokenHelpers.TryAsEntityHandle(token);
            var ast = decompiler.Decompile(new List<EntityHandle>()
            {
                method.Value
            });

            return ast;
        }

        #region Factories

        /// <summary>
        /// Creates an IDelegateReader with the default assembly provider.
        /// The default assembly provider builds a full recursive assembly graph of all assemblies that are provided to it.
        /// </summary>
        /// <param name="throwOnMissing">If set to true, will throw if assembly could not be resolved</param>
        /// <returns></returns>
        public static IDelegateReader CreateWithDefaultAssemblyProvider(bool throwOnMissing = true) =>
            new DelegateReader(new RecursiveAssemblyProvider(throwOnMissing));

        /// <summary>
        /// Creates an IDelegateReader with the default assembly provider and a caching layer on top.
        /// The default assembly provider builds a full recursive assembly graph of all assemblies that are provided to it.
        /// </summary>
        /// <param name="throwOnMissing">If set to true, will throw if assembly could not be resolved</param>
        /// <returns></returns>
        public static IDelegateReader CreateCachedWithDefaultAssemblyProvider(bool throwOnMissing = true) =>
            new CachedDelegateReader(
                new DelegateReader(new RecursiveAssemblyProvider(throwOnMissing)));

        /// <summary>
        /// Creates an IDelegateReader with a constant set of assemblies.
        /// </summary>
        /// <param name="assemblies">A const`ant set of assemblies. The decompiler will reference only these assemblies.</param>
        /// <param name="addOnPrepare"></param>
        /// <param name="throwOnMissing">If set to true, will throw if assembly could not be resolved</param>
        /// <returns></returns>
        public static IDelegateReader CreateWithSetOfAssemblies(IList<Assembly> assemblies, bool addOnPrepare, bool throwOnMissing = true) =>
            new DelegateReader(new StaticAssemblyProvider(assemblies, addOnPrepare, throwOnMissing));

        /// <summary>
        /// Creates an IDelegateReader with a constant set of assemblies and a caching layer on top.
        /// </summary>
        /// <param name="assemblies">A constant set of assemblies. The decompiler will reference only these assemblies.</param>
        /// <param name="addOnPrepare">If this value is set to true, the assembly provider would </param>
        /// <param name="throwOnMissing">If set to true, will throw if assembly could not be resolved</param>
        /// <returns></returns>
        public static IDelegateReader CreateCachedWithSetOfAssemblies(IList<Assembly> assemblies, bool addOnPrepare, bool throwOnMissing = true) =>
            new CachedDelegateReader(
                new DelegateReader(new StaticAssemblyProvider(assemblies, addOnPrepare, throwOnMissing)));

        #endregion
    }
}