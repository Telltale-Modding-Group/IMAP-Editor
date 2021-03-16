using System;
using System.Runtime.InteropServices;
using LibTelltaleNative;

/** 
 * C# Implementation of LibTelltale. I've decided to port most of the library here, since it is a nice and simple API for handing Telltale's files (now PropertySets full support are only here.)
 * 
 * Members are left all protected but the classes are left final because in the future I may make then inheritable.
 * 
 * Remember that the library HAS to be called 'LibTelltale.dll' in the Dll directory as the exe or the one defined by SetDllDirectory.
 * 
 * MUST BE A 64 BIT APPLICATION TO USE LIBTELLTALE!!
 * 
 * This source was written by Lucas Saragosa - The author of LibTelltale. You may not edit this file and redistribute, contact me on github for problems OR contact me about why you want to redist it.
 * 
 * This is the main source file for the C# library and this contains the base of the library. Use all other classes in MetaStreamed to open specific formats. Hope you enjoy the lib :)
 * 
*/

namespace LibTelltale {

	public class Config {

		/// <summary>
		/// The Minimum version required of the LibTelltale DLL for this library to work.
		/// </summary>
		public static readonly Version MIN_VERSION = Version.Parse("2.9.3");

		/// <summary>
		/// If the given game ID uses the old telltale tool, for games before and including Game of Thrones.
		/// </summary>
		public static readonly uint GAMEFLAG_OLD_TELLTALE_TOOL = 1;

		/// <summary>
		/// If the given game ID uses .ttarch2 archives
		/// </summary>
		public static readonly uint GAMEFLAG_TTARCH2 = 2;

		/// <summary>
		/// If the given game ID uses .ttarch archives.
		/// </summary>
		public static readonly uint GAMEFLAG_TTARCH = 4;

		/// <summary>
		/// Meta Stream Version 5. All games using .ttarch2 up to Minecraft: Story Mode (by date)
		/// </summary>
		public static readonly uint META_V5 =  0x4D535635;
		/// <summary>
		/// Meta Stream Version 6. The most recent and probably not going to change. All games in .ttarch2 after Minecraft: Story Mode
		/// </summary>
		public static readonly uint META_V6 =  0x4D535636;
		/// <summary>
		/// Meta Stream Version 2, Meta Binary. Texas hold'em, CSI: 3 Dimensions of Murder, Bone
		/// </summary>
		public static readonly uint META_BIN = 0x4D42494E;
		/// <summary>
		/// Meta Stream version 3. All games using .ttarch after meta binary games
		/// </summary>
		public static readonly uint META_TRE = 0x4D545245;

		public static readonly uint OPEN_OK = 0;
		/// <summary>
		/// The metastreamed file has a bad format and could not be opened. Contact me on github to report it, along with the game its from and its file name.
		/// </summary>
		public static readonly uint OPEN_BAD_FORMAT = 1;
		/// <summary>
		/// Can happen when a CRC (hash) is not recognised. This is likely the most common error. In a meta stream, the type name will default to 'unknown_t' (value of 2)
		/// </summary>
		public static readonly uint OPEN_CRC_UNIMPL = 2;
		/// <summary>
		/// You passed bad or null arguments to the Open function
		/// </summary>
		public static readonly uint OPEN_BAD_ARGS = 3;
		/// <summary>
		/// When you attempt to load a .vers (MetaStreamedFile_Vers) which is already loaded or one with the same structure is.
		/// </summary>
		public static readonly uint OPEN_VERS_ALREADY_LOADED = 4;
		static Config() {
			if (MIN_VERSION > Version.Parse (GetVersion ()))
				throw new LibTelltaleException (String.Format("Cannot use LibTelltale v{0}, the minimum version required is v{1}", GetVersion(), MIN_VERSION));
		}

		/// <summary>
		/// Clears the mapped libraries.
		/// </summary>
		public static void ClearMappedLibraries(){
			Config0.LibTelltale_ClearMappedLibs ();
		}

		/// <summary>
		/// Maps a library. The library id can be found in libtelltale.h on github, and the dll_name is the dll file name that libtelltale should search using.
		/// </summary>
		/// <param name="lib_id">Lib identifier.</param>
		/// <param name="dll_name">Dll name.</param>
		public static void MapLibrary(string lib_id, string dll_name){
			if (lib_id == null || dll_name == null)
				return;
			IntPtr ptr1 = Marshal.StringToHGlobalAnsi(lib_id);
			IntPtr ptr2 = Marshal.StringToHGlobalAnsi(dll_name);
			Config0.LibTelltale_MapLib (ptr1,ptr2);
			Marshal.FreeHGlobal(ptr1);
			Marshal.FreeHGlobal(ptr2);
		}

