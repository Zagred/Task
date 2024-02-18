using System;
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
        private static List<string> CsFiles(List<string> files)
        {
            List<string> csFiles = new List<string>();
            foreach (var path in files)
            {
                foreach (var cs in Directory.GetFiles(path, "*.cs"))
                {
                    csFiles.Add(cs);
                }
            }
            return csFiles;
        }
        private static void xmlArguments(string path)
        {
            List<string> Folders = Init_Dict(path);
            List<string> xmlFiles = XmlFiles(Folders);
            List<string> csFiles = CsFiles(Folders);
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
            CallMethodByName(arguments, csFiles);

        }

        public static void CallMethodByName(List<string> args, List<string> cs)
        {
            List<string> notMonitoring = new List<string>();
            foreach (var arg in args)
            {
                // Console.WriteLine(arg);
                foreach (string path in cs)
                {
                    string fileText = null;
                    fileText = File.ReadAllText(path);
                    if (fileText.Contains(arg) && !fileText.Contains($"[MonitoringTask((int)eMonitoringTasks.{arg})]") && path.Contains("ShoogerProcessors"))
                    {
                        notMonitoring.Add(arg);
                    }
                }
                //if (arg.Length != 0)
                //{
                //    MethodInfo method = null;
                //    string methodName = arg;
                //    Type type = typeof(Program);

                //    method = type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

                //    if (method != null)
                //    {
                //        //Console.WriteLine(method.GetParameters());
                //    }
                //    else
                //    {
                //        Console.WriteLine($"The method {methodName} does not exist");

                //    }
                //}
            }

            foreach (string element in notMonitoring)
            {
                Console.WriteLine(element);
            }
        }

        static void Main(string[] args)
        {

            string basePath = @"C:\Users\PlamenPandev\Desktop\Projects\shared";
            xmlArguments(basePath);

        }
    }
}
