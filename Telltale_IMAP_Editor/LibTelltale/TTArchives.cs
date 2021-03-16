using System;
using System.Runtime.InteropServices;
using LibTelltaleNative;
using LibTelltale;

/// <summary>
/// This namespace is all to do with the loading/writing of TTArchive bundles, .ttarch and .ttarch2
/// </summary>
namespace TTArchives {

	public struct TTArchiveOrTTArchive2 {
		public byte isTTArchive2;
		public IntPtr archive;
		public IntPtr archive2;

		public static TTArchiveOrTTArchive2 CreateFromArchive(TTArchive archive){
			TTArchiveOrTTArchive2 ret = new TTArchiveOrTTArchive2 ();
			ret.archive = archive.reference;
			ret.archive2 = IntPtr.Zero;
			ret.isTTArchive2 = 1;
			return ret;
		}

		public static TTArchiveOrTTArchive2 CreateFromArchive(TTArchive2 archive){
			TTArchiveOrTTArchive2 ret = new TTArchiveOrTTArchive2 ();
			ret.archive2 = archive.reference;
			ret.archive = IntPtr.Zero;
			ret.isTTArchive2 = 2;
			return ret;
		}

	}

	/// <summary>
	/// Constants which get returned from flush and open, use this to set custom options too.
	/// </summary>
	public static class Constants {
		public static readonly uint TTARCH_OPEN_OK = 0;
		public static readonly uint TTARCH_OPEN_BAD_STREAM = 1;
		public static readonly uint TTARCH_OPEN_BAD_HEADER = 2;
		public static readonly uint TTARCH_OPEN_BAD_VERSION = 4;
		public static readonly uint TTARCH_OPEN_BAD_DATA = 5;
		public static readonly uint TTARCH_OPEN_BAD_KEY = 6;
		public static readonly uint TTARCH_OPEN_LIB_ERR = 7;
		public static readonly uint TTARCH_OPEN_BAD_ARCHIVE = 8;
		public static readonly uint TTARCH_FLUSH_OK = 0;
		public static readonly uint TTARCH_FLUSH_BAD_STREAM = 1;
		public static readonly uint TTARCH_FLUSH_BAD_ARCHIVE = 2;
		public static readonly uint TTARCH_FLUSH_DATA_ERR = 3;
		public static readonly uint TTARCH_FLUSH_LIB_ERR = 4;
		public static readonly uint TTARCH_FLUSH_BAD_OPTIONS = 5;
		public static readonly uint TTARCH_FLUSH_COMPRESS_DEFAULT = 1;
		public static readonly uint TTARCH_FLUSH_COMPRESS_OODLE = 2;
		public static readonly uint TTARCH_FLUSH_ENCRYPT = 4;
		public static readonly uint TTARCH_FLUSH_SKIP_CRCS =	8;
		public static readonly uint TTARCH_FLUSH_RAW = 16;
		public static readonly uint TTARCH_FLUSH_NO_TMPFILE = 32;
		public static readonly uint TTARCH_FLUSH_V0 = 0;
		public static readonly uint TTARCH_FLUSH_V1 = 128;
		public static readonly uint TTARCH_FLUSH_V2 = 256;
		public static readonly uint TTARCH_FLUSH_V3 = 384;
		public static readonly uint TTARCH_FLUSH_V4 = 512;
		public static readonly uint TTARCH_FLUSH_V7 = 896;
		public static readonly uint TTARCH_FLUSH_V8 = 1024;
		public static readonly uint TTARCH_FLUSH_V9 = 1152;
	}

	/// <summary>
	/// Represents an entry in a .ttarch archive. Do not touch the referencee pointer or any value in any of the fields! The only field useful is the backend.name.
	/// </summary>
	public struct TTArchiveEntry {
		public _TTArchiveEntry backend;
		public IntPtr reference;
	}

	/// <summary>
	/// The full struct of an entry in memory, important that you do not edit fields in here!
	/// </summary>
	public struct _TTArchiveEntry {
		public IntPtr override_stream; // To override it, use set stream!
		public ulong offset;
		public uint size;
		[MarshalAs(UnmanagedType.LPStr)]
		public string name;
		public ulong name_crc;
		public byte flags;
	}

