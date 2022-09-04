using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IMAP_App.TelltaleTypes;
using IMAP_App.Utilities;
using System.Runtime.InteropServices;

namespace IMAP_App.TelltaleIMAP
{
    public class IMAP
    {
        private int mName_BlockSize { get { return mName.Length + 8; } }

        public string mName { get; set; }

        private uint mMappedEvents_ArrayCapacity
        { 
            get 
            {
                uint totalByteSize = 8;

                for(int i = 0; i < mMappedEvents.Length; i++)
                {
                    totalByteSize += mMappedEvents[i].GetByteSize();
                }

                return totalByteSize; 
            } 
        }

        private int mMappedEvents_ArrayLength { get { return mMappedEvents.Length; } }

        public EventMapping[] mMappedEvents { get; set; }

        public IMAP() { }

        public IMAP(BinaryReader reader, bool showConsole = false)
        {
            reader.ReadInt32(); //mName Block Size [4 bytes] //mName block size (size + string len)
            mName = ByteFunctions.ReadString(reader); //mName [x bytes]

            //--------------------------mMappedEvents--------------------------
            uint arrayCapacity = reader.ReadUInt32();  //mMappedEvents DCArray Capacity [4 bytes]
            int arrayLength = reader.ReadInt32(); //mMappedEvents DCArray Length [4 bytes]
            mMappedEvents = new EventMapping[arrayLength];

            for (int i = 0; i < mMappedEvents.Length; i++)
            {
                mMappedEvents[i] = new EventMapping(reader);
            }

            if(showConsole)
                PrintConsole();
        }

        public void WriteBinaryData(BinaryWriter writer)
        {
            writer.Write(mName_BlockSize); //mName Block Size [4 bytes] //mName block size (size + string len)
            ByteFunctions.WriteString(writer, mName); //mName [x bytes]

            //---------------------------- mMappedEvents ----------------------------
            writer.Write(mMappedEvents_ArrayCapacity); //mMappedEvents DCArray Capacity [4 bytes]
            writer.Write(mMappedEvents_ArrayLength); //mMappedEvents DCArray Length [4 bytes]
            for (int i = 0; i < mMappedEvents_ArrayLength; i++)
            {
                mMappedEvents[i].WriteBinaryData(writer);
            }
        }

        public uint GetByteSize()
        {
            uint totalSize = 0;

            totalSize += (uint)Marshal.SizeOf(mName_BlockSize); //mName Block Size [4 bytes] //mName block size (size + string len)
            totalSize += (uint)Marshal.SizeOf(mName.Length); //mName (strength length prefix) [4 bytes]
            totalSize += (uint)mName.Length;  //mName [x bytes]

            totalSize += (uint)Marshal.SizeOf(mMappedEvents_ArrayCapacity); //mMappedEvents DCArray Capacity [4 bytes]
            totalSize += (uint)Marshal.SizeOf(mMappedEvents_ArrayLength); //mMappedEvents DCArray Length [4 bytes]
            for (int i = 0; i < mMappedEvents_ArrayLength; i++)
            {
                totalSize += mMappedEvents[i].GetByteSize();
            }

            return totalSize;
        }

        public void PrintConsole()
        {
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("||||||||||| IMAP Data |||||||||||");
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("IMAP mName Block Size = {0}", mName_BlockSize);
            Console.WriteLine("IMAP mName = {0}", mName);

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("----------- mMappedEvents -----------");
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("IMAP mMappedEvents_ArrayCapacity = {0}", mMappedEvents_ArrayCapacity);
            Console.WriteLine("IMAP mMappedEvents_ArrayLength = {0}", mMappedEvents_ArrayLength);
            for (int i = 0; i < mMappedEvents_ArrayLength; i++)
            {
                Console.WriteLine("IMAP mMappedEvents {0} = {1}", i, mMappedEvents[i]);
            }
        }
    }
}
