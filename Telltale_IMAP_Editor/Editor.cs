using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibTelltale;
using System.IO;

namespace Telltale_IMAP_Editor
{
    public class Editor
    {
        //our custom list of the event mapping objects
        public List<EventMappingItem> eventMappingItems;
        
        //the path of the currently opened file (will be blank/null if there isn't one)
        public string currentlyOpenFilePath;
        public string currentlyOpenFilePath_gameID;

        //LibTelltale objects
        //our imap file
        private InputMapper inputMapper;
        //byte stream for reading the imap
        private ByteStream byteStream;
        //byte stream for writing to the imap file
        private ByteOutStream byteOutStream;
        //our context for reading/writing
        private TTContext context;

        /// <summary>
        /// Checks if there is a file that is currently open
        /// </summary>
        /// <returns></returns>
        public bool FileCurrentlyOpen()
        {
            return !string.IsNullOrEmpty(currentlyOpenFilePath) && File.Exists(currentlyOpenFilePath);
        }

        private void Open_Precheck()
        {
            //if we already have a context object, dispose of it
            if (context != null)
                context.Dispose();

            //if we already have an input mapper object, dispose of it
            if (inputMapper != null)
                inputMapper.Dispose();

            if (eventMappingItems != null)
                eventMappingItems.Clear();
        }

        /// <summary>
        /// Opens an IMAP file with the given path
        /// </summary>
        /// <param name="path"></param>
        public void Open_IMAP(string path, string gameID)
        {
            Open_Precheck();

            //tell LibTelltale what the game version will be for the file to be opened
            context = new TTContext(gameID);

            //creates a bytestream of the given file
            byteStream = new ByteStream(path);

            //sets the stream on the context object to the new bytestream of the file we want to open
            context.NextStream(byteStream, false);

            //create our input mapper object
            inputMapper = new InputMapper(context);

            //open the file (should return 0 if sucessful)
            if (inputMapper.Open() != 0)
                return;

            //initalize our custom list of event mapping items
            eventMappingItems = new List<EventMappingItem>();

            //run a loop for the amount of input mappins there are, and fill up the list
            for (int i = 0; i < inputMapper.GetMappings(); i++)
            {
                //get the mapping from the file at the index
                InputMapper.EventMapping eventMapping = inputMapper.GetMapping((uint)i);

                //create our custom event mapping item object
                EventMappingItem eventMappingItem = new EventMappingItem(eventMapping);

                //add it to our list
                eventMappingItems.Add(eventMappingItem);
            }

            //set our currently open file path to the sucessfully opened one
            currentlyOpenFilePath = path;
            currentlyOpenFilePath_gameID = gameID;
        }

        /// <summary>
        /// Saves and overwrites the original currently opened file.
        /// </summary>
        public void Save_IMAP()
        {
            //creates a bytestream of the given file
            byteOutStream = new ByteOutStream(currentlyOpenFilePath);

            //get the file name of the currently opened file path
            string fileName = Path.GetFileName(currentlyOpenFilePath);

            //create a writing stream
            context.NextWrite(byteOutStream, fileName, true);

            //writes the stream to the attached context
            inputMapper.Flush();

            //finish the writing of the file
            context.FinishCurrentWrite(true, false);
        }

        /// <summary>
        /// Saves a different copy of the originaly opened file under a different path
        /// </summary>
        public void SaveAs_IMAP(string newFilePath)
        {
            //creates a bytestream of the given file
            byteOutStream = new ByteOutStream(newFilePath);

            //get the file name of the currently opened file path
            string fileName = Path.GetFileName(newFilePath);

            //create a writing stream
            context.NextWrite(byteOutStream, fileName, true);

            //writes the stream to the attached context
            inputMapper.Flush();

            //finish the writing of the file
            context.FinishCurrentWrite(true, false);
        }

        /// <summary>
        /// Removes an IMAP entry at the given index
        /// </summary>
        /// <param name="index"></param>
        public void Remove_IMAP_Entry(int index)
        {
            //get our event map at the given index
            InputMapper.EventMapping eventMapping = inputMapper.GetMapping((uint)index);

            //remove the mapping
            inputMapper.RemoveMapping(eventMapping);

            //remove the item from our custom list
            eventMappingItems.RemoveAt(index);
        }

        /// <summary>
        /// Updates an IMAP entry at the given index with new changes
        /// </summary>
        /// <param name="index"></param>
        public void Update_IMAP_Entry(int index, string inputCode, string eventType, string luaFunctionName, string controllerIndex)
        {
            //get the item from our list at the given index
            EventMappingItem eventMappingItem = eventMappingItems[index];

            //apply changes on the item that was at the given index
            eventMappingItem.ApplyChanges(inputCode, eventType, luaFunctionName, controllerIndex);
        }

        /// <summary>
        /// Adds a new IMAP entry
        /// </summary>
        public void Add_IMAP_Entry()
        {
            //create some default values
            InputMapper.InputCode new_inputCode = InputMapper.InputCode.BACKSPACE;
            InputMapper.Event new_eventType = InputMapper.Event.BEGIN;
            string new_luaFunctionName = "New Lua Function Name";
            int new_controllerIndexOverride = -1;

            //create the object with the default values
            InputMapper.EventMapping eventMapping = InputMapper.CreateMapping(new_luaFunctionName, new_inputCode, new_eventType, new_controllerIndexOverride);

            //add it to the file
            inputMapper.AddMapping(eventMapping);

            //create a custom event mapping item object for our list
            EventMappingItem eventMappingItem = new EventMappingItem(eventMapping);

            //add it to our custom list
            eventMappingItems.Add(eventMappingItem);
        }

        /// <summary>
        /// Housekeeping for cleaning, clearing any objects to prevent any memory issues.
        /// </summary>
        public void CloseApplication()
        {
            //if we already have a context object, dispose of it
            if (context != null)
                context.Dispose();

            //if we already have an input mapper object, dispose of it
            if (inputMapper != null)
                inputMapper.Dispose();

            //if we already have a reading byte stream object, dispose of it
            if (byteStream != null)
                byteStream.Dispose();

            //f we already have a writing byte stream object, dispose of it
            if (byteOutStream != null)
                byteOutStream.Dispose();

            if (eventMappingItems != null)
                eventMappingItems.Clear();
        }
    }
}
