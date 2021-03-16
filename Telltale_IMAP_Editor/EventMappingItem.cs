using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibTelltale;
using System.Runtime.InteropServices;

namespace Telltale_IMAP_Editor
{
    public class EventMappingItem
    {
        public InputMapper.EventMapping eventMapping;

        public string InputKeyCode 
        { 
            get
            {
                return eventMapping.mMapping.mInputCode.ToString();
            }
        }

        public string EventType
        {
            get
            {
                return eventMapping.mMapping.mEvent.ToString();
            }
        }

        public string LuaScriptFunctionName
        {
            get
            {
                return InputMapper.GetScriptFunction(eventMapping);
            }
        }

        public string ControllerIndexOverride
        {
            get
            {
                return eventMapping.mMapping.mControllerIndexOverride.ToString();
            }
        }

        public EventMappingItem(InputMapper.EventMapping eventMapping)
        {
            this.eventMapping = eventMapping;
        }

        public void ApplyChanges(string user_InputKeyCode, string user_EventType, string user_LuaScriptFunctionName, string user_ControllerIndexOverride)
        {
            //set the values
            eventMapping.mMapping.mInputCode = Parse_InputCode_String(user_InputKeyCode);
            eventMapping.mMapping.mEvent = Parse_EventType_String(user_EventType);
            eventMapping.mMapping.mControllerIndexOverride = Parse_ControllerIndexOverride_String(user_ControllerIndexOverride);
            eventMapping.mMapping.mScriptFunction = Parse_LuaScriptFunctionName_String(user_LuaScriptFunctionName);
            //MetaStreamedFile_InputMapper.SetScriptFunction(eventMapping, user_LuaScriptFunctionName);

            //update the event mapping object
            InputMapper.UpdateEventMapping(eventMapping);
        }

        public static InputMapper.InputCode Parse_InputCode_String(string inputString)
        {
            object attemptedParse = Enum.Parse(typeof(InputMapper.InputCode), inputString);

            return (InputMapper.InputCode)attemptedParse;
        }

        public static InputMapper.Event Parse_EventType_String(string inputString)
        {
            object attemptedParse = Enum.Parse(typeof(InputMapper.Event), inputString);

            return (InputMapper.Event)attemptedParse;
        }

        public static IntPtr Parse_LuaScriptFunctionName_String(string inputString)
        {
            return Marshal.StringToHGlobalAnsi(inputString);
        }

        public static int Parse_ControllerIndexOverride_String(string inputString)
        {
            return int.Parse(inputString);
        }
    }
}
