using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using Microsoft.Office.Interop.Excel;
using TestLibrary;

namespace Task
{
    internal class Program
    {

        private static List<string> Init_Dict(string folder)
        {
            List<string> folderContent = new List<string>();
            foreach (var directory in Directory.GetDirectories(folder))
            {
                folderContent.Add(directory);
                folderContent.AddRange(Init_Dict(directory));

            }
            return folderContent;
        }
        private static List<string> XmlFiles(List<string> files)
        {
            List<string> xmlFiles = new List<string>();
            foreach (var path in files)
            {
                foreach (var xml in Directory.GetFiles(path, "*.xml"))
                {
                    xmlFiles.Add(xml);
                }
            }
            return xmlFiles;
        }

        private static void xmlArguments(string path)
        {
            
            List<string> Folders = Init_Dict(path);
            
            List<string> xmlFiles = XmlFiles(Folders);
            List<string> arguments = new List<string>();

            var htmlDoc = new HtmlDocument();

            foreach (var xml in xmlFiles)
            {
                htmlDoc.Load(xml);

                var tags = htmlDoc.DocumentNode.Descendants("Arguments");
                foreach (var tag in tags)
                {
                    string formatElement = tag.InnerText.Trim();
                    
                    arguments.Add(formatElement);
                }
            }
            CallMethodByName(arguments);

        }

        public static void CallMethodByName(List<string> args)
        {

            List<string> notMonitoring = new List<string>();
            foreach (var arg in args)
            {

                if (arg.Length != 0)
                {
                    try
                    {
                        Assembly SampleAssembly;
                        SampleAssembly = Assembly.LoadFrom(@"C:\Users\PlamenPandev\Desktop\Projects\shared\ShoogerProcessors\bin\Debug\ShoogerProcessors.exe");
                        MethodInfo Method = SampleAssembly.GetType("ShoogerProcessors.Program").GetMethod(arg, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                        if (Method != null)
                        {
                            Method.CustomAttributes.First().ToString();
                            Console.WriteLine(Method.CustomAttributes.First().ToString());
                        }
                        
                    }
                    catch
                    {
                        notMonitoring.Add(arg);
                    }
                }
            }
            foreach (string element in notMonitoring)
            {
                Console.WriteLine(element);
            }
        }


        public static void LibraryTest()
        {
            Assembly SampleAssembly, LibraryAssembly;
            SampleAssembly = Assembly.LoadFrom(@"C:\Users\PlamenPandev\Desktop\Projects\TaskSecond\TaskSecond\bin\Release\TaskSecond.exe");
            //LibraryAssembly = Assembly.LoadFrom(@"C:\Users\PlamenPandev\Desktop\Projects\TestLibrary\TestLibrary\bin\Debug\TestLibrary.dll");
            LibraryAssembly = Assembly.LoadFrom(@"C:\Users\PlamenPandev\Desktop\Projects\TestLibrary\TestLibrary\bin\Debug\TestLibrary.dll");
            List<string> arguments = new List<string>();
            List<string> libArguments = new List<string>();
            // MethodInfo MethodInfo = SampleAssembly.GetType("TaskSecond.Program") 
            //MethodInfo MethodInfo = SampleAssembly.GetType("TestLibrary.Library")
            // MethodInfo[] Method = SampleAssembly.GetType("TestLibrary.Library").GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var libMethods = LibraryAssembly.GetType("TestLibrary.Library").GetMethods();
            var mainMethods=SampleAssembly.GetType("TaskSecond.Program").GetMethods();
            
            foreach(var method in libMethods)
            {
                libArguments.Add(method.Name);
                foreach (var arg in mainMethods)
                {
                    if (method.Name == arg.Name)
                    {
                        arguments.Add(arg.Name);
                    }
                }
            }
            
            foreach(var method in arguments)
            {
                libArguments.Remove(method);
            }
            foreach (var arg in libArguments)
            {
                Console.WriteLine(arg);
            }

        }
        static void Main(string[] args)
        {

            //string basePath = @"C:\Users\PlamenPandev\Desktop\Projects\shared";
            //xmlArguments(basePath);
            LibraryTest();
            
        }
    }
}