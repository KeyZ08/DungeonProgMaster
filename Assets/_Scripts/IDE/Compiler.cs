using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeCompiler = CSharpCompiler.CodeCompiler;

namespace DPM.Infrastructure.IDE
{
    public static class Compiler 
    {
        public static List<string> Compile(string program, string source, string methods)
        {
            var script = source;
            script = Regex.Replace(script, @"#region Task[\s\S]*?#endregion", program);
            script = Regex.Replace(script, @"#region Methods[\s\S]*?#endregion", methods);

            var CodeCompiler = new CodeCompiler();
            CompilerResults results = CodeCompiler.CompileAssemblyFromSource(new CompilerParameters(), script);
            if (results.Errors.HasErrors)
            {
                throw new Exception(results.Errors[0].ErrorText);
            }
            var cls = results.CompiledAssembly.GetType("Commands.CommandsCompiler");
            var method = cls.GetMethod("Script");

            var timeout = 10;
            var task = Task.Run(() => (List<string>)method.Invoke(null, null));
            task.Wait(timeout);
            if (task.IsCompleted)
                return task.Result;
            else
                throw new Exception("Слишком долгое выполнение кода.");
        }
    }
}
