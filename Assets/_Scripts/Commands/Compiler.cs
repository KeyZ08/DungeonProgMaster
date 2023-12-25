using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Generic;

public static class Compiler
{
    public static List<string> Compile(string program, string source)
    {
        var script = source;
        script = script.Replace("// to do", program);

        CSharpCodeProvider provider = new CSharpCodeProvider();
        CompilerResults results = provider.CompileAssemblyFromSource(new CompilerParameters(), script);
        //UnityEngine.Debug.Log(results.Errors[0]);
        var cls = results.CompiledAssembly.GetType("Commands.CommandsCompiler");
        var method = cls.GetMethod("Script");
        return (List<string>)method.Invoke(null, null);
    }

    public static void TestCompiling(string source)
    {
        var program = @"
                for (int i = 0; i < 12; i++)
                    Player.Go();
            ";

        var result = Compile(program, source);
        foreach (var el in result)
            UnityEngine.Debug.Log(el);
    }
}
