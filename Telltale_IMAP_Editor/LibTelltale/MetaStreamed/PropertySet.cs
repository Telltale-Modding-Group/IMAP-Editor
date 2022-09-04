using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using LibTelltaleNative;
using LibTelltale;
using System.Collections;

/*
 * At the moment, this library will not open the following (known) .prop sets. Here are the names, followed by its unknown CRC64 if you can figure it out:
 * - scriptoutput.prop										| THIS WILL LIKELY CRASH THE PROGRAM! there are so many property sets in the prop, and the lib gets confused! (i need to fix)
 * - chorecorder_agent_session.prop E6 4A FB F3 3D 2A 04 5F |
 * - chorecorder_session.prop       E6 4A FB F3 3D 2A 04 5F |
 * - project_chorecorder.prop								| i know the type is map<string,chorecorderparameters,less<string>>, but chorecorderparameters? 
 * - material_*.prop likely to not work						| it contains a type called T3MaterialData which is super hard to decipher and is quite useless to edit tbh
 * - test.prop												| contains T3MaterialData
 * - module_animation_constraints							| contain slots of IK which there is no need to support
 * - module_dlgProps_production_exchangeNodes.prop			| F0 AC CB 4E 15 E1 BC CE (int/enum)
 * - Below I haven't seen examples of it with data, so I cant add support yet
 * - any footstep1 banks (module_footsteps2)				| i know the types its just the actual value i dont know yet i havent seen it with data.  foots2 ive seen
 * - module_particleemitter.prop							| dcarray of particlepropconnect? theres probably more after this property; whats particlepropconnect?
 * - project_idle.prop/prefs_idle.prop						| type 'idleslotdefaults' not supported
 * - project_localization.prop								| type 'localization::language' got to many unknown values
 * - project_fonts.prop										| Used to work, but different games have lots of versions and its hard to keep track
 * - platform.prop - TWD DE									| C0 70 29 23 49 47 04 11
 * - project_render_system.prop - TWD DE					| Some type of map<x,y,less<x or y>> 3e9f3ec23c34f712
 * 
 */
namespace LibTelltale
{
	/// <summary>
	/// AnimOrChore, a property type which holds a handle reference to either an animation or chore.
	/// </summary>
	public struct AnimOrChore {
		public Handle mAnim, mChore;
	}

	/// <summary>
	/// Computation parameters for material enlightens.
	/// </summary>
	public struct T3MaterialEnlightenPrecomputeParams
	{
		public float mPrecomputeVisiblity;//About 80% sure
		public int unknown;//even an int? list size oof?
	}

	/// <summary>
	/// A rectangle.
	/// </summary>
	public struct Rect
	{
		public int left, right, top, bottom;
	};

	/// <summary>
	/// A script enum, which is the type of dialog this is (in game)
	/// </summary>
	public static class DialogMode
	{

		public static readonly string AMIBENT = "Ambient";

		public static readonly string CUTSCENE = "Cutscene";

		public static readonly string BG = "Background";

	}

	/// <summary>
	/// Xbox Gamepad buttons
	/// </summary>
	public static class GamepadButton
	{

		public static readonly string A = "A";

		public static readonly string X = "X";

		public static readonly string B = "B";

		public static readonly string Y = "Y";

	}

	/// <summary>
	/// Character light composer rigs source quadrant types
	/// </summary>
	public static class LightComposer_LightSourceQuadrant
	{

		public static readonly string AUTO = "Auto";

		public static readonly string FRONTRIGHT = "FrontRight";

		public static readonly string FRONTLEFT = "FrontLeft";

		public static readonly string BACKRIGHT = "BackRight";

		public static readonly string BACKLEFT = "BackLeft";

	}

	/// <summary>
	/// Constraints for procedural look ats.
	/// </summary>
    public struct ProceduralLookAtConstraint {
		public float minA, maxA;//??
		public float valueB, valueC;
		public int valueD, valueE;
	}

	//Do not edit the array size (add or remove), only edit if you want.
	/// <summary>
	/// A sound event name reference. Its a list since some newer games have 2 not one. Do not edit the size of the list, onyl edit the handles (ie the crc or name) in the list.
	/// </summary>
    public struct SoundEventName {
		public List<Handle> mSoundEvents;
	}

	/// <summary>
	/// Camera zones.
	/// </summary>
	public static class LightComposer_CameraZone
	{

		public static readonly string AUTO = "Auto";

		public static readonly string ZONE_0 = "Zone 0";

		public static readonly string ZONE_1 = "Zone 1";

		public static readonly string ZONE_2 = "Zone 2";

		public static readonly string ZONE_3 = "Zone 3";

		public static readonly string ZONE_4 = "Zone 4";

		public static readonly string ZONE_5 = "Zone 5";

		public static readonly string ZONE_6 = "Zone 6";

		public static readonly string ZONE_7 = "Zone 7";

	}

	/// <summary>
	/// Light composer node locations
	/// </summary>
	public static class LightComposer_NodeLocation {

		public static readonly string HEAD = "Head";

		public static readonly string ROOT = "Root";

	}

	/// <summary>
	/// Light environment internal data.
	/// </summary>
	public struct T3LightEnvInternalData//all are signed
    {
		public int valueA_1, valueA_2;
		public int valueB_1, valueB_2;
		public int valueC_1, valueC_2;
		public int valueD;//looks different from the others.
		public IntPtr reference;
	}

	/// <summary>
	/// A polar type.
	/// </summary>
	public struct Polar
	{
		public float mR, mTheta, mPhi;
	};

	/// <summary>
	/// An RGBA colour
	/// </summary>
	public struct Colour
	{
		public float r, g, b, a;
	};

	/// <summary>
	/// A range (int)
	/// </summary>
	public struct TRangeInt {
		public int min, max;
	}
	/// <summary>
	/// A range (float)
	/// </summary>
	public struct TRangeFloat {
		public float min, max;
	}

	/// <summary>
	/// Text colour styles. No idea on the values i've only seen None. You could try other strings
	/// </summary>
    public static class TextColourStyle {

		public static readonly string NONE = "None";
	
	}

	/// <summary>
	/// A vector with 2 planes
	/// </summary>
	public struct Vector2 {
		public float x;
		public float y;
	}

	/// <summary>
	/// A vector with 3 planes
	/// </summary>
	public struct Vector3
	{
		public float x;
		public float y;
		public float z;
	}

	/// <summary>
	/// A phoneme table key
	/// </summary>
    public struct PhonemeKey {
		public float mFadeInTime;
		public float mHoldTime;
		public float mFadeOutTime;
		public float mTargetContribution;
		public Handle mPhoneme;
	}

	/// <summary>
	/// A particle level of detail key
	/// </summary>
	public struct ParticleLODKey
	{
		//no clue on these, u?
		public float a;
		public float b;
		public float c;
		public float d;
	}

	/// <summary>
	/// A chase forward vector enum. Only seen the player type.
	/// </summary>
    public static class ChaseForwardVector {
		public static readonly string PLAYER = "Player";
	}

	/// <summary>
	/// A quaternion type
	/// </summary>
	public struct Quaternion
	{
		public float x;
		public float y;
		public float z;
		public float w;
	}

	/// <summary>
	/// A vector 4
	/// </summary>
	public struct Vector4
	{
		public float x;
		public float y;
		public float z;
		public float w;
	}

	/// <summary>
	/// Resource group information. Contains an ARGB colour too
	/// </summary>
	public struct ResourceGroupInfo
	{
		public float a;
		public float r;
		public float g;
		public float b;
		public int i;//??
	}

	//Used in scenes mostly, holds data for a meshes/objects location (Im 99% sure)
    public struct LocationInfo {
		public IntPtr reference;
		public string mGroup;//if it has no group, will be an empty
		public Vector3 mRotation;
		public float mScale;
		public Vector3 mPosition;
	}


	/// <summary>
	/// This class holds all of the key types supported (which is about 99% of all of the types there are) in the library.
	/// </summary>
	public static class KeyTypes
	{