	/// <summary>
	/// Handles a .ttarch archive
	/// </summary>
	public sealed class TTArchive : IDisposable {

		/// <summary>
		/// Sets the name of the entry, do not use entry.name = ...
		/// </summary>
		public static void SetEntryName(TTArchiveEntry entry, string name){
			Native.hTTArchive_EntrySetName (entry.reference, name);
		}

		/// <summary>
		/// Creates the TTArchive entry, using the given name and input stream of bytes.
		/// </summary>
		/// <returns>The TTArchive entry.</returns>
		public static TTArchiveEntry CreateTTArchiveEntry(string name, ByteStream stream){
			TTArchiveEntry ret = new TTArchiveEntry();
			ret.reference = Native.TTArchive_EntryCreate (name, stream == null ? IntPtr.Zero : stream.reference);
			ret.backend = (_TTArchiveEntry)Marshal.PtrToStructure (ret.reference, typeof(_TTArchiveEntry));
			return ret;
		}

		[StructLayout(LayoutKind.Sequential)]
		protected struct ttarch {
			public IntPtr game_key;
			public IntPtr entries;
			public IntPtr stream;
			public IntPtr flushstream;
			public IntPtr reserved;
			public uint options;
		};

		protected ttarch handle;

		/// <summary>
		/// Reference pointer, do not touch.
		/// </summary>
		public readonly IntPtr reference;

		protected readonly string gameID;

		public string GetGameID(){
			return this.gameID;
		}

		protected ByteStream instream;

		protected ByteOutStream outstream;

		/// <summary>
		/// Shortcut to get the name string of the entry.
		/// </summary>
		public string GetEntryName(TTArchiveEntry e){
			return e.backend.name;
		}

		/// <summary>
		/// Gets the archive options.
		/// </summary>
		/// <returns>The archive options.</returns>
		public uint GetArchiveOptions(){
			UpdateAndSync (true);
			return this.handle.options;
		}

		/// <summary>
		/// Sets an archive option, use the options from constants.
		/// </summary>
		/// <param name="op">Op.</param>
		public void SetArchiveOption(uint op){
			this.handle.options |= op;
			UpdateAndSync (false);
		}

		/// <summary>
		/// Convenience method to unset an option from constants.
		/// </summary>
		/// <param name="op">Op.</param>
		public void UnsetArchiveOption(uint op){
			this.handle.options &= ~op;
			UpdateAndSync (false);
		}

		/// <summary>
		/// Gets the archive version.
		/// </summary>
		/// <returns>The archive version.</returns>
		public uint GetArchiveVersion(){
			return (GetArchiveOptions () >> 7) & 15;
		}

		/// <summary>
		/// Sets the archive version. Must only use versions from constants.
		/// </summary>
		/// <param name="version">Version.</param>
		public void SetArchiveVersion(uint version){
			this.handle.options &= 127;
			this.handle.options |= (version & 15) << 7;
			UpdateAndSync (false);
		}

		/// <summary>
		/// Initializes a new  <see cref="LibTelltale.TTArchives.TTArchive"/>. The game ID should be the game ID from the github page, and will throw an exception in the case that the game for the ID
		/// could not be found.
		/// </summary>
		/// <param name="gameid">The Game ID</param>
		public TTArchive(string gameid){
			reference = Native.hTTArchive_Create ();
			if (this.reference.Equals (IntPtr.Zero))
				throw new LibTelltaleException ("Could not create backend archive");	
			this.UpdateAndSync (true);
			IntPtr key = Marshal.StringToHGlobalAnsi (gameid);
			this.handle.game_key = Config0.LibTelltale_GetKey (key);
			if (this.handle.game_key.Equals (IntPtr.Zero))
				throw new LibTelltaleException (String.Format("Could not find a key for the game ID {0}", gameid));
			Marshal.FreeHGlobal (key);
			this.gameID = gameid;
			this.SetArchiveVersion (Config.GetGameArchiveVersion (Config.GetGameFlags (gameid)));
			this.UpdateAndSync (false);
		}