		/// <summary>
		/// Gets the game flags for a given game id, returning 0 if the game id is invalid. 
		/// </summary>
		public static ulong GetGameFlags(string gameid){
			return Config0.LibTelltale_GetGameFlags (gameid);
		}

		/// <summary>
		/// This number is an index for its release date, for example Texas Hold'em is 1 and Walking Dead: Collection is 35 (the highest). This is not episode based its game based (increments per game).
		/// </summary>
		public static uint GetGameNumber(string gameid){
			return (uint)((GetGameFlags (gameid) >> 32) & 0xFF);
		}

		/// <summary>
		/// Gets the game archive version from the given game flags which can be returned by get game flags.
		/// </summary>
		public static uint GetGameArchiveVersion(ulong gameFlags){
			return (uint)(gameFlags >> 7) & 15;
		}

		/// <summary>
		/// Creates a char pointer in memory, which can be deleted by C++'s delete[] which is hMemory_FreeArray
		/// </summary>
		/// <param name="s"></param>
		public static IntPtr CreateString(string s)
		{
			IntPtr tmp1 = Marshal.StringToHGlobalAnsi(s);
			IntPtr ret = Native.hMemory_CreateArray(tmp1);
			Marshal.FreeHGlobal(tmp1);
			return ret;
		}

		/// <summary>
		/// This frees all the dynamically defined constants memory which the library has allocated. It is safe to call this before you call any other methods from
		/// the library, although it may take as long as half a second to re-initialize. You should call this after you are finished with the library, as it can free
		/// around 0.1 - 1 megabytes (looks small, but it will be more as the library grows.)
		/// </summary>
		public static void Free()
        {
			Config0.LibTelltale_FreeLib();
        }

		/// <summary>
		/// Gets a game encryption key by its ID.
		/// </summary>
		/// <returns>The game encryption key.</returns>
		/// <param name="game_id">Game identifier.</param>
		public static string GetGameEncryptionKey(string game_id){
			IntPtr ptr = Marshal.StringToHGlobalAnsi(game_id);
			string ret = Marshal.PtrToStringAnsi (Config0.LibTelltale_GetKey (ptr));
			Marshal.FreeHGlobal(ptr);
			return ret;
		}

		/// <summary>
		/// Returns one of the META_x meta header versions. Useful if you want to write a file without knowing the meta version (however you are warned that the header needs the 
		/// previously opened meta stream as it contains the required classes or it wont load in game. Will return 0 if the game ID is invalid.
		/// </summary>
		/// <param name="game_id"></param>
		/// <returns></returns>
		public static uint GetMetaVersion(string game_id)
        {
			IntPtr ptr = Marshal.StringToHGlobalAnsi(game_id);
			uint ret = Config0.LibTelltale_GetGameMetaVersion(ptr, 0);
			Marshal.FreeHGlobal(ptr);
			return ret;
		}

		/// <summary>
		/// When you report to Github, this is the method you use to get the CRC so I can add it and its type.
		/// Returns the last unknown CRC which can have caused a meta streamed files Open to return 2 (crc unimpl). 
		/// </summary>
		public static string GetLastErrorCRC()
        {
			return Marshal.PtrToStringAnsi(Config0.LibTelltale_GetLastUnknownCRC());
        }

		/// <summary>
		/// Gets the version of LibTelltale.
		/// </summary>
		/// <returns>The version.</returns>
		public static string GetVersion(){
			return Marshal.PtrToStringAnsi (Config0.LibTelltale_Version ());
		}

		/// <summary>
		/// Encrypts using the blowfish algorithm the given data with the given game ID encryption key. 
		/// The modified boolean is for the new modified algorithm telltale wrote for all .ttarch2 archives, and .ttarch archives with version 7+. (Open a .ttarch see the first byte).
		/// </summary>
		public static MemoryHelper.Bytes BlowfishEncrypt(byte[] data, string gameID, bool modified){
			MemoryHelper.Bytes ret = new MemoryHelper.Bytes ();
			ret.mem = Native.hMemory_Alloc ((uint)data.Length);
			Marshal.Copy (data, 0, ret.mem, data.Length);
			IntPtr tmp = Marshal.StringToHGlobalAnsi(gameID);
			Config0.LibTelltale_BlowfishEncrypt (ret.mem, (uint)data.Length, (byte)(modified ? 1 : 0), Config0.LibTelltale_GetKey(tmp));
			Marshal.FreeHGlobal (tmp);
			ret.bytes = new byte[data.Length];
			Marshal.Copy (ret.mem, ret.bytes, 0, data.Length);
			return ret;
		}

