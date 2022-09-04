using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using IMAP_App.Main;
using IMAP_App.Utilities;

namespace IMAP_App
{
    class Program
    {
        private static void PrintBlock()
        {
            Console.WriteLine(".imap to .json converter.");
            Console.WriteLine("");
            Console.WriteLine("Converts Telltale Tool .imap (InputMapping) files to .json text files.");
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------- .imap to .json --------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("usage: (app.exe) -json input.imap       //converts single imap file to json.");
            Console.WriteLine("usage: (app.exe) -json inputFolderPath  //converts multiple imap files to json.");
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------- .json to .imap --------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("usage: (app.exe) -imap input.json       //converts single json file to imap.");
            Console.WriteLine("usage: (app.exe) -imap inputFolderPath  //converts multiple json files to imap.");
            Console.WriteLine("");
        }

        /// <summary>
        /// Main application method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //if there are no arguments then print the app usage
            if(args.Length == 0)
            {
                PrintBlock();
                return;
            }

            //if there are more arguments then requested, don't continue and print the app usage
            if (args.Length > 2)
            {
                Console.WriteLine("ERROR, the app will only accept 2 arguments, refer to the usabillity text on how to use the app properly.");
                PrintBlock();
                return;
            }

            //get the first argument which should be the app mode
            string firstArgument = args[0];
            bool jsonMode = firstArgument.Equals("-json");
            bool imapMode = firstArgument.Equals("-imap");

            //if none of the modes are recongized, don't continue and print the app usage
            if (jsonMode == false && imapMode == false)
            {
                Console.WriteLine("ERROR, unrecognized mode {0} , refer to the usabillity text on how to use the app properly.", firstArgument);
                PrintBlock();
                return;
            }

            //if there is a mode selected, but no path provided, don't continue and print the app usage
            if (args.Length == 1)
            {
                Console.WriteLine("ERROR, mode selected {0} but no path was given, refer to the usabillity text on how to use the app properly.", firstArgument);
                PrintBlock();
                return;
            }

            //get the second argument which should be the path
            string secondArgument = args[1];
            bool isFile = File.Exists(secondArgument);
            bool isDirectory = Directory.Exists(secondArgument);

            //if the path is not a valid folder/file, don't continue and print the app usage
            if (isFile == false && isDirectory == false)
            {
                Console.WriteLine("ERROR, file/folder path is not valid or doesn't exist, refer to the usabillity text on how to use the app properly.", secondArgument);
                PrintBlock();
                return;
            }

            //if the path is a singular file
            if (isFile)
            {
                //imap --> json
                if (jsonMode)
                {
                    string sourceImapPath = secondArgument;
                    string destJsonPath = Path.ChangeExtension(sourceImapPath, IMAP_Master.jsonExtension);

                    IMAP_Master imap_file = new IMAP_Master();
                    imap_file.Read_IMAP_File(sourceImapPath);
                    imap_file.Write_IMAP_JSON(destJsonPath);
                }

                //json --> imap
                if (imapMode)
                {
                    string sourceJsonPath = secondArgument;
                    string destImapPath = Path.ChangeExtension(sourceJsonPath, IMAP_Master.imapExtension);

                    IMAP_Master imap_file = new();
                    imap_file.Read_IMAP_JSON(sourceJsonPath);
                    imap_file.Write_Final_IMAP(destImapPath);
                }
            }

            //if the path is an entire directory
            if (isDirectory)
            {
                //imap --> json
                if (jsonMode)
                {
                    string sourceImapFolderPath = secondArgument;
                    List<string> filesInFolder = new List<string>(Directory.GetFiles(sourceImapFolderPath));

                    filesInFolder = IOManagement.FilterFiles(filesInFolder, IMAP_Master.imapExtension);

                    if (filesInFolder.Count < 1)
                    {
                        Console.WriteLine("No .imap files were found, aborting.");
                        return;
                    }

                    foreach (string file in filesInFolder)
                    {
                        string sourceImapPath = file;
                        string destJsonPath = Path.ChangeExtension(sourceImapPath, IMAP_Master.jsonExtension);

                        IMAP_Master imap_file = new IMAP_Master();
                        imap_file.Read_IMAP_File(sourceImapPath);
                        imap_file.Write_IMAP_JSON(destJsonPath);
                    }
                }

                //json --> imap
                if (imapMode)
                {
                    string sourceJsonFolderPath = secondArgument;
                    List<string> filesInFolder = new List<string>(Directory.GetFiles(sourceJsonFolderPath));

                    filesInFolder = IOManagement.FilterFiles(filesInFolder, IMAP_Master.jsonExtension);

                    if (filesInFolder.Count < 1)
                    {
                        Console.WriteLine("No .json files were found, aborting.");
                        return;
                    }

                    foreach (string file in filesInFolder)
                    {
                        string sourceJsonPath = file;
                        string destImapPath = Path.ChangeExtension(sourceJsonPath, IMAP_Master.imapExtension);

                        IMAP_Master imap_file = new();
                        imap_file.Read_IMAP_JSON(sourceJsonPath);
                        imap_file.Write_Final_IMAP(destImapPath);
                    }
                }
            }
        }
    }
}
