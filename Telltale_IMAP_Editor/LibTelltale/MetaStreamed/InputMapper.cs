using System;
using System.Runtime.InteropServices;
using LibTelltaleNative;
using LibTelltale;

namespace LibTelltale
{
	/// <summary>
	/// Represents an Input Mapper (.imap). These files map key bindings to lua script functions for different scenes in telltale episodes.
	/// </summary>
	public sealed class InputMapper : AbstractMetaStreamedFile {

		/// <summary>
		/// All input codes currently known and supported. If there are any which you find that cause errors report on the github. 
		/// </summary>
		public enum InputCode {
			BACKSPACE = 8,
			NUM_0 = 48,
			NUM_1 = 49,
			NUM_2 = 50,
			NUM_3 = 51,
			NUM_4 = 52,
			NUM_5 = 53,
			NUM_6 = 54,
			NUM_7 = 55,
			NUM_8 = 56,
			NUM_9 = 57,
			BUTTON_A = 512,
			BUTTON_B = 513,
			BUTTON_X = 514,
			BUTTON_Y = 515,
			BUTTON_L = 516,
			BUTTON_R = 517,
			BUTTON_BACK = 518,
			BUTTON_START = 519,
			TRIGGER_L = 520,
			TRIGGER_R = 521,
			DPAD_UP = 524,
			DPAD_DOWN = 525,
			DPAD_RIGHT = 526,
			DPAD_LEFT = 527,
			CONFIRM = 768,
			CANCEL = 769,
			MOUSE_MIDDLE = 770,
			MOUSE_LEFT_DOUBLE = 772,
			MOUSE_MOVE = 784,
			ROLLOVER = 800,
			WHEEL_UP = 801,
			WHEEL_DOWN = 802,
			MOUSE_LEFT = 816,
			MOUSE_RIGHT = 817,
			LEFT_STICK = 1024,
			RIGHT_STICK = 1025,
			TRIGGER = 1026,
			TRIGGER_LEFT_MOVE = 1027,
			TRIGGER_RIGHT_MOVE = 1028,
			SWIPE_LEFT = 1296,
			SWIPE_RIGHT = 1298,
			DOUBLE_TAP_SCREEN = 1304,
			SHIFT = 16,
			ENTER = 13,
			ESCAPE = 27,
			LEFT_ARROW = 37,
			RIGHT_ARROW = 39,
			DOWN_ARROW = 40,
			UP_ARROW = 38,
			KEY_A = 65,
			KEY_B = 66,
			KEY_C = 67,
			KEY_D = 68,
			KEY_E = 69,
			KEY_F = 70,
			KEY_G = 71,
			KEY_H = 72,
			KEY_I = 73,
			KEY_J = 74,
			KEY_K = 75,
			KEY_L = 76,
			KEY_M = 77,
			KEY_N = 78,
			KEY_O = 79,
			KEY_P = 80,
			KEY_Q = 81,
			KEY_R = 82,
			KEY_S = 83,
			KEY_T = 84,
			KEY_U = 85,
			KEY_V = 86,
			KEY_W = 87,
			KEY_X = 88,
			KEY_Y = 89,
			KEY_Z = 90,
		};

		/// <summary>
		/// An event mapping event. This says if the event is when a key is pressed/clicked (BEGIN), or whether its when its released (END).
		/// </summary>
		public enum Event {
			BEGIN = 0,
			END = 1
		};

		/// <summary>
		/// An event mapping, which are all contained in a backend list vector of an InputMapper. See the mMapping field for the backend structure in memory, which contains the values. Use
		/// the static methods to set the function names.
		/// </summary>
		public struct EventMapping {
			public _EventMapping mMapping;
			public IntPtr reference;
		}

		/// <summary>
		/// The backend event mapping in memory. Do not edit these name (use static methods in the input mapper class). If you edit the fields, once done call the static 
		/// method UpdateEventMapping to update it in memory for when you rewrite it.
		/// </summary>
		public struct _EventMapping {
			public InputCode mInputCode;
			public Event mEvent;
			public IntPtr mScriptFunction;
			public int mControllerIndexOverride;
		}

		/// <summary>
		/// Updates the event mapping after you have edited any of the fields in it (apart from set script function).
		/// </summary>
		public static void UpdateEventMapping(EventMapping mapping){
			Marshal.StructureToPtr (mapping.mMapping, mapping.reference, false);
		}

		/// <summary>
		/// Gets the script function as a string from the given event mapping.
		/// </summary>
		public static string GetScriptFunction(EventMapping mapping){
			return Marshal.PtrToStringAnsi (mapping.mMapping.mScriptFunction);
		}