		/// <summary>
		/// Convert a plain text resource description lua script to an encrypted one able to be read by any telltale game.
		/// The modified boolean is for the new modified algorithm telltale wrote for all .ttarch2 archives, and .ttarch archives with version 7+. (Open a .ttarch see the first byte).
		/// The islenc boolean specifies if the file is a .lenc, which uses a slightly different format.
		/// </summary>
		public static MemoryHelper.Bytes EncryptResourceDescription(byte[] data, string gameID, bool modified, bool islenc){
			unsafe {
				MemoryHelper.Bytes ret = new MemoryHelper.Bytes ();
				ret.mem = Native.hMemory_Alloc ((uint)data.Length);
				Marshal.Copy (data, 0, ret.mem, data.Length);
				IntPtr tmp = Marshal.StringToHGlobalAnsi (gameID);
				uint i = 0;
				uint* outz = &i;
				IntPtr ret1 = Config0.LibTelltale_EncryptResourceDescription (ret.mem, (uint)data.Length, outz, (byte)(modified ? 1 : 0), Config0.LibTelltale_GetKey (tmp), (byte)(islenc ? 1 : 0));
				Marshal.FreeHGlobal (tmp);
				Native.hMemory_Free (ret.mem);
				ret.mem = ret1;
				ret.bytes = new byte[*outz];
				Marshal.Copy (ret.mem, ret.bytes, 0, (int)*outz);
				return ret;
			}
		}
			
		/// <summary>
		/// Convert an encrypted lua script to an unencrypted plain text one.
		/// The modified boolean is for the new modified algorithm telltale wrote for all .ttarch2 archives, and .ttarch archives with version 7+. (Open a .ttarch see the first byte).
		/// </summary>
		public static MemoryHelper.Bytes DecryptResourceDescription(byte[] data, string gameID, bool modified){
			unsafe {
				MemoryHelper.Bytes ret = new MemoryHelper.Bytes ();
				ret.mem = Native.hMemory_Alloc ((uint)data.Length);
				Marshal.Copy (data, 0, ret.mem, data.Length);
				IntPtr tmp = Marshal.StringToHGlobalAnsi (gameID);
				uint i = 0;
				uint* outz = &i;
				IntPtr ret1 = Config0.LibTelltale_DecryptResourceDescription (ret.mem, (uint)data.Length, outz, (byte)(modified ? 1 : 0), Config0.LibTelltale_GetKey (tmp));
				Marshal.FreeHGlobal (tmp);
				Native.hMemory_Free (ret.mem);
				ret.mem = ret1;
				ret.bytes = new byte[*outz];
				Marshal.Copy (ret.mem, ret.bytes, 0, (int)*outz);
				return ret;
			}
		}

		public static MemoryHelper.Bytes EncryptScript(byte[] data, string gameID, bool modified, bool islenc){
			MemoryHelper.Bytes ret = new MemoryHelper.Bytes ();
			ret.mem = Native.hMemory_Alloc ((uint)data.Length);
			Marshal.Copy (data, 0, ret.mem, data.Length);
			IntPtr tmp = Marshal.StringToHGlobalAnsi (gameID);
			IntPtr ret1 = Config0.LibTelltale_EncryptScript (ret.mem, (uint)data.Length, (byte)(modified ? 1 : 0), Config0.LibTelltale_GetKey (tmp), (byte)(islenc ? 1 : 0));
			Marshal.FreeHGlobal (tmp);
			Native.hMemory_Free (ret.mem);
			ret.mem = ret1;
			ret.bytes = new byte[data.Length];
			Marshal.Copy (ret.mem, ret.bytes, 0, data.Length);
			return ret;
		}

		public static MemoryHelper.Bytes DecryptScript(byte[] data, string gameID, bool modified){
			MemoryHelper.Bytes ret = new MemoryHelper.Bytes ();
			ret.mem = Native.hMemory_Alloc ((uint)data.Length);
			Marshal.Copy (data, 0, ret.mem, data.Length);
			IntPtr tmp = Marshal.StringToHGlobalAnsi (gameID);
			IntPtr ret1 = Config0.LibTelltale_DecryptScript (ret.mem, (uint)data.Length,  (byte)(modified ? 1 : 0), Config0.LibTelltale_GetKey (tmp));
			Marshal.FreeHGlobal (tmp);
			Native.hMemory_Free (ret.mem);
			ret.mem = ret1;
			ret.bytes = new byte[data.Length];
			Marshal.Copy (ret.mem, ret.bytes, 0, data.Length);
			return ret;
		}

