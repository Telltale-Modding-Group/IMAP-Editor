using System;
using System.Runtime.InteropServices;
using LibTelltaleNative;
using LibTelltale;

namespace LibTelltale
{
	/// <summary>
	/// The handle for .vers meta streamed files. These files hold data about seriailzed file formats, but old ones. Also known as serialized version infos / meta version info.
	/// </summary>
	public sealed class SerializedVersionInfo : AbstractMetaStreamedFile {
		protected IntPtr reference;
		protected TTContext ctx;
		protected _Vers vers;

		/// <summary>
		/// The unmanaged memory structure of a vers block and its sub blocks.
		/// </summary>
		public struct _Vers {
			[MarshalAs(UnmanagedType.LPStr)]
			public string mTypeName;
			[MarshalAs(UnmanagedType.LPStr)]
			public string mFullTypeName;//If this is a MSV5+ .vers, then this is the fully qualified name of it. Else its the same as mTypeName
			public byte mbBlocked;
			public uint mBlockLengh;
			public uint mVersion;
			public IntPtr mBlockVarNames;
			public IntPtr mBlocks;
			public IntPtr ctx;
		}

		/// <summary>
		/// Creates an instance from the given context and ptr (internal)
		/// </summary>
		public SerializedVersionInfo(TTContext ctx,IntPtr vers){
			this.reference = vers;
			this.ctx = ctx;
			UpdateAndSync (true);
		}

		/// <summary>
		/// Creates an instance from the given context
		/// </summary>
		public SerializedVersionInfo(TTContext ctx){
			this.ctx = ctx;
			this.reference = Native.Vers_Create (ctx.Internal_Get());
			UpdateAndSync (true);
		}

		public bool Equals(SerializedVersionInfo other){
			return other.vers.mVersion == vers.mVersion && String.Equals (vers.mTypeName, other.vers.mTypeName);
		}

		/// <summary>
		/// Gets the name of this .vers main blocks type name
		/// </summary>
		/// <returns>The version type name.</returns>
		public string GetVersionTypeName(){
			return this.vers.mTypeName;
		}

		/// <summary>
		/// Gets the length of this main vers block in bytes when loaded in memory
		/// </summary>
		/// <returns>The block length.</returns>
		public uint GetBlockLength(){
			return this.vers.mBlockLengh;
		}

		/// <summary>
		/// Gets a sub block by its index (use get count, for iteration)
		/// </summary>
		public _Vers GetVersBlock(int index){
			return (_Vers)Marshal.PtrToStructure (Native.VersBlocks_DCArray_At (this.vers.mBlocks, index), typeof(_Vers));
		}

		/// <summary>
		/// Gets the name of a sub blocks variable name. 
		/// </summary>
		/// <returns>The variable name.</returns>
		public string GetVarName(int index){
			if (this.vers.mBlockVarNames.Equals (IntPtr.Zero))
				return "";
			IntPtr ptr = this.vers.mBlockVarNames;
			ptr = new IntPtr (Marshal.ReadInt64 (new IntPtr(ptr.ToInt64() + (8*index))));
			return Marshal.PtrToStringAnsi (ptr);
		}

		/// <summary>
		/// Gets the inner (sub) block count.
		/// </summary>
		public int GetInnerBlockCount(){
			return Native.VersBlocks_DCArray_Size (this.vers.mBlocks);
		}

		/// <summary>
		/// If this vers block is blocked. Not sure on what this means
		/// </summary>
		public bool IsBlocked(){
			return this.vers.mbBlocked != 0;
		}

		/// <summary>
		/// Gets the versio of this .vers.
		/// </summary>
		/// <returns>The version.</returns>
		public uint GetVersion(){
			return this.vers.mVersion;
		}

		/// <summary>
		/// Open from the current context
		/// </summary>
		public override int Open(){
			int i = Native.Vers_Open (this.reference);
			if (i == Config.OPEN_OK)
				UpdateAndSync (true);
			return i;
		}

		/// <summary>
		/// Writes this instance to the current context
		/// </summary>
		public override bool Flush(){
			bool r = Native.Vers_Flush (this.reference, ctx.Internal_Get ()) != 0;
			if (r)
				UpdateAndSync (true);
			return r;
		}

		/// <summary>
		/// Frees all backend memory from this serialized version info 
		/// </summary>
		public override void Dispose(){
			Native.Vers_Free (this.reference);
		}

		public override IntPtr Internal_Get(){
			return this.reference;
		}

		public override TTContext GetContext(){
			return this.ctx;
		}

		protected void UpdateAndSync(bool retrieve){
			if (retrieve) {
				this.vers = (_Vers)Marshal.PtrToStructure (this.reference, typeof(_Vers));
			} else {
				Marshal.StructureToPtr (this.vers, this.reference, false);
			}
		}

	}
}

