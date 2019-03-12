using System;
using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;
using System.Threading.Tasks;
using System.Threading;

namespace RvtConverter
{
    /// <summary>
    /// plugin属性将该类定义为一个插件.
    /// 
    /// Plugin Id: used to uniquely identify the plugin when combined with the Developer Id.
    /// 
    /// Developer Id: used in combination with the plugin Id to uniquely identify the plugin.
    /// 
    /// DisplayName: 
    /// </summary>
    [Plugin("RvtConverterCommandHandler", "ADSK", DisplayName = "轻量化引擎")]
    /// <summary>
    /// An attribute identifying the name file that defines the localised text for 
    /// ribbon tabs and commands (see following attributes). Strings defined in the 
    /// name file will override any properties defined on the attributes. Text defined
    /// in the xaml layout file overrides strings defined in the name file.
    /// </summary>
    [Strings("CustomRibbon.name")]
    /// <summary>
    /// An attribute identifying a xaml file that defines the layout of associated
    /// ribbon tabs and commands (see following attributes).
    /// </summary>
    [RibbonLayout("CustomRibbon.xaml")]
    /// <summary>
    /// An attribute that defines a ribbon tab - must be accomnpanied by a 
    /// RibbonLayout attribute in order for the tab to appear in the GUI.
    /// 
    /// Tab Id: used to uniquely the ribbon tab within the plugin, and must 
    /// correspond to the tab Id used in the xaml layout file.
    /// 
    /// DisplayName: provides text for the ribbon tab, unless overriddden in 
    /// the name file (localised) or the localised xaml layout file.
    /// 
    /// LoadForCanExecute: if True, the plugin is fully loaded to ensure that the
    /// CanExecuteRibbonTab method is called. The CanExecuteRibbonTab method can 
    /// be used to make a ribbon tab contextual i.e. only visible when
    /// specified conditions are met. Otherwise the tab is visible by default.
    /// </summary>
    [RibbonTab("ID_CustomTab_1", DisplayName = "轻量化引擎")]
    [RibbonTab("ID_CustomTab_2", DisplayName = "AACustom Tab 2 - non-localised", LoadForCanExecute = true)]
    /// <summary>
    /// Defines a command that will perform an action within the application.
    /// 
    /// Command Id: used to uniquely identify the command within the plugin,
    /// and must correspond to the button Id, defined in the xaml layout definition.
    /// 
    /// DisplayName: Provides text for the command wherever it appears in the GUI,
    /// unless overridden by the button Text, defined in the xaml ribbon definition.
    /// The advantage of defining button Text in the xaml is that the plugin can be 
    /// localized by providing language-specific xaml files.
    ///
    /// Icon: defines the standard image used for the command wherever it appears 
    /// in the GUI, unless overridden by the button Image, defined in the xaml ribbon
    /// definition. This must be a 16x16 pixel image and it should be located next to 
    /// the plugin dll, or in an "Images" subdirectory next to the plugin dll.
    /// 
    /// LargeIcon: defines the large image used for the command when the command is 
    /// displayed as a Large button, as defined in the xaml ribbon definition. It must
    /// be a 32x32 pixel image. It must be located as per Icon.
    /// 
    /// CanToggle: defines whether the button as it appears in the ribbon can toggle 
    /// on and off. Specify True to make the button toggle.
    /// 
    /// ToolTip: defines text that will appear when the user hovers over the command
    /// button in the ribbon. This is not localised. Localised tooltips can be provided
    /// if they are defined in a name file.
    /// 
    /// ExtendedToolTip: defines additonal text that describes the purpose of the command
    /// in more detail than ToolTip. This is not localised. Localised tooltips can be provided
    /// if they are defined in a name file.
    /// 
    /// Shortcut: defines a keyboard shortcut that can be used to activate the command 
    /// e.g. Ctrl+B. If the shortcut conflicts with a shortcut already defined in the 
    /// application it will not be set. 
    /// 
    /// ShortcutWindowTypes: If you have defined a DockPanePlugin, you can specify that
    /// the Shortcut only applies when that window is active. Use the Id for the 
    /// DockPanePlugin.
    /// 
    /// CallCanExecute: Determines the conditions in which CanExecuteCommand should be 
    /// called. If the CanExecuteCommand is not called the default command state is disabled. 
    /// Options are: 
    /// Always (i.e. the CanExecuteCommand is always called, regardless of model state)  
    /// DocumentNotClear (i.e. the CanExecuteCommand is called only when a model is open)
    /// CurrentSelectionSingle (i.e. the CanExecuteCommand is called only when a single item
    /// is selected in the open model)
    /// CurrentSelectionMultiple (the CanExecuteCommand is called only when multiple items 
    /// are selected in the open model)
    /// 
    /// LoadForCanExecute: Commands are enabled by default, but if this is False the
    /// plugin will not be fully loaded until the first time a command is executed. If
    /// this is True the plugin will be fully loaded at application startup in order to
    /// call the CanExecuteCommand method defined by the plugin author.
    /// </summary>
    [Command("ID_Button_1", DisplayName = "启动", Icon = "run_16.ico", LargeIcon = "run_32.ico", ToolTip = "启动服务", ExtendedToolTip = "xxxxxxxxxxxxxxx")]
    [Command("ID_Button_2", CanToggle = true, Shortcut = "Shift+X")]
    [Command("ID_Button_3", CanToggle = true, Shortcut = "Shift+Z")]
    [Command("ID_Button_4", LoadForCanExecute = true)]
    [Command("ID_Button_5")]
    [Command("ID_Button_6")]
    [Command("ID_Button_7")]
    [Command("ID_Button_8")]
    [Command("ID_Button_A", CanToggle = true)]
    [Command("ID_Button_B", CanToggle = true)]
    [Command("ID_Button_C", CanToggle = true)]
    [Command("ID_Button_D", CanToggle = true)]
    [Command("ID_Button_E", CanToggle = true)]
    [Command("ID_Button_F", CanToggle = true)]
    [Command("ID_Button_G", CanToggle = true)]
    [Command("ID_Button_H", CanToggle = true)]
    public class RvtConverterCommandHandler : CommandHandlerPlugin
    {
        /// <summary>
        /// 初始化状态
        /// </summary>
        public RvtConverterCommandHandler()
        {

            m_button2_on = false;
            m_button3_on = false;
            m_radio_group_DEF = "ID_Button_D";
            m_buttonG_on = false;
            m_buttonH_on = false;
            Log.InitLog4Net();
            Watcher.FileWatcher.Strat();
        }

