using System;
using System.Runtime.InteropServices;

namespace LibTelltaleNative
{

	/// <summary>
	/// Native access to the telltale library DLL. This is a private class because it is used internally.
	/// </summary>
	class Native {



		[DllImport("LibTelltale.dll")]
		public static extern IntPtr PropertySet_NewKeyInfo (ulong crc);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr Abstract_DCArray_At(IntPtr buf, int index);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr Scene_Create();

		[DllImport("LibTelltale.dll")]
		public static extern void Scene_Free(IntPtr scene);

		[DllImport("LibTelltale.dll")]
		public static extern bool Scene_Flush(IntPtr ctx, IntPtr scene);

		[DllImport("LibTelltale.dll")]
		public static extern int Scene_Open(IntPtr ctx, IntPtr scene);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr Abstract_DCArray_Create();

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr Buffer_DCArray_Create();

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr AgentMap_CreateAgentEntry();

		[DllImport("LibTelltale.dll")]
		public static extern void Abstract_DCArray_Remove(IntPtr buf, IntPtr Buffer);

		[DllImport("LibTelltale.dll")]
		public static extern void Abstract_DCArray_Add(IntPtr buf, IntPtr buffer);

		[DllImport("LibTelltale.dll")]
		public static extern int Abstract_DCArray_Size(IntPtr buf);

