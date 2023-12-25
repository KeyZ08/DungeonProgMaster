using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class Compiler
{
    public static List<string> Compile(string program, string source)
    {
        var script = source;
        script = Regex.Replace(script, @"#region[\s\S]*?#endregion", program);

        CSharpCodeProvider provider = new CSharpCodeProvider();
        CompilerResults results = provider.CompileAssemblyFromSource(new CompilerParameters(), script);
        //Debug.Log(results.Errors.Count);
        //Debug.Log(results.Errors[0]);
        var cls = results.CompiledAssembly.GetType("Commands.CommandsCompiler");
        var method = cls.GetMethod("Script");

        var timeout = 10;
        var task = Task.Run(() => (List<string>)method.Invoke(null, null));
        task.Wait(timeout);
        if (task.IsCompleted)
            return task.Result;
        else
            throw new Exception("Ваша программа работает очень долго");
    }

    public static void TestCompiling(string source)
    {
        var program = @"
                for (int i = 0; i < 12; i++)
                    Player.Go();
            ";

        var result = Compile(program, source);
        foreach (var el in result)
            Debug.Log(el);
    }
}
