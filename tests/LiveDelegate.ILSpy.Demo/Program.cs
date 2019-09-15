using ICSharpCode.Decompiler.CSharp.Syntax;
using System;

namespace LiveDelegate.ILSpy.Demo
{
    delegate int MyFunc(int num);

    class Program
    {
        static IDelegateReader reader = DelegateReader.CreateCachedWithDefaultAssemblyProvider();

        static SyntaxTree GetSyntaxTree(MyFunc func)
        {
            return reader.Read(func);
        }

        static void Main(string[] args)
        {
            var tree = GetSyntaxTree(x =>
            {
                if (x >= 0 && x < 5) return 1;
                else if (x >= 5 && x < 10) return 2;
                return 3;
            });
            Console.WriteLine(tree.ToString());
        }
    }
}