		/// <summary>
		/// Opens a readable byte input stream for the given entry.
		/// </summary>
		public ByteStream StreamOpen(TTArchiveEntry entry){
			return new ByteStream (Native.TTArchive_StreamOpen (reference, entry.reference));
		}

		/// <summary>
		/// Removes the entry from the archive.
		/// </summary>
		/// <param name="e">The Entry to remove. Must not be null</param>
		/// <param name="delete">If set to <c>true</c> then the entry will be freed to save memory. Use this only if you are removing and deleting the entry because you don't need it</param>
		public void RemoveEntry(TTArchiveEntry e, bool delete){
			Native.hTTArchive_EntryRemove (this.reference, e.reference, delete);
		}

		/// <summary>
		/// Finds an entry by its full file name, for example FindEntry("Boot.lua"). Returns a nullable value, if not found.
		/// </summary>
		public TTArchiveEntry? FindEntry(string name){
			TTArchiveEntry r = new TTArchiveEntry ();	
			IntPtr entryp = Native.TTArchive_EntryFind(reference, name);
			if (entryp.Equals (IntPtr.Zero))
				return null;
			r.backend = (_TTArchiveEntry)Marshal.PtrToStructure (entryp, typeof(_TTArchiveEntry));
			r.reference = entryp;
			return r;
		}

		/// <summary>
		/// Sets the stream for entry, which will be the overriden stream to use when writing (flushing) the archive.
		/// </summary>
		public void SetStreamForEntry(TTArchiveEntry entry, ByteStream stream){
			Native.TTArchive_StreamSet (entry.reference, stream.reference);
			entry.backend = (_TTArchiveEntry)Marshal.PtrToStructure (entry.reference, typeof(_TTArchiveEntry));
		}

		/// <summary>
		/// Adds the entry to the backend vector list ready for when you write back the archive.
		/// </summary>
		/// <param name="entry">Entry.</param>
		public void AddEntry(TTArchiveEntry entry){
			Native.hTTArchive_EntryAdd (this.reference, entry.reference);
			UpdateAndSync (true);
		}

		/// <summary>
		/// Gets an entry at the given file index. This is used when you want to loop through all files, or if you already know the index in the entry list.
		/// </summary>
		/// <returns>The entry.</returns>
		public TTArchiveEntry? GetEntry(uint index){
			TTArchiveEntry r = new TTArchiveEntry ();
			IntPtr entryp = Native.hTTArchive_GetEntryAt (reference, index);
			if (entryp.Equals (IntPtr.Zero))
				return null;
			r.backend = (_TTArchiveEntry)Marshal.PtrToStructure (entryp, typeof(_TTArchiveEntry));
			r.reference = entryp;
			return r;
		}

		/// <summary>
		/// Clears the entries.
		/// </summary>
		public void ClearEntries(){
			Native.hTTArchive_ClearEntries (this.reference);
			UpdateAndSync (true);
		}

		/// <summary>
		/// Writes all entries with the given options to the outstream in this archive instance. Returns TTARCH_FLUSH_OK is all goes well (0), else see the constants value.
		/// </summary>
		public int Flush(){
			if (outstream == null)
				throw new LibTelltaleException ("No stream set");
			int ret = Native.TTArchive_Flush (this.reference,IntPtr.Zero);
			if (ret == Constants.TTARCH_FLUSH_OK) {
				UpdateAndSync (true);
			}
			return ret;
		}

		/// <summary>
		/// Releases all resources used by this <see cref="LibTelltale.TTArchives.TTArchive"/> object.
		/// This also frees all memory within the backend TTArchive and all its entries so make sure you call this after you need every entry otherwise you will get unmanaged memory errors.
		/// </summary>
		public void Dispose(){
			Native.TTArchive_Free (this.reference);
			Native.hTTArchive_Delete (this.reference);
			this.instream = null;
			this.outstream = null;
		}