		/// <summary>
		/// Sets the script function for the given mapping.
		/// </summary>
		public static void SetScriptFunction(EventMapping mapping, string func){
			IntPtr ptr = Marshal.StringToHGlobalAnsi (func);
			Native.InputMapper_SetEntryName(mapping.reference, ptr);
			Marshal.FreeHGlobal (ptr);
			mapping.mMapping = (_EventMapping)Marshal.PtrToStructure(mapping.reference, typeof(_EventMapping));
		}

		/// <summary>
		/// Updates the memory so the library can know the changes you have made to the _EventMapping struct (you dont need to call this when you have used SetScriptFunction though)
		/// </summary>
		/// <param name="mapping"></param>
		public static void UpdateMapping(EventMapping mapping)
        {
			Marshal.StructureToPtr(mapping.mMapping, mapping.reference, false);
		}

		/// <summary>
		/// Creates an event mapping with all of the data as the parameters.
		/// </summary>
		public static EventMapping CreateMapping(string function, InputCode inputCode, Event ev, int mControllerIndexOverride){
			EventMapping mapping = new EventMapping ();
			mapping.reference = Native.hInputMapping_CreateMapping ();
			mapping.mMapping = new _EventMapping ();
			mapping.mMapping.mScriptFunction = IntPtr.Zero;
			mapping.mMapping.mControllerIndexOverride = mControllerIndexOverride;
			mapping.mMapping.mEvent = ev;
			mapping.mMapping.mInputCode = inputCode;
			IntPtr ptr = Marshal.StringToHGlobalAnsi (function);
			mapping.mMapping.mScriptFunction = Native.hMemory_CreateArray (ptr);
			Marshal.FreeHGlobal (ptr);
			Marshal.StructureToPtr (mapping.mMapping, mapping.reference, false);
			return mapping;
		}

		protected IntPtr reference;

		protected TTContext context;

		/// <summary>
		/// Creates a new input mapper, ready to be edited and opened using Open.
		/// </summary>
		public InputMapper(TTContext context){
			this.reference = Native.hInputMapper_Create ();
			this.context = context;
		}

		public override TTContext GetContext(){
			return this.context;
		}

		public override IntPtr Internal_Get(){
			return this.reference;
		}

		public override bool Flush(){
			return Native.InputMapper_Flush (context.Internal_Get (), this.reference);
		}

		public override int Open(){
			return Native.InputMapper_Open (context.Internal_Get (), this.reference);
		}

		/// <summary>
		/// Deletes this Input Mapper and all backend memory associated.
		/// </summary>
		public override void Dispose(){
			Native.hInputMapper_Delete (this.reference);
		}

		protected IntPtr Mappings(){
			return Native.hInputMapper_Mappings (this.reference);
		}

		/// <summary>
		/// Gets the amount of mappings in this input mapper.
		/// </summary>
		public uint GetMappings(){
			return (uint)Native.InputMapper_DCArray_Size (this.Mappings());
		}

		/// <summary>
		/// Clears the mappings of this input mapper.
		/// </summary>
		public void ClearMappings(){
			Native.InputMapper_DCArray_Clear (this.Mappings ());
		}

		/// <summary>
		/// Used for iteration to get the mapping at the specified index.
		/// </summary>
		public EventMapping GetMapping(uint index){
			EventMapping mapping = new EventMapping ();
			mapping.reference = Native.InputMapper_DCArray_At (this.Mappings (), (int)index);
			mapping.mMapping = ( _EventMapping)Marshal.PtrToStructure (mapping.reference, typeof(_EventMapping));
			return mapping;
		}

		/// <summary>
		/// Frees and deletes a given mapping. Mostly this is called when you remove a mapping, but this is useful to get rid of all memory in the event mapping (mainly the script function char*)
		/// </summary>
		public void FreeAndDeleteMapping(EventMapping mapping){
			Native.hInputMapperMapping_Delete (mapping.reference);
		}

		/// <summary>
		/// Adds a new mapping.
		/// </summary>
		public void AddMapping(EventMapping mapping){
			Native.InputMapper_DCArray_Add (this.Mappings(), mapping.reference);
		}

		/// <summary>
		/// Removes a mapping.
		/// </summary>
		public void RemoveMapping(EventMapping mapping){
			Native.InputMapper_DCArray_Remove (this.Mappings(), mapping.reference);
			this.FreeAndDeleteMapping (mapping);
		}

	}
}

