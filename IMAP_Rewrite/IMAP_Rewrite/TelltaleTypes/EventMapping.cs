using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using IMAP_App.Utilities;

namespace IMAP_App.TelltaleTypes
{
    public struct EventMapping
    {
        public int mInputCode { get; set; }

        public int mEvent { get; set; }

        public string mScriptFunction { get; set; }

        public int mControllerIndexOverride { get; set; }


        private int mScriptFunction_BlockSize { get { return mScriptFunction.Length + 8; } }

        public EventMapping(BinaryReader reader)
        {
            mInputCode = reader.ReadInt32();
            mEvent = reader.ReadInt32();
            reader.ReadInt32(); //mScriptFunction_BlockSize
            mScriptFunction = ByteFunctions.ReadString(reader);
            mControllerIndexOverride = reader.ReadInt32();
        }

        public void WriteBinaryData(BinaryWriter writer)
        {
            writer.Write(mInputCode);
            writer.Write(mEvent);
            writer.Write(mScriptFunction_BlockSize);
            ByteFunctions.WriteString(writer, mScriptFunction);
            writer.Write(mControllerIndexOverride);
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += (uint)Marshal.SizeOf(mInputCode); //mInputCode [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mEvent); //mEvent [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mScriptFunction_BlockSize); //mScriptFunction_BlockSize [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mScriptFunction.Length); //mScriptFunction Length [4 bytes]
            totalByteSize += (uint)mScriptFunction.Length; //mScriptFunction [x bytes]
            totalByteSize += (uint)Marshal.SizeOf(mControllerIndexOverride); //mControllerIndexOverride [4 bytes]

            return totalByteSize;
        }

        public override string ToString() => string.Format("[EventMapping] mInputCode: {0} mEvent: {1} mScriptFunction: {2} mControllerIndexOverride: {3}", mInputCode, mEvent, mScriptFunction, mControllerIndexOverride);
    }
}