        /// <summary>
        /// Executes a command when a button in the ribbon is pressed.
        /// </summary>
        /// <param name="commandId">Identifies the command associated with the button 
        /// that was pressed, by the Id defined in the command attribute.</param>
        /// <param name="parameters">Not currently used by Navisworks. If command is
        /// invoked programmatically by plugin author it can be used to pass additional
        /// information.</param>
        /// <returns>Not used by Navisworks. If command is invoked programmatically by 
        /// plugin author then it can be used to return additional information.</returns>
        public override int ExecuteCommand(string commandId, params string[] parameters)
        {
            switch (commandId)
            {
                case "ID_Button_1":
                    {

                        Watcher.FileWatcher.Work();
                        m_button1_on = !m_button1_on;
                        break;
                    }
                case "ID_Button_2":
                    {
                        m_button2_on = !m_button2_on;
                        MessageBox.Show("Custom Tab 2 visibility changing!");
                        break;
                    }
                case "ID_Button_3":
                    {
                        m_button3_on = !m_button3_on;
                        MessageBox.Show("Button 4 visibility changing!");
                        break;
                    }
                case "ID_Button_D":
                case "ID_Button_E":
                case "ID_Button_F":
                    {
                        m_radio_group_DEF = commandId;
                        break;
                    }
                case "ID_Button_G":
                    {
                        m_buttonG_on = !m_buttonG_on;
                        break;
                    }
                case "ID_Button_H":
                    {
                        m_buttonH_on = !m_buttonH_on;
                        break;
                    }
                default:
                    {
                        MessageBox.Show("You have clicked on the command with ID = '" + commandId + "'");
                        break;
                    }
            }

            return 0;
        }

