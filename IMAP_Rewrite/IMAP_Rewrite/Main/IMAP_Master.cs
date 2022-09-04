using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IMAP_App.Utilities;
using IMAP_App.TelltaleMeta;
using IMAP_App.TelltaleTypes;
using IMAP_App.TelltaleIMAP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft;

namespace IMAP_App.Main
{
    /// <summary>
    /// This is the master class object for a IMAP file.
    /// </summary>
    public class IMAP_Master
    {
        public static string imapExtension = ".imap";
        public static string jsonExtension = ".json";

        //meta header versions (objects at the top of the file)
        public MSV6 msv6;
        public MSV5 msv5;
        public MTRE mtre;

        //main imap object
        public IMAP imap;

        /// <summary>
        /// Reads in a IMAP file from the disk.
        /// </summary>
        /// <param name="filePath"></param>
        public void Read_IMAP_File(string filePath)
        {
            //read meta version of the file (to determine the correct meta version to parse)
            string metaVersion = Read_IMAP_File_MetaVersionOnly(filePath);

            //begin reading the file in binary
            using (BinaryReader reader = new(File.OpenRead(filePath)))
            {
                //read meta header
                switch (metaVersion)
                {
                    case "6VSM": msv6 = new(reader); break; //if the keyword is 6vsm then the version if MSV6
                    case "5VSM": msv5 = new(reader); break; //if the keyword is 5vsm then the version if MSV6
                    case "ERTM": mtre = new(reader); break; //if the keyword is ertm then the version if MTRE
                    default:
                        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                        Console.WriteLine("ERROR! '{0}' meta stream version is not supported!", metaVersion);
                        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                        return;
                }

                //read the imap object
                imap = new(reader, false);
            }
        }

        //get the current meta object
        public object Get_Meta_Object()
        {
            if (msv6 != null) return msv6;
            else if (msv5 != null) return msv5;
            else if (mtre != null) return mtre;
            else return null;
        }

        /// <summary>
        /// Writes a final .imap file to disk
        /// </summary>
        /// <param name="destinationPath"></param>
        public void Write_Final_IMAP(string destinationPath)
        {
            //write a new imap file in binary
            using(BinaryWriter writer = new BinaryWriter(File.OpenWrite(destinationPath)))
            {
                //write the meta header in binary
                if (msv6 != null)
                {
                    msv6.mDefaultSectionChunkSize = imap.GetByteSize();
                    msv6.WriteBinaryData(writer);
                }
                else if (msv5 != null)
                {
                    msv5.mDefaultSectionChunkSize = imap.GetByteSize();
                    msv5.WriteBinaryData(writer);
                }
                else if (mtre != null)
                {
                    mtre.WriteBinaryData(writer);
                }

                //write the main imap object
                if (imap != null) imap.WriteBinaryData(writer);
            }
        }

        /// <summary>
        /// Reads a json file and serializes it into the appropriate imap version that was serialized in the json file.
        /// </summary>
        /// <param name="filePath"></param>
        public void Read_IMAP_JSON(string filePath)
        {
            //read the data from the json file
            string jsonText = File.ReadAllText(filePath);

            //parse the data into a json array
            JArray jarray = JArray.Parse(jsonText);

            //meta object
            JObject metaObject = jarray[0] as JObject;

            //parsed meta stream version from the json document
            string metaStreamVersion = "";

            //loop through each property to get the value of the variable 'mMetaStreamVersion' to determine what version of the meta header to parse.
            foreach (JProperty property in metaObject.Properties())
            {
                if (property.Name.Equals("mMetaStreamVersion")) metaStreamVersion = (string)property.Value; break;
            }

            //deserialize the appropriate json object
            if (metaStreamVersion.Equals("6VSM")) msv6 = metaObject.ToObject<MSV6>();
            else if (metaStreamVersion.Equals("5VSM")) msv5 = metaObject.ToObject<MSV5>();
            else if (metaStreamVersion.Equals("ERTM")) mtre = metaObject.ToObject<MTRE>();

            //imap object
            JObject imapObject = jarray[1] as JObject;

            //convert the json object to an imap object
            imap = imapObject.ToObject<IMAP>();
        }

        public void Write_IMAP_JSON(string destinationPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(destinationPath);
            string fileDirectory = Path.GetDirectoryName(destinationPath);
            string newPath = fileDirectory + "/" + fileName + jsonExtension;

            //open a stream writer to create the text file and write to it
            using (StreamWriter file = File.CreateText(newPath))
            {
                //get our json seralizer
                JsonSerializer serializer = new();

                List<object> jsonObjects = new();
                jsonObjects.Add(Get_Meta_Object());
                jsonObjects.Add(imap);

                //seralize the data and write it to the configruation file
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, jsonObjects);
            }
        }


        /// <summary>
        /// Reads a imap file on the disk and returns the meta version that is being used.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public static string Read_IMAP_File_MetaVersionOnly(string sourceFile)
        {
            string metaStreamVersion = "";

            using (BinaryReader reader = new BinaryReader(File.OpenRead(sourceFile)))
            {
                for(int i = 0; i < 4; i++) metaStreamVersion += reader.ReadChar();
            }

            return metaStreamVersion;
        }
    }
}
