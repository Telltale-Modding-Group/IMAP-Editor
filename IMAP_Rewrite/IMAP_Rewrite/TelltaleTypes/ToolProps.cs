using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMAP_App.Utilities;
using System.Runtime.InteropServices;

namespace IMAP_App.TelltaleTypes
{
    public struct ToolProps
    {
        public bool mbHasProps;

        public ToolProps(BinaryReader reader)
        {
            mbHasProps = ByteFunctions.ReadBoolean(reader);
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += 1; //mbHasProps [1 bytes]

            return totalByteSize;
        }

        public override string ToString() => string.Format("[ToolProps] mbHasProps: {0}", mbHasProps);
    }
}