		public static readonly KeyTypeHandle<LocationInfo> TYPE_LOCATION_INFO = new KeyTypeHandle<LocationInfo>("locationinfo", (x, old) =>
		{
			if (x.mGroup != null && x.mGroup.Length > 0)
			{
				IntPtr group = Marshal.ReadIntPtr(x.reference);
				if (!group.Equals(IntPtr.Zero))
				{
					Native.hMemory_FreeArray(group);
				}
				Marshal.WriteIntPtr(x.reference, Config.CreateString(x.mGroup));
			}
			Marshal.WriteInt32(x.reference, 16, BitConverter.SingleToInt32Bits(x.mRotation.x));
			Marshal.WriteInt32(x.reference, 20, BitConverter.SingleToInt32Bits(x.mRotation.y));
			Marshal.WriteInt32(x.reference, 24, BitConverter.SingleToInt32Bits(x.mRotation.z));
			Marshal.WriteInt32(x.reference, 28, BitConverter.SingleToInt32Bits(x.mScale));
			Marshal.WriteInt32(x.reference, 32, BitConverter.SingleToInt32Bits(x.mPosition.x));
			Marshal.WriteInt32(x.reference, 36, BitConverter.SingleToInt32Bits(x.mPosition.y));
			Marshal.WriteInt32(x.reference, 40, BitConverter.SingleToInt32Bits(x.mPosition.z));
			return x.reference;
		}, x =>
		{
			LocationInfo inf = new LocationInfo();
			inf.reference = x;
			IntPtr group = Marshal.ReadIntPtr(x);
            if (group.Equals(IntPtr.Zero))
            {
				inf.mGroup = "";
            }
            else
            {
				inf.mGroup = Marshal.PtrToStringAnsi(group);
            }
			inf.mPosition = new Vector3();
			inf.mRotation = new Vector3();
			inf.mRotation.x = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x, 16));
			inf.mRotation.y = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x, 20));
			inf.mRotation.z = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x, 24));
			inf.mScale =	  BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x, 28));
			inf.mPosition.x = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x, 32));
			inf.mPosition.y = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x, 36));
			inf.mPosition.z = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x, 40));
			return inf;
		});

		//dont really need to use this or any delete methods since the lib does that for u
		public static void DeleteInternalLightData(T3LightEnvInternalData e)
		{
			Native.hMemory_Delete(e.reference);
		}

		//use when passing as a value for the lib. dont use delete after that
		public static void CreateInternalLightData(ref T3LightEnvInternalData ret)
		{
			IntPtr ptr = Native.hMemory_AllocNew(28);//sizeof(SoundEventName1)
			Marshal.WriteInt32(a(ptr, 0), ret.valueA_1);
			Marshal.WriteInt32(a(ptr, 4), ret.valueA_2);
			Marshal.WriteInt32(a(ptr, 8), ret.valueB_1);
			Marshal.WriteInt32(a(ptr, 12), ret.valueB_2);
			Marshal.WriteInt32(a(ptr, 16), ret.valueC_1);
			Marshal.WriteInt32(a(ptr, 20), ret.valueC_2);
			Marshal.WriteInt32(a(ptr, 24), ret.valueD);
			ret.reference = ptr;
		}

		public static readonly KeyTypeHandle<T3LightEnvInternalData> TYPE_T3LIGHT_INTERNAL_DATA = new KeyTypeHandle<T3LightEnvInternalData>("t3lightenvinternaldata", (x, old) =>
		{
			IntPtr ptr = x.reference;
			T3LightEnvInternalData ret = x;
			Marshal.WriteInt32(a(ptr, 0), ret.valueA_1);
			Marshal.WriteInt32(a(ptr, 4), ret.valueA_2);
			Marshal.WriteInt32(a(ptr, 8), ret.valueB_1);
			Marshal.WriteInt32(a(ptr, 12), ret.valueB_2);
			Marshal.WriteInt32(a(ptr, 16), ret.valueC_1);
			Marshal.WriteInt32(a(ptr, 20), ret.valueC_2);
			Marshal.WriteInt32(a(ptr, 24), ret.valueD);
			return x.reference;
		}, ptr =>
		{
			T3LightEnvInternalData ret = new T3LightEnvInternalData();
			ret.valueA_1 = Marshal.ReadInt32(a(ptr, 0));
			ret.valueA_2 = Marshal.ReadInt32(a(ptr, 4));
			ret.valueB_1 = Marshal.ReadInt32(a(ptr, 8));
			ret.valueB_2 = Marshal.ReadInt32(a(ptr, 12));
			ret.valueC_1 = Marshal.ReadInt32(a(ptr, 16));
			ret.valueC_2 = Marshal.ReadInt32(a(ptr, 20));
			ret.valueD = Marshal.ReadInt32(a(ptr, 24));
			return ret;
		});

		public static readonly KeyTypeHandle<SoundEventName> TYPE_SOUNDEVENT_NAME0 = new KeyTypeHandle<SoundEventName>("SoundEventName<0>", (x, old) =>
		{//VALUE TO PTR
			if (e(old)) throw new LibTelltaleException("Cannot create a sound event name, can only edit");
			//Nothing needed here. When you edit the list handles it will automatically update the backend memory
			return old;
		}, x => //PTR TO VALUE
		{
			IntPtr array = Marshal.ReadIntPtr(x);
			SoundEventName ret = new SoundEventName();
			ret.mSoundEvents = new List<Handle>();
			int size = Native.Abstract_DCArray_Size(array);
			for(int i = 0; i < size; i++)
            {
				ret.mSoundEvents.Add(new Handle(Native.Abstract_DCArray_At(array, i)));
            }
			return ret;
		});

		public static readonly KeyTypeHandle<string> TYPE_DUMMY_POS = new KeyTypeHandle<string>("ScriptEnum:AIDummyPos", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_BLEND_TYPES = new KeyTypeHandle<string>("ScriptEnum:BlendTypes", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		///<summary>The string param is the capital on the first letter. Valid ones are: 'Blue', 'Green', 'Red' etc.. not 'blue' or 'BLUE' etc</summary>
		public static readonly KeyTypeHandle<string> TYPE_UI_COLOUR = new KeyTypeHandle<string>("ScriptEnum:UIColor", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_RETICLE_ACTIONS = new KeyTypeHandle<string>("ScriptEnum:ReticleActions", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_STRUGGLE_TYPE = new KeyTypeHandle<string>("ScriptEnum:StruggleType", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_AI_PATROL_TYPE = new KeyTypeHandle<string>("ScriptEnum:AIPatrolType", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_MENU_ALIGNMENT = new KeyTypeHandle<string>("ScriptEnum:MenuAlignment", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_MENU_ALIGNMENT_VERTICAL = new KeyTypeHandle<string>("ScriptEnum:MenuVerticalAlignment", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_RETICLE_DISPLAY_MODE = new KeyTypeHandle<string>("ScriptEnum:Reticle Display Mode", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_CONTROLLER_BUTTONS = new KeyTypeHandle<string>("ScriptEnum:ControllerButtons", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_USEABLE_TYPE = new KeyTypeHandle<string>("ScriptEnum:UseableType", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_AI_AGENT_STATE = new KeyTypeHandle<string>("ScriptEnum:AIAgentState", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_TEXT_COLOUR_STYLE = new KeyTypeHandle<string>("ScriptEnum:TextColorStyle", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_QTE_TYPE = new KeyTypeHandle<string>("ScriptEnum:QTE_Type", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		private static IntPtr alloc(int z)
		{
			return Native.hMemory_AllocNew(z);
		}

		private static bool e(IntPtr e)
		{
			return e.Equals(IntPtr.Zero);
		}

		private static IntPtr a(IntPtr ptr, int i)
		{
			return new IntPtr(ptr.ToInt64() + i);
		}

		private struct boolean
		{
			public byte val;
		}

		public static readonly KeyTypeHandle<int> TYPE_SOUND_REVERB_PRESET = new KeyTypeHandle<int>("soundreverbpreset", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_RENDER_LIGHTMAP_UV_GENERATION_TYPE = new KeyTypeHandle<int>("enumrenderlightmapuvgenerationtype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_MSAA_QUALITY = new KeyTypeHandle<int>("0", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3LIGHT_ENV_SHADOW_TYPE = new KeyTypeHandle<int>("enumt3lightenvshadowtype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_HBAO_QUALITY_LEVEL = new KeyTypeHandle<int>("enumhbaoqualitylevel", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3LIGHT_CINEMATIC_RIG_LEVEL_OF_DETAIL = new KeyTypeHandle<int>("t3lightcinematicriglod", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_HBAO_RESOLUTION = new KeyTypeHandle<int>("enumhbaoresolution", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ANITALIAS_TYPE = new KeyTypeHandle<int>("enumrenderantialiastype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_TAA_JITTER_TYPE = new KeyTypeHandle<int>("enumrendertaajittertype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<PhonemeKey> TYPE_PHONEME_KEY = new KeyTypeHandle<PhonemeKey>("phonemekey", (key, old) =>
		{
			if (e(old)) throw new LibTelltaleException("No old ptr");
			IntPtr ptr = old;
			Marshal.WriteInt64(ptr,(long)(key.mPhoneme == null ? 0 : key.mPhoneme.GetCRC()));
			Marshal.WriteInt32(a(ptr, 8), BitConverter.SingleToInt32Bits(key.mFadeInTime));
			Marshal.WriteInt32(a(ptr, 12), BitConverter.SingleToInt32Bits(key.mHoldTime));
			Marshal.WriteInt32(a(ptr, 16), BitConverter.SingleToInt32Bits(key.mFadeOutTime));
			Marshal.WriteInt32(a(ptr, 20), BitConverter.SingleToInt32Bits(key.mTargetContribution));
			return ptr;
		}, ptr =>
		{
			PhonemeKey ret = new PhonemeKey();
			ret.mPhoneme = new Handle(Marshal.ReadIntPtr(ptr));
			ret.mFadeInTime = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(ptr, 8)));
			ret.mHoldTime= BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(ptr, 12)));
			ret.mFadeOutTime = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(ptr, 16)));
			ret.mTargetContribution = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(ptr, 20)));
			return ret;
		});

		public static readonly KeyTypeHandle<int> TYPE_HBAO_PARTICIPATION_TYPE = new KeyTypeHandle<int>("enumhbaoparticipationtype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});
		public static readonly KeyTypeHandle<int> TYPE_EMITTER_CONSTRAINT_TYPE = new KeyTypeHandle<int>("enumemitterconstrainttype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3DETAIL_SHADING_TYPE = new KeyTypeHandle<int>("enumt3detailshadingtype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_EMITTER_TRIGGER_ENABLE = new KeyTypeHandle<int>("enumemittertriggerenable", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3LIGHT_ENV_MOBILITY = new KeyTypeHandle<int>("enumt3lightenvmobility", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3_NPR_SPECULAR_TYPE = new KeyTypeHandle<int>("enumt3nprspeculartype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_PARTICLE_COUNT_TYPE = new KeyTypeHandle<int>("enumemitterparticlecounttype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_PARTICLE_PROP_DRIVER = new KeyTypeHandle<int>("enumparticlepropdriver", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENLIGHTEN_MODULE_QUALITY = new KeyTypeHandle<int>("enlightenmodule::enumequality", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENLIGHTEN_MODULE_AGENT_USAGE = new KeyTypeHandle<int>("enlightenmodule::enumeagentusage", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3MATERIAL_NORMAL_SPACE_TYPE = new KeyTypeHandle<int>("enumt3materialnormalspacetype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<T3MaterialEnlightenPrecomputeParams> TYPE_T3_ENLIGHTEN_PRECOMPUTE_PARAMS = new 
			KeyTypeHandle<T3MaterialEnlightenPrecomputeParams>("t3materialenlightenprecomputeparams", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(8) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.mPrecomputeVisiblity));
			Marshal.WriteInt32(a(ptr,4), x.unknown);
			return ptr;
		}, x =>
		{
			T3MaterialEnlightenPrecomputeParams ret = new T3MaterialEnlightenPrecomputeParams();
			ret.mPrecomputeVisiblity = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.unknown = Marshal.ReadInt32(a(x,4));
			return ret;
		});

		public static readonly KeyTypeHandle<int> TYPE_T3LIGHT_ENV_SHADOW_QUALITY = new KeyTypeHandle<int>("enumt3lightenvshadowquality", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_LIGHT_TYPE = new KeyTypeHandle<int>("lighttype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});


		public static readonly KeyTypeHandle<int> TYPE_TANGENT_MODES = new KeyTypeHandle<int>("enumetangentmodes", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_LIGHT_CELL_BLEND_MODE = new KeyTypeHandle<int>("enumlightcellblendmode", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENLIGHTEN_MODULE_PROBE_RES_WITH_DEFAULT = new KeyTypeHandle<int>("enlightenmodule::enumeproberesolutionwithdefault", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		/// <summary>
		/// The int value is the mLangID
		/// </summary>
		public static readonly KeyTypeHandle<int> TYPE_LANGUAGE_RESOURCE_PROXY = new KeyTypeHandle<int>("languageresproxy", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_FLAGS_T3LIGHT_ENV_GROUP_SET = new KeyTypeHandle<int>("flagst3lightenvgroupset", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENLIGHTEN_MODULE_CUBEMAP_SETTINGS = new KeyTypeHandle<int>("enlightenmodule::enlightencubemapsettings", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENLIGHTEN_MODULE_INSTANCE_TYPE = new KeyTypeHandle<int>("enlightenmodule::enumeinstancetype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_EMITTER_SPRITE_ANIMATION_SELECTION = new KeyTypeHandle<int>("enumemitterspriteanimationselection", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_FACEBACK_TYPE = new KeyTypeHandle<int>("enlightenmodule::enumbackfacetype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENLIGHTEN_MODULE_PROBE_SAMPLE_METHOD = new KeyTypeHandle<int>("enlightenmodule::enumprobesamplemethod", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENLIGHTEN_MODULE_UPDATE_METHOD_WITH_DEFAULT = new KeyTypeHandle<int>("enlightenmodule::enumeupdatemethodwithdefault", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_HBAO_PRESET = new KeyTypeHandle<int>("enumhbaopreset", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_DEPTH_OF_FIELD_LEVEL = new KeyTypeHandle<int>("enumdofqualitylevel", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_BOKEH_LEVEL = new KeyTypeHandle<int>("enumbokehqualitylevel", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_GLOW_QUALITY_LEVEL = new KeyTypeHandle<int>("enumglowqualitylevel", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENLIGHTEN_MODULE_SIMPLIFY_MODE = new KeyTypeHandle<int>("enlightenmodule::enumesimplifymode", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3LIGHT_LEVEL_OF_DETAIL = new KeyTypeHandle<int>("t3lightenvlod", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3LIGHT_ENV_LOD_BEHAVIOUR = new KeyTypeHandle<int>("enumt3lightenvlodbehavior", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_BOKEH_OCCLUSION_TYPE = new KeyTypeHandle<int>("enumbokehocclusiontype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENLIGHTEN_BAKE_BEHAVIOUR = new KeyTypeHandle<int>("enumt3lightenvenlightenbakebehavior", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_BLEND_MODE = new KeyTypeHandle<int>("blendmode", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ZTEST_FUNCTION = new KeyTypeHandle<int>("ztestfunction", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_FOOTSTEPS_MATERIAL = new KeyTypeHandle<int>("soundfootsteps::enummaterial", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_CAMERA_FACING_TYPES = new KeyTypeHandle<int>("camerafacingtypes", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_DEPTH_OF_FIELD_TYPE = new KeyTypeHandle<int>("enumdepthoffieldtype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3LIGHT_ENV_TYPE = new KeyTypeHandle<int>("enumt3lightenvtype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_PROCEDURAL_LOOKAT_COMPUTE_STAGE = new KeyTypeHandle<int>("procedural_lookat::enumlookatcomputestage", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_T3MATERIAL_LIGHT_MODEL_TYPE = new KeyTypeHandle<int>("enumt3materiallightmodeltype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_RENDER_MASK_TEST = new KeyTypeHandle<int>("enumrendermasktest", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_RENDER_MASK_WRITE = new KeyTypeHandle<int>("enumrendermaskwrite", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_TONEMAP_TYPE = new KeyTypeHandle<int>("enumtonemaptype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_EMITTERS_ENABLE_TYPE = new KeyTypeHandle<int>("enumemittersenabletype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_SCENE_AGENT_QUALITY_SETTINGS = new KeyTypeHandle<int>("scene::agentqualitysettings", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_EMITTER_SPRITE_ANIMATION_TYPE = new KeyTypeHandle<int>("enumemitterspriteanimationtype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_ENUM_FONTTOOL_LANGUAGESET = new KeyTypeHandle<int>("fonttool::enumlanguageset", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_TEXT_ALIGNMENT_HORIZONTAL = new KeyTypeHandle<int>("enumhtextalignmenttype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_PARTICLE_AFFECTOR_TYPE = new KeyTypeHandle<int>("enumparticleaffectortype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_PARTICLE_GEOMETRY_TYPE = new KeyTypeHandle<int>("enumparticlegeometrytype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_COLOUR_TYPE = new KeyTypeHandle<int>("enumemittercolortype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_EMITTER_SPAWN_SHAPE = new KeyTypeHandle<int>("enumemitterspawnshape", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_LIGHT_ENV_GROUP = new KeyTypeHandle<int>("enumt3lightenvgroup", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_HBAO_INTERLEAVING = new KeyTypeHandle<int>("enumhbaodeinterleaving", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_TEXT_ALIGNMENT_VERTICAL = new KeyTypeHandle<int>("enumvtextalignmenttype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_HBAO_BLUR_QUALITY = new KeyTypeHandle<int>("enumhbaoblurquality", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_HBAO_PERPIXEL_NORMALS = new KeyTypeHandle<int>("enumhbaoperpixelnormals", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_PARTICLE_SORT_MODE = new KeyTypeHandle<int>("enumparticlesortmode", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_TEXT_ORIENTATION = new KeyTypeHandle<int>("enumtextorientationtype", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<int> TYPE_NAVCAM_MODE = new KeyTypeHandle<int>("navcam::enummode", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<PropertySet> TYPE_PROPERTY_SET = new KeyTypeHandle<PropertySet>("propertyset", (x, old) =>
		{
			return e(old) ? x.Internal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new PropertySet(x);
		});

		public static readonly KeyTypeHandle<ProceduralLookAtConstraint> TYPE_PROCEDURAL_LOOKAT_CONSTRAINT = new KeyTypeHandle<ProceduralLookAtConstraint>("procedural_lookat::constraint", (x, old) =>
		{
			if (e(old)) throw new LibTelltaleException("Cannot create a ProceduralLookAtConstraint, only edit one");
			IntPtr ptr = old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.minA));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.maxA));
			Marshal.WriteInt32(a(ptr, 8), BitConverter.SingleToInt32Bits(x.valueB));
			Marshal.WriteInt32(a(ptr, 12), BitConverter.SingleToInt32Bits(x.valueC));
			Marshal.WriteInt32(a(ptr, 16), x.valueD);
			Marshal.WriteInt32(a(ptr, 20), x.valueE);
			return ptr;
		}, x =>
		{
			ProceduralLookAtConstraint ret = new ProceduralLookAtConstraint();
			ret.minA = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.maxA = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			ret.valueB = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 8)));
			ret.valueC = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 12)));
			ret.valueD = Marshal.ReadInt32(a(x, 16));
			ret.valueE = Marshal.ReadInt32(a(x, 20));
			return ret;
		});

		public static readonly KeyTypeHandle<Handle> TYPE_MESH = new KeyTypeHandle<Handle>("handle<d3dmesh>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_PRELOADPACKAGE_DIALOG = new KeyTypeHandle<Handle>("handle<preloadpackage::runtimedatadialog>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_PRELOADPACKAGE_SCENE = new KeyTypeHandle<Handle>("handle<preloadpackage::runtimedatascene>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_LANGUAGE_DATABASE = new KeyTypeHandle<Handle>("handle<languagedatabase>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_INPUT_MAPPER = new KeyTypeHandle<Handle>("handle<inputmapper>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_EVENT_STORAGE = new KeyTypeHandle<Handle>("handle<eventstorage>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_SOUNDEVENT_SNAPSHOTDATA = new KeyTypeHandle<Handle>("handle<soundeventsnapshotdata>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_SOUNDEVENT_DATA = new KeyTypeHandle<Handle>("handle<soundeventdata>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_HANDLE_BLEND_MODE = new KeyTypeHandle<Handle>("handle<blendmode>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_AGENT_MAP = new KeyTypeHandle<Handle>("handle<agentmap>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_ACTOR_AGENT_MAPPER = new KeyTypeHandle<Handle>("handle<actoragentmapper>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_EVENTSTORAGE_PAGE = new KeyTypeHandle<Handle>("handle<eventstoragepage>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_SOUNDBUS_SNAPSHOT = new KeyTypeHandle<Handle>("handle<soundbussnapshot::snapshotsuite>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_PHYSICS_DATA = new KeyTypeHandle<Handle>("handle<physicsdata>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_LIGHT_PROBE_DATA = new KeyTypeHandle<Handle>("handle<lightprobedata>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_REVERB_DEFINITION = new KeyTypeHandle<Handle>("handle<soundreverbdefinition>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_SOUNDBUSSNAPSHOT_SNAPSHOT = new KeyTypeHandle<Handle>("handle<soundbussnapshot::snapshot>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_STYLE = new KeyTypeHandle<Handle>("handle<styleguide>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_PHONEME_TABLE = new KeyTypeHandle<Handle>("handle<phonemetable>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_LANGUAGE_RES = new KeyTypeHandle<Handle>("handle<languageres>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_LANGUAGE_RESOURCE = new KeyTypeHandle<Handle>("handle<languageresource>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_PARTICLE_PROPERTIES = new KeyTypeHandle<Handle>("handle<particleproperties>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_SOUND_AMBIENCE = new KeyTypeHandle<Handle>("handle<soundambience::ambiencedefinition>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_VOICE_DATA = new KeyTypeHandle<Handle>("handle<voicedata>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_RULES = new KeyTypeHandle<Handle>("handle<rules>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_RULE = new KeyTypeHandle<Handle>("handle<rule>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_RESOURCE_BUNDLE = new KeyTypeHandle<Handle>("handle<resourcebundle>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_HANDLE_PROPERTYSET = new KeyTypeHandle<Handle>("handle<propertyset>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_AUDIO_DATA = new KeyTypeHandle<Handle>("handle<audiodata>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_WALK_BOXES = new KeyTypeHandle<Handle>("handle<walkboxes>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_SKELETON = new KeyTypeHandle<Handle>("handle<skeleton>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_DIALOG = new KeyTypeHandle<Handle>("handle<dlg>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_FONT = new KeyTypeHandle<Handle>("handle<font>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_SPRITE = new KeyTypeHandle<Handle>("handle<particlesprite>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_CHORE = new KeyTypeHandle<Handle>("handle<chore>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_TEXTURE = new KeyTypeHandle<Handle>("handle<t3texture>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_OVERLAY_DATA = new KeyTypeHandle<Handle>("handle<t3overlaydata>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_BLEND_GRAPH_MANAGER = new KeyTypeHandle<Handle>("handle<blendgraphmanager>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_SOUND_DATA = new KeyTypeHandle<Handle>("handle<sounddata>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_ANIMATION = new KeyTypeHandle<Handle>("handle<animation>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_DIALOG_RESOURCE = new KeyTypeHandle<Handle>("handle<dialogresource>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_BLEND_GRAPH = new KeyTypeHandle<Handle>("handle<blendgraph>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_TRANSITION_MAP = new KeyTypeHandle<Handle>("handle<transitionmap>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<Handle> TYPE_SCENE = new KeyTypeHandle<Handle>("handle<scene>", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		/// <summary>
		/// A symbol is a type which holds a reference to anything (I think). Its a CRC64 but of what thats up to you. You probably won't use this type ever. Basically a Handle (<T>)
		/// </summary>
		public static readonly KeyTypeHandle<Handle> TYPE_SYMBOL = new KeyTypeHandle<Handle>("symbol", (x, old) =>
		{
			return e(old) ? x.Interal_Get() : old;//VALUE -> PTR
		}, x =>
		{//PTR -> VALUE
			return new Handle(x);
		});

		public static readonly KeyTypeHandle<AnimOrChore> TYPE_ANIMORCHORE = new KeyTypeHandle<AnimOrChore>("animorchore", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(16) : old;
			Marshal.WriteIntPtr(ptr, x.mAnim.Interal_Get());
			Marshal.WriteIntPtr(a(ptr, 8), x.mChore.Interal_Get());
			return ptr;
		}, x =>
		{
			AnimOrChore aoc = new AnimOrChore();
			aoc.mAnim = new Handle(Marshal.ReadIntPtr(x));
			aoc.mChore = new Handle(Marshal.ReadIntPtr(a(x, 8)));
			return aoc;
		});

		public static readonly KeyTypeHandle<bool> TYPE_BOOL = new KeyTypeHandle<bool>("bool", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(1) : old;
			Marshal.WriteByte(ptr, (byte)(x ? 1 : 0));
			return ptr;
		}, x =>
		{
			boolean b = new boolean();
			b = (boolean)Marshal.PtrToStructure(x, typeof(boolean));
			return b.val != 0;
		});

		public static readonly KeyTypeHandle<float> TYPE_FLOAT = new KeyTypeHandle<float>("float", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x));
			return ptr;
		}, x =>
		{
			return BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
		});

		public static readonly KeyTypeHandle<Polar> TYPE_POLAR = new KeyTypeHandle<Polar>("polar", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(12) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.mR));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.mTheta));
			Marshal.WriteInt32(a(ptr, 8), BitConverter.SingleToInt32Bits(x.mPhi));
			return ptr;
		}, x =>
		{
			Polar ret = new Polar();
			ret.mR = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.mTheta = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			ret.mPhi = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 8)));
			return ret;
		});

		public static readonly KeyTypeHandle<Colour> TYPE_COLOUR = new KeyTypeHandle<Colour>("color", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(16) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.a));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.r));
			Marshal.WriteInt32(a(ptr, 8), BitConverter.SingleToInt32Bits(x.g));
			Marshal.WriteInt32(a(ptr, 12), BitConverter.SingleToInt32Bits(x.b));
			return ptr;
		}, x =>
		{
			Colour ret = new Colour();
			ret.a = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.r = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			ret.g = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 8)));
			ret.b = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 12)));
			return ret;
		});

		public static readonly KeyTypeHandle<Rect> TYPE_RECT = new KeyTypeHandle<Rect>("rect", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(16) : old;
			Marshal.WriteInt32(ptr, x.left);
			Marshal.WriteInt32(a(ptr, 4), x.right);
			Marshal.WriteInt32(a(ptr, 8), x.top);
			Marshal.WriteInt32(a(ptr, 12), x.bottom);
			return ptr;
		}, x =>
		{
			Rect ret = new Rect();
			ret.left = Marshal.ReadInt32(x);
			ret.right = Marshal.ReadInt32(a(x, 4));
			ret.top = Marshal.ReadInt32(a(x, 8));
			ret.bottom = Marshal.ReadInt32(a(x, 12));
			return ret;
		});

		public static readonly KeyTypeHandle<Quaternion> TYPE_QUATERNION = new KeyTypeHandle<Quaternion>("quaternion", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(16) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.x));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.y));
			Marshal.WriteInt32(a(ptr, 8), BitConverter.SingleToInt32Bits(x.z));
			Marshal.WriteInt32(a(ptr, 12), BitConverter.SingleToInt32Bits(x.w));
			return ptr;
		}, x =>
		{
			Quaternion ret = new Quaternion();
			ret.x = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.y = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			ret.z = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 8)));
			ret.w = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 12)));
			return ret;
		});

		public static readonly KeyTypeHandle<ParticleLODKey> TYPE_PARTICLE_LOD_KEY = new KeyTypeHandle<ParticleLODKey>("particlelodkey", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(16) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.a));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.b));
			Marshal.WriteInt32(a(ptr, 8), BitConverter.SingleToInt32Bits(x.c));
			Marshal.WriteInt32(a(ptr, 12), BitConverter.SingleToInt32Bits(x.d));
			return ptr;
		}, x =>
		{
			ParticleLODKey ret = new ParticleLODKey();
			ret.a = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.b = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			ret.c = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 8)));
			ret.d = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 12)));
			return ret;
		});

		public static readonly KeyTypeHandle<Vector4> TYPE_VEC4 = new KeyTypeHandle<Vector4>("vector4", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(16) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.x));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.y));
			Marshal.WriteInt32(a(ptr, 8), BitConverter.SingleToInt32Bits(x.z));
			Marshal.WriteInt32(a(ptr, 12), BitConverter.SingleToInt32Bits(x.w));
			return ptr;
		}, x =>
		{
			Vector4 ret = new Vector4();
			ret.x = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.y = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			ret.z = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 8)));
			ret.w = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 12)));
			return ret;
		});

		public static readonly KeyTypeHandle<ResourceGroupInfo> TYPE_RESOURCE_GROUP_INFO = new KeyTypeHandle<ResourceGroupInfo>("resourcegroupinfo", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(16) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.a));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.r));
			Marshal.WriteInt32(a(ptr, 8), BitConverter.SingleToInt32Bits(x.g));
			Marshal.WriteInt32(a(ptr, 12), BitConverter.SingleToInt32Bits(x.b));
			Marshal.WriteInt32(a(ptr, 16), x.i);
			return ptr;
		}, x =>
		{
			ResourceGroupInfo ret = new ResourceGroupInfo();
			ret.a = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.r = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			ret.g = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 8)));
			ret.b = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 12)));
			ret.i = Marshal.ReadInt32(a(x, 16));

			return ret;
		});

		public static readonly KeyTypeHandle<Vector3> TYPE_VEC3 = new KeyTypeHandle<Vector3>("vector3", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(12) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.x));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.y));
			Marshal.WriteInt32(a(ptr, 8), BitConverter.SingleToInt32Bits(x.z));
			return ptr;
		}, x =>
		{
			Vector3 ret = new Vector3();
			ret.x = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.y = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			ret.z = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 8)));
			return ret;
		});

		public static readonly KeyTypeHandle<TRangeFloat> TYPE_RANGE_FLOAT = new KeyTypeHandle<TRangeFloat>("trange<float>", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(8) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.min));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.max));
			return ptr;
		}, x =>
		{
			TRangeFloat ret = new TRangeFloat();
			ret.min = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.max = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			return ret;
		});

		public static readonly KeyTypeHandle<TRangeInt> TYPE_RANGE_INT = new KeyTypeHandle<TRangeInt>("trange<int32>", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(8) : old;
			Marshal.WriteInt32(ptr, x.min);
			Marshal.WriteInt32(a(ptr, 4), x.max);
			return ptr;
		}, x =>
		{
			TRangeInt ret = new TRangeInt();
			ret.min = Marshal.ReadInt32(x);
			ret.max = Marshal.ReadInt32(a(x, 4));
			return ret;
		});

		public static readonly KeyTypeHandle<Vector2> TYPE_VEC2 = new KeyTypeHandle<Vector2>("vector2", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(8) : old;
			Marshal.WriteInt32(ptr, BitConverter.SingleToInt32Bits(x.x));
			Marshal.WriteInt32(a(ptr, 4), BitConverter.SingleToInt32Bits(x.y));
			return ptr;
		}, x =>
		{
			Vector2 ret = new Vector2();
			ret.x = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(x));
			ret.y = BitConverter.Int32BitsToSingle(Marshal.ReadInt32(a(x, 4)));
			return ret;
		});

		public static readonly KeyTypeHandle<int> TYPE_INT = new KeyTypeHandle<int>("int32", (x, old) =>
		{
			IntPtr ptr = e(old) ? alloc(4) : old;
			Marshal.WriteInt32(ptr, x);
			return ptr;
		}, x =>
		{
			return Marshal.ReadInt32(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_LIGHTCOMPOSER_CAMERA_ZONE = new KeyTypeHandle<string>("ScriptEnum:LightComposerCameraZone", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_LIGHTCOMPOSER_LIGHT_SOURCE_QUADRANT = new KeyTypeHandle<string>("ScriptEnum:LightComposerLightSourceQuadrant", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_LIGHTCOMPOSER_NODE_LOCATION = new KeyTypeHandle<string>("ScriptEnum:LightComposerNodeLocation", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_GAMEPAD_BUTTON = new KeyTypeHandle<string>("scriptenum:gamepadbutton", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_DIALOG_MODE = new KeyTypeHandle<string>("scriptenum:dialogmode", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeHandle<string> TYPE_STRING = new KeyTypeHandle<string>("String", (x, old) =>
		{
			IntPtr ptr;
			if (e(old))
			{
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			else
			{
				ptr = old;
				Native.hMemory_FreeArray(ptr);
				IntPtr tmp = Marshal.StringToHGlobalAnsi(x);
				ptr = Native.hMemory_CreateArray(tmp);
				Marshal.FreeHGlobal(tmp);
			}
			return ptr;
		}, x =>
		{
			return Marshal.PtrToStringAnsi(x);
		});

		public static readonly KeyTypeListHandle<string> TYPE_ARRAY_STRING = new KeyTypeListHandle<string>("dcarray<string>", KeyTypes.TYPE_STRING);

		public static readonly KeyTypeListHandle<Handle> TYPE_LIST_SYMBOL = new KeyTypeListHandle<Handle>("list<symbol>", KeyTypes.TYPE_SYMBOL);

		public static readonly KeyTypeListHandle<KeyTypeList<Handle>> TYPE_LIST_LIST_SYMBOL = new KeyTypeListHandle<KeyTypeList<Handle>>("list<list<symbol>>", KeyTypes.TYPE_LIST_SYMBOL);

		public static readonly KeyTypeListHandle<int> TYPE_LIST_INT32 = new KeyTypeListHandle<int>("set<int,less<int>>", KeyTypes.TYPE_INT);//be careful as its a set, no duplicates !!

		public static readonly KeyTypeMapHandle<string, string> TYPE_MAP_STRING_STRING = new KeyTypeMapHandle<string, string>("map<string,string,less<string>>", TYPE_STRING, TYPE_STRING);

		public static readonly KeyTypeMapHandle<string,int> TYPE_MAP_STRING_INT = new KeyTypeMapHandle<string,int>("map<string,int,less<string>>", TYPE_STRING, TYPE_INT);

		public static readonly KeyTypeListHandle<Handle> TYPE_ARRAY_SOUND_DATA = new KeyTypeListHandle<Handle>("dcarray<handle<sounddata>>", TYPE_CHORE);

		public static readonly KeyTypeListHandle<KeyTypeMap<string, string>> TYPE_ARRAY_MAP_STRING_STRING = new KeyTypeListHandle<KeyTypeMap<string, string>>
			("dcarray<map<string,string,less<string>>>", TYPE_MAP_STRING_STRING);

		public static readonly KeyTypeMapHandle<int, string> TYPE_MAP_INT_STRING = new KeyTypeMapHandle<int, string>
			("map<int,string,less<int>>", TYPE_INT, TYPE_STRING);

		public static readonly KeyTypeMapHandle<string, KeyTypeList<string>> TYPE_MAP_STRING_ARRAY_STRING = new KeyTypeMapHandle<string, KeyTypeList<string>>
			("map<string,dcarray<string>,less<string>>", TYPE_STRING, TYPE_ARRAY_STRING);

		public static readonly KeyTypeMapHandle<int, Handle> TYPE_MAP_INT_SYMBOL = new KeyTypeMapHandle<int, Handle>
			("map<int,symbol,less<int>>", TYPE_INT, TYPE_SYMBOL);

		public static readonly KeyTypeMapHandle<Handle, PropertySet> TYPE_MAP_SYMBOL_PROPERTYSET = new KeyTypeMapHandle<Handle, PropertySet>
			("map<symbol,propertyset,less<symbol>>", TYPE_SYMBOL, TYPE_PROPERTY_SET);

		public static readonly KeyTypeMapHandle<string, Handle> TYPE_MAP_STRING_PROPERTYSET_HANDLE = new KeyTypeMapHandle<string, Handle>
			("map<string,handle<propertyset>,less<string>>", TYPE_STRING, TYPE_HANDLE_PROPERTYSET);

		public static readonly KeyTypeMapHandle<int, KeyTypeList<Handle>> TYPE_MAP_FOOTSTEPMATERIAL_ARRAY_SOUNDDATA = new KeyTypeMapHandle<int, KeyTypeList<Handle>>
			("map<soundfootsteps::enummaterial,dcarray<handle<sounddata>>,less<soundfootsteps::enummaterial>>", TYPE_FOOTSTEPS_MATERIAL, TYPE_ARRAY_SOUND_DATA);

		public static readonly KeyTypeMapHandle<string, PropertySet> TYPE_MAP_STRING_PROPERTY_SET =
			new KeyTypeMapHandle<string, PropertySet>("map<string,propertyset,less<string>>",
				TYPE_STRING, TYPE_PROPERTY_SET);

		public static readonly KeyTypeMapHandle<Handle,string> TYPE_MAP_SYMBOL_STRING =
			new KeyTypeMapHandle<Handle,string>("map<symbol,string,less<symbol>>",
				TYPE_SYMBOL, TYPE_STRING);

		public static readonly KeyTypeMapHandle<string,bool> TYPE_MAP_STRING_BOOL =
			new KeyTypeMapHandle<string, bool>("map<string,bool,less<string>>",
				TYPE_STRING, TYPE_BOOL);

		public static readonly KeyTypeMapHandle<Handle, int> TYPE_MAP_SYMBOL_INT = new KeyTypeMapHandle<Handle, int>("map<symbol,int,less<symbol>>", TYPE_SYMBOL, TYPE_INT);

		public static readonly KeyTypeMapHandle<Handle, bool> TYPE_MAP_SYMBOL_BOOL = new KeyTypeMapHandle<Handle, bool>("map<symbol,bool,less<symbol>>", TYPE_SYMBOL, TYPE_BOOL);

		public static readonly KeyTypeListHandle<PropertySet> TYPE_LIST_PROPERTYSET = new KeyTypeListHandle<PropertySet>("list<propertyset>", TYPE_PROPERTY_SET);

		public static readonly KeyTypeListHandle<Colour> TYPE_ARRAY_COLOUR = new KeyTypeListHandle<Colour>("dcarray<color>", TYPE_COLOUR);

		public static readonly KeyTypeListHandle<Colour> TYPE_SET_COLOUR = new KeyTypeListHandle<Colour>("set<color,less<color>>", TYPE_COLOUR);//set!!

		public static readonly KeyTypeListHandle<Handle> TYPE_ARRAY_SYMBOL = new KeyTypeListHandle<Handle>("dcarray<symbol>", TYPE_SYMBOL);

		public static readonly KeyTypeListHandle<ProceduralLookAtConstraint> TYPE_ARRAY_LOOKAT_CONSTRAINT =
			new KeyTypeListHandle<ProceduralLookAtConstraint>("dcarray<procedural_lookat::constraint>", TYPE_PROCEDURAL_LOOKAT_CONSTRAINT);

		public static readonly KeyTypeListHandle<Vector2> TYPE_ARRAY_VEC2 = new KeyTypeListHandle<Vector2>("dcarray<vector2>", TYPE_VEC2);

		public static readonly KeyTypeListHandle<Handle> TYPE_SET_SYMBOL = new KeyTypeListHandle<Handle>("set<symbol,less<symbol>>", TYPE_SYMBOL);//set, no duplicates!!

		public static readonly KeyTypeListHandle<Handle> TYPE_ARRAY_CHORE = new KeyTypeListHandle<Handle>("dcarray<handle<chore>>", TYPE_CHORE);

		public static readonly KeyTypeListHandle<Handle> TYPE_ARRAY_MESH = new KeyTypeListHandle<Handle>("dcarray<handle<d3dmesh>>", TYPE_MESH);

		public static readonly KeyTypeListHandle<string> TYPE_LIST_STRING = new KeyTypeListHandle<string>("list<string>", TYPE_STRING);

		public static readonly KeyTypeListHandle<string> TYPE_SET_STRING = new KeyTypeListHandle<string>("set<string,less<string>>", TYPE_STRING);

		public static readonly KeyTypeListHandle<PropertySet> TYPE_ARRAY_PROPERTYSET = new KeyTypeListHandle<PropertySet>("dcarray<propertyset>", TYPE_PROPERTY_SET);

		public static readonly KeyTypeListHandle<Handle> TYPE_ARRAY_TEXTURE_HANDLE = new KeyTypeListHandle<Handle>("dcarray<handle<t3texture>>", TYPE_TEXTURE);

		public static readonly KeyTypeListHandle<Handle> TYPE_ARRAY_HANDLE_PROPERTYSET = new KeyTypeListHandle<Handle>("dcarray<handle<propertyset>>", TYPE_HANDLE_PROPERTYSET);

		public static readonly KeyTypeListHandle<float> TYPE_LIST_FLOAT = new KeyTypeListHandle<float>("list<float>", TYPE_FLOAT);

		public static readonly KeyTypeListHandle<int> TYPE_ARRAY_INT = new KeyTypeListHandle<int>("dcarray<int>", TYPE_INT);

		public static readonly KeyTypeListHandle<bool> TYPE_ARRAY_BOOL = new KeyTypeListHandle<bool>("dcarray<bool>", TYPE_BOOL);

	}

	/// <summary>
	/// Represents a list in a property set, where T is a KeyTypeHandle. Do not call Dispose on this, the library handles that.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class KeyTypeList<T>
	{

		protected readonly IntPtr reference;

		protected readonly AbstractKeyTypeHandle<T> handle;

		public KeyTypeList(IntPtr r, AbstractKeyTypeHandle<T> h)
		{
			this.handle = h;
			this.reference = r;
		}

		/// <summary>
		/// Clears this list
		/// </summary>
		public void Clear()
        {
			Native.PropertySet_DCArray_Clear(this.reference);
        }

		/// <summary>
		/// Imports all values from the given nonnull list 
		/// </summary>
		/// <param name="list"></param>
		public void ImportList(List<T> list)
        {
			list.ForEach(x => Add(x));
        }

		/// <summary>
		/// Removes an element at the given index
		/// </summary>
		/// <param name="index"></param>
		public void Remove(int index)
        {
			if (index >= this.Size()) return;
			Native.PropertySet_DCArray_Remove(this.reference, Native.PropertySet_DCArray_At(this.reference, index));
        }

		/// <summary>
		/// Adds a value to this list, returning its property entry.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public PropertySet.PropertyEntry Add(T value)
        {
			IntPtr tmpptr = Native.hPropertySet_CreateEntry();
			PropertySet._PropertyEntry entry = (PropertySet._PropertyEntry)Marshal.PtrToStructure(tmpptr, typeof(PropertySet._PropertyEntry));
			entry.mValue = this.handle.ValueToPtr(value, IntPtr.Zero);
			entry.mKeyNameCrc = 0;
			entry.mKeyName = Native.hMemory_AllocNew(0);
			entry.mTypeHandle = this.handle.GetTypeHandle();
			Marshal.StructureToPtr(entry, tmpptr, false);
			Native.PropertySet_DCArray_Add(this.reference, tmpptr);
			PropertySet.PropertyEntry ret = new PropertySet.PropertyEntry();
			ret.backend = entry;
			ret.reference = tmpptr;
			return ret;
        }

		/// <summary>
		/// The size of this list
		/// </summary>
		/// <returns></returns>
		public int Size()
        {
			return Native.PropertySet_DCArray_Size(this.reference);
        }

		/// <summary>
		/// Gets or sets an element in this list by index. The index is not checked and if its out of bounds your program is likely to crash or have undefined behaviour
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public T this[int i] {

			get {
				IntPtr ptr = Native.PropertySet_DCArray_At(this.reference, i);
				PropertySet._PropertyEntry back = (PropertySet._PropertyEntry)Marshal.PtrToStructure(ptr, typeof(PropertySet._PropertyEntry));
				return handle.PtrToValue(back.mValue);
			}
			set
            {
				if (value == null) return;
				IntPtr ptr = Native.PropertySet_DCArray_At(this.reference, i);
				PropertySet._PropertyEntry back = (PropertySet._PropertyEntry)Marshal.PtrToStructure(ptr, typeof(PropertySet._PropertyEntry));
				back.mValue = this.handle.ValueToPtr(value, back.mValue);
				Marshal.StructureToPtr(back, ptr, false);
            }
		}

		public IntPtr Internal_Get()
        {
			return this.reference;
        }

    }

	/// <summary>
	/// An entry in a property set map
	/// </summary>
	struct MapEntry
	{
		public IntPtr Key;
		public IntPtr Value;
	}

	/// <summary>
	/// A HashMap (Dictionary) in a propertyset, where K and V are KeyTypeHandles
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="V"></typeparam>
	public class KeyTypeMap<K, V> {

		protected readonly KeyTypeMapHandle<K, V> handle;

		protected readonly IntPtr reference;

		public KeyTypeMap(IntPtr r, KeyTypeMapHandle<K, V> handle)
        {
			this.reference = r;
			this.handle = handle;
        }

		/// <summary>
		/// Gets or sets a value in this map by index. Index cannot be out of bounds.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public KeyValuePair<K, V> this[int i]
        {
			get
			{
				IntPtr ptr = Native.PropertySet_DCArray_At(this.reference, i);
				MapEntry back = (MapEntry)Marshal.PtrToStructure(ptr, typeof(MapEntry));
				PropertySet._PropertyEntry kentry = (PropertySet._PropertyEntry)Marshal.PtrToStructure(back.Key, typeof(PropertySet._PropertyEntry));
				K key = this.handle.GetKeyElementTypeHandle().PtrToValue(kentry.mValue);
				PropertySet._PropertyEntry ventry = (PropertySet._PropertyEntry)Marshal.PtrToStructure(back.Value, typeof(PropertySet._PropertyEntry));
				V val = this.handle.GetValueElementTypeHandle().PtrToValue(ventry.mValue);
				return new KeyValuePair<K, V>(key, val);
			}
			set
			{
				IntPtr ptr = Native.PropertySet_DCArray_At(this.reference, i);
				MapEntry back = (MapEntry)Marshal.PtrToStructure(ptr, typeof(MapEntry));
				back.Key = Native.hPropertySet_CreateEntry();
				PropertySet._PropertyEntry kentry = (PropertySet._PropertyEntry)Marshal.PtrToStructure(back.Key, typeof(PropertySet._PropertyEntry));
				kentry.mValue = this.handle.GetKeyElementTypeHandle().ValueToPtr(value.Key, back.Key);
				kentry.mKeyNameCrc = 0;
				kentry.mKeyName = Native.hMemory_AllocNew(0);
				kentry.mTypeHandle = this.handle.GetKeyElementTypeHandle().GetTypeHandle();
				Marshal.StructureToPtr(kentry, back.Key, false);
				back.Value = Native.hPropertySet_CreateEntry();
				PropertySet._PropertyEntry ventry = (PropertySet._PropertyEntry)Marshal.PtrToStructure(back.Value, typeof(PropertySet._PropertyEntry));
				ventry.mValue = this.handle.GetValueElementTypeHandle().ValueToPtr(value.Value, back.Value);
				ventry.mKeyNameCrc = 0;
				ventry.mKeyName = Native.hMemory_AllocNew(0);
				ventry.mTypeHandle = this.handle.GetValueElementTypeHandle().GetTypeHandle();
				Marshal.StructureToPtr(ventry, back.Value, false);
				Marshal.StructureToPtr(back, ptr, false);
			}
		}

		/// <summary>
		/// Adds a key-value mapping to this map
		/// </summary>
		/// <param name="value"></param>
		public void AddPair(KeyValuePair<K,V> value)
        {
			IntPtr ptr = Native.hPropertySetMap_CreateEntry();
			MapEntry back = (MapEntry)Marshal.PtrToStructure(ptr, typeof(MapEntry));
			back.Key = Native.hPropertySet_CreateEntry();
			PropertySet._PropertyEntry kentry = (PropertySet._PropertyEntry)Marshal.PtrToStructure(back.Key, typeof(PropertySet._PropertyEntry));
			kentry.mValue = this.handle.GetKeyElementTypeHandle().ValueToPtr(value.Key, IntPtr.Zero);
			kentry.mKeyNameCrc = 0;
			kentry.mKeyName = Native.hMemory_AllocNew(0);
			kentry.mTypeHandle = this.handle.GetKeyElementTypeHandle().GetTypeHandle();
			Marshal.StructureToPtr(kentry, back.Key, false);
			back.Value = Native.hPropertySet_CreateEntry();
			PropertySet._PropertyEntry ventry = (PropertySet._PropertyEntry)Marshal.PtrToStructure(back.Value, typeof(PropertySet._PropertyEntry));
			ventry.mValue = this.handle.GetValueElementTypeHandle().ValueToPtr(value.Value, IntPtr.Zero);
			ventry.mKeyNameCrc = 0;
			ventry.mKeyName = Native.hMemory_AllocNew(0);
			ventry.mTypeHandle = this.handle.GetValueElementTypeHandle().GetTypeHandle();
			Marshal.StructureToPtr(ventry, back.Value, false);
			Marshal.StructureToPtr(back, ptr, false);
			Native.PropertySetMap_DCArray_Add(this.reference, ptr);
		}

		/// <summary>
		/// Equivalent of Put (at least in Java)
		/// </summary>
		/// <param name="k"></param>
		/// <param name="v"></param>
		public void Add(K k, V v)
        {
			AddPair(new KeyValuePair<K, V>(k, v));
        }

		/// <summary>
		/// Removes a map entry at the given index.
		/// </summary>
		public void Remove(int index)
        {
			if (index >= this.Size()) return;
			Native.PropertySetMap_DCArray_Remove(this.reference, Native.PropertySetMap_DCArray_At(this.reference, index));
		}

		public KeyTypeMap(KeyTypeMapHandle<K, V> handle)
        {
			this.handle = handle;
			this.reference = Native.PropertySetMap_DCArray_Create();
        }

		/// <summary>
		/// Gets the size of this map
		/// </summary>
		/// <returns></returns>
		public int Size()
        {
			return Native.PropertySetMap_DCArray_Size(this.reference);
        }

		public IntPtr Internal_Get()
        {
			return this.reference;
        }

		/// <summary>
		/// Imports all values from a dictionary into this map
		/// </summary>
		/// <param name="dict"></param>
		public void ImportDictionary(Dictionary<K,V> dict)
        {
			foreach(KeyValuePair<K,V> pair in dict)
            {
				AddPair(pair);
            }
        }

		/// <summary>
		/// Clears all entries in this map
		/// </summary>
		public void Clear()
        {
			Native.PropertySetMap_DCArray_Clear(this.reference);
        }

	}

	/// <summary>
	/// Handle for key type maps, used internally.
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="V"></typeparam>
	public class KeyTypeMapHandle<K,V> : AbstractKeyTypeHandle<KeyTypeMap<K,V>>
	{

		protected readonly AbstractKeyTypeHandle<V> valueHandle;

		protected readonly AbstractKeyTypeHandle<K> keyHandle;

		public KeyTypeMapHandle(string keyType, AbstractKeyTypeHandle<K> keyHandle, AbstractKeyTypeHandle<V> valueHandle) : base(keyType)
		{
			this.keyHandle = keyHandle;
			this.valueHandle = valueHandle;
		}

		public string GetTypeName()
		{
			return this.typeName;
		}


		public IntPtr GetTypeHandle()
		{
			return this.handle;
		}

		public AbstractKeyTypeHandle<K> GetKeyElementTypeHandle()
		{
			return this.keyHandle;
		}

		public AbstractKeyTypeHandle<V> GetValueElementTypeHandle()
		{
			return this.valueHandle;
		}

        public override IntPtr ValueToPtr(KeyTypeMap<K, V> value, IntPtr old)
        {
			return value.Internal_Get();
        }

        public override KeyTypeMap<K, V> PtrToValue(IntPtr ptr)
        {
			return new KeyTypeMap<K, V>(ptr, this);
        }

    }



	/// <summary>
	/// No dispose since its static in memory. Used internally for map handles.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class KeyTypeListHandle<T> : AbstractKeyTypeHandle<KeyTypeList<T>>
    {

		protected readonly AbstractKeyTypeHandle<T> elementHandle;
		public KeyTypeListHandle(string keyType, AbstractKeyTypeHandle<T> elementHandle) : base(keyType)
        {
			this.elementHandle = elementHandle;
		}

		public string GetTypeName()
        {
			return this.typeName;
        }

        public sealed override IntPtr ValueToPtr(KeyTypeList<T> value, IntPtr old)
        {
			return value.Internal_Get();
        }

		public AbstractKeyTypeHandle<T> GetElementTypeHandle()
        {
			return this.elementHandle;
        }

        public sealed override KeyTypeList<T> PtrToValue(IntPtr ptr)
        {
			return new KeyTypeList<T>(ptr, this.elementHandle);
		}
    }

	/// <summary>
	/// A static value in memory (in the library). This handles the serialization and deserialization of types in a propertyset.
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class KeyTypeHandle<T> : AbstractKeyTypeHandle<T> {

		protected readonly Func<T, IntPtr, IntPtr> value2Ptr;

		protected readonly Func<IntPtr, T> ptr2Value;

		public KeyTypeHandle(string keyType, Func<T, IntPtr, IntPtr> f,Func<IntPtr, T> p) : base(keyType) {
			this.value2Ptr = f;
			this.ptr2Value = p;
		}

		/// <summary>
		/// Converts a pointer of this type to its C# value
		/// </summary>
		public sealed override T PtrToValue(IntPtr ptr){
			return this.ptr2Value.Invoke (ptr);
		}

		/// <summary>
		/// Converts a value of this type to its pointer value.
		/// </summary>
		public sealed override IntPtr ValueToPtr (T value, IntPtr old){
			if (value == null)
				return IntPtr.Zero;
			return this.value2Ptr.Invoke (value, old);
		}

	}

	/// <summary>
	/// Internal use, backend for any key type handle.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class AbstractKeyTypeHandle<T>
	{

		protected AbstractKeyTypeHandle(string keyType)
        {
			this.typeName = keyType.ToLower();
			IntPtr ptr1 = Marshal.StringToHGlobalAnsi(keyType);
			IntPtr ptr2 = Native.hMemory_CreateArray(ptr1);
			this.handle = Native.PropertySet_NewKeyInfo(keyType.Equals("0") ? 5415194591246565170l : Native.ClassToCRC(ptr2));
			Marshal.FreeHGlobal(ptr1);
			Native.hMemory_FreeArray(ptr2);
			this.typeName = keyType;
			if (this.handle == IntPtr.Zero)
				throw new LibTelltaleException(String.Format("\n\n\n***\t\tCould not register property type '{0}' (could not be found).\t\t***" +
					"\n\t\t\t\t*Please report this on the Github!*\n\n\n", keyType));
		}

		public abstract IntPtr ValueToPtr(T value, IntPtr old);
		public abstract T PtrToValue(IntPtr ptr);

		protected readonly IntPtr handle;

		protected readonly string typeName;

		public string GetTypeName()
        {
			return this.typeName;
        }

		public IntPtr GetTypeHandle()
        {
			return this.handle;
        }

	}

	/// <summary>
	/// A Dynamic array. This must be of primitive types or structs with only primitive values which don't have a destructor. This is because its a void pointer in memory.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DCArray<T> : IDisposable {

		public void Dispose()
		{
			Native.Abstract_DCArray_Delete(this.reference);
		}

		public KeyTypeHandle<T> GetTypeHandle()
        {
			return this.handle;
        }

		public IntPtr Internal_Get()
        {
			return this.reference;
        }

		protected readonly KeyTypeHandle<T> handle;

		protected IntPtr reference;

		public DCArray(KeyTypeHandle<T> handle, IntPtr ptr)
		{
			this.handle = handle;
			this.reference = ptr;
		}

		public DCArray(KeyTypeHandle<T> handle)
        {
			this.handle = handle;
			this.reference = Native.Abstract_DCArray_Create();
        }

		public T this[int i] {
            set
            {
				throw new LibTelltaleException("Cannot set at index, use Add");
            }
            get
            {
				return this.handle.PtrToValue(Native.Abstract_DCArray_At(this.reference, i));
            }
		}

		public void Remove(T value)
        {
			Native.Abstract_DCArray_Remove(this.reference, this.handle.ValueToPtr(value, IntPtr.Zero));
		}

		public void Add(T value)
        {
			Native.Abstract_DCArray_Add(this.reference, this.handle.ValueToPtr(value, IntPtr.Zero));
        }

		public int Size()
        {
			return Native.Abstract_DCArray_Size(this.reference);
        }

		public void Clear()
        {
			Native.Abstract_DCArray_Clear(this.reference);
        }

	}


	/// <summary>
	/// Telltales PropertySet, which holds key-value mappings and their types. Also contains parent properties (globals in the lua scripts) which are almost a 'base' to the property set.
	/// You must call Dispose when you are completely done with this property set.
	/// </summary>
	public class PropertySet : AbstractMetaStreamedFile {

		private bool disposed = false;

		/// <summary>
		/// A property set entry.
		/// </summary>
		public struct PropertyEntry
		{
			public _PropertyEntry backend;
			public IntPtr reference;
		}

		/// <summary>
		/// Gets a propertyset by its index. Cannot set with this, use SetProperty
		/// </summary>
		public PropertyEntry this[int n] {
            set
            {
				throw new LibTelltaleException("You cannot set a property entry, you must set the value of the property using static SetProperty(..) or remove it create a new one.");
            }
            get
            {
				return GetProperty(n);
            }
		}

		/// <summary>
		/// Gets the type name of the given property as a string.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		public static string GetPropertyTypeName(PropertyEntry entry)
        {
			return Marshal.PtrToStringAnsi(Native.hPropertySet_GetTypeName(entry.reference, 0));
        }

		protected static string _GetPropertyTypeName(IntPtr entry)
		{
			return Marshal.PtrToStringAnsi(Native.hPropertySet_GetTypeName(entry, 0));
		}

		/// <summary>
		/// Gets the full qualified name of the given property as a string.
		/// </summary>
		public static string GetQualifiedPropertyTypeName(PropertyEntry entry)
		{
			return Marshal.PtrToStringAnsi(Native.hPropertySet_GetTypeName(entry.reference, 1));
		}

		/// <summary>
		/// Backend property entry. Do not edit any fields.
		/// </summary>
		public struct _PropertyEntry
		{
			public IntPtr mKeyName;
			public ulong mKeyNameCrc;//update on set key name!
			public IntPtr mValue;
			public IntPtr mTypeHandle;//Internal for the library
		}

		/// <summary>
		/// Gets a list of all properties in this propertyset. Suggested to not call this lots, the general idea is to load the propertyset then call this and once your done call Dispose
		/// on this propertyset.
		/// </summary>
		/// <returns></returns>
		public List<PropertyEntry> GetProperties()
        {
			List<PropertyEntry> ret = new List<PropertyEntry>();
			for(int i = 0; i < this.GetNumProperties(); i++)
            {
				ret.Add(GetProperty(i));
            }
			return ret;
        }

		/// <summary>
		/// Gets the amount of properties in this set.
		/// </summary>
		/// <returns></returns>
		public int GetNumProperties(){	
			return Native.PropertySet_DCArray_Size (this.Props ());
		}

		/// <summary>
		/// Clears all properties in this property set. This will cause memory leaks if you don't handle deleting the entries beforehand.
		/// </summary>
		[Obsolete("You shouln't be having to clear a propertyset, this will cause memory leaks if you don't delete the entries beforehand")]
		public void ClearProperties(){
			Native.PropertySet_DCArray_Clear (this.Props ());
		}

		/// <summary>
		/// Removes (Does not delete, so you need to take care of this) the given entry from this propertyset.
		/// </summary>
		/// <param name="entry"></param>
		public void RemoveProperty(PropertyEntry entry){
			Native.PropertySet_DCArray_Remove (this.Props (), entry.reference);
		}

		/// <summary>
		/// Adds a propertyset entry. This entry will be automatically deleted when you call Dispose on this propertyset.
		/// </summary>
		/// <param name="entry"></param>
		public void AddProperty(PropertyEntry entry){
			Native.PropertySet_DCArray_Add (this.Props (), entry.reference);
		}

		/// <summary>
		/// Sets the value of the property set entry. Use KeyTypes.x to get the handle (the type of the property).
		/// </summary>
		public static void SetProperty<T>(PropertyEntry entry, AbstractKeyTypeHandle<T> handle, T value){
			entry.backend.mValue = handle.ValueToPtr (value, entry.backend.mValue);
			Marshal.StructureToPtr (entry.backend, entry.reference, false);
		}

		/// <summary>
		/// Gets the property entry value. Use KeyTypes.x to get the handle (the type of the property)
		/// </summary>
		public static T GetProperty<T>(AbstractKeyTypeHandle<T> handle, PropertyEntry entry){
			return handle.PtrToValue (entry.backend.mValue);
		}

		/// <summary>
		/// Gets the property entry value, if its a list.
		/// </summary>
		public static KeyTypeList<T> GetPropertyList<T>(KeyTypeListHandle<T> handle, PropertyEntry entry)
        {
			return handle.PtrToValue(entry.backend.mValue);
        }

		/// <summary>
		/// Adds a new property entry.
		/// </summary>
		public void AddNewProperty<T>(string mKeyName, AbstractKeyTypeHandle<T> handle, T value){
			this.AddProperty(CreateProperty(mKeyName, handle,value));
		}

		/// <summary>
		/// Gets a property entries of the specified type.
		/// </summary>
		public List<PropertyEntry> GetPropertiesOfType<T>(AbstractKeyTypeHandle<T> handle){
			List<PropertyEntry> ret = new List <PropertyEntry>();
			for (int i = 0; i < this.GetNumProperties (); i++) {
				if (_GetPropertyTypeName (Native.PropertySet_DCArray_At (this.Props (), i)).Equals(handle.GetTypeName())) {
					ret.Add (this.GetProperty (i));
				}
			}
			return ret;
		}

		/// <summary>
		/// Creates a property entry which is a map.
		/// </summary>
		public static PropertyEntry CreatePropertyMap<K,V>(out KeyTypeMap<K,V> list, string mKeyName, KeyTypeMapHandle<K,V> handle)
		{
			PropertyEntry ret = new PropertyEntry();
			ret.reference = Native.hPropertySet_CreateEntry();
			ret.backend = new _PropertyEntry();
			IntPtr nombre, ptr2;
			nombre = Marshal.StringToHGlobalAnsi(mKeyName);
			ret.backend.mKeyName = ptr2 = Native.hMemory_CreateArray(nombre);
			ret.backend.mKeyNameCrc = Native.ClassToCRC(ptr2);
			Marshal.FreeHGlobal(nombre);
			ret.backend.mTypeHandle = handle.GetTypeHandle();
			Marshal.StructureToPtr(ret.backend, ret.reference, false);
			ret.backend.mValue = Native.PropertySet_DCArray_Create();
			list = handle.PtrToValue(ret.backend.mValue);
			Marshal.StructureToPtr(ret.backend, ret.reference, false);
			return ret;
		}

		/// <summary>
		/// Creates a property entry map which is the K or V in a map or T in a list
		/// </summary>
		public static KeyTypeMap<K,V> CreatePropertySubMap<K,V>(KeyTypeMapHandle<K,V> handle)
		{
			return handle.PtrToValue(Native.PropertySetMap_DCArray_Create());
		}

		/// <summary>
		/// Creates a property entr ylist which is a T in a list (so a list of lists)
		/// </summary>
		public static KeyTypeList<T> CreatePropertySubList<T>(KeyTypeListHandle<T> handle)
        {
			return handle.PtrToValue(Native.PropertySet_DCArray_Create());
        }

		/// <summary>
		/// Create a new property list entry
		/// </summary>
		public static PropertyEntry CreatePropertyList<T>(out KeyTypeList<T> list, string mKeyName, KeyTypeListHandle<T> handle)
        {
			PropertyEntry ret = new PropertyEntry();
			ret.reference = Native.hPropertySet_CreateEntry();
			ret.backend = new _PropertyEntry();
			IntPtr nombre, ptr2;
			nombre = Marshal.StringToHGlobalAnsi(mKeyName);
			ret.backend.mKeyName = ptr2 = Native.hMemory_CreateArray(nombre);
			ret.backend.mKeyNameCrc = Native.ClassToCRC(ptr2);
			Marshal.FreeHGlobal(nombre);
			ret.backend.mTypeHandle = handle.GetTypeHandle();
			Marshal.StructureToPtr(ret.backend, ret.reference, false);
			ret.backend.mValue = Native.PropertySet_DCArray_Create();
			list = handle.PtrToValue(ret.backend.mValue);
			Marshal.StructureToPtr(ret.backend, ret.reference, false);
			return ret;
		}

		/// <summary>
		/// Creates a new property entry
		/// </summary>
		public static PropertyEntry CreateProperty<T>(string mKeyName, AbstractKeyTypeHandle<T> handle, T value){
			PropertyEntry ret = new PropertyEntry ();
			ret.reference = Native.hPropertySet_CreateEntry ();
			ret.backend = new _PropertyEntry ();
			IntPtr nombre, ptr2;
			nombre = Marshal.StringToHGlobalAnsi (mKeyName);
			ret.backend.mKeyName = ptr2 = Native.hMemory_CreateArray (nombre);
			ret.backend.mKeyNameCrc = Native.ClassToCRC (ptr2);
			Marshal.FreeHGlobal (nombre);
			ret.backend.mTypeHandle = handle.GetTypeHandle ();
			if (value != null)
				ret.backend.mValue = handle.ValueToPtr (value, IntPtr.Zero);
			Marshal.StructureToPtr (ret.backend, ret.reference, false);
			return ret;
		}

		/// <summary>
		/// Gets the key name of the given property. Will be a number if the key name is not found (a CRC lookup)
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		public static string GetPropertyKeyName(PropertyEntry entry){
			return Marshal.PtrToStringAnsi (entry.backend.mKeyName);
		}

		/// <summary>
		/// Gets the property entry at the given index. Useful for iteration
		/// </summary>
		public PropertyEntry GetProperty(int index){
			PropertyEntry entry = new PropertyEntry ();
			IntPtr ptr = Native.PropertySet_DCArray_At (this.Props (), index);
			entry.backend = (_PropertyEntry)Marshal.PtrToStructure (ptr, typeof(_PropertyEntry));
			entry.reference = ptr;
			return entry;
		}

		protected IntPtr Props(){
			return Native.hPropertySet_GetKeyMap (this.reference);
		}

		/// <summary>
		/// Creates a property set from the given context
		/// </summary>
		public PropertySet(TTContext c){
			this.reference = Native.hPropertySet_Create ();
			this.ctx = c;
			if (this.ctx == null)
				throw new LibTelltaleException ("No context passed");
		}

		public PropertySet(IntPtr ptr)
        {
			this.reference = ptr;
        }

		/// <summary>
		/// Gets the parent properties of this property set
		/// </summary>
		public Handle[] GetParentProperties(){
			IntPtr parents = this.Parents ();
			Handle[] ret = new Handle[Native.Abstract_DCArray_Size(parents)];
			for (int i = 0; i < ret.Length; i++) {
				ret [i] = new Handle(Native.Abstract_DCArray_At (parents, i));
			}
			return ret;
		}

		/// <summary>
		/// Adds the property set file name as a parent to this one. Must be a file name with the .prop (eg 'module_scene.prop')
		/// </summary>
		/// <param name="propName"></param>
		public void AddParentProperties(string propName){
			Native.Abstract_DCArray_Add(this.Parents(), new Handle(propName).Interal_Get());
		}

		/// <summary>
		/// The amount of parent properties in this property set
		/// </summary>
		/// <returns></returns>
		public int GetNumParentProperties(){
			return Native.Buffer_DCArray_Size (this.Parents());
		}

		/// <summary>
		/// Gets the parent property at the given index, useful for iteration
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string GetParentProperties(int index){
			return Marshal.PtrToStringAnsi(Native.Buffer_DCArray_At(this.Parents (), index));
		}

		/// <summary>
		/// Clears all the parent properties in this propertyset (memory is cleared too)
		/// </summary>
		public void ClearParentProperties(){
			Native.Buffer_DCArray_Clear (this.Parents ());
		}

		/// <summary>
		/// Seta a property flag for this property set.
		/// </summary>
		public void SetFlag(uint flag){
			Native.hPropertySet_SetFlags (this.reference, this.GetFlags () | flag);
		}

		/// <summary>
		/// Sets the property set version. Can be either 0, 1 or 2. This is generaally unused. Use version 2 for all new games
		/// </summary>
		/// <param name="version"></param>
		public void SetVersion(int version){
			Native.hPropertySet_SetVersion (this.reference, version);
		}

		/// <summary>
		/// Sets the flags for this property set.
		/// </summary>
		public void SetFlags(uint flags){
			Native.hPropertySet_SetFlags (this.reference, flags);
		}

		/// <summary>
		/// Removes a parent property file name
		/// </summary>
		/// <param name="propName"></param>
		public void RemoveParentProperties(string propName){
			int index = -1;
			int i = 0;
			foreach(Handle p in this.GetParentProperties()){
				if(p.GetName().Equals(propName)) {
					index = i;
					break;
				}
				i++;
			}
			Native.Abstract_DCArray_Remove (this.Parents (), Native.Abstract_DCArray_At (this.Parents (), index));
		}

		protected IntPtr Parents(){
			return Native.hPropertySet_GetParents (this.reference);
		}

		protected TTContext ctx;

		protected IntPtr reference;

		public override TTContext GetContext(){
			return this.ctx;
		}

		public override int Open(){
			return Native.PropertySet_Open (this.ctx.Internal_Get (), this.reference);
		}

		/// <summary>
		/// Gets the flags of this property set
		/// </summary>
		public uint GetFlags(){
			return Native.hPropertySet_GetFlags (this.reference);
		}

		/// <summary>
		/// Gets the version of this propertyset. Normally 2
		/// </summary>
		public int GetVersion(){
			return Native.hPropertySet_GetVersion (this.reference);
		}

		public override bool Flush(){
			return Native.PropertySet_Flush (this.ctx.Internal_Get (), this.reference) != 0;
		}

		public override IntPtr Internal_Get(){
			return this.reference;
		}

		/// <summary>
		/// Deletes this PropertySet and all memory associated (in the entries too.)
		/// </summary>
		public override void Dispose(){
			if (this.disposed) return;
			Native.hPropertySet_Delete (this.reference);
			this.disposed = true;
		}

	}
}