		/// <summary>
		/// Decrypts using the blowfish algorithm the given data with the given game ID encryption key. 
		/// The modified boolean is for the new modified algorithm telltale wrote for all .ttarch2 archives, and .ttarch archives with version 7+. (Open a .ttarch see the first byte).
		/// </summary>
		public static MemoryHelper.Bytes BlowfishDecrypt(byte[] data, string gameID, bool modified){
			MemoryHelper.Bytes ret = new MemoryHelper.Bytes ();
			ret.mem = Native.hMemory_Alloc ((uint)data.Length);
			Marshal.Copy (data, 0, ret.mem, data.Length);
			IntPtr tmp = Marshal.StringToHGlobalAnsi(gameID);
			Config0.LibTelltale_BlowfishDecrypt (ret.mem, (uint)data.Length, (byte)(modified ? 1 : 0), Config0.LibTelltale_GetKey(tmp));
			Marshal.FreeHGlobal (tmp);
			ret.bytes = new byte[data.Length];
			Marshal.Copy (ret.mem, ret.bytes, 0, data.Length);
			return ret;
		}

	}

	/// <summary>
	/// Any exception which is because you may have forgotten to set a stream etc.
	/// </summary>
	public class LibTelltaleException : Exception {
		public LibTelltaleException(string message) : base(message) {}
	}

	/// <summary>
	/// A meta class description in memory
	/// </summary>
	public struct _MetaClassDesc {
		public IntPtr mTypeName;
		public uint mVersion;//The version isnt actually a number, its some sort of CRCed string which I haven't got to yet. Version '0' for ints and booleans was the crc of 0 in memory (0,0,0,0) so im guessing its a 
		//string length since I tried all combinations of numbers from 0 - 0xFFFFFFFF. However until I find the string values for it, its going to be a uint32.
		public ulong mTypeNameCrc;
		public uint mVersionCrc;//this is for now the same as the mversion until i find out the above. the library will crc it you dont have to do that. although this is the same as the version (unless v0, then mVersoin is 0)
	}

	/// <summary>
	/// A meta class description as described in any meta streamed file meta stream header.
	/// </summary>
	public struct MetaClassDescription {
		public _MetaClassDesc backend;
		public IntPtr reference;
	}

	public class MemoryHelper
	{

		/// <summary>
		/// Represents bytes which have been read from a stream, which are eligible to be freed (save RAM :D). Use FreeReadBytes
		/// </summary>
		public struct Bytes
		{
			/// <summary>
			/// The backend memory ptr
			/// </summary>
			public IntPtr mem;
			/// <summary>
			/// The bytes which you read
			/// </summary>
			public byte[] bytes;
		}

		/// <summary>
		/// Frees the bytes read by Read.
		/// </summary>
		public static void FreeReadBytes(Bytes? b)
		{
			if (b.HasValue)
			{
				Native.hMemory_Free(b.GetValueOrDefault().mem);
			}
		}

	}

	/// <summary>
	/// Represents the meta stream header in any meta streamed file. This is the header you see in most telltale files (5VSM, 6VSM, ERTM, NIBM, SEBM). MOCM,4VSM are not supported (they arent released).
	/// </summary>
	public sealed class MetaStream : IDisposable {

		/// <summary>
		/// Use this to create a meta class description entry for a meta stream. 
		/// </summary>
		public static MetaClassDescription CreateMetaClass(string typeName, uint version){
			IntPtr ptr = Native.hMetaStream_CreateDesc ();
			MetaClassDescription desc = new MetaClassDescription ();
			desc.reference = ptr;
			IntPtr tmp =  Marshal.StringToHGlobalAnsi (typeName);
			desc.backend = new _MetaClassDesc ();
			desc.backend.mTypeName = Native.hMemory_CreateArray (tmp);
			desc.backend.mVersion = version;
			desc.reference = ptr;
			Marshal.StructureToPtr (desc.backend, ptr, false);
			Marshal.FreeHGlobal (tmp);
			return desc;
		}

		protected IntPtr reference;

		public IntPtr Internal_Get(){
			return this.reference;
		}
	
		public MetaStream(IntPtr r) {
			reference = r;
		}

		/// <summary>
		/// Create a new meta stream
		/// </summary>
		public MetaStream(){
			this.reference = Native.hMetaStream_Create ();
		}

		/// <summary>
		/// Removes a meta class description entry
		/// </summary>
		public void RemoveClass(MetaClassDescription desc){
			Native.MetaStreamClasses_DCArray_Remove (this._GClasses (), desc.reference);
		}

		/// <summary>
		/// Adds a meta class description entry
		/// </summary>
		public void AddClass(MetaClassDescription desc){
			Native.MetaStreamClasses_DCArray_Add (this._GClasses (), desc.reference);
		}

