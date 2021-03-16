using System;
using System.Collections.Generic;
using System.Text;
using LibTelltale;
using LibTelltaleWrapper.MetaStreamed;
using System.Runtime.InteropServices;
using LibTelltaleNative;

namespace LibTelltaleWrapper.MetaStreamed
{
    /// <summary>
    /// An AgentMapper (.amap), maps agent data such as: names, styles, idles, models and actors
    /// Warning: Until opened, all functions will cause a crash/undefined behaviour
    /// </summary>
    public class AgentMapper : AbstractMetaStreamedFile
    {

        /// <summary>
        /// An agent map entry
        /// </summary>
        public struct Entry {
            public string mAgentName;
            public string mActorName;
            public string[] mModels;
            public string[] mGuides;
            public string[] mStyleIdles;
            public IntPtr reference;
        }

        /// <summary>
        /// Once you have toggled with values in the entry struct, call this to update the backend memory so the library knows about your changes.
        /// </summary>
        /// <param name="ret"></param>
        public static void UpdateAgent(Entry ret)
        {
            Native.hMemory_FreeArray(Marshal.ReadIntPtr(ret.reference));
            Native.hMemory_FreeArray(Marshal.ReadIntPtr(new IntPtr(ret.reference.ToInt64() + 8)));
            IntPtr m = Marshal.ReadIntPtr(new IntPtr(ret.reference.ToInt64() + 16));
            IntPtr g = Marshal.ReadIntPtr(new IntPtr(ret.reference.ToInt64() + 24));
            IntPtr i = Marshal.ReadIntPtr(new IntPtr(ret.reference.ToInt64() + 32));
            if (!m.Equals(IntPtr.Zero))Native.Buffer_DCArray_Clear(m);
            if (!g.Equals(IntPtr.Zero)) Native.Buffer_DCArray_Clear(g);
            if (!i.Equals(IntPtr.Zero)) Native.Buffer_DCArray_Clear(i);
            Marshal.WriteIntPtr(ret.reference, Config.CreateString(ret.mAgentName));
            Marshal.WriteIntPtr(new IntPtr(ret.reference.ToInt64() + 8), Config.CreateString(ret.mActorName));
            for (int x = 0; x < ret.mModels.Length; x++)
            {
                Native.Buffer_DCArray_Add(m, Config.CreateString(ret.mModels[x]));
            }
            for (int x = 0; x < ret.mGuides.Length; x++)
            {
                Native.Buffer_DCArray_Add(g, Config.CreateString(ret.mGuides[x]));
            }
            for (int x = 0; x < ret.mStyleIdles.Length; x++)
            {
                Native.Buffer_DCArray_Add(i, Config.CreateString(ret.mStyleIdles[x]));
            }
        }

        /// <summary>
        /// Creates a new agent entry, which can be passed to AddEntry
        /// </summary>
        /// <param name="name"></param>
        /// <param name="actor"></param>
        /// <param name="models"></param>
        /// <param name="guides"></param>
        /// <param name="idles"></param>
        /// <returns></returns>
        public static Entry CreateAgent(string name, string actor, string[] models, string[] guides, string[] idles)
        {
            Entry ret = new Entry();
            ret.mActorName = actor;
            ret.mAgentName = name;
            ret.mModels = models;
            ret.mGuides = guides;
            ret.mStyleIdles = idles;
            ret.reference = Native.AgentMap_CreateAgentEntry();
            Marshal.WriteIntPtr(ret.reference, Config.CreateString(name));
            Marshal.WriteIntPtr(new IntPtr(ret.reference.ToInt64() + 8), Config.CreateString(actor));
            IntPtr m = Native.Buffer_DCArray_Create();
            IntPtr g = Native.Buffer_DCArray_Create();
            IntPtr i = Native.Buffer_DCArray_Create();
            Marshal.WriteIntPtr(new IntPtr(ret.reference.ToInt64() + 16), m);
            Marshal.WriteIntPtr(new IntPtr(ret.reference.ToInt64() + 24), g);
            Marshal.WriteIntPtr(new IntPtr(ret.reference.ToInt64() + 32), i);
            for(int x = 0; x < models.Length; x++)
            {
                Native.Buffer_DCArray_Add(m, Config.CreateString(models[x]));
            }
            for (int x = 0; x < guides.Length; x++)
            {
                Native.Buffer_DCArray_Add(g, Config.CreateString(guides[x]));
            }
            for (int x = 0; x < idles.Length; x++)
            {
                Native.Buffer_DCArray_Add(i, Config.CreateString(idles[x]));
            }
            return ret;
        }

