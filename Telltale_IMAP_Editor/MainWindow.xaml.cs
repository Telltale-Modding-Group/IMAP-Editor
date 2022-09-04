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
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibTelltale;
using ControlzEx.Theming;
using Telltale_IMAP_Editor.Utillities;

namespace Telltale_IMAP_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //main class for editor functionality
        private Editor editor;

        public string appTheme;
        private bool isLightTheme;

        //imap file window opener
        private OpenFile_SetGameVersion openFile_SetGameVersion;

        public MainWindow()
        {
            //xaml initalization
            InitializeComponent();

            //our app initalization
            InitalizeApplication();

            //update our UI
            UpdateUI();
        }

        /// <summary>
        /// Initalizes the application and its objects
        /// </summary>
        public void InitalizeApplication()
        {
            //initalize our main editor object
            editor = new Editor();

            //show our list of items
            ui_eventmappings_listview.ItemsSource = editor.eventMappingItems;

            //build an array of enum types for the input mapper from libtelltale
            Array inputCode_enumArray = Enum.GetValues(typeof(InputMapper.InputCode));
            Array event_enumArray = Enum.GetValues(typeof(InputMapper.Event));

            //assign the enum lists to the respective dropdowns
            ui_inputkeycode_combobox.ItemsSource = inputCode_enumArray;
            ui_eventtype_combobox.ItemsSource = event_enumArray;
        }

        /// <summary>
        /// Updates the UI elements with data from the main editor object
        /// </summary>
        public void UpdateUI()
        {
            //if the editor object doesn't exist for whatever reason, don't continue
            if (editor == null)
                return;

            //get the UI theme based on what is toggled
            appTheme = isLightTheme ? "Light.Blue" : "Dark.Blue";

            //set the theme of all of our windows to what we toggled
            ThemeManager.Current.ChangeTheme(this, appTheme);

            //show the path of the currently open imap file
            ui_filelocation_textbox.Text = editor.currentlyOpenFilePath;

            //do we have a file open?
            bool isFileOpen = editor.FileCurrentlyOpen();

            //make sure our UI matches our in editor event item list
            ui_eventmappings_listview.Items.Refresh();
            ui_eventmappings_listview.ItemsSource = editor.eventMappingItems;
            ui_eventmappings_listview.IsEnabled = isFileOpen;

            //do we have an item selected?
            bool isSelected = ui_eventmappings_listview.SelectedItem != null;
            Visibility isSelected_visible = ui_eventmappings_listview.SelectedItem != null ? Visibility.Visible : Visibility.Hidden;

            //enable these if an item is selected
            ui_eventtype_combobox.IsEnabled = isSelected;
            ui_inputkeycode_combobox.IsEnabled = isSelected;
            ui_controllerindexoverride_textbox.IsEnabled = isSelected;
            ui_luascriptfunctionname_textbox.IsEnabled = isSelected;

            ui_eventtype_combobox.Visibility = isSelected_visible;
            ui_inputkeycode_combobox.Visibility = isSelected_visible;
            ui_controllerindexoverride_textbox.Visibility = isSelected_visible;
            ui_luascriptfunctionname_textbox.Visibility = isSelected_visible;

            //if we have an item selected, fill out the UI with the data
            if (isSelected)
            {
                //get the event mapping object
                EventMappingItem eventMappingItem = (EventMappingItem)ui_eventmappings_listview.SelectedItem;

                //assign and show the data
                ui_eventtype_combobox.Text = eventMappingItem.EventType;
                ui_inputkeycode_combobox.Text = eventMappingItem.InputKeyCode;
                ui_luascriptfunctionname_textbox.Text = eventMappingItem.LuaScriptFunctionName;
                ui_controllerindexoverride_textbox.Text = eventMappingItem.ControllerIndexOverride;
            }

            //set the labels
            ui_eventtype_label.IsEnabled = isSelected;
            ui_inputkeycode_label.IsEnabled = isSelected;
            ui_luascriptfunctionname_label.IsEnabled = isSelected;
            ui_controllerindexoverride_label.IsEnabled = isSelected;

            ui_eventtype_label.Visibility = isSelected_visible;
            ui_inputkeycode_label.Visibility = isSelected_visible;
            ui_luascriptfunctionname_label.Visibility = isSelected_visible;
            ui_controllerindexoverride_label.Visibility = isSelected_visible;

            //enable the save changes button only when something is selected
            ui_eventmappings_savechanges_button.IsEnabled = isSelected;
            ui_eventmappings_savechanges_button.Visibility = isSelected_visible;

            //enable these elements only if there is a file open
            ui_eventmappings_add_button.IsEnabled = isFileOpen;
            ui_eventmappings_remove_button.IsEnabled = isFileOpen;
            ui_menu_file_save.IsEnabled = isFileOpen;
            ui_menu_file_saveAs.IsEnabled = isFileOpen;
            ui_filelocation_textbox.IsEnabled = isFileOpen;
        }

        /// <summary>
        /// Defocus effect for when a new window is shown
        /// </summary>
        private void UI_Effect_DeFocusWindow()
        {
            //create a blur effect
            System.Windows.Media.Effects.BlurEffect blurEffect = new System.Windows.Media.Effects.BlurEffect();
            blurEffect.Radius = 10;

            //assing the blur effect and darken the window a bit
            this.Effect = blurEffect;
            this.Opacity = 0.75;
        }

        /// <summary>
        /// Clears the effects applied to the window
        /// </summary>
        private void UI_Effect_ClearEffects()
        {
            //remove the effect and reset opacity
            this.Effect = null;
            this.Opacity = 1;
        }

        //-------------------------------- XAML FUNCTIONS --------------------------------

        private void ui_menu_file_open_Click(object sender, RoutedEventArgs e)
        {
            //de focus the window since we are showing a prompt
            UI_Effect_DeFocusWindow();

            //initalize the open file window
            openFile_SetGameVersion = new OpenFile_SetGameVersion(editor);
            ThemeManager.Current.ChangeTheme(openFile_SetGameVersion, appTheme); //change the theme to match
            openFile_SetGameVersion.ShowDialog(); //freezes this current window until the prompt is finished/closed

            //clear the de focus effect
            UI_Effect_ClearEffects();

            //update the UI to reflect any new changes
            UpdateUI();
        }

        private void ui_menu_file_save_Click(object sender, RoutedEventArgs e)
        {
            //call the main function in the editor for saving the imap file
            editor.Save_IMAP();

            //update the UI to reflect any new changes
            UpdateUI();
        }

        private void ui_menu_file_saveAs_Click(object sender, RoutedEventArgs e)
        {
            //defocus the window because we are showing a prompt
            UI_Effect_DeFocusWindow();

            string filePath = "";
            string filter = "Input Mapping files (*.imap)|*.imap"; //we only want imap files

            //opens a save file dialog
            IOManagement.SaveFilePath(ref filePath, filter, "Save an .imap file.");

            //if the user canceled, then don't continue
            if (string.IsNullOrEmpty(filePath))
            {
                //clear the focus effect
                UI_Effect_ClearEffects();

                //update the UI
                UpdateUI();

                //stop here
                return;
            }

            //call the editor function for saving as a different imap file
            editor.SaveAs_IMAP(filePath);

            //clear the focus effect
            UI_Effect_ClearEffects();

            //update the UI to reflect any new changes
            UpdateUI();
        }

        private void ui_eventmappings_remove_button_Click(object sender, RoutedEventArgs e)
        {
            //call the editor function for removing an imap entry
            editor.Remove_IMAP_Entry(ui_eventmappings_listview.SelectedIndex);

            //save the changes to file
            editor.Save_IMAP();

            //update the UI to reflect the changes
            UpdateUI();
        }

        private void ui_eventmappings_add_button_Click(object sender, RoutedEventArgs e)
        {
            //call the editor function for adding an imap entry
            editor.Add_IMAP_Entry();

            //newest added element is always at the bottom of the list
            int newestItemIndex = ui_eventmappings_listview.Items.Count - 1;

            //set the selected index to the newest item
            ui_eventmappings_listview.SelectedIndex = newestItemIndex;

            //update the UI to reflect the changes
            UpdateUI();
        }

        private void ui_eventmappings_savechanges_button_Click(object sender, RoutedEventArgs e)
        {
            //update the entry on the file
            editor.Update_IMAP_Entry(ui_eventmappings_listview.SelectedIndex, ui_inputkeycode_combobox.Text, ui_eventtype_combobox.Text, ui_luascriptfunctionname_textbox.Text, ui_controllerindexoverride_textbox.Text);

            //write the changes to file
            editor.Save_IMAP();

            //update the UI
            UpdateUI();
        }

        private void ui_eventmappings_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update the UI to reflect any new changes
            UpdateUI();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //cleans up any of the mess left behind
            editor.CloseApplication();

            //shut down the app (just in case the process is left running)
            Application.Current.Shutdown();
        }

        private void ui_eventmappings_contextmenu_add_Click(object sender, RoutedEventArgs e)
        {
            //call the editor function for adding an imap entry
            editor.Add_IMAP_Entry();

            //newest added element is always at the bottom of the list
            int newestItemIndex = ui_eventmappings_listview.Items.Count - 1;

            //set the selected index to the newest item
            ui_eventmappings_listview.SelectedIndex = newestItemIndex;

            //update the UI to reflect the changes
            UpdateUI();
        }

        private void ui_eventmappings_contextmenu_remove_Click(object sender, RoutedEventArgs e)
        {
            //call the editor function for removing an imap entry
            editor.Remove_IMAP_Entry(ui_eventmappings_listview.SelectedIndex);

            //save the changes to file
            editor.Save_IMAP();

            //update the UI to reflect the changes
            UpdateUI();
        }

        private void ui_filelocation_textbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //de focus the window since we are showing a prompt
            UI_Effect_DeFocusWindow();

            //initalize the open file window
            openFile_SetGameVersion = new OpenFile_SetGameVersion(editor);
            ThemeManager.Current.ChangeTheme(openFile_SetGameVersion, appTheme); //change the theme to match
            openFile_SetGameVersion.ShowDialog(); //freezes this current window until the prompt is finished/closed

            //clear the de focus effect
            UI_Effect_ClearEffects();

            //update the UI to reflect any new changes
            UpdateUI();
        }

        private void ui_window_help_button_Click(object sender, RoutedEventArgs e)
        {
            //our help URL
            string helpURL = "https://github.com/Telltale-Modding-Group/IMAP-Editor/wiki";

            //create a process that opens the URL with the user's default web browser
            var processStartInfo = new ProcessStartInfo
            {
                FileName = helpURL,
                UseShellExecute = true
            };

            //start the process
            Process.Start(processStartInfo);
        }

        private void ui_menu_toggleTheme_Click(object sender, RoutedEventArgs e)
        {
            //reverse the theme
            isLightTheme = !isLightTheme;

            //update the UI
            UpdateUI();
        }
    }
}