		/// <summary>
		/// The amount of meta class description entries in this meta stream
		/// </summary>
		public int GetClasses(){
			return Native.MetaStreamClasses_DCArray_Size (this._GClasses ());
		}

		/// <summary>
		/// Similar to AddClass but creates it with the given name and version.
		/// </summary>
		public void AddNewClass(string typeName, uint version){
			this.AddClass (CreateMetaClass (typeName, version));
		}

		/// <summary>
		/// Opens this meta stream from the given input stream of bytes
		/// </summary>
		public bool Open(ByteStream stream){
			return Native.hMetaStream_Open (this.reference, stream.reference) != 0;
		}

		/// <summary>
		/// Writes this meta stream to the given output stream of bytes
		/// </summary>
		public void Flush(ByteOutStream stream){
			Native.hMetaStream_Flush (this.reference, stream.reference);
		}

		/// <summary>
		/// Gets the version of this meta stream (will be one of Config.META_x)
		/// </summary>
		public uint GetVersion(){
			return Native.hMetaStream_GetVersion (this.reference);
		}

		/// <summary>
		/// GetVersion() as a string (eg returns MSV6)
		/// </summary>
		public string GetVersionAsString(){
			byte[] str = BitConverter.GetBytes (GetVersion ());
			Array.Reverse (str);
			return System.Text.ASCIIEncoding.Default.GetString (str);
		}

		/// <summary>
		/// Gets the flags of this meta stream
		/// </summary>
		public uint GetFlags(){
			return Native.hMetaStream_GetFlags(this.reference);
		}

		/// <summary>
		/// Gets the size of the file this meta stream describes. Automatically set by TTContext on flush!
		/// </summary>
		public uint GetPayloadSize(){
			return Native.hMetaStream_GetPayloadSize (this.reference);
		}

		/// <summary>
		/// If this is a texture (or .bundle? not got to it) then this is the size of the raw texture data
		/// </summary>
		public uint GetTextureSize(){
			return Native.hMetaStream_GetTextureSize (this.reference);
		}

		/// <summary>
		/// Gets the class version of a given type name (searches for a meta class description entry with the type name and returns its version)
		/// </summary>
		public uint GetClassVersion(string typeName){
			return Native.hMetaStream_GetClassVersion (this.reference, typeName, 0);
		}

		/// <summary>
		/// Determines whether this instance has a class of the specified type name.
		/// </summary>
		public bool HasClass(string typeName){
			return GetClassVersion (typeName) != 0;
		}

		/// <summary>
		/// Closes this meta stream. This resets all inner class descriptions, the version,flags,sizes etc all to 0. Ready for the next open.
		/// </summary>
		public void Close(){
			Native.hMetaStream_Close (this.reference);
		}

		/// <summary>
		/// Clears the meta class description classes
		/// </summary>
		public void ClearClasses(){
			Native.MetaStreamClasses_DCArray_Clear (this._GClasses ());
		}

		/// <summary>
		/// Gets a meta class at the specified index (used for iteration)
		/// </summary>
		public MetaClassDescription GetMetaClass(int index){
			MetaClassDescription desc = new MetaClassDescription ();
			desc.backend = (_MetaClassDesc)Marshal.PtrToStructure (desc.reference = Native.MetaStreamClasses_DCArray_At(this._GClasses(), index), typeof(_MetaClassDesc));
			return desc;
		}

		protected IntPtr _GClasses(){
			return Native.hMetaStream_GetClasses (this.reference);
		}

		/// <summary>
		/// Sets the meta version. Must be one of the Config.META_x
		/// </summary>
		public void SetMetaVersion(uint Version){
			if (!(Version == Config.META_BIN || Version == Config.META_TRE || Version == Config.META_V5 || Version == Config.META_V6))
				return;
			Native.hMetaStream_SetVersion (this.reference, Version);
		}

		/// <summary>
		/// Deletes the backend memory of this. Suggested not to use this, just let the TTContext handle it.
		/// </summary>
		public void Dispose(){
			if (this.disposed) return;
			Native.hMetaStream_Delete (this.reference);
			this.reference = IntPtr.Zero;
			this.disposed = true;
		}

		private bool disposed = false;

		/// <summary>
		/// Sets the flags for this meta stream, useful for ORing. 
		/// </summary>
		/// <param name="flags">Flags.</param>
		public void SetFlags(uint flags){
			Native.hMetaStream_SetFlags (reference, flags);
		}

	}


	/// <summary>
	/// Telltale file reading (and writing, see writingcontext) context. Use it when reading or writing any files or archives, and you are advised not to make too many of these objects.
	/// </summary>
	public sealed class TTContext : IDisposable {
		protected IntPtr reference;
		private bool disposed = false;


