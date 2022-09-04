using System;
using System.Collections.Generic;
using System.Text;
using LibTelltaleNative;
using LibTelltale;
using System.Runtime.InteropServices;

namespace LibTelltaleWrapper.MetaStreamed
{

    /// <summary>
    /// A Telltale .scene Scene. Contains a list of agents and their properties, as well as if the scene is hidden and the scene properties.
    /// </summary>
    public class Scene : AbstractMetaStreamedFile
    {

        /// <summary>
        /// Agent Info, which represents an Agent in a Scene
        /// </summary>
        public struct AgentInfo {
            public string mAgentName;
            public PropertySet mAgentSceneProps;
            public IntPtr reference;
        }

        /// <summary>
        /// If you have edited the agents name (you shouldn't be reassigning the properties!) this lets the library know about it
        /// </summary>
        public static void UpdateAgent(AgentInfo agent)
        {
            IntPtr name = Marshal.ReadIntPtr(agent.reference, 8);
            if (!name.Equals(IntPtr.Zero))
            {
                Native.hMemory_FreeArray(name);
            }
            Marshal.WriteIntPtr(agent, 8, Config.CreateString(agent.mAgentName));
        }

        /// <summary>
        /// Creates a new agent from the given context and agent name.
        /// </summary>
        public static AgentInfo CreateAgent(TTContext ctx, string agentName)
        {
            return CreateAgent(agentName, new PropertySet(ctx));
        }

        /// <summary>
        /// Creates an agent, but from existing NEW properties
        /// </summary>
        public static AgentInfo CreateAgent(string agentName, PropertySet props)
        {
            AgentInfo ret = new AgentInfo();
            ret.reference = Native.hMemory_AllocNew(16);//ptr size * 2 (8*2)
            ret.mAgentName = agentName;
            Marshal.WriteIntPtr(ret.reference, 8, Config.CreateString(agentName));
            Marshal.WriteIntPtr(ret.reference, props.Internal_Get());
            return ret;
        }

        /// <summary>
        /// Removes and deletes an agent from this scene.
        /// </summary>
        public void RemoveAndDeleteAgent(AgentInfo agent)
        {
            Native.hMemory_FreeArray(Marshal.ReadIntPtr(agent.reference, 8));
            (new PropertySet(Marshal.ReadIntPtr(agent.reference))).Dispose();
            Native.Abstract_DCArray_Remove(Agents(), agent.reference);
        }

        /// <summary>
        /// Adds an agent to this scene
        /// </summary>
        public void AddAgent(AgentInfo agent)
        {
            Native.Abstract_DCArray_Add(Agents(), agent.reference);
        }

        /// <summary>
        /// Adds and creates a new agent
        /// </summary>
        public void AddNewAgent(string agentName, PropertySet hAgentSceneProps)
        {
            AddAgent(CreateAgent(agentName, hAgentSceneProps));
        }

        /// <summary>
        /// Gets the list of agents in this scene. Do not call this lots, aimly call it once.
        /// </summary>
        /// <returns></returns>
        public List<AgentInfo> GetAgents()
        {
            List<AgentInfo> ret = new List<AgentInfo>(GetNumAgents());
            for(int i = 0; i < ret.Capacity; i++)
            {
                ret.Add(this[i]);
            }
            return ret;
        }

        /// <summary>
        /// Gets an agent at the index, useful for iteration
        /// </summary>
        public AgentInfo this[int n]
        {
            get
            {
                if (n >= GetNumAgents()) throw new LibTelltaleException("AIOOB! "+ n+" >= "+GetNumAgents());
                IntPtr ai = Native.Abstract_DCArray_At(Agents(), n);
                AgentInfo ret = new AgentInfo();
                ret.reference = ai;
                ret.mAgentSceneProps = new PropertySet(Marshal.ReadIntPtr(ai));
                ret.mAgentName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(ai,8));
                return ret;
            }
            set
            {
                throw new LibTelltaleException("Not supported");
            }
        } 

        protected IntPtr References()
        {
            return Marshal.ReadIntPtr(this.reference, 24);
        }

        protected IntPtr Agents()
        {
            return Marshal.ReadIntPtr(this.reference,16);
        }

        /// <summary>
        /// Gets the amount of agents in this scene
        /// </summary>
        public int GetNumAgents()
        {
            return Native.Abstract_DCArray_Size(Agents());
        }

