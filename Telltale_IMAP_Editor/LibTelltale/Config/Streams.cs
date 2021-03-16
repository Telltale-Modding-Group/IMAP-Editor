using System;
using System.Runtime.InteropServices;
using LibTelltaleNative;
using LibTelltale;

namespace LibTelltale
{
	/// <summary>
	/// An input stream of bytes which can be read from and positioned.
	/// </summary>
	public sealed class ByteStream : IDisposable {

		public void Dispose(){
			Native.hByteStream_Delete (this.reference);
		}

		/// <summary>
		/// The reference, do not touch!
		/// </summary>
		public readonly IntPtr reference = IntPtr.Zero;

		/// <summary>
		/// Used internally to create from the given stream pointer.
		/// </summary>
		/// <param name="r">The red component.</param>
		public ByteStream(IntPtr r){
			this.reference = r;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteStream"/> class. The filepath is the existing file location of the file to read from.
		/// </summary>
		/// <param name="filepath">Filepath.</param>
		public ByteStream(string filepath){
			this.reference = Native.hFileStream_Create (filepath);
			if (this.reference.Equals (IntPtr.Zero) || !IsValid())
				throw new LibTelltaleException ("Could not create backend filestream");	
		}

		/// <summary>
		/// Determines whether this instance is valid, and if it is not then the archive will return a stream error. This is if the backend byte buffer is non existent or the file does not exist.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
		public bool IsValid(){
			return Native.hByteStream_Valid (reference) != 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteStream"/> class, with the given size of zeros. Useful if you need an empty stream.
		/// </summary>
		/// <param name="initalSize">Inital size.</param>
		public ByteStream(uint initalSize){
			this.reference = Native.hByteStream_Create (initalSize);
			if (this.reference.Equals (IntPtr.Zero))
				throw new LibTelltaleException ("Could not create backend stream");	
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteStream"/> class, which reads from the given input byte buffer. Do not set the byte array to null until you are done with this stream.
		/// </summary>
		/// <param name="buffer">Buffer.</param>
		public ByteStream(byte[] buffer){
			IntPtr ptr = Marshal.AllocHGlobal (buffer.Length);
			Marshal.Copy (buffer, 0, ptr, buffer.Length);
			this.reference = Native.hByteStream_CreateFromBuffer (ptr, (uint)buffer.Length);
			if (this.reference.Equals (IntPtr.Zero))
				throw new LibTelltaleException ("Could not create backend stream");	
		}

		/// <summary>
		/// Reads a single byte.
		/// </summary>
		/// <returns>The single byte.</returns>
		public byte ReadSingleByte(){
			return Native.hByteStream_ReadByte (reference);
		}

		/// <summary>
		/// Gets the backend buffer this stream is reading from, or null if there is none (for example if its reading from a file).
		/// </summary>
		/// <returns>The buffer.</returns>
		public byte[] GetBuffer(){
			IntPtr ptr = Native.hByteStream_GetBuffer (reference);
			if (ptr.Equals (IntPtr.Zero))
				return null;
			byte[] ret = new byte[GetSize ()];
			Marshal.Copy (ptr,ret, 0, (int)GetSize ());
			//Dont want to free the buffer!!
			return ret;
		}

		/// <summary>
		/// Read the specified amount of bytes, and increases the position by length. Returns a byte struct which allows this memory to later be freed once your done with it.
		/// </summary>
		/// <param name="length">Length.</param>
		public MemoryHelper.Bytes? Read(int length){
			IntPtr read = Native.hByteStream_ReadBytes (reference, (uint)length);
			if (read.Equals (IntPtr.Zero))
				return null;
			byte[] ret1 = new byte[length];
			Marshal.Copy (read, ret1, 0, length);
			MemoryHelper.Bytes ret = new MemoryHelper.Bytes ();
			ret.bytes = ret1;
			ret.mem = read;
			return ret;
		}

		/// <summary>
		/// Reads an unsigned integer of the specified bit width. Valid widths are <= 64 and a multiple of eight and >= 8.
		/// </summary>
		public ulong ReadInt(byte bit_width){
			return Native.hByteStream_ReadInt(reference, bit_width);
		}

		/// <summary>
		/// Reads a null terminated string.
		/// </summary>
		/// <returns>The null terminated string.</returns>
		public string ReadNullTerminatedString(){
			return Marshal.PtrToStringAnsi (Native.hByteStream_ReadString0 (reference));
		} 

		/// <summary>
		/// Reads an ASCII string of the given byte length
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="len">Length.</param>
		public string ReadString(uint len){
			return Marshal.PtrToStringAnsi(Native.hByteStream_ReadString(reference, len));
		}

		/// <summary>
		/// Determines whether this instance is little endian.
		/// </summary>
		/// <returns><c>true</c> if this instance is little endian; otherwise, <c>false</c>.</returns>
		public bool IsLittleEndian(){
			return Native.hByteStream_IsLittleEndian (reference) != 0;
		}

		/// <summary>
		/// Positions to the specified offset.
		/// </summary>
		/// <param name="offset">Offset.</param>
		public void Position(ulong offset){
			Native.hByteStream_Position (reference, offset);
		}

		/// <summary>
		/// Sets the endianness of the stream to read ints by.
		/// </summary>
		/// <param name="little">If set to <c>true</c> little.</param>
		public void SetEndian(bool little){
			Native.hByteStream_SetEndian (reference, little);
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <returns>The position.</returns>
		public ulong GetPosition(){
			return Native.hByteStream_GetPosition (reference);
		}

		/// <summary>
		/// Gets the size of this stream.
		/// </summary>
		/// <returns>The size.</returns>
		public int GetSize(){
			return (int)Native.hByteStream_GetSize (reference);
		}

	}

	/// <summary>
	/// An output stream of bytes
	/// </summary>
	public sealed class ByteOutStream : IDisposable {

		//Do not touch this!
		public readonly IntPtr reference = IntPtr.Zero;

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteOutStream"/> class, which writes to the passed in file which doesnt have to exist. However its directory must exist of undefined behaviour!
		/// </summary>
		/// <param name="filepath">Filepath.</param>
		public ByteOutStream(string filepath){
			this.reference = Native.hFileOutStream_Create (filepath);
			if (this.reference.Equals (IntPtr.Zero) || !IsValid())
				throw new LibTelltaleException ("Could not create backend filestream");	
		}

		/// <summary>
		/// Internal use
		/// </summary>
		/// <param name="ptr">Ptr.</param>
		public ByteOutStream(IntPtr ptr){
			this.reference = ptr;
		}

		/// <summary>
		/// Determines whether this stream is valid.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
		public bool IsValid(){
			return Native.hByteOutStream_Valid (reference) != 0;
		}

		/// <summary>
		/// Writes an unsigned int to this stream. You will have to cast to a ulong on calling if not a uint64!
		/// </summary>
		/// <param name="bit_width">Bit width.</param>
		/// <param name="num">Number.</param>
		public void WriteInt(byte bit_width, ulong num){
			Native.hByteOutStream_WriteInt (reference, bit_width, num);
		}

		/// <summary>
		/// Write the specified buffer to this stream.
		/// </summary>
		/// <param name="buf">Buffer.</param>
		public void Write(byte[] buf){
			IntPtr ptr = Native.hMemory_Alloc ((uint)buf.Length);
			Marshal.Copy (buf, 0, ptr, buf.Length);
			Native.hByteOutStream_WriteBytes (reference, ptr, (uint)buf.Length);
			Native.hMemory_Free (ptr);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteOutStream"/> class, with the initial buffer size to save time reallocating the memory.
		/// </summary>
		/// <param name="initalSize">Inital size.</param>
		public ByteOutStream(uint initalSize){
			this.reference = Native.hByteOutStream_Create (initalSize);
			if (this.reference.Equals (IntPtr.Zero))
				throw new LibTelltaleException ("Could not create backend stream");	
		}

		/// <summary>
		/// Determines whether this instance is little endian.
		/// </summary>
		/// <returns><c>true</c> if this instance is little endian; otherwise, <c>false</c>.</returns>
		public bool IsLittleEndian(){
			return Native.hByteOutStream_IsLittleEndian (reference) != 0;
		}

		/// <summary>
		/// Gets the backend buffer if this is not writing to a file. This is useful if you are writing to a byte[] and you want to get the output array.
		/// </summary>
		/// <returns>The buffer.</returns>
		public byte[] GetBuffer(){
			IntPtr ptr = Native.hByteOutStream_GetBuffer (reference);
			if (ptr.Equals (IntPtr.Zero))
				return null;
			byte[] ret = new byte[GetSize ()];
			Marshal.Copy (ptr,ret, 0, (int)GetSize ());
			//Dont want to free the buffer!!
			return ret;
		}

		/// <summary>
		/// Position to the specified offset.
		/// </summary>
		/// <param name="offset">Offset.</param>
		public void Position(ulong offset){
			Native.hByteOutStream_Position (reference, offset);
		}

		/// <summary>
		/// Sets the endian.
		/// </summary>
		public void SetEndian(bool little){
			Native.hByteOutStream_SetEndian (reference, little);
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <returns>The position.</returns>
		public ulong GetPosition(){
			return Native.hByteOutStream_GetPosition (reference);
		}

		public void Dispose(){
			Native.hByteOutStream_Delete (this.reference);
		}

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <returns>The size.</returns>
		public ulong GetSize(){
			return Native.hByteOutStream_GetSize (reference);
		}

	}
}