		/// <summary>
		/// Deletes the backend memory of this context. This deletes the current stream and out stream, so be careful.
		/// </summary>
		public void Dispose(){
			if (this.disposed) return;
			Native.hTTContext_Delete (this.reference);
			this.disposed = true;
		}

		/// <summary>
		/// Creates a context which will search files from the given archive.
		/// </summary>
		public TTContext(TTArchives.TTArchive archive, string gameID){
			IntPtr str1 = Marshal.StringToHGlobalAnsi (gameID);
			IntPtr ptr = Native.hMemory_CreateArray (str1);
			if (archive == null) {
				this.reference = Native.hTTContext_Create ( ptr, IntPtr.Zero);
			} else {
				IntPtr ptr2 = Native.hTTArchiveOrTTArchive2_Create ();
				Marshal.StructureToPtr (TTArchives.TTArchiveOrTTArchive2.CreateFromArchive (archive), ptr2, false);
				this.reference = Native.hTTContext_Create (ptr, ptr2);
			}
			Native.hMemory_FreeArray (ptr);
			Marshal.FreeHGlobal (str1);
			this.OverrideNewMeta(gameID);
		}

		/// <summary>
		/// Updates the current game ID of this context. If there is an archive attached even, this does not update its game key.
		/// The game ID has information about the way in which the next writes should be formatted so the game you are writing
		/// for (sometimes reading files too) has the correct version and wont crash (although if it does contact me!).
		/// This should be called before NextWrite (or NextRead just in case).
		/// </summary>
		public void NextGameID(string id) {
			IntPtr ptr = Marshal.StringToHGlobalAnsi (id);
			Native.hTTContext_NextGame (this.reference, ptr);
			Marshal.FreeHGlobal(ptr);
		}

		/// <summary>
		/// Creates a context which will search files from the given archive.
		/// </summary>
		public TTContext(TTArchives.TTArchive2 archive, string gameID) {
			IntPtr str1 = Marshal.StringToHGlobalAnsi (gameID);
			IntPtr ptr = Native.hMemory_CreateArray (str1);
			if (archive == null) {
				this.reference = Native.hTTContext_Create ( ptr, IntPtr.Zero);
			} else {
				IntPtr ptr2 = Native.hTTArchiveOrTTArchive2_Create ();
				Marshal.StructureToPtr (TTArchives.TTArchiveOrTTArchive2.CreateFromArchive (archive), ptr2, false);
				this.reference = Native.hTTContext_Create (ptr, ptr2);
			}
			Native.hMemory_FreeArray (ptr);
			Marshal.FreeHGlobal (str1);
			this.OverrideNewMeta(gameID);
		}

		/// <summary>
		/// Factory method to quickly set the meta version, meaning you don't have to have opened a file before a write. This is good for testing because when you put it in the game,
		/// the meta streams need to be correct.
		/// </summary>
		public void OverrideNewMeta(uint metaVersion)
        {
			MetaStream m = new MetaStream();
			m.SetMetaVersion(metaVersion);
			this.OverrideMetaStream(m, true);
        }

		/// <summary>
		/// Factory method to quickly set the meta version by its game ID, meaning you don't have to have opened a file before a write. This is good for testing because when you put it in the game,
		/// the meta streams need to be correct.
		/// </summary>
		public void OverrideNewMeta(string gameID)
		{
			MetaStream m = new MetaStream();
			m.SetMetaVersion(Config.GetMetaVersion(gameID));
			this.OverrideMetaStream(m, true);
		}

		/// <summary>
		/// Creates a context which doesn't read from an archive. When reading meshes for example, textures may not be accessible since it requires the archive for the textures.
		/// </summary>
		public TTContext(string gameID) : this((TTArchives.TTArchive)null, gameID) { }

		/// <summary>
		/// Switches this context to the next archive. The del parameter specified if the previous meta/in and out stream should be deleted and disposed if they exist.
		/// NOTE: This does NOT delete the previous archive! This is because they are big and alot of the time you want to keep them open.
		/// </summary>
		public void NextArchive(TTArchives.TTArchive archive, bool del){
			IntPtr ptr = Native.hTTArchiveOrTTArchive2_Create ();
			Marshal.StructureToPtr (TTArchives.TTArchiveOrTTArchive2.CreateFromArchive (archive), ptr, false);
			Native.hTTContext_NextArchive (this.reference, ptr, del);
		}

