using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

using Phidgets;

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

        #region Enums (none)


        #endregion

        #region Structures (none)


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

        #region InterfaceKit Phidget Properties

        private string _iKAddress;
        public string IkAddress
        {
            get => _iKAddress;
            set
            {
                if (_iKAddress == value)
                    return;
                _iKAddress = value;
                OnPropertyChanged();
            }
        }

        private bool? _iKAttached;
        public bool? IkAttached
        {
            get => _iKAttached;
            set
            {
                if (_iKAttached == value)
                    return;
                _iKAttached = value;
                OnPropertyChanged();
            }
        }

        private bool? _ikAttachedToServer;
        public bool? IkAttachedToServer
        {
            get => _ikAttachedToServer;
            set
            {
                if (_ikAttachedToServer == value)
                    return;
                _ikAttachedToServer = value;
                OnPropertyChanged();
            }
        }

        private string _ikClass;

        public string IkClass
        {
            get => _ikClass;
            set
            {
                if (_ikClass == value)
                    return;
                _ikClass = value;
                OnPropertyChanged();
            }
        }

        private string _ikID;
        public string IkID
        {
            get => _ikID;
            set
            {
                if (_ikID == value)
                    return;
                _ikID = value;
                OnPropertyChanged();
            }
        }


        private string _ikLabel;
        public string IkLabel
        {
            get => _ikLabel;
            set
            {
                if (_ikLabel == value)
                    return;
                _ikLabel = value;
                OnPropertyChanged();
            }
        }

        private string _ikLibraryVersion;
        public string IkLibraryVersion
        {
            get => _ikLibraryVersion;
            set
            {
                if (_ikLibraryVersion == value)
                    return;
                _ikLibraryVersion = value;
                OnPropertyChanged();
            }
        }

        private string _ikName;
        public string IkName
        {
            get => _ikName;
            set
            {
                if (_ikName == value)
                    return;
                _ikName = value;
                OnPropertyChanged();
            }
        }

        private int? _ikPort;
        public int? IkPort
        {
            get => _ikPort;
            set
            {
                if (_ikPort == value)
                    return;
                _ikPort = value;
                OnPropertyChanged();
            }
        }


        private int? _ikSerialNumber;

        public int? IkSerialNumber
        {
            get => _ikSerialNumber;
            set
            {
                if (_ikSerialNumber == value)
                    return;
                _ikSerialNumber = value;
                OnPropertyChanged();
            }
        }


        private string _ikServerID;
        public string IkServerID
        {
            get => _ikServerID;
            set
            {
                if (_ikServerID == value)
                    return;
                _ikServerID = value;
                OnPropertyChanged();
            }
        }


        private string _ikType;
        public string IkType
        {
            get => _ikType;
            set
            {
                if (_ikType == value)
                    return;
                _ikType = value;
                OnPropertyChanged();
            }
        }

        private int? _ikVersion;
        public int? IkVersion
        {
            get => _ikVersion;
            set
            {
                if (_ikVersion == value)
                    return;
                _ikVersion = value;
                OnPropertyChanged();
            }
        }



        #endregion

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

            ActiveInterfaceKit.Attach += ActiveInterfaceKit_Attach;
            ActiveInterfaceKit.Detach += ActiveInterfaceKit_Detach;

            ActiveInterfaceKit.OutputChange += ActiveInterfaceKit_OutputChange;
            ActiveInterfaceKit.InputChange += ActiveInterfaceKit_InputChange; ;
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

            OpenInterfaceKitCommand.RaiseCanExecuteChanged();
            CloseInterfaceKitCommand.RaiseCanExecuteChanged();

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ActiveInterfaceKit_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        {
            InterfaceKit ifk = (InterfaceKit)sender;

            switch (e.Index)
            {
                case 0:
                    DI0 = e.Value;
                    break;
                case 1:
                    DI1 = e.Value;
                    break;
                case 2:
                    DI2 = e.Value;
                    break;
                case 3:
                    DI3 = e.Value;
                    break;
                case 4:
                    DI4 = e.Value;
                    break;
                case 5:
                    DI5 = e.Value;
                    break;
                case 6:
                    DI6 = e.Value;
                    break;
                case 7:
                    DI7 = e.Value;
                    break;
                case 8:
                    DI8 = e.Value;
                    break;
                case 9:
                    DI9 = e.Value;
                    break;
                case 10:
                    DI10 = e.Value;
                    break;
                case 11:
                    DI11 = e.Value;
                    break;
                case 12:
                    DI12 = e.Value;
                    break;
                case 13:
                    DI13 = e.Value;
                    break;
                case 14:
                    DI14 = e.Value;
                    break;
                case 15:
                    DI15 = e.Value;
                    break;
            }
        }

        #region Digital Inputs

        private bool? _dI0;
        public bool? DI0
        {
            get => _dI0;
            set
            {
                if (_dI0 == value)
                    return;
                _dI0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI1;
        public bool? DI1
        {
            get => _dI1;
            set
            {
                if (_dI1 == value)
                    return;
                _dI1 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI2;
        public bool? DI2
        {
            get => _dI2;
            set
            {
                if (_dI2 == value)
                    return;
                _dI2 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI3;
        public bool? DI3
        {
            get => _dI3;
            set
            {
                if (_dI3 == value)
                    return;
                _dI3 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI4;
        public bool? DI4
        {
            get => _dI4;
            set
            {
                if (_dI4 == value)
                    return;
                _dI4 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI5;
        public bool? DI5
        {
            get => _dI5;
            set
            {
                if (_dI5 == value)
                    return;
                _dI5 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI6;
        public bool? DI6
        {
            get => _dI6;
            set
            {
                if (_dI6 == value)
                    return;
                _dI6 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI7;
        public bool? DI7
        {
            get => _dI7;
            set
            {
                if (_dI7 == value)
                    return;
                _dI7 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI8;
        public bool? DI8
        {
            get => _dI8;
            set
            {
                if (_dI8 == value)
                    return;
                _dI0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI9;
        public bool? DI9
        {
            get => _dI9;
            set
            {
                if (_dI9 == value)
                    return;
                _dI9 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI10;
        public bool? DI10
        {
            get => _dI10;
            set
            {
                if (_dI10 == value)
                    return;
                _dI10 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI11;
        public bool? DI11
        {
            get => _dI11;
            set
            {
                if (_dI11 == value)
                    return;
                _dI11 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI12;
        public bool? DI12
        {
            get => _dI12;
            set
            {
                if (_dI12 == value)
                    return;
                _dI12 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI13;
        public bool? DI13
        {
            get => _dI13;
            set
            {
                if (_dI13 == value)
                    return;
                _dI13 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI14;
        public bool? DI14
        {
            get => _dI14;
            set
            {
                if (_dI14 == value)
                    return;
                _dI14 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI15;
        public bool? DI15
        {
            get => _dI15;
            set
            {
                if (_dI15 == value)
                    return;
                _dI15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Digital Outputs

        private bool? _dO0;
        public bool? DO0
        {
            get => _dO0;
            set
            {
                if (_dO0 == value)
                    return;
                _dO0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO1;
        public bool? DO1
        {
            get => _dO1;
            set
            {
                if (_dO1 == value)
                    return;
                _dO1 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO2;
        public bool? DO2
        {
            get => _dO2;
            set
            {
                if (_dO2 == value)
                    return;
                _dO2 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO3;
        public bool? DO3
        {
            get => _dO3;
            set
            {
                if (_dO3 == value)
                    return;
                _dO3 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO4;
        public bool? DO4
        {
            get => _dO4;
            set
            {
                if (_dO4 == value)
                    return;
                _dO4 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO5;
        public bool? DO5
        {
            get => _dO5;
            set
            {
                if (_dO5 == value)
                    return;
                _dO5 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO6;
        public bool? DO6
        {
            get => _dO6;
            set
            {
                if (_dO6 == value)
                    return;
                _dO6 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO7;
        public bool? DO7
        {
            get => _dO7;
            set
            {
                if (_dO7 == value)
                    return;
                _dO7 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO8;
        public bool? DO8
        {
            get => _dO8;
            set
            {
                if (_dO8 == value)
                    return;
                _dO0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO9;
        public bool? DO9
        {
            get => _dO9;
            set
            {
                if (_dO9 == value)
                    return;
                _dO9 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO10;
        public bool? DO10
        {
            get => _dO10;
            set
            {
                if (_dO10 == value)
                    return;
                _dO10 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO11;
        public bool? DO11
        {
            get => _dO11;
            set
            {
                if (_dO11 == value)
                    return;
                _dO11 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO12;
        public bool? DO12
        {
            get => _dO12;
            set
            {
                if (_dO12 == value)
                    return;
                _dO12 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO13;
        public bool? DO13
        {
            get => _dO13;
            set
            {
                if (_dO13 == value)
                    return;
                _dO13 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO14;
        public bool? DO14
        {
            get => _dO14;
            set
            {
                if (_dO14 == value)
                    return;
                _dO14 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dO15;
        public bool? DO15
        {
            get => _dO15;
            set
            {
                if (_dO15 == value)
                    return;
                _dO15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private void ActiveInterfaceKit_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        {
            InterfaceKit ifk = (InterfaceKit)sender;
            var outputs = ifk.outputs;
            InterfaceKitDigitalOutputCollection doc = outputs;

            switch (e.Index)
            {
                case 0:
                    DO0 = e.Value;
                    break;
                case 1:
                    DO1 = e.Value;
                    break;
                case 2:
                    DO2 = e.Value;
                    break;
                case 3:
                    DO3 = e.Value;
                    break;
                case 4:
                    DO4 = e.Value;
                    break;
                case 5:
                    DO5 = e.Value;
                    break;
                case 6:
                    DO6 = e.Value;
                    break;
                case 7:
                    DO7 = e.Value;
                    break;
                case 8:
                    DO8 = e.Value;
                    break;
                case 9:
                    DO9 = e.Value;
                    break;
                case 10:
                    DO10 = e.Value;
                    break;
                case 11:
                    DO11 = e.Value;
                    break;
                case 12:
                    DO12 = e.Value;
                    break;
                case 13:
                    DO13 = e.Value;
                    break;
                case 14:
                    DO14 = e.Value;
                    break;
                case 15:
                    DO15 = e.Value;
                    break;
            }

        }

        private void ActiveInterfaceKit_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                InterfaceKit ifk = (InterfaceKit)sender;
                //Phidget device = (Phidget)e.Device;
                //var b = e.GetType();
                Log.Trace($"ActiveInterfaceKit_Attach {ifk.Address},{ifk.Port} S#:{ifk.SerialNumber}", Common.LOG_CATEGORY);
                // TODO(crhodes)
                // This is where properties should be grabbed
                UpdateInterfaceKitProperties();

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void UpdateInterfaceKitProperties()
        {
            if (ActiveInterfaceKit.Attached)
            {
                IkAddress = ActiveInterfaceKit.Address;
                IkAttached = ActiveInterfaceKit.Attached;
                IkAttachedToServer = ActiveInterfaceKit.AttachedToServer;
                IkClass = ActiveInterfaceKit.Class.ToString();
                IkID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveInterfaceKit.ID);
                IkLabel = ActiveInterfaceKit.Label;
                IkLibraryVersion = Phidget.LibraryVersion;  // This is a static field
                IkName = ActiveInterfaceKit.Name;
                IkPort = ActiveInterfaceKit.Port;
                IkSerialNumber = ActiveInterfaceKit.SerialNumber; // This throws exception
                //IkServerID = ActiveInterfaceKit.ServerID;
                IkType = ActiveInterfaceKit.Type;
                IkVersion = ActiveInterfaceKit.Version;
            }
            else
            {
                // NOTE(crhodes)
                // Commented out properties throw exceptions when Phidget not attached
                // Just clear field

                //IkAddress = ActiveInterfaceKit.Address;
                IkAddress = "";
                IkAttached = ActiveInterfaceKit.Attached;
                //IkAttachedToServer = ActiveInterfaceKit.AttachedToServer;
                IkAttachedToServer = false;
                IkClass = ActiveInterfaceKit.Class.ToString();
                //IkID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveInterfaceKit.ID);
                //IkLabel = ActiveInterfaceKit.Label;
                IkLabel = "";
                //IkLibraryVersion = ActiveInterfaceKit.LibraryVersion;
                IkLibraryVersion = Phidget.LibraryVersion;
                //IkName = ActiveInterfaceKit.Name;
                IkName = "";
                //IkSerialNumber = ActiveInterfaceKit.SerialNumber;
                IkSerialNumber = null;
                //IkServerID = ActiveInterfaceKit.ServerID;
                IkServerID = "";
                //IkType = ActiveInterfaceKit.Type;
                IkType = "";
                //IkVersion = ActiveInterfaceKit.Version;
                IkVersion = null;
            }
        }

        private void ActiveInterfaceKit_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                InterfaceKit ifk = (InterfaceKit)sender;
                var a = e;
                var b = e.GetType();
                Log.Trace($"ActiveInterfaceKit_Detach {ifk.Address},{ifk.SerialNumber}", Common.LOG_CATEGORY);

                // TODO(crhodes)
                // What kind of cleanup?  Maybe set ActiveInterfaceKit to null.  Clear UI
                UpdateInterfaceKitProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        //private bool _isEnabled = true;
        //public bool IsEnabled
        //{
        //    get => _isEnabled;
        //    set
        //    {
        //        if (_isEnabled == value)
        //            return;
        //        _isEnabled = value;
        //        CloseInterfaceKitCommand.RaiseCanExecuteChanged();
        //        OpenInterfaceKitCommand.RaiseCanExecuteChanged();
        //        OnPropertyChanged();
        //    }
        //}
        
        public bool OpenInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (IkAttached is not null) 
                return !(Boolean)IkAttached;
            else 
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

        private void ClearDigitalInputsAndOutputs()
        {
            DI0 = DO0 = null;
            DI1 = DO1 = null;
            DI2 = DO2 = null;
            DI3 = DO3 = null;
            DI4 = DO4 = null;
            DI5 = DO5 = null;
            DI6 = DO6 = null;
            DI7 = DO7 = null;
            DI8 = DO8 = null;
            DI9 = DO9 = null;
            DI10 = DO10 = null;
            DI11 = DO11 = null;
            DI12 = DO12 = null;
            DI13 = DO13 = null;
            DI14 = DO14 = null;
            DI15 = DO15 = null;
        }
        public void CloseInterfaceKit()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseInterfaceKit";

            ActiveInterfaceKit.Close();
            UpdateInterfaceKitProperties();
            ActiveInterfaceKit = null;
            ClearDigitalInputsAndOutputs();

            OpenInterfaceKitCommand.RaiseCanExecuteChanged();
            CloseInterfaceKitCommand.RaiseCanExecuteChanged();

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
            //return true;
            if (IkAttached is not null) 
                return (Boolean)IkAttached;
            else
                return false;
        }

        #endregion

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

        #endregion

        #region Public Methods


        #endregion

        #region Protected Methods


        #endregion

        #region Private Methods


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
