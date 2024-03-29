﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsScripts;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace test
{
    public class AdvancedTest
    {
        public void teste1()
        {
            String text = File.ReadAllText("script1.txt");
            String[] references = new String[] { "System.dll", "mscorlib.dll" };
            String srt = Generator.GenerateFullClass(references, null, "azulteste", "bla", text, "Method");

            Console.WriteLine(srt);
            StreamWriter sw = File.CreateText("teste.cs");
            sw.Write(srt);
            sw.Close();

            String erro;
            Assembly Assembly = Compilers.GenerateAssembly(srt, references, out erro);
            if (erro != null)
            {
                MessageBox.Show(erro);
                return;
            }

            Executor.Execute("azulteste.bla", Assembly, "Method");

            Console.ReadLine();
        }


        public void teste2()
        {
            String text = File.ReadAllText("script2.txt");
            List<String> references = new List<String> { "System.dll", "mscorlib.dll", "test.exe" };
            List<String> imports = new List<String> { "test" };

            Generator GenerateClassCode = new PloobsScripts.Generator(references, imports, "TesteInter");
            GenerateClassCode.GenerateClass("teste", "interteste");

            GenerateClassCode.GenerateMethod("handle", text, typeof(void), System.CodeDom.MemberAttributes.Public);

            String srt = GenerateClassCode.GetCode();

            Console.WriteLine(srt);
            StreamWriter sw = File.CreateText("teste2.cs");
            sw.Write(srt);
            sw.Close();

            String erro;
            Assembly Assembly = Compilers.GenerateAssembly(srt, references, out erro);
            if (erro != null)
            {
                MessageBox.Show(erro);
                return;
            }

            interteste interteste = Executor.BindTypeFromAssembly<interteste>(Assembly, GenerateClassCode.TypeName);

            interteste.handle();

            Console.ReadLine();
        }

        public void teste3()
        {
            ScriptParsed ScriptParsed = Parser.ParseScriptFile("script3.txt");

            List<String> references = new List<string>()
            {             
            "test.exe"
            };

            List<String> usings = new List<string>()
            {
                "test"
            };
            usings.AddRange(ScriptParsed.UsingStatements);

            Generator GenerateClassCode = new PloobsScripts.Generator(references,usings, "TesteInter");
            GenerateClassCode.GenerateClass("teste", "interteste");

            GenerateClassCode.GenerateMethod("handle", ScriptParsed.MethodCode, typeof(void), System.CodeDom.MemberAttributes.Public);

            String srt = GenerateClassCode.GetCode(ScriptParsed);

            Console.WriteLine(srt);
            StreamWriter sw = File.CreateText("teste3.cs");
            sw.Write(srt);
            sw.Close();

            String erro;
            Assembly Assembly = Compilers.GenerateAssembly(srt, references, out erro);
            if (erro != null)
            {
                MessageBox.Show(erro);
                return;
            }

            interteste interteste = Executor.BindTypeFromAssembly<interteste>(Assembly, GenerateClassCode.TypeName);

            interteste.handle();

            Console.ReadLine();

        }



        public void teste4()
        {
            ScriptParsed ScriptParsed = Parser.ParseScriptFile("script4.txt");

            List<String> references = new List<string>()
            {             
            "test.exe"
            };

            List<String> usings = new List<string>()
            {
                "test","System","System.Collections.Generic"
            };
            usings.AddRange(ScriptParsed.UsingStatements);

            Generator GenerateClassCode = new PloobsScripts.Generator(references, usings, "TesteInter");
            GenerateClassCode.GenerateClass("teste", "interteste");

            GenerateClassCode.GenerateMethod("handle", ScriptParsed.MethodCode, typeof(void), System.CodeDom.MemberAttributes.Public);

            String srt = GenerateClassCode.GetCode(ScriptParsed);

            Console.WriteLine(srt);
            StreamWriter sw = File.CreateText("teste3.cs");
            sw.Write(srt);
            sw.Close();

            String erro;
            Assembly Assembly = Compilers.GenerateAssembly(srt, references, out erro);
            if (erro != null)
            {
                MessageBox.Show(erro);
                return;
            }

            interteste interteste = Executor.BindTypeFromAssembly<interteste>(Assembly, GenerateClassCode.TypeName);

            interteste.handle();
            interteste.handle();
            interteste.handle();

            Console.ReadLine();

        }

    }

    public interface interteste
    {
        void handle();
    }
}