		/// <summary>
		/// Opens and adds all entries from the given archive InStream. Do not use when you have previously loaded an archive, in that case use a new instance of this class.
		/// </summary>
		public int Open(){
			if (instream == null)
				throw new LibTelltaleException ("No stream set");
			int ret = Native.TTArchive_Open (this.reference);
			if (ret == Constants.TTARCH_OPEN_OK) {
				UpdateAndSync (true);
			}
			return ret;
		}

		/// <summary>
		/// The amount of entries in this archive.
		/// </summary>
		/// <returns>The entry count.</returns>
		public uint GetEntryCount(){
			return Native.hTTArchive_GetEntryCount (reference);
		}

		/// <summary>
		/// Gets or sets the input stream to read the archive from in Open
		/// </summary>
		/// <value>The in stream.</value>
		public ByteStream InStream {
			get{ return instream; }
			set { instream = value;  if(instream != null)this.handle.stream = instream.reference;  if(instream != null)UpdateAndSync (false); }
		}

		/// <summary>
		/// Gets or sets the output stream to write the archive to in Flush.
		/// </summary>
		/// <value>The out stream.</value>
		public ByteOutStream OutStream {
			get{ return outstream; }
			set { outstream = value; if(outstream != null)this.handle.flushstream = outstream.reference;  if(outstream != null)UpdateAndSync (false);}
		}

		protected void UpdateAndSync(bool retrieve){
			if (retrieve) {
				this.handle = (ttarch)Marshal.PtrToStructure (reference, typeof(ttarch));
			} else {
				Marshal.StructureToPtr (this.handle, reference, false);
			}
		}

	}

	/// <summary>
	/// Represents a file entry in a TTArchive2 (.ttarch2) archive.
	/// </summary>
	public struct TTArchive2Entry {
		public _TTArchive2Entry backend;
		public IntPtr reference;
	}

	/// <summary>
	/// Backend structure of an entry in memory. Do not edit fields, the only useful field in this struct is the name, which you can retrieve to your liking.
	/// </summary>
	public struct _TTArchive2Entry {
		public ulong offset;
		public uint size;
		public ulong name_crc;
		[MarshalAs(UnmanagedType.LPStr)]
		public string name;
		public IntPtr override_stream; // To override it, use set stream!
		public byte flags;
	}

	/// <summary>
	/// A .ttarch2 archive
	/// </summary>
	public sealed class TTArchive2 : IDisposable {

		/// <summary>
		/// Sets the name of the given entry. Do not use a direct set to the entry.backend.name!
		/// </summary>
		public static void SetEntryName(TTArchive2Entry entry, string name){
			Native.TTArchive2_EntrySetName (entry.reference, name);
		}

