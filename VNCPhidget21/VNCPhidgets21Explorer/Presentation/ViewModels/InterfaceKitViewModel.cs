using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class InterfaceKitViewModel : EventViewModelBase, IInterfaceKitViewModel, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public InterfaceKitViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // TODO(crhodes)
            //

            OpenInterfaceKitCommand = new DelegateCommand(OpenInterfaceKit, OpenInterfaceKitCanExecute);
            CloseInterfaceKitCommand = new DelegateCommand(CloseInterfaceKit, CloseInterfaceKitCanExecute);
            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);

            // TODO(crhodes)
            // For now just hard code this.  Can have UI let us choose later.

            ConfigFileName = "phidgetconfig.json";
            LoadUIConfig();

            //SayHelloCommand = new DelegateCommand(
            //    SayHello, SayHelloCanExecute);

            Message = "InterfaceKitViewModel says hello";           

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }
        private void LoadUIConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(ConfigFileName);

            Resources.PhidgetConfig? phidgetConfig = JsonSerializer.Deserialize<Resources.PhidgetConfig>(jsonString);
            this.Hosts2 = phidgetConfig.Hosts.ToList<Resources.Host>();
            this.Sensors2 = phidgetConfig.Sensors.ToList<Resources.Sensor>();

            //LoggingUIConfig = jsonLoggingUIConfig.ConvertJSONToLoggingUIConfig();

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums


        #endregion

        #region Structures


        #endregion

        #region Fields and Properties


        private Resources.PhidgetConfig _phidgetConfig;
        public Resources.PhidgetConfig PhidgetConfig
        {
            get => _phidgetConfig;
            set
            {
                if (_phidgetConfig == value)
                    return;
                _phidgetConfig = value;
                OnPropertyChanged();
            }
        }

        //private Resources.Host[] _hosts;
        //public Resources.Host[] Hosts
        //{
        //    get => _hosts;
        //    set
        //    {
        //        if (_hosts == value)
        //            return;
        //        _hosts = value;
        //        OnPropertyChanged();
        //    }
        //}
        private IEnumerable<Resources.Host> _Hosts2;
        public IEnumerable<Resources.Host> Hosts2
        {
            get
            {
                if (null == _Hosts2)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_Hosts =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Hosts").Elements("Host")
                    //    select new Host(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _Hosts2;
            }

            set
            {
                _Hosts2 = value;
                OnPropertyChanged();
            }
        }

        private Resources.Host _selectedHost;
        public Resources.Host SelectedHost
        {
            get => _selectedHost;
            set
            {
                if (_selectedHost == value)
                    return;
                _selectedHost = value;
                InterfaceKits2 = _selectedHost.InterfaceKits.ToList<Resources.InterfaceKit>();
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.InterfaceKit> _InterfaceKits2;
        public IEnumerable<Resources.InterfaceKit> InterfaceKits2
        {
            get
            {
                if (null == _InterfaceKits2)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_InterfaceKits =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("InterfaceKits").Elements("InterfaceKit")
                    //    select new InterfaceKit(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _InterfaceKits2;
            }

            set
            {
                _InterfaceKits2 = value;
                OnPropertyChanged();
            }
        }

        private Resources.InterfaceKit _selectedInterfaceKit;
        public Resources.InterfaceKit SelectedInterfaceKit
        {
            get => _selectedInterfaceKit;
            set
            {
                if (_selectedInterfaceKit == value)
                    return;
                _selectedInterfaceKit = value;

                OnPropertyChanged();
            }
        }

        private InterfaceKitEx _activeInterfaceKit;
        public InterfaceKitEx ActiveInterfaceKit
        {
            get => _activeInterfaceKit;
            set
            {
                if (_activeInterfaceKit == value)
                    return;
                _activeInterfaceKit = value;

                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.Sensor> _Sensors2;
        public IEnumerable<Resources.Sensor> Sensors2
        {
            get
            {
                if (null == _Sensors2)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_Sensors =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Sensors").Elements("Sensor")
                    //    select new Sensor(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _Sensors2;
            }

            set
            {
                _Sensors2 = value;
                OnPropertyChanged();
            }
        }

        //private Resources.Sensor[] _sensors;

        //public Resources.Sensor[] Sensors
        //{
        //    get => _sensors;
        //    set
        //    {
        //        if (_sensors == value)
        //            return;
        //        _sensors = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public ICommand SayHelloCommand { get; private set; }


        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged();
            }
        }

        #region Hosts

        private Host _HOST;
        public Host HOST
        {
            get { return _HOST; }
            set { _HOST = value; }
        }

        private Dictionary<string, Host> _HostD;
        public Dictionary<string, Host> HostD
        {
            get
            {
                if (_HostD == null)
                {
                    _HostD = new Dictionary<string, Host>();
                }
                return _HostD;
            }
            set
            {
                _HostD = value;
            }
        }


        private IEnumerable<Host> _Hosts;
        public IEnumerable<Host> Hosts
        {
            get
            {
                if (null == _Hosts)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_Hosts =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Hosts").Elements("Host")
                    //    select new Host(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _Hosts;
            }

            set
            {
                _Hosts = value;
            }
        }

        #endregion

        #region InterfaceKits

        private InterfaceKitEx _IK;
        public InterfaceKitEx IK
        {
            get { return _IK; }
            set { _IK = value; }
        }

        private Dictionary<string, InterfaceKitEx> _InterfaceKitsD;
        public Dictionary<string, InterfaceKitEx> InterfaceKitsD
        {
            get
            {
                if (null == _InterfaceKitsD)
                {
                    _InterfaceKitsD = new Dictionary<string, InterfaceKitEx>();
                }
                return _InterfaceKitsD;
            }
            set
            {
                _InterfaceKitsD = value;
            }
        }

        private Collection<string> _InterfaceKits;
        public Collection<string> InterfaceKits
        {
            get
            {
                if (null == _InterfaceKits)
                {
                    _InterfaceKits = new Collection<string>();
                }
                return _InterfaceKits;
            }
            set
            {
                _InterfaceKits = value;
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        #region Commands


        private string _ConfigFileName;

        public string ConfigFileName
        {
            get => _ConfigFileName;
            set
            {
                if (_ConfigFileName == value) return;
                _ConfigFileName = value;
                OnPropertyChanged();
            }
        }

        public string ConfigFIleNameToolTip { get; set; }

        #region Command ConfigFIleName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Message = "ConfigFileName_DoubleClick";
        }

        #endregion

        #region OpenInterfaceKit Command

        public DelegateCommand OpenInterfaceKitCommand { get; set; }
        public string OpenInterfaceKitContent { get; set; } = "Open";
        public string OpenInterfaceKitToolTip { get; set; } = "OpenInterfaceKit ToolTip";

        // Can get fancy and use Resources
        //public string OpenInterfaceKitContent { get; set; } = "ViewName_OpenInterfaceKitContent";
        //public string OpenInterfaceKitToolTip { get; set; } = "ViewName_OpenInterfaceKitContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenInterfaceKitContent">OpenInterfaceKit</system:String>
        //    <system:String x:Key="ViewName_OpenInterfaceKitContentToolTip">OpenInterfaceKit ToolTip</system:String>  

        public void OpenInterfaceKit()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenInterfaceKit";

            ActiveInterfaceKit = new InterfaceKitEx(
                SelectedHost.IPAddress, 
                SelectedHost.Port, 
                SelectedInterfaceKit.SerialNumber,
                SelectedInterfaceKit.Enable, 
                SelectedInterfaceKit.Embedded);

            ActiveInterfaceKit.Open();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenInterfaceKitEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenInterfaceKitEvent>().Publish(
            //      new OpenInterfaceKitEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenInterfaceKitEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenInterfaceKitEvent>().Subscribe(OpenInterfaceKit);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool OpenInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region CloseInterfaceKit Command

        public DelegateCommand CloseInterfaceKitCommand { get; set; }
        public string CloseInterfaceKitContent { get; set; } = "Close";
        public string CloseInterfaceKitToolTip { get; set; } = "CloseInterfaceKit ToolTip";

        // Can get fancy and use Resources
        //public string CloseInterfaceKitContent { get; set; } = "ViewName_CloseInterfaceKitContent";
        //public string CloseInterfaceKitToolTip { get; set; } = "ViewName_CloseInterfaceKitContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseInterfaceKitContent">CloseInterfaceKit</system:String>
        //    <system:String x:Key="ViewName_CloseInterfaceKitContentToolTip">CloseInterfaceKit ToolTip</system:String>  

        public void CloseInterfaceKit()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseInterfaceKit";

            ActiveInterfaceKit.Close();
            ActiveInterfaceKit = null;

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseInterfaceKitEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseInterfaceKitEvent>().Publish(
            //      new CloseInterfaceKitEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseInterfaceKitEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseInterfaceKitEvent>().Subscribe(CloseInterfaceKit);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool CloseInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        // End Cut One


        #endregion

        #endregion

        #region Public Methods


        #endregion

        #region Protected Methods


        #endregion

        #region Private Methods

        #region SayHello Command

        private void SayHello()
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = $"Hello from {this.GetType()}";

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }
        
        private bool SayHelloCanExecute()
        {
            return true;
        }
        
        #endregion
        
        #endregion

        #region IInstanceCount

        private static int _instanceCountVM;

        public int InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