		[DllImport("LibTelltale.dll")]
		public static extern void Abstract_DCArray_Delete(IntPtr rptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hMemory_Delete(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void Abstract_DCArray_Clear(IntPtr buf);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr PropertySetMap_DCArray_At(IntPtr buf, int index);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr PropertySetMap_DCArray_Create();

		[DllImport("LibTelltale.dll")]
		public static extern void PropertySetMap_DCArray_Remove(IntPtr buf, IntPtr Buffer);

		[DllImport("LibTelltale.dll")]
		public static extern void PropertySetMap_DCArray_Add(IntPtr buf, IntPtr buffer);

		[DllImport("LibTelltale.dll")]
		public static extern int PropertySetMap_DCArray_Size(IntPtr buf);

		[DllImport("LibTelltale.dll")]
		public static extern void PropertySetMap_DCArray_Clear(IntPtr buf);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr PropertySet_DCArray_At (IntPtr buf, int index);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr PropertySet_DCArray_Create();

		[DllImport("LibTelltale.dll")]
		public static extern void InputMapper_SetEntryName(IntPtr ptr, IntPtr ptr2);

		[DllImport("LibTelltale.dll")]
		public static extern void PropertySet_DCArray_Remove(IntPtr buf, IntPtr Buffer);

		[DllImport("LibTelltale.dll")]
		public static extern void PropertySet_DCArray_Add (IntPtr buf, IntPtr buffer);

		[DllImport("LibTelltale.dll")]
		public static extern int PropertySet_DCArray_Size(IntPtr buf);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr PropertySet_Create_FontConf();

		[DllImport("LibTelltale.dll")]
		public static extern void PropertySet_Delete_FontConf(IntPtr p);

		[DllImport("LibTelltale.dll")]
		public static extern void PropertySet_DCArray_Clear (IntPtr buf);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hPropertySetMap_CreateEntry();

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hPropertySet_GetTypeName(IntPtr buf, byte full);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr Buffer_DCArray_At (IntPtr buf, int index);


		[DllImport("LibTelltale.dll")]
		public static extern IntPtr ActorAgentMap_Create();

		[DllImport("LibTelltale.dll")]
		public static extern void ActorAgentMap_Delete(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern bool ActorAgentMap_Flush(IntPtr ctx, IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern int ActorAgentMap_Open(IntPtr ctx, IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void AgentMap_DeleteAgentEntry(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr AgentMap_Create();

		[DllImport("LibTelltale.dll")]
		public static extern void AgentMap_Delete(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern int AgentMap_Open(IntPtr ctx, IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern bool AgentMap_Flush(IntPtr ctx, IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void Buffer_DCArray_Remove(IntPtr buf, IntPtr Buffer);

		[DllImport("LibTelltale.dll")]
		public static extern void Buffer_DCArray_Add (IntPtr buf, IntPtr buffer);

		[DllImport("LibTelltale.dll")]
		public static extern int Buffer_DCArray_Size(IntPtr buf);

		[DllImport("LibTelltale.dll")]
		public static extern void Buffer_DCArray_Clear (IntPtr buf);

		[DllImport("LibTelltale.dll")]
		public static extern void hPropertySet_SetVersion (IntPtr buf, int ver);

		[DllImport("LibTelltale.dll")]
		public static extern void hPropertySet_SetFlags (IntPtr buf, uint flags);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hPropertySet_Create();

		[DllImport("LibTelltale.dll")]
		public static extern void hPropertySet_Delete (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern int PropertySet_Open (IntPtr ctx,IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern byte PropertySet_Flush (IntPtr ctx, IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hMetaStream_CreateDesc();

		[DllImport("LibTelltale.dll")]
		public static extern uint hPropertySet_GetFlags (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hPropertySet_GetParents (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hPropertySet_GetKeyMap (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hPropertySet_CreateEntry ();

		[DllImport("LibTelltale.dll")]
		public static extern int hPropertySet_GetVersion (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern ulong ClassToCRC (IntPtr clazz);

		[DllImport("LibTelltale.dll")]
		public static extern void hPropertySet_DeleteEntry (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern uint hTTArchiveOrTTArchive2_Delete(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTArchiveOrTTArchive2_Create();

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hInputMapping_CreateMapping();

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hMemory_CreateArray(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern int InputMapper_DCArray_Size (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hMemory_FreeArray (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void InputMapper_DCArray_Clear (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTContext_NextGame(IntPtr ptr, IntPtr file);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr InputMapper_DCArray_At(IntPtr ptr, int index);

		[DllImport("LibTelltale.dll")]
		public static extern void InputMapper_DCArray_Add (IntPtr ptr, IntPtr desc_);

		[DllImport("LibTelltale.dll")]
		public static extern void InputMapper_DCArray_Remove (IntPtr ptr, IntPtr desc_);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hInputMapper_Mappings (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hInputMapper_Delete(IntPtr imap);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hInputMapper_Create();

		[DllImport("LibTelltale.dll")]
		public static extern bool InputMapper_Flush (IntPtr ctx, IntPtr imap);

		[DllImport("LibTelltale.dll")]
		public static extern int InputMapper_Open (IntPtr ctx, IntPtr imap);

		[DllImport("LibTelltale.dll")]
		public static extern int MetaStreamClasses_DCArray_Size (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void MetaStreamClasses_DCArray_Clear (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr MetaStreamClasses_DCArray_At(IntPtr ptr, int index);

		[DllImport("LibTelltale.dll")]
		public static extern void MetaStreamClasses_DCArray_Add (IntPtr ptr, IntPtr desc_);

		[DllImport("LibTelltale.dll")]
		public static extern void MetaStreamClasses_DCArray_Remove (IntPtr ptr, IntPtr desc_);

		[DllImport("LibTelltale.dll")]
		public static extern int VersBlocks_DCArray_Size (IntPtr dcarray);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr VersBlocks_DCArray_At(IntPtr dcarray, int index);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTContext_Create(IntPtr archive, IntPtr f);

		[DllImport("LibTelltale.dll")]
		public static extern byte hTTContext_NextStream (IntPtr ctx, IntPtr strm, bool del);

		[DllImport("LibTelltale.dll")]
		public static extern uint hTTContext_FileStart (IntPtr strm);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTContext_CurrentStream (IntPtr strm);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTContext_CurrentMeta(IntPtr strm);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTContext_FindArchiveEntry(IntPtr ctx, ulong crc64);

		[DllImport("LibTelltale.dll")]
		public static extern void hInputMapperMapping_Delete(IntPtr mapping);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTContext_NextArchive (IntPtr prop, IntPtr archive, bool del);

		[DllImport("LibTelltale.dll")]
		public static extern byte hTTContext_NextWrite (IntPtr p, IntPtr strm, [MarshalAs (UnmanagedType.LPStr)] string fileName, bool del);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTContext_OverrideMeta(IntPtr p, IntPtr meta, bool del);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTContext_CurrentOutStream(IntPtr ctx);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTContext_CurrentFile (IntPtr ctx);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTContext_OpenStream(IntPtr strm, [MarshalAs(UnmanagedType.LPStr)] string entryName);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hMetaStream_GetClasses (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hMetaStream_SetVersion(IntPtr stream, uint Version);

		[DllImport("LibTelltale.dll")]
		public static extern void hMetaStream_SetFlags (IntPtr stream, uint flags);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hMetaStream_Create ();

		[DllImport("LibTelltale.dll")]
		public static extern byte hMetaStream_Open (IntPtr meta, IntPtr stream);

		[DllImport("LibTelltale.dll")]
		public static extern void hMetaStream_Close (IntPtr meta);

		[DllImport("LibTelltale.dll")]
		public static extern uint hMetaStream_GetVersion (IntPtr m);

		[DllImport("LibTelltale.dll")]
		public static extern uint hMetaStream_GetFlags(IntPtr m);

		[DllImport("LibTelltale.dll")]
		public static extern uint hMetaStream_GetPayloadSize (IntPtr meta);

		[DllImport("LibTelltale.dll")]
		public static extern uint hMetaStream_GetTextureSize(IntPtr meta);

		[DllImport("LibTelltale.dll")]
		public static extern uint hMetaStream_GetClassVersion (IntPtr meta, [MarshalAs (UnmanagedType.LPStr)] string typeName, uint default_value);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTContext_FinishWrite (IntPtr p, bool del, bool update);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTContext_NextArchive (IntPtr ctx, IntPtr archive);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr Vers_Create (IntPtr ctx);

		[DllImport("LibTelltale.dll")]
		public static extern int Vers_Open (IntPtr vers);

		[DllImport("LibTelltale.dll")]
		public static extern byte Vers_Flush (IntPtr vers, IntPtr wctx);

		[DllImport("LibTelltale.dll")]
		public static extern void Vers_Free(IntPtr vers);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive_EntryRemove (IntPtr a, IntPtr b, bool c);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive2_EntryRemove (IntPtr a, IntPtr b, bool c);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive_EntrySetName (IntPtr a, [MarshalAs(UnmanagedType.LPStr)] string name);

		[DllImport("LibTelltale.dll")]
		public static extern int TTArchive2_Open (IntPtr a);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive2_StreamOpen (IntPtr a, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive2_Free (IntPtr a);

		[DllImport("LibTelltale.dll")]
		public static extern int TTArchive2_Flush (IntPtr a,IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive2_StreamSet (IntPtr a, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive2_EntrySetName (IntPtr a, [MarshalAs(UnmanagedType.LPStr)] string name);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive2_EntryFind (IntPtr a, [MarshalAs(UnmanagedType.LPStr)] string name);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive2_EntryCreate ([MarshalAs(UnmanagedType.LPStr)] string name, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive_StreamOpen(IntPtr a, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTContext_Delete(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hMetaStream_Delete(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive_Delete (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive2_Delete (IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteStream_Delete(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteOutStream_Delete(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern int TTArchive_Open (IntPtr a);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive_Free (IntPtr a);

		[DllImport("LibTelltale.dll")]
		public static extern int TTArchive_Flush(IntPtr a, IntPtr func);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive_EntryCreate([MarshalAs(UnmanagedType.LPStr)] string name, IntPtr strm);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive_StreamSet (IntPtr a, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive_EntryFind (IntPtr a, [MarshalAs (UnmanagedType.LPStr)] string name);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteStream_Position(IntPtr stream, ulong off);

		[DllImport("LibTelltale.dll")]
		public static extern byte hByteStream_IsLittleEndian(IntPtr stream);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteStream_SetEndian(IntPtr s, bool little_endian);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteStream_ReadInt(IntPtr s, uint bitwidth);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteStream_GetPosition(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteStream_GetSize(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_ReadBytes(IntPtr s, uint size);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive2_EntryAdd(IntPtr p, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern void hMetaStream_Flush (IntPtr c, IntPtr str);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive_EntryAdd(IntPtr p, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern byte hByteStream_ReadByte(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_Create (uint size);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_CreateFromBuffer (IntPtr p, uint size);

		[DllImport("LibTelltale.dll") ]
		public static extern IntPtr hByteStream_ReadString(IntPtr s, uint len);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_ReadString0(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hFileStream_Create([MarshalAs(UnmanagedType.LPStr)] string filepath);

		[DllImport("LibTelltale.dll")]
		public static extern byte hByteStream_Valid(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern void hMemory_Free(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hMemory_Alloc(uint size);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hMemory_AllocNew(int size);

		[DllImport("LibTelltale.dll")]
		public static extern byte hByteOutStream_Valid(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern uint hTTArchive2_GetEntryCount(IntPtr archive);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTArchive2_GetEntryAt (IntPtr archive, uint index);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTArchive_GetEntryAt (IntPtr archive, uint index);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive2_ClearEntries (IntPtr archive);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteOutStream_Position(IntPtr stream, ulong off);

		[DllImport("LibTelltale.dll")]
		public static extern byte hByteOutStream_IsLittleEndian(IntPtr stream);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteOutStream_SetEndian(IntPtr s, bool little_endian);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteOutStream_WriteInt (IntPtr s, uint bitwidth, ulong num);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteOutStream_GetPosition(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteOutStream_GetBuffer(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_GetBuffer(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteOutStream_GetSize(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteOutStream_WriteBytes(IntPtr s, IntPtr buf, uint size);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteOutStream_Create (uint size);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hFileOutStream_Create([MarshalAs(UnmanagedType.LPStr)] string filepath);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive_ClearEntries (IntPtr p);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTArchive2_Create ();

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTArchive_Create ();

		[DllImport("LibTelltale.dll")]
		public static extern uint hTTArchive_GetEntryCount(IntPtr archive);

	}

	class Config0
	{

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr Handle_Create(IntPtr n, ulong crc);

		[DllImport("LibTelltale.dll")]
		public static extern void LibTelltale_FreeLib();

		[DllImport("LibTelltale.dll")]
		public static extern void Handle_Delete(IntPtr ptr);

		[DllImport("LibTelltale.dll")]
		public static extern uint LibTelltale_GetGameMetaVersion(IntPtr game, byte save);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr LibTelltale_GetLastUnknownCRC();

		[DllImport("LibTelltale.dll")]
		public static extern ulong LibTelltale_GetGameFlags([MarshalAs(UnmanagedType.LPStr)] string gameid);

		[DllImport("LibTelltale.dll")]
		public static extern void LibTelltale_BlowfishEncrypt(IntPtr data, uint size, byte modified, IntPtr k);

		[DllImport("LibTelltale.dll")]
		public static extern void LibTelltale_BlowfishDecrypt(IntPtr data, uint size, byte modified, IntPtr k);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr LibTelltale_DecryptScript(IntPtr data, uint size, byte modified, IntPtr k);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr LibTelltale_EncryptScript(IntPtr data, uint size, byte modified, IntPtr k, byte islenc);

		[DllImport("LibTelltale.dll")]
		public static extern unsafe IntPtr LibTelltale_DecryptResourceDescription(IntPtr data, uint size, uint* outz, byte modified, IntPtr k);

		[DllImport("LibTelltale.dll")]
		public static extern unsafe IntPtr LibTelltale_EncryptResourceDescription(IntPtr data, uint size, uint* outz, byte modified, IntPtr k, byte islenc);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr LibTelltale_Version();

		[DllImport("LibTelltale.dll")]
		public static extern void LibTelltale_MapLib(IntPtr id, IntPtr name);

		[DllImport("LibTelltale.dll")]
		public static extern void LibTelltale_ClearMappedLibs();

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr LibTelltale_GetKey(IntPtr id);

	}

}

