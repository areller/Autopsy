# Autopsy

This project is currently under development, stay tuned :)  

![](https://travis-ci.com/areller/Autopsy.svg?branch=master)  

[![Nuget](https://img.shields.io/nuget/v/Autopsy.ILSpy)](https://www.nuget.org/packages/Autopsy.ILSpy)

Use Autopsy to read the syntax of compiled delegates.  

## Autopsy.ILSpy
Uses [ILSpy](https://github.com/icsharpcode/ILSpy) to decompile delegates by reading their IL.

```C#
IDelegateReader reader = DelegateReader.CreateCachedWithDefaultAssemblyProvider();

SyntaxTree GetSyntaxTree(MyFunc func)
{
    return reader.Read(func);
}

var tree = GetSyntaxTree(x =>
{
    if (x >= 0 && x < 5) return 1;
    else if (x >= 5 && x < 10) return 2;
    return 3;
});
Console.WriteLine(tree.ToString());
```

Prints

```C#
internal int <Main>b__2_0 (int x)
{
        if (x >= 0 && x < 5) {
                return 1;
        }
        if (x >= 5 && x < 10) {
                return 2;
        }
        return 3;
}
```

See [Demo Project](./tests/Autopsy.ILSpy.Demo)

## Autopsy.Roslyn

*Coming soon*