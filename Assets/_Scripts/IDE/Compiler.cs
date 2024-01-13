using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPM.Infrastructure
{
    public static class Compiler
    {
        public static List<string> Compile(string program, string source, string methods)
        {
            var script = source;
            script = Regex.Replace(script, @"#region Task[\s\S]*?#endregion", program);
            script = Regex.Replace(script, @"#region Methods[\s\S]*?#endregion", methods);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromSource(new CompilerParameters(), script);
            if (results.Errors.HasErrors)
            {
                //Debug.Log(results.Errors[0].Line - 55);
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
                throw new Exception("���� ��������� �������� ����� �����");
        }
    }
}
