using ICSharpCode.Decompiler.CSharp.Syntax;
using System;

namespace Autopsy.ILSpy
{
    public interface IDelegateReader
    {
        /// <summary>
        /// Reads the C# syntax tree of a compiled delegate
        /// </summary>
        /// <param name="delegate">A compiled delegate</param>
        /// <returns>The C# syntax tree of that delegate</returns>
        SyntaxTree Read(Delegate @delegate);
    }
}