        /// <summary>
        /// 通过ID更新状态，初始化时自动执行
        /// </summary>
        /// <param name="commandId">The Id of the command being updated, as defined in 
        /// the command attribute.</param>
        /// <returns>The CommandState indicates if the command is enabled, checked (if a 
        /// toggle command) and visible. The associated button in the ribbon will be 
        /// displayed accordingly.</returns>
        public override CommandState CanExecuteCommand(String commandId)
        {
            CommandState state = new CommandState();
            switch (commandId)
            {
                // Button 1 is only enabled when Button 2 is toggled on.
                case "ID_Button_1":
                    {
                        // state.IsEnabled = m_button2_on;
                        state.IsEnabled = true;
                        state.IsChecked = m_button1_on;
                        if (m_button1_on)
                            state.OverrideDisplayName = "停止服务";
                        else
                            state.OverrideDisplayName = "启动服务";
                        break;
                    }
                // Button 2 is always enabled. It's toggle state is set according to m_button2_on.
                case "ID_Button_2":
                    {
                        state.IsEnabled = true;
                        state.IsChecked = m_button2_on;
                        // when m_button2_on is true, Custom Tab 2 should be visible, so we can update
                        // our button text if we wish
                        if (m_button2_on)
                            state.OverrideDisplayName = "Hide Tab 2";
                        else
                            state.OverrideDisplayName = "Show Tab 2";
                        break;
                    }
                // Button 3 is always enabled. It's toggle state is set according to m_button3_on.
                case "ID_Button_3":
                    {
                        state.IsEnabled = true;
                        state.IsChecked = m_button3_on;
                        // when m_button3_on is true, Button 4 should be visible, so we can update
                        // our button text if we wish
                        if (m_button3_on)
                            state.OverrideDisplayName = "Hide Button 4";
                        else
                            state.OverrideDisplayName = "Show Button 4";
                        break;
                    }
                // Button 4 is only visible and enabled when Button 3 is toggled on (i.e. IsChecked=True).
                case "ID_Button_4":
                    {
                        state.IsVisible = m_button3_on;
                        state.IsEnabled = m_button3_on;
                        break;
                    }
                // Toggle Buttons D, E and F are mutually exclusive in their toggle state.
                case "ID_Button_D":
                case "ID_Button_E":
                case "ID_Button_F":
                    {
                        state.IsEnabled = true;
                        state.IsChecked = (m_radio_group_DEF == commandId);
                        break;
                    }
                case "ID_Button_G":
                    {
                        state.IsEnabled = true;
                        state.IsChecked = m_buttonG_on;
                        break;
                    }
                case "ID_Button_H":
                    {
                        state.IsEnabled = true;
                        state.IsChecked = m_buttonH_on;
                        break;
                    }
                default:
                    {
                        state.IsVisible = true;
                        state.IsEnabled = true;
                        state.IsChecked = false;
                        break;
                    }
            }

            return state;
        }

        /// <summary>
        /// Override this method to display assistance to the user for the command with the specified Id.
        /// </summary>
        public override bool TryShowCommandHelp(String commandId)
        {
            MessageBox.Show("Showing Help for command with the Id " + commandId);
            return true;
        }

        /// <summary>
        /// Indicates if a ribbon should be visible or not. This is used for contextual
        /// tabs that should only be visibile under conditions defined by the plugin author.
        /// </summary>
        /// <param name="ribbonTabId">The Id of the ribbon tab, as defined in the attribute.</param>
        /// <returns>True indicates that ribbon tab is visible, otherwise false.</returns>
        public override bool CanExecuteRibbonTab(String ribbonTabId)
        {
            // The second ribbon tab is only visible when Button 2 is toggled on (i.e. m_button2_on is true).
            if (ribbonTabId.Equals("ID_CustomTab_2"))
            {
                return m_button2_on;
            }

            return true;
        }

        private bool m_button1_on;
        private bool m_button2_on;
        private bool m_button3_on;
        private bool m_buttonG_on;
        private bool m_buttonH_on;
        private String m_radio_group_DEF;
        private Thread workThread;
    }
}