		/// <summary>
		/// Switches this context to the next archive. The del parameter specified if the previous meta/in and out stream should be deleted and disposed if they exist.
		/// NOTE: This does NOT delete the previous archive! This is because they are big and alot of the time you want to keep them open.
		/// </summary>
		public void NextArchive(TTArchives.TTArchive2 archive, bool del){
			IntPtr ptr = Native.hTTArchiveOrTTArchive2_Create ();
			Marshal.StructureToPtr (TTArchives.TTArchiveOrTTArchive2.CreateFromArchive (archive), ptr, false);
			Native.hTTContext_NextArchive (this.reference, ptr, del);
		}

		/// <summary>
		/// Finds an entry (returning its name) in the current archive by its crc64 of its file name. Used mostly internally, but can be useful.
		/// Returns an empty string if there is no current archive.
		/// </summary>
		public string FindArchiveEntry(ulong crc64){
			IntPtr str = Native.hTTContext_FindArchiveEntry (this.reference, crc64);
			if (str.Equals (IntPtr.Zero))
				return "";
			return Marshal.PtrToStringAnsi (str);
		}

		/// <summary>
		/// If a previous NextRead has been set, then this is the start offset of the file after the meta header.
		/// </summary>
		public uint GetFileStart(){
			return Native.hTTContext_FileStart (this.reference);
		}

		/// <summary>
		/// Gets the meta stream header of the current reading/writing file in this context.
		/// </summary>
		/// <returns>The current meta.</returns>
		public MetaStream GetCurrentMeta(){
			IntPtr ptr = Native.hTTContext_CurrentMeta (this.reference);
			if(ptr.Equals(IntPtr.Zero))
				return null;
			return new MetaStream (ptr);
		}

		/// <summary>
		/// Overrides the current meta stream, deleting the meta stream in memory with the del parameter. Useful when writing a file, but you haven't read a file first to set its meta.
		/// </summary>
		public void OverrideMetaStream(MetaStream stream,bool del){
			Native.hTTContext_OverrideMeta(this.reference, stream.Internal_Get(), del);
		}
			
		/// <summary>
		/// Finalizes the current write after NextWrite and wanted file's Flush().
		/// The second parameter specifies if the entry in the archive's stream should be updated with this new one (you can forget about the byteoutstream object, it will be handled). This makes it easier
		/// to edit archives. If there is no archive attached then this does nothing. The delete parameter is still important however, as the byte output stream writing to will still be disposed of
		/// because the backend bytes of the file are copied to a bytestream since an archive stream is a readable one. So this means the last writing stream will be deleted, just not the bytes of it.
		/// The entry which this sets the stream for is the entry in the attached archive with the name passed in the NextWrite method which should have been previously called. If the entry doesn't exist
		/// this also does nothing too, so make sure you have at least created the entry.
		/// </summary>
		public void FinishCurrentWrite(bool del, bool updatearc){
			Native.hTTContext_FinishWrite (this.reference, del,updatearc);
		}

		/// <summary>
		/// Gets name (or an empty string if not writing) of the current file which is being written.
		/// </summary>
		public string GetCurrentWritingFile(){
			IntPtr nptr = Native.hTTContext_CurrentFile (this.reference);
			if (nptr.Equals (IntPtr.Zero))
				return "";
			return Marshal.PtrToStringAnsi (nptr);
		}

		/// <summary>
		/// Updates and initializes this context to the new writing stream. This doesn't affect the reading streams, but does reset and delete if del the previous outstream.
		/// You are required to pass the full file name of the writing file, this has to be the EXACT file name with extension and casing! If you read it, has to be the same as
		/// it was before. This requires a meta stream to be set by next read or override current meta, or it wont work. The stream parameter can be a direct new ByteOutStream, since it only needs
		/// to be handled by the library and only will be. See FinishedCurrentWrite which explains what happens after you have called Flush on the file type object (eg InputMapper.Flush()).
		/// The stream parameter can be null, and if it is the out stream will be handled internally. This is useful if you are planning to write to back to a telltale archive. (Using update = true
		/// on the FinishCurrentWrite). This returns a boolean which is if the next write initializer worked. If it returns false, this means that there is no meta stream current. You would then
		/// need to use override metastream and set a new one. This is because lots of Telltale's games have the versions in the meta streams and the library needs to know the version to serialize it as :D
		/// </summary>
		public bool NextWrite(ByteOutStream stream, string fileName,bool del){
			bool ret;
			if (stream != null) {
				ret = Native.hTTContext_NextWrite (this.reference, stream.reference, fileName, del) != 0;
			} else {
				ret = Native.hTTContext_NextWrite (this.reference, Native.hByteOutStream_Create(0), fileName, del) != 0;
			}
			return ret;
		}

		/// <summary>
		/// Gets the current output stream this context is writing to.
		/// </summary>
		public ByteOutStream GetCurrentOutStream(){
			return new ByteOutStream (Native.hTTContext_CurrentOutStream (this.reference));
		}