		/// <summary>
		/// Creates a TTArchive2 entry, with the given name and input stream of bytes.
		/// </summary>
		/// <returns>The TT archive2 entry.</returns>
		public static TTArchive2Entry CreateTTArchive2Entry(string name, ByteStream stream){ 
			if (stream != null) {
				TTArchive2Entry ret = new TTArchive2Entry ();
				ret.reference = Native.TTArchive2_EntryCreate (name, stream.reference);
				ret.backend = (_TTArchive2Entry)Marshal.PtrToStructure (ret.reference, typeof(_TTArchive2Entry));
				return ret;
			} else {
				TTArchive2Entry ret = new TTArchive2Entry ();
				ret.reference = Native.TTArchive2_EntryCreate (name, IntPtr.Zero);
				ret.backend = (_TTArchive2Entry)Marshal.PtrToStructure (ret.reference, typeof(_TTArchive2Entry));
				return ret;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		protected struct ttarch2 {
			public uint options;
			public IntPtr game_key;
			public IntPtr entries;
			public IntPtr stream;
			public IntPtr flushstream;
			public byte flags;
		};

		protected ttarch2 handle;

		public readonly IntPtr reference;

		protected readonly string gameID;

		public string GetGameID(){
			return this.gameID;
		}

		protected ByteStream instream;

		protected ByteOutStream outstream;

		/// <summary>
		/// Gets the options for this archive.
		/// </summary>
		/// <returns>The archive options.</returns>
		public uint GetArchiveOptions(){
			UpdateAndSync (true);
			return this.handle.options;
		}

		/// <summary>
		/// Removes the entry from the archive.
		/// </summary>
		/// <param name="e">The Entry to remove. Must not be null</param>
		/// <param name="delete">If set to <c>true</c> then the entry will be freed to save memory. Use this only if you are removing and deleting the entry because you don't need it</param>
		public void RemoveEntry(TTArchive2Entry e, bool delete){
			Native.hTTArchive2_EntryRemove (this.reference, e.reference, delete);
		}

		/// <summary>
		/// Gets the archive version.
		/// </summary>
		/// <returns>The archive version.</returns>
		public uint GetArchiveVersion(){
			return (GetArchiveOptions () >> 7) & 15;
		}

		/// <summary>
		/// Sets the archive version.
		/// </summary>
		/// <param name="version">Version.</param>
		public void SetArchiveVersion(uint version){
			this.handle.options &= 127;
			this.handle.options |= (version & 15) << 7;
			UpdateAndSync (false);
		}

		/// <summary>
		/// Sets the archive an option from the constants class.
		/// </summary>
		/// <param name="op">Op.</param>
		public void SetArchiveOption(uint op){
			this.handle.options |= op;
			UpdateAndSync (false);
		}

		/// <summary>
		/// Unsets the archive option.
		/// </summary>
		/// <param name="op">Op.</param>
		public void UnsetArchiveOption(uint op){
			this.handle.options &= ~op;
			UpdateAndSync (false);
		}

		/// <summary>
		/// Initializes a new  <see cref="LibTelltale.TTArchives.TTArchive2"/>. The game ID should be the game ID from the github page, and will throw an exception in the case that the game for the ID
		/// could not be found.
		/// </summary>
		/// <param name="gameid">The Game ID</param>
		public TTArchive2(string gameid){
			reference = Native.hTTArchive2_Create ();
			if (this.reference.Equals (IntPtr.Zero))
				throw new LibTelltaleException ("Could not create backend archive");	
			this.UpdateAndSync (true);
			IntPtr key = Marshal.StringToHGlobalAnsi (gameid);
			this.handle.game_key = Config0.LibTelltale_GetKey (key);
			if (this.handle.game_key.Equals (IntPtr.Zero))
				throw new LibTelltaleException (String.Format("Could not find a key for the game ID {0}", gameid));
			Marshal.FreeHGlobal (key);
			this.gameID = gameid;
			this.SetArchiveVersion (Config.GetGameArchiveVersion (Config.GetGameFlags (gameid)));
			this.UpdateAndSync (false);
		}

		/// <summary>
		/// Opens a readable byte stream of the given entry.
		/// </summary>
		/// <returns>The open.</returns>
		/// <param name="entry">Entry.</param>
		public ByteStream StreamOpen(TTArchive2Entry entry){
			return new ByteStream (Native.TTArchive2_StreamOpen (this.reference, entry.reference));
		}

		/// <summary>
		/// Shortcut to get the name string of the entry.
		/// </summary>
		public string GetEntryName(TTArchive2Entry entry){
			return entry.backend.name;
		}

		/// <summary>
		/// Finds an entry by its name
		/// </summary>
		/// <returns>The entry.</returns>
		/// <param name="name">Name.</param>
		public TTArchive2Entry? FindEntry(string name){
			TTArchive2Entry r = new TTArchive2Entry ();	
			IntPtr entryp = Native.TTArchive2_EntryFind(reference, name);
			if (entryp.Equals (IntPtr.Zero))
				return null;
			r.backend = (_TTArchive2Entry)Marshal.PtrToStructure (entryp, typeof(_TTArchive2Entry));
			r.reference = entryp;
			return r;
		}

		/// <summary>
		/// Sets the input byte stream for the given entry.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="stream">Stream.</param>
		public void SetStreamForEntry(TTArchive2Entry entry, ByteStream stream){
			Native.TTArchive2_StreamSet (entry.reference, stream.reference);
			entry.backend = (_TTArchive2Entry)Marshal.PtrToStructure (entry.reference, typeof(_TTArchive2Entry));
		}

		/// <summary>
		/// Adds an entry to the archive
		/// </summary>
		/// <param name="entry">Entry.</param>
		public void AddEntry(TTArchive2Entry entry){
			Native.hTTArchive2_EntryAdd (this.reference, entry.reference);
			UpdateAndSync (true);
		}

		/// <summary>
		/// Gets an entry by its index in the archive entries backend list. Useful for iterating over all entries. It is nullable.
		/// </summary>
		/// <returns>The entry.</returns>
		/// <param name="index">Index.</param>
		public TTArchive2Entry? GetEntry(uint index){
			TTArchive2Entry r = new TTArchive2Entry ();
			IntPtr entryp = Native.hTTArchive2_GetEntryAt (reference, index);
			if (entryp.Equals (IntPtr.Zero))
				return null;
			r.backend = (_TTArchive2Entry)Marshal.PtrToStructure (entryp, typeof(_TTArchive2Entry));
			r.reference = entryp;
			return r;
		}

		/// <summary>
		/// Clears the entries in this archive.
		/// </summary>
		public void ClearEntries(){
			Native.hTTArchive2_ClearEntries (this.reference);
			UpdateAndSync (true);
		}

		/// <summary>
		/// Writes all entries with the options set in this archive to the OutStream as a valid .ttarch2.
		/// </summary>
		public int Flush(){
			if (outstream == null)
				throw new LibTelltaleException ("No stream set");
			int ret = Native.TTArchive2_Flush (this.reference,IntPtr.Zero);
			if (ret == Constants.TTARCH_FLUSH_OK) {
				UpdateAndSync (true);
			}
			return ret;
		}

		/// <summary>
		/// Releases all resources used by this <see cref="LibTelltale.TTArchives.TTArchive2"/> object.
		/// This also frees all memory within the backend TTArchive and all its entries so make sure you call this after you need every entry otherwise you will get unmanaged memory errors.
		/// </summary>
		public void Dispose(){
			Native.TTArchive2_Free (this.reference);
			Native.hTTArchive2_Delete (this.reference);
			this.instream = null;
			this.outstream = null;
		}

		/// <summary>
		/// Opens and adds all entries from the given .ttarch archive input stream. This also sets the options, and allows once the outstream is set for you to write back the archive to a stream.
		/// </summary>
		public int Open(){
			if (instream == null)
				throw new LibTelltaleException ("No stream set");
			int ret = Native.TTArchive2_Open (this.reference);
			if (ret == Constants.TTARCH_OPEN_OK) {
				UpdateAndSync (true);
			}
			return ret;
		}

		/// <summary>
		/// Gets the amount of entries in this archive.
		/// </summary>
		/// <returns>The entry count.</returns>
		public uint GetEntryCount(){
			return Native.hTTArchive2_GetEntryCount (reference);
		}

		/// <summary>
		/// Gets or sets the input stream to read the archive from in Open.
		/// </summary>
		/// <value>The in stream.</value>
		public ByteStream InStream {
			get{ return instream; }
			set { instream = value;  if(instream != null)this.handle.stream = instream.reference;  if(instream != null)UpdateAndSync (false); }
		}

		/// <summary>
		/// Gets or sets the output stream to write the archive to in Flush.
		/// </summary>
		/// <value>The out stream.</value>
		public ByteOutStream OutStream {
			get{ return outstream; }
			set { outstream = value; if(outstream != null)this.handle.flushstream = outstream.reference;  if(outstream != null)UpdateAndSync (false);}
		}

		protected void UpdateAndSync(bool retrieve){
			if (retrieve) {
				this.handle = (ttarch2)Marshal.PtrToStructure (reference, typeof(ttarch2));
			} else {
				Marshal.StructureToPtr (this.handle, reference, false);
			}
		}

	}

}