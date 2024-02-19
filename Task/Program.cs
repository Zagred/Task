using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
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

        static void Main(string[] args)
        {

            string basePath = @"C:\Users\PlamenPandev\Desktop\Projects\Task\Task";
            xmlArguments(basePath);

        }
    }
}