		/// <summary>
		/// Gets the current stream.
		/// </summary>
		/// <returns>The current stream.</returns>
		public ByteStream GetCurrentStream(){
			IntPtr ptr = Native.hTTContext_CurrentStream (this.reference);
			if(ptr.Equals(IntPtr.Zero))
				return null;
			return new ByteStream (ptr);
		}

		/// <summary>
		/// Opens a stream from the backend archive. Bascially TTArchive<2> open stream.
		/// </summary>
		public ByteStream OpenArchiveStream(string archiveEntryName){
			IntPtr ptr = Native.hTTContext_OpenStream (this.reference,archiveEntryName);
			if(ptr.Equals(IntPtr.Zero))
				return null;
			return new ByteStream (ptr);
		}

		/// <summary>
		/// Updates this context to the new reading stream. This does read the meta stream but not the file (since you need it specific).
		/// If del is true, this deletes the old reading streams backend memory so if you have a reference to it still then be careful (DO NOT call its dispose!) or set this to false!
		/// </summary>
		public bool NextStream(ByteStream stream, bool del){
			return Native.hTTContext_NextStream (this.reference, stream.reference, del) != 0;
		}

		public IntPtr Internal_Get () {
			return reference;
		}

	}

	/// <summary>
	/// Base for all meta streamed files, excluding .vers (serialized version info; they specify formats), and all implementing classes derive from this interface.
	/// </summary>
	public abstract class AbstractMetaStreamedFile : IDisposable {

		/// <summary>
		/// Opens after NextStream has been called on the current context. Returns one of the constants in config, where OPEN_OK (0) is successfull.
		/// </summary>
		public abstract int Open ();

		/// <summary>
		/// Writes this meta streamed file to the attached context, returning if it was successfull. Must be called after NextWrite in a TTContext, followed by FinishCurrentWrite.
		/// </summary>
		public abstract bool Flush();

		/// <summary>
		/// Used internally to get the backend pointer in memory to this object.
		/// </summary>
		public abstract IntPtr Internal_Get();

		/// <summary>
		/// Gets the attached context this meta streamed file uses.
		/// </summary>
		public abstract TTContext GetContext();

		//0> is if the stream is invalid
		public int ReadFromContext(ByteStream stream)
        {
			if (!GetContext().NextStream(stream, true)) return -1;
			return Open();
        }

		public bool WriteToContext(ByteOutStream stream, string fileName)
        {
			if (GetContext().GetCurrentMeta() == null) return false;
			bool ret1 = GetContext().NextWrite(stream, fileName, true);
			if (!ret1) return false;
			bool ret = Flush();
			if (!ret) return false;
			GetContext().FinishCurrentWrite(true, true);
			return true;
        }

		public abstract void Dispose();

	}
    public sealed class Handle : IDisposable {

		protected h v;

		protected bool disposed = false;

		protected readonly IntPtr reference;

		protected struct h {
			public IntPtr name;
			public ulong crc;
		}


		public Handle(string name)
        {
			IntPtr tmp = Marshal.StringToHGlobalAnsi(name);
			ulong crc = Native.ClassToCRC(tmp);
			this.reference = Config0.Handle_Create(tmp, crc);
			Marshal.FreeHGlobal(tmp);
			this.v = (h)Marshal.PtrToStructure(reference, typeof(h));
        }

		public IntPtr Interal_Get()
        {
			return this.reference;
        }

		public Handle(IntPtr ptr)
        {
			this.reference = ptr;
        }

		public string FindReferenced(TTContext ctx)
        {
			return ctx.FindArchiveEntry(GetCRC());
        }

		public string GetName()
        {
			IntPtr ptr = this.v.name;
			if (ptr.Equals(IntPtr.Zero)) return "";
			return Marshal.PtrToStringAnsi(ptr);
        }

		public ulong GetCRC()
        {
			return this.v.crc;
        }

		public void SetCRC(ulong crc)
        {
			this.v.crc = crc;
			Marshal.StructureToPtr(v, reference, false);
        }

		public void SetName(string name)
        {
			IntPtr ptr = Marshal.StringToHGlobalAnsi(name);
            if (!this.v.name.Equals(IntPtr.Zero))
            {
				Native.hMemory_FreeArray(this.v.name);
			}
			this.v.name = Native.hMemory_CreateArray(ptr);
			Marshal.FreeHGlobal(ptr);
			Marshal.StructureToPtr(v, reference, false);
		}

		public void Dispose()
        {
			if (this.disposed) return;
			Config0.Handle_Delete(this.reference);
			this.disposed = true;
        }

	}	


}