        /// <summary>
        /// Gets the amount of entries in this agent map
        /// </summary>
        public int GetNumEntries()
        {
            return Native.Buffer_DCArray_Size(Agents());
        }

        /// <summary>
        /// Adds an entry to this agent map
        /// </summary>
        public void AddEntry(Entry entry)
        {
            Native.Abstract_DCArray_Add(this.Agents(), entry.reference);
        }

        /// <summary>
        /// Returns all entries packed nicely into an array
        /// </summary>
        public Entry[] GetEntries()
        {
            Entry[] ret = new Entry[GetNumEntries()];
            for(int i = 0; i < ret.Length; i++)
            {
                ret[i] = GetEntry(i);
            }
            return ret;
        }

        /// <summary>
        /// Gets an entry at the given index, for iteration
        /// </summary>
        public Entry GetEntry(int index)
        {
            IntPtr ptr = Native.Abstract_DCArray_At(Agents(), index);
            Entry ret = new Entry();
            ret.mAgentName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(new IntPtr(ptr.ToInt64() + 0)));
            ret.mActorName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(new IntPtr(ptr.ToInt64() + 8)));
            IntPtr m = Marshal.ReadIntPtr(new IntPtr(ptr.ToInt64() + 16));
            IntPtr g = Marshal.ReadIntPtr(new IntPtr(ptr.ToInt64() + 24));
            IntPtr i = Marshal.ReadIntPtr(new IntPtr(ptr.ToInt64() + 32));
            string[] guides = new string[Native.Buffer_DCArray_Size(g)];
            string[] models = new string[Native.Buffer_DCArray_Size(m)];
            string[] idles = new string[Native.Buffer_DCArray_Size(i)];
            for(int x = 0; x < guides.Length;x++)
            {
                guides[x] = Marshal.PtrToStringAnsi(Native.Buffer_DCArray_At(g, x));
            }
            for (int x = 0; x < idles.Length; x++)
            {
                idles[x] = Marshal.PtrToStringAnsi(Native.Buffer_DCArray_At(i, x));
            }
            for (int x = 0; x < models.Length; x++)
            {
                models[x] = Marshal.PtrToStringAnsi(Native.Buffer_DCArray_At(m, x));
            }
            ret.mGuides = guides;
            ret.mModels = models;
            ret.mStyleIdles = idles;
            ret.reference = ptr;
            return ret;
        }

        protected IntPtr Agents()
        {
            return Marshal.ReadIntPtr(this.reference);
        }

        protected bool disposed = false;

        protected TTContext ctx;

        protected IntPtr reference;

        public override void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                Native.AgentMap_Delete(this.reference);
            }
        }


        /// <summary>
        /// Creates an agent map from the given context.
        /// </summary>
        /// <param name="context"></param>
        public AgentMapper(TTContext context)
        {
            this.ctx = context;
            this.reference = Native.AgentMap_Create();
        }

        /// <summary>
        /// Writes this agent map to the current outstream in the attached context.
        /// </summary>
        public override bool Flush()
        {
            return Native.AgentMap_Flush(this.GetContext().Internal_Get(), this.reference);
        }

        public override TTContext GetContext()
        {
            return ctx;
        }

        public override IntPtr Internal_Get()
        {
            return this.reference;
        }

        /// <summary>
        /// Opens this agent map from the attached context.
        /// </summary>
        public override int Open()
        {
            return Native.AgentMap_Open(this.GetContext().Internal_Get(), this.reference);
        }
    }
}
