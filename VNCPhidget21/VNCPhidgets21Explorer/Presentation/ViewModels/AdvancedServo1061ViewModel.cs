using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class AdvancedServo1061ViewModel : EventViewModelBase, IAdvancedServo1061ViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public AdvancedServo1061ViewModel(
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

            // Turn on logging of PropertyChanged from VNC.Core
            // We display the logging in 
            //LogOnPropertyChanged = true;

            // TODO(crhodes)
            //

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);
            OpenAdvancedServoCommand = new DelegateCommand(OpenAdvancedServo, OpenAdvancedServoCanExecute);
            CloseAdvancedServoCommand = new DelegateCommand(CloseAdvancedServo, CloseAdvancedServoCanExecute);

            // TODO(crhodes)
            // For now just hard code this.  Can have UI let us choose later.

            ConfigFileName = "phidgetconfig.json";
            LoadUIConfig();

            //SayHelloCommand = new DelegateCommand(
            //    SayHello, SayHelloCanExecute);
                
            Message = "AdvancedServo1061ViewModel says hello";           

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(ConfigFileName);

            Resources.PhidgetConfig? phidgetConfig = JsonSerializer.Deserialize<Resources.PhidgetConfig>(jsonString);
            this.Hosts = phidgetConfig.Hosts.ToList();
            //this.Sensors2 = phidgetConfig.Sensors.ToList();

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties


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

        private IEnumerable<Resources.Host> _Hosts;
        public IEnumerable<Resources.Host> Hosts
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
                AdvancedServos = _selectedHost.AdvancedServos.ToList<Resources.AdvancedServo>();
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.AdvancedServo> _AdvancedServos;
        public IEnumerable<Resources.AdvancedServo> AdvancedServos
        {
            get
            {
                if (null == _AdvancedServos)
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

                return _AdvancedServos;
            }

            set
            {
                _AdvancedServos = value;
                OnPropertyChanged();
            }
        }

        private Resources.AdvancedServo _selectedAdvancedServo;
        public Resources.AdvancedServo SelectedAdvancedServo
        {
            get => _selectedAdvancedServo;
            set
            {
                if (_selectedAdvancedServo == value)
                    return;
                _selectedAdvancedServo = value;

                OpenAdvancedServoCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        private AdvancedServoEx _activeAdvancedServo;
        public AdvancedServoEx ActiveAdvancedServo
        {
            get => _activeAdvancedServo;
            set
            {
                if (_activeAdvancedServo == value)
                    return;
                _activeAdvancedServo = value;

                if (_activeAdvancedServo is not null)
                {
                    PhidgetDevice = _activeAdvancedServo.AdvancedServo;
                }
                else
                {
                    // TODO(crhodes)
                    // PhidgetDevice = null ???
                    // Will need to declare Phidgets.Phidget?
                    PhidgetDevice = null;
                }

                OnPropertyChanged();
            }
        }

        private Phidgets.Phidget _phidgetDevice;
        public Phidgets.Phidget PhidgetDevice
        {
            get => _phidgetDevice;
            set
            {
                if (_phidgetDevice == value)
                    return;
                _phidgetDevice = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand SayHelloCommand { get; private set; }
        
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

        #region AdvancedServo Properties

        //private string _asAddress;
        //public string AsAddress
        //{
        //    get => _asAddress;
        //    set
        //    {
        //        if (_asAddress == value)
        //            return;
        //        _asAddress = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private bool? _asAttached;
        //public bool? AsAttached
        //{
        //    get => _asAttached;
        //    set
        //    {
        //        if (_asAttached == value)
        //            return;
        //        _asAttached = value;
        //        OnPropertyChanged();
        //    }
        //}

        private bool? _deviceAttached;
        public bool? DeviceAttached
        {
            get => _deviceAttached;
            set
            {
                if (_deviceAttached == value)
                    return;
                _deviceAttached = value;
                OnPropertyChanged();
            }
        }

        //private bool? _asAttachedToServer;
        //public bool? AsAttachedToServer
        //{
        //    get => _asAttachedToServer;
        //    set
        //    {
        //        if (_asAttachedToServer == value)
        //            return;
        //        _asAttachedToServer = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _asClass;

        //public string AsClass
        //{
        //    get => _asClass;
        //    set
        //    {
        //        if (_asClass == value)
        //            return;
        //        _asClass = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _asID;
        //public string AsID
        //{
        //    get => _asID;
        //    set
        //    {
        //        if (_asID == value)
        //            return;
        //        _asID = value;
        //        OnPropertyChanged();
        //    }
        //}


        //private string _asLabel;
        //public string AsLabel
        //{
        //    get => _asLabel;
        //    set
        //    {
        //        if (_asLabel == value)
        //            return;
        //        _asLabel = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _asLibraryVersion;
        //public string AsLibraryVersion
        //{
        //    get => _asLibraryVersion;
        //    set
        //    {
        //        if (_asLibraryVersion == value)
        //            return;
        //        _asLibraryVersion = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _asName;
        //public string AsName
        //{
        //    get => _asName;
        //    set
        //    {
        //        if (_asName == value)
        //            return;
        //        _asName = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private int? _asPort;
        //public int? AsPort
        //{
        //    get => _asPort;
        //    set
        //    {
        //        if (_asPort == value)
        //            return;
        //        _asPort = value;
        //        OnPropertyChanged();
        //    }
        //}


        //private int? _asSerialNumber;

        //public int? AsSerialNumber
        //{
        //    get => _asSerialNumber;
        //    set
        //    {
        //        if (_asSerialNumber == value)
        //            return;
        //        _asSerialNumber = value;
        //        OnPropertyChanged();
        //    }
        //}


        //private string _asServerID;
        //public string AsServerID
        //{
        //    get => _asServerID;
        //    set
        //    {
        //        if (_asServerID == value)
        //            return;
        //        _asServerID = value;
        //        OnPropertyChanged();
        //    }
        //}


        //private string _asType;
        //public string AsType
        //{
        //    get => _asType;
        //    set
        //    {
        //        if (_asType == value)
        //            return;
        //        _asType = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private int? _asVersion;
        //public int? AsVersion
        //{
        //    get => _asVersion;
        //    set
        //    {
        //        if (_asVersion == value)
        //            return;
        //        _asVersion = value;
        //        OnPropertyChanged();
        //    }
        //}

        #endregion

        #endregion

        #region Commands

        public string ConfigFileNameToolTip { get; set; }

        #region Command ConfigFileName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Message = "ConfigFileName_DoubleClick";
        }

        #endregion

        #region OpenAdvancedServo Command

        public DelegateCommand OpenAdvancedServoCommand { get; set; }
        public string OpenAdvancedServoContent { get; set; } = "Open";
        public string OpenAdvancedServoToolTip { get; set; } = "OpenAdvancedServo ToolTip";

        // Can get fancy and use Resources
        //public string OpenAdvancedServoContent { get; set; } = "ViewName_OpenAdvancedServoContent";
        //public string OpenAdvancedServoToolTip { get; set; } = "ViewName_OpenAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenAdvancedServoContent">OpenAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_OpenAdvancedServoContentToolTip">OpenAdvancedServo ToolTip</system:String>  

        public void OpenAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenAdvancedServo";

            ActiveAdvancedServo = new AdvancedServoEx(
                SelectedHost.IPAddress,
                SelectedHost.Port,
                SelectedAdvancedServo.SerialNumber,
                SelectedAdvancedServo.Enable,
                SelectedAdvancedServo.Embedded);

            ActiveAdvancedServo.AdvancedServo.Attach += ActiveAdvancedServo_Attach;
            ActiveAdvancedServo.AdvancedServo.Detach += ActiveAdvancedServo_Detach;

            // NOTE(crhodes)
            // Capture Digital Input and Output changes so we can update the UI
            // The AdvancedServoEx attaches to these events also.
            // Itlogs the changes if xxx is set to true.

            //ActiveAdvancedServo.OutputChange += ActiveAdvancedServo_OutputChange;
            //ActiveAdvancedServo.InputChange += ActiveAdvancedServo_InputChange;

            //// NOTE(crhodes)
            //// Let's do see if we can watch some analog data stream in.

            //ActiveAdvancedServo.SensorChange += ActiveAdvancedServo_SensorChange;

            ActiveAdvancedServo.Open();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish(
            //      new OpenAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Subscribe(OpenAdvancedServo);

            // End Cut Four

            //OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            //CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool OpenAdvancedServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedAdvancedServo is not null)
            {
                if (DeviceAttached is not null)
                    return !(Boolean)DeviceAttached;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CloseAdvancedServo Command

        public DelegateCommand CloseAdvancedServoCommand { get; set; }
        public string CloseAdvancedServoContent { get; set; } = "Close";
        public string CloseAdvancedServoToolTip { get; set; } = "CloseAdvancedServo ToolTip";

        // Can get fancy and use Resources
        //public string CloseAdvancedServoContent { get; set; } = "ViewName_CloseAdvancedServoContent";
        //public string CloseAdvancedServoToolTip { get; set; } = "ViewName_CloseAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseAdvancedServoContent">CloseAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_CloseAdvancedServoContentToolTip">CloseAdvancedServo ToolTip</system:String>  

        private void ClearDigitalInputsAndOutputs()
        {
            //DI0 = DO0 = null;
            //DI1 = DO1 = null;
            //DI2 = DO2 = null;
            //DI3 = DO3 = null;
            //DI4 = DO4 = null;
            //DI5 = DO5 = null;
            //DI6 = DO6 = null;
            //DI7 = DO7 = null;
            //DI8 = DO8 = null;
            //DI9 = DO9 = null;
            //DI10 = DO10 = null;
            //DI11 = DO11 = null;
            //DI12 = DO12 = null;
            //DI13 = DO13 = null;
            //DI14 = DO14 = null;
            //DI15 = DO15 = null;
        }

        public void CloseAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseAdvancedServo";

            ActiveAdvancedServo.AdvancedServo.Attach -= ActiveAdvancedServo_Attach;
            ActiveAdvancedServo.AdvancedServo.Detach -= ActiveAdvancedServo_Detach;

            ActiveAdvancedServo.Close();
            UpdateAdvancedServoProperties();
            ActiveAdvancedServo = null;
            ClearDigitalInputsAndOutputs();

            //OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            //CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseAdvancedServoEvent>().Publish(
            //      new CloseAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseAdvancedServoEvent>().Subscribe(CloseAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool CloseAdvancedServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #endregion

        #region Event Handlers

        //private void PopulateSensorValues(AdvancedServoAnalogSensor interfaceKitAnalogSensor)
        //{

        //}

        //private void ActiveAdvancedServo_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        //{
        //    Phidgets.AdvancedServo ifk = (Phidgets.AdvancedServo)sender;

        //    AdvancedServoAnalogSensor sensor = ifk.sensors[0];

        //    //SIRaw0 = sensor.RawValue;
        //    //SIDataRate0 = sensor.DataRate;
        //    //SIDataRateMax0 = sensor.DataRateMax;
        //    //SIDataRateMin0 = sensor.DataRateMin;
        //    //SISensitivity0= sensor.Sensitivity;

        //    //var sValue = sensor0.Value;
        //    //var eValue = e.Value;

        //    // NOTE(crhodes)
        //    // DataRateMin and DataRateMax do not change.
        //    // Populate in Attach event

        //    switch (e.Index)
        //    {
        //        case 0:
        //            sensor = ifk.sensors[0];
        //            AI0 = sensor.Value;
        //            AIRaw0 = sensor.RawValue;
        //            AIDataRate0 = sensor.DataRate;
        //            //AIDataRateMax0 = sensor.DataRateMax;
        //            //AIDataRateMin0 = sensor.DataRateMin;
        //            AISensitivity0 = sensor.Sensitivity;
        //            break;
        //        case 1:
        //            sensor = ifk.sensors[1];
        //            AI1 = sensor.Value;
        //            AIRaw1 = sensor.RawValue;
        //            AIDataRate1 = sensor.DataRate;
        //            //AIDataRateMax1 = sensor.DataRateMax;
        //            //AIDataRateMin1 = sensor.DataRateMin;
        //            AISensitivity1 = sensor.Sensitivity;
        //            break;
        //        case 2:
        //            sensor = ifk.sensors[2];
        //            AI2 = sensor.Value;
        //            AIRaw2 = sensor.RawValue;
        //            AIDataRate2 = sensor.DataRate;
        //            //AIDataRateMax2 = sensor.DataRateMax;
        //            //AIDataRateMin2 = sensor.DataRateMin;
        //            AISensitivity2 = sensor.Sensitivity;
        //            break;
        //        case 3:
        //            sensor = ifk.sensors[3];
        //            AI3 = sensor.Value;
        //            AIRaw3 = sensor.RawValue;
        //            AIDataRate3 = sensor.DataRate;
        //            //AIDataRateMax3 = sensor.DataRateMax;
        //            //AIDataRateMin3 = sensor.DataRateMin;
        //            AISensitivity3 = sensor.Sensitivity;
        //            break;
        //        case 4:
        //            sensor = ifk.sensors[4];
        //            AI4 = sensor.Value;
        //            AIRaw4 = sensor.RawValue;
        //            AIDataRate4 = sensor.DataRate;
        //            //AIDataRateMax4 = sensor.DataRateMax;
        //            //AIDataRateMin4 = sensor.DataRateMin;
        //            AISensitivity4 = sensor.Sensitivity;
        //            break;
        //        case 5:
        //            sensor = ifk.sensors[5];
        //            AI5 = sensor.Value;
        //            AIRaw5 = sensor.RawValue;
        //            AIDataRate5 = sensor.DataRate;
        //            //AIDataRateMax5 = sensor.DataRateMax;
        //            //AIDataRateMin5 = sensor.DataRateMin;
        //            AISensitivity5 = sensor.Sensitivity;
        //            break;
        //        case 6:
        //            sensor = ifk.sensors[6];
        //            AI6 = sensor.Value;
        //            AIRaw6 = sensor.RawValue;
        //            AIDataRate6 = sensor.DataRate;
        //            //AIDataRateMax6 = sensor.DataRateMax;
        //            //AIDataRateMin6 = sensor.DataRateMin;
        //            AISensitivity6 = sensor.Sensitivity;
        //            break;
        //        case 7:
        //            sensor = ifk.sensors[7];
        //            AI7 = sensor.Value;
        //            AIRaw7 = sensor.RawValue;
        //            AIDataRate7 = sensor.DataRate;
        //            //AIDataRateMax7 = sensor.DataRateMax;
        //            //AIDataRateMin7 = sensor.DataRateMin;
        //            AISensitivity7 = sensor.Sensitivity;
        //            break;
        //    }
        //}

        //private void ActiveAdvancedServo_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        //{
        //    Phidgets.AdvancedServo ifk = (Phidgets.AdvancedServo)sender;

        //    switch (e.Index)
        //    {
        //        case 0:
        //            DI0 = e.Value;
        //            break;
        //        case 1:
        //            DI1 = e.Value;
        //            break;
        //        case 2:
        //            DI2 = e.Value;
        //            break;
        //        case 3:
        //            DI3 = e.Value;
        //            break;
        //        case 4:
        //            DI4 = e.Value;
        //            break;
        //        case 5:
        //            DI5 = e.Value;
        //            break;
        //        case 6:
        //            DI6 = e.Value;
        //            break;
        //        case 7:
        //            DI7 = e.Value;
        //            break;
        //        case 8:
        //            DI8 = e.Value;
        //            break;
        //        case 9:
        //            DI9 = e.Value;
        //            break;
        //        case 10:
        //            DI10 = e.Value;
        //            break;
        //        case 11:
        //            DI11 = e.Value;
        //            break;
        //        case 12:
        //            DI12 = e.Value;
        //            break;
        //        case 13:
        //            DI13 = e.Value;
        //            break;
        //        case 14:
        //            DI14 = e.Value;
        //            break;
        //        case 15:
        //            DI15 = e.Value;
        //            break;
        //    }
        //}


        //private void ActiveAdvancedServo_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        //{
        //    Phidgets.AdvancedServo ifk = (Phidgets.AdvancedServo)sender;
        //    var outputs = ifk.outputs;
        //    AdvancedServoDigitalOutputCollection doc = outputs;

        //    switch (e.Index)
        //    {
        //        case 0:
        //            DO0 = e.Value;
        //            break;
        //        case 1:
        //            DO1 = e.Value;
        //            break;
        //        case 2:
        //            DO2 = e.Value;
        //            break;
        //        case 3:
        //            DO3 = e.Value;
        //            break;
        //        case 4:
        //            DO4 = e.Value;
        //            break;
        //        case 5:
        //            DO5 = e.Value;
        //            break;
        //        case 6:
        //            DO6 = e.Value;
        //            break;
        //        case 7:
        //            DO7 = e.Value;
        //            break;
        //        case 8:
        //            DO8 = e.Value;
        //            break;
        //        case 9:
        //            DO9 = e.Value;
        //            break;
        //        case 10:
        //            DO10 = e.Value;
        //            break;
        //        case 11:
        //            DO11 = e.Value;
        //            break;
        //        case 12:
        //            DO12 = e.Value;
        //            break;
        //        case 13:
        //            DO13 = e.Value;
        //            break;
        //        case 14:
        //            DO14 = e.Value;
        //            break;
        //        case 15:
        //            DO15 = e.Value;
        //            break;
        //    }

        //}

        private void ActiveAdvancedServo_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                Log.Trace($"ActiveAdvancedServo_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
                // TODO(crhodes)
                // This is where properties should be grabbed
                UpdateAdvancedServoProperties();

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void UpdateAdvancedServoProperties()
        {
            // TODO(crhodes)
            // May not need this anymore.  Consider moving into ActiveAdvancedServo_{Attach,Detach}
            if (ActiveAdvancedServo.AdvancedServo.Attached)
            {
                //AsAddress = ActiveAdvancedServo.AdvancedServo.Address;
                //AsAttached = ActiveAdvancedServo.AdvancedServo.Attached;
                DeviceAttached = ActiveAdvancedServo.AdvancedServo.Attached;
                //AsAttachedToServer = ActiveAdvancedServo.AdvancedServo.AttachedToServer;
                //AsClass = ActiveAdvancedServo.AdvancedServo.Class.ToString();
                //AsID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveAdvancedServo.AdvancedServo.ID);
                //AsLabel = ActiveAdvancedServo.AdvancedServo.Label;
                //AsLibraryVersion = Phidget.LibraryVersion;  // This is a static field
                //AsName = ActiveAdvancedServo.AdvancedServo.Name;
                //AsPort = ActiveAdvancedServo.AdvancedServo.Port;
                //AsSerialNumber = ActiveAdvancedServo.AdvancedServo.SerialNumber;
                ////AsServerID = ActiveAdvancedServo.AdvancedServo.ServerID;
                //AsType = ActiveAdvancedServo.AdvancedServo.Type;
                //AsVersion = ActiveAdvancedServo.AdvancedServo.Version;

                //var sensors = ActiveAdvancedServo.sensors;
                //AdvancedServoAnalogSensor sensor = null;

                //// NOTE(crhodes)
                //// The DataRateMin and DataRateMax do not change.
                //// Populate them here instead of SensorChange event

                //// TODO(crhodes)
                //// May want to grab initial values for all fields here.

                //for (int i = 0; i < sensors.Count; i++)
                //{
                //    sensor = sensors[i];

                //    switch (i)
                //    {
                //        case 0:
                //            AIDataRateMax0 = sensor.DataRateMax;
                //            AIDataRateMin0 = sensor.DataRateMin;
                //            break;
                //        case 1:
                //            AIDataRateMax1 = sensor.DataRateMax;
                //            AIDataRateMin1 = sensor.DataRateMin;
                //            break;
                //        case 2:
                //            AIDataRateMax2 = sensor.DataRateMax;
                //            AIDataRateMin2 = sensor.DataRateMin;
                //            break;
                //        case 3:
                //            AIDataRateMax3 = sensor.DataRateMax;
                //            AIDataRateMin3 = sensor.DataRateMin;
                //            break;
                //        case 4:
                //            AIDataRateMax4 = sensor.DataRateMax;
                //            AIDataRateMin4 = sensor.DataRateMin;
                //            break;
                //        case 5:
                //            AIDataRateMax5 = sensor.DataRateMax;
                //            AIDataRateMin5 = sensor.DataRateMin;
                //            break;
                //        case 6:
                //            AIDataRateMax6 = sensor.DataRateMax;
                //            AIDataRateMin6 = sensor.DataRateMin;
                //            break;
                //        case 7:
                //            AIDataRateMax7 = sensor.DataRateMax;
                //            AIDataRateMin7 = sensor.DataRateMin;
                //            break;
                //    }
                //}
            }
            else
            {
                DeviceAttached = null;
                // NOTE(crhodes)
                // Commented out properties throw exceptions when Phidget not attached
                // Just clear field

                //AsAddress = ActiveAdvancedServo.Address;
                //AsAddress = "";
                //AsAttached = ActiveAdvancedServo.AdvancedServo.Attached;
                ////AsAttachedToServer = ActiveAdvancedServo.AttachedToServer;
                //AsAttachedToServer = false;
                //// This doesn't throw exception but let's clear anyway
                ////AsClass = ActiveAdvancedServo.Class.ToString();
                //AsClass = "";
                ////AsID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveAdvancedServo.ID);
                //AsID = "";
                ////AsLabel = ActiveAdvancedServo.Label;
                //AsLabel = "";
                ////AsLibraryVersion = ActiveAdvancedServo.LibraryVersion;
                //AsLibraryVersion = Phidget.LibraryVersion;
                ////AsName = ActiveAdvancedServo.Name;
                //AsName = "";
                ////AsSerialNumber = ActiveAdvancedServo.SerialNumber;
                //AsSerialNumber = null;
                ////AsServerID = ActiveAdvancedServo.ServerID;
                //AsServerID = "";
                ////AsType = ActiveAdvancedServo.Type;
                //AsType = "";
                ////AsVersion = ActiveAdvancedServo.Version;
                //AsVersion = null;
            }

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();
        }

        private void ActiveAdvancedServo_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                var a = e;
                var b = e.GetType();
                Log.Trace($"ActiveAdvancedServo_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);

                // TODO(crhodes)
                // What kind of cleanup?  Maybe set ActiveAdvancedServo to null.  Clear UI
                UpdateAdvancedServoProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

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
