using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlayerTest
{
    class Compiler
    {
        public static object Compile(string program)
        {
            var script = File.ReadAllText(@"C:\Users\sasha\source\repos\PlayerTest\PlayerTest\Script.cs");
            script = script.Replace("// to do", program);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromSource(new CompilerParameters(), script);

            var cls = results.CompiledAssembly.GetType("Player.Program");
            var method = cls.GetMethod("Script");
            return method.Invoke(null, null);
        }
    }

    class Program
    {
        static void Main()
        {
            var program = @"
                for (int i = 0; i < 12; i++)
                    Player.Go();
            ";

            var result = (List<string>)Compiler.Compile(program);
            foreach (var el in result)
                Console.WriteLine(el);
        }
    }
}