using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using LibTelltaleNative;
using LibTelltale;

namespace LibTelltaleWrapper.MetaStreamed
{
    /// <summary>
    /// An ActorAgentMap (.aam), mostly used in older games but in some newer ones too. Maps agent data, use GetActorAgents to get the containing propertyset
    /// Warning: Until opened, all functions will cause a crash/undefined behaviour
    /// </summary>
    public class ActorAgentMap : AbstractMetaStreamedFile
    {

        protected bool disposed = false;

        protected TTContext ctx;

        protected IntPtr reference;

        public override void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                Native.ActorAgentMap_Delete(this.reference);
            }
        }

        /// <summary>
        /// Gets the agent propertyset. You may edit this.
        /// </summary>
        public PropertySet GetActorAgents()
        {
            return new PropertySet(Marshal.ReadIntPtr(new IntPtr(this.reference.ToInt64() + 8)));
        }

        /// <summary>
        /// Sets the groups for this actor agent map. Not sure on these, they are a list at the end of the file.
        /// </summary>
        /// <param name="groups"></param>
        public void SetGroups(string[] groups)
        {
            IntPtr array = Marshal.ReadIntPtr(this.reference);
            Native.Buffer_DCArray_Clear(array);
            for (int i = 0; i < groups.Length; i++)
            {
                IntPtr ptr = Config.CreateString(groups[i]);
                Native.Buffer_DCArray_Add(array, ptr);
            }
        }

        /// <summary>
        /// Gets all the groups in this actor agent map. Not sure on these, they are a list at the end of the file.
        /// </summary>
        /// <returns></returns>
        public string[] GetGroups()
        {
            IntPtr array = Marshal.ReadIntPtr(this.reference);
            string[] ret = new string[Native.Abstract_DCArray_Size(array)];
            for(int i = 0; i < ret.Length; i++)
            {
                ret[i] = Marshal.PtrToStringAnsi(Native.Abstract_DCArray_At(array, i));
            }
            return ret;
        }

        /// <summary>
        /// Creates an actor agent map from the given context.
        /// </summary>
        /// <param name="context"></param>
        public ActorAgentMap(TTContext context)
        {
            this.ctx = context;
            this.reference = Native.ActorAgentMap_Create();
        }

        /// <summary>
        /// Writes this actor agent map to the current outstream in the attached context.
        /// </summary>
        /// <returns></returns>
        public override bool Flush()
        {
            return Native.ActorAgentMap_Flush(this.GetContext().Internal_Get(), this.reference);
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
        /// Opens this actor agent map from the attached context.
        /// </summary>
        /// <returns></returns>
        public override int Open()
        {
            return Native.ActorAgentMap_Open(this.GetContext().Internal_Get(), this.reference);
        }
    }
}
