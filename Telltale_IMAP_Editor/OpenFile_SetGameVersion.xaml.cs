using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telltale_IMAP_Editor.Utillities;
using ControlzEx.Theming;
using System.IO;

namespace Telltale_IMAP_Editor
{
    /// <summary>
    /// Interaction logic for OpenFile_SetGameVersion.xaml
    /// </summary>
    public partial class OpenFile_SetGameVersion
    {
        //our main editor object
        private Editor editor;

        /// <summary>
        /// Opens a window for opening a new imap file
        /// </summary>
        /// <param name="editor"></param>
        public OpenFile_SetGameVersion(Editor editor)
        {
            //xaml initalization
            InitializeComponent();

            //get the editor object from the main window
            this.editor = editor;

            //initalize this window
            InitalizeWindow();

            //update the UI elements
            UpdateUI();
        }

        /// <summary>
        /// Initalizes the window and it's objects.
        /// </summary>
        public void InitalizeWindow()
        {
            //fill the combobox ui with our list of game versions
            ui_versions_combobox.ItemsSource = SetGameVersion.Get_Versions_ToStringList(true);
        }

        /// <summary>
        /// Updates the UI elements for this window
        /// </summary>
        public void UpdateUI()
        {
            //if the user has a valid selected item
            bool versionSelected = ui_versions_combobox.SelectedItem != null;

            //enable the open button only if they selected a game version
            ui_open_button.IsEnabled = versionSelected;
        }

        /// <summary>
        /// Does a couple of checks to make sure that the file path for the imap is valid and proper.
        /// </summary>
        /// <returns></returns>
        private bool OpenPrecheck()
        {
            //get our file path
            string filePath = ui_path_textbox.Text;

            //if the user didn't even bother to hit browse
            if(string.IsNullOrEmpty(filePath))
            {
                MessageBoxes.Error("No .imap file path was given! Please browse for an .imap file.", "Can't Open");
                return false; //precheck failed
            }

            //if the user somehow selected a file that doesn't exist
            if(File.Exists(filePath) == false)
            {
                MessageBoxes.Error("Given .imap file path does not exist! Please browse for an .imap.", "Can't Open");
                return false; //precheck failed
            }

            //otherwise, prechecks passed
            return true;
        }

        /// <summary>
        /// Opens a OpenFileDialog for getting an .imap file
        /// </summary>
        private void Get_IMAP_Path()
        {
            string filePath = ""; //the path of our soon to be .imap
            string filter = "Input Mapping files (*.imap)|*.imap"; //we only want .imaps

            //open a window for selecting a file
            IOManagement.GetFilePath(ref filePath, filter, "Open an .imap file.");

            //if the user changed their mind then don't continue cause we got nothing
            if (string.IsNullOrEmpty(filePath))
                return;

            //otherwise, show the path in our UI element and store it there
            ui_path_textbox.Text = filePath;
        }

        //---------------------------------- XAML FUNCTIONS ----------------------------------

        private void ui_versions_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update the UI elements
            UpdateUI();
        }

        private void ui_open_button_Click(object sender, RoutedEventArgs e)
        {
            //do some prechecks to make sure that we have everything
            if (OpenPrecheck() == false)
                return;

            //parse the selected value
            SetGameVersion.Versions selectedVersion = SetGameVersion.Get_Versions_ParseIntValue(ui_versions_combobox.SelectedIndex);

            //call the main editor function for opening the imap file
            editor.Open_IMAP(ui_path_textbox.Text, SetGameVersion.Get_GameID_FromVersion(selectedVersion));

            //close this window since we are done
            Close();
        }

        private void ui_cancel_button_Click(object sender, RoutedEventArgs e)
        {
            //close this window since we are done
            Close();
        }

        private void ui_pathBrowse_button_Click(object sender, RoutedEventArgs e)
        {
            //call our function for getting an imap file
            Get_IMAP_Path();
        }

        private void ui_path_textbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //call our function for getting an imap file
            Get_IMAP_Path();
        }
    }
}