        /// <summary>
        /// References the given scene handle. Not sure on this, so be careful as it might create game behaviour which is not what you want
        /// </summary>
        public void AddReferencedScene(Handle hScene)
        {
            Native.Abstract_DCArray_Add(this.References(), hScene.Interal_Get());
        }

        /// <summary>
        /// Gets all referenced scenes in this scene
        /// </summary>
        public Handle[] GetReferencedScenes()
        {
            Handle[] ret = new Handle[GetNumReferencedScenes()];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new Handle(Marshal.ReadIntPtr(Native.Abstract_DCArray_At(References(), i))); 
            }
            return ret;
        }

        /// <summary>
        /// Gets the amount of referenced scenes in this scene
        /// </summary>
        /// <returns></returns>
        public int GetNumReferencedScenes()
        {
            return Native.Abstract_DCArray_Size(References());
        }

        /// <summary>
        /// If this scene is hidden
        /// </summary>
        public bool IsHidden()
        {
            return Marshal.ReadByte(this.reference) != 0;
        }

        /// <summary>
        /// Gets the name of this scene
        /// </summary>
        public string GetName()
        {
            return Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(this.reference, 8));
        }

        /// <summary>
        /// Gets the scene properties of this scene, creating them and setting them if they are not created yet.
        /// </summary>
        /// <returns></returns>
        public PropertySet GetSceneProperties()
        {
            IntPtr prop = Marshal.ReadIntPtr(this.reference, 32);
            if (prop.Equals(IntPtr.Zero))
            {
                PropertySet ret =  new PropertySet(GetContext());
                Marshal.WriteIntPtr(this.reference, 32, ret.Internal_Get());
                return ret;
            }
            return new PropertySet(prop);
        }

        /// <summary>
        /// Sets the name of this scene to the new name 
        /// </summary>
        public void SetName(string name)
        {
            if(!Marshal.ReadIntPtr(new IntPtr(this.reference.ToInt64() + 8)).Equals(IntPtr.Zero))
            {
                Native.hMemory_FreeArray(new IntPtr(this.reference.ToInt64() + 8));
            }
            Marshal.WriteIntPtr(new IntPtr(this.reference.ToInt64() + 8), Config.CreateString(name));
        }

        /// <summary>
        /// Toggles if this scene is hidden
        /// </summary>
        public void SetHidden(bool enabled)
        {
            Marshal.WriteByte(this.reference, (byte)(enabled ? 1 : 0));
        }

        protected TTContext ctx;

        protected bool disposed = false;

        protected IntPtr reference;

        /// <summary>
        /// Creates a scene from the given context, and backend scene reference pointer.
        /// </summary>
        public Scene(IntPtr reference, TTContext ctx)
        {
            this.reference = reference;
            this.ctx = ctx;
        }

        /// <summary>
        /// Creates a new scene from the given context.
        /// </summary>
        /// <param name="ctx"></param>
        public Scene(TTContext ctx)
        {
            this.ctx = ctx;
            this.reference = Native.Scene_Create();
        }

        /// <summary>
        /// Disposes of this scene. Call this when you are done with this and don't need to open or write it again.
        /// </summary>
        public override void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                Native.Scene_Free(this.reference);
            }
        }

        /// <summary>
        /// Writes to the attached context this scene and all its contents in a serialized format for Telltale to be able to read.
        /// </summary>
        /// <returns></returns>
        public override bool Flush()
        {
            return Native.Scene_Flush(ctx.Internal_Get(), reference);
        }

        /// <summary>
        /// Gets the attached context
        /// </summary>
        /// <returns></returns>
        public override TTContext GetContext()
        {
            return ctx;
        }

        /// <summary>
        /// Gets the backend memory pointer.
        /// </summary>
        /// <returns></returns>
        public override IntPtr Internal_Get()
        {
            return reference;
        }

        /// <summary>
        /// Opens and reads this scene from the attached context. Returns 0 on success, and 5 if warned. A return of 5 means that an agent or more wasn't loaded because it contained
        /// an unknown property type. This means that exporting the scene back using Flush will cause those agents not to be existent and Telltale won't like that!
        /// If this returns 5, then viewing the scene is what you are limited too. Although the library doesn't stop you. If this returns 5, use GetLastErrorCRC from Config and 
        /// tell me that value on a github issue.
        /// </summary>
        public override int Open()
        {
            return Native.Scene_Open(ctx.Internal_Get(), reference);
        }
    }
}
