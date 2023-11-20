﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;

using DevExpress.XtraRichEdit.API.Native;

using Phidgets;
using Phidgets.Events;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

using VNCPhidgetConfig = VNCPhidget21.Configuration;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public enum ServoType
    {
        DEFAULT,
        //RAW_us_MODE,
        HITEC_HS322HD,
        HITEC_HS5245MG,
        HITEC_805BB,
        HITEC_HS422,
        TOWERPRO_MG90,
        HITEC_HSR1425CR,
        HITEC_HS785HB,
        HITEC_HS485HB,
        HITEC_HS645MG,
        HITEC_815BB,
        FIRGELLI_L12_30_50_06_R,
        FIRGELLI_L12_50_100_06_R,
        FIRGELLI_L12_50_210_06_R,
        FIRGELLI_L12_100_50_06_R,
        FIRGELLI_L12_100_100_06_R,
        USER_DEFINED,
        //INVALID
    }

    public partial class AdvancedServo1061ViewModel 
        : EventViewModelBase, IAdvancedServo1061ViewModel, IInstanceCountVM
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
            //PerformanceFileName_DoubleClick_Command = new DelegateCommand(PerformanceFileName_DoubleClick);

            OpenAdvancedServoCommand = new DelegateCommand(OpenAdvancedServo, OpenAdvancedServoCanExecute);
            RefreshAdvancedServoCommand = new DelegateCommand(RefreshAdvancedServo, RefreshAdvancedServoCanExecute);
            SetAdvancedServoDefaultsCommand = new DelegateCommand<string>(SetAdvancedServoDefaults, SetAdvancedServoDefaultsCanExecute);
            CloseAdvancedServoCommand = new DelegateCommand(CloseAdvancedServo, CloseAdvancedServoCanExecute);

            //ConfigureServoCommand = new DelegateCommand(ConfigureServo, ConfigureServoCanExecute);

            ConfigureServo2Command = new DelegateCommand<string>(ConfigureServo2, ConfigureServo2CanExecute);

            SetPositionRangeCommand = new DelegateCommand<string>(SetPositionRange, SetPositionRangeCanExecute);

            //PlayPerformanceCommand = new DelegateCommand<string>(PlayPerformance, PlayPerformanceCanExecute);
            //PlaySequenceCommand = new DelegateCommand<string>(PlaySequence, PlaySequenceCanExecute);

            // TODO(crhodes)
            // For now just hard code this.  Can have UI let us choose later.

            ConfigFileName = "hostconfig.json";
            //PerformanceConfigFileName = "advancedservoperformancesconfig.json";

            LoadUIConfig();
            //LoadPerformancesConfig();

            //SayHelloCommand = new DelegateCommand(
            //    SayHello, SayHelloCanExecute);

            Message = "AdvancedServo1061ViewModel says hello";           

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            var jsonOptions = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };

            string jsonString = File.ReadAllText(ConfigFileName);

            VNCPhidgetConfig.HostConfig? hostConfig 
                = JsonSerializer.Deserialize<VNCPhidgetConfig.HostConfig>(jsonString, jsonOptions);

            this.Hosts = hostConfig.Hosts.ToList();

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //private void LoadPerformancesConfig()
        //{
        //    Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

        //    var jsonOptions = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };

        //    string jsonString = File.ReadAllText(PerformanceConfigFileName);

        //    Resources.AdvancedServoPerformanceConfig? performancesConfig
        //        = JsonSerializer.Deserialize<Resources.AdvancedServoPerformanceConfig>(jsonString, jsonOptions);

        //    this.AdvancedServoPerformances = performancesConfig.AdvancedServoPerformances.ToList();

        //    AvailableAdvancedServoPerformances =
        //        performancesConfig.AdvancedServoPerformances
        //        .ToDictionary(k => k.Name, v => v);

        //    Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        //}

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

        public string ConfigFileNameToolTip { get; set; } = "DoubleClick to select new file";

        private string _performanceConfigFileName;
        public string PerformanceConfigFileName
        {
            get => _performanceConfigFileName;
            set
            {
                if (_performanceConfigFileName == value) return;
                _performanceConfigFileName = value;
                OnPropertyChanged();
            }
        }

        public string PerformanceFileNameToolTip { get; set; } = "DoubleClick to select new file";

        private VNCPhidgetConfig.HostConfig _hostConfig;
        public VNCPhidgetConfig.HostConfig HostConfig
        {
            get => _hostConfig;
            set
            {
                if (_hostConfig == value)
                    return;
                _hostConfig = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<VNCPhidgetConfig.Host> _Hosts;
        public IEnumerable<VNCPhidgetConfig.Host> Hosts
        {
            get => _Hosts;
            set
            {
                _Hosts = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.Host _selectedHost;
        public VNCPhidgetConfig.Host SelectedHost
        {
            get => _selectedHost;
            set
            {
                if (_selectedHost == value)
                    return;
                _selectedHost = value;
                AdvancedServos = _selectedHost.AdvancedServos.ToList<VNCPhidgetConfig.AdvancedServo>();
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

        private bool _logPhidgetEvents = false;
        public bool LogPhidgetEvents
        {
            get => _logPhidgetEvents;
            set
            {
                if (_logPhidgetEvents == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();

                if (ActiveAdvancedServo is not null) ActiveAdvancedServo.LogPhidgetEvents = value;
            }
        }

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

        private bool _logVelocityChangeEvents = false;
        public bool LogVelocityChangeEvents
        {
            get => _logVelocityChangeEvents;
            set
            {
                if (_logVelocityChangeEvents == value)
                    return;
                _logVelocityChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private bool _logPositionChangeEvents = false;
        public bool LogPositionChangeEvents
        {
            get => _logPositionChangeEvents;
            set
            {
                if (_logPositionChangeEvents == value)
                    return;
                _logPositionChangeEvents = value;
                OnPropertyChanged();
            }
        }

        //private bool _logPerformanceStep = false;
        //public bool LogPerformanceStep
        //{
        //    get => _logPerformanceStep;
        //    set
        //    {
        //        if (_logPerformanceStep == value)
        //            return;
        //        _logPerformanceStep = value;
        //        OnPropertyChanged();
        //    }
        //}

        #region AdvancedServo Properties

        private IEnumerable<VNCPhidgetConfig.AdvancedServo> _AdvancedServos;
        public IEnumerable<VNCPhidgetConfig.AdvancedServo> AdvancedServos
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

        //public Phidgets.ServoServo.ServoType ServoTypeEnum;

        private IEnumerable<VNCPhidgetConfig.AdvancedServo> _AdvancedServoTypes;
        public IEnumerable<VNCPhidgetConfig.AdvancedServo> AdvancedServoTypes
        {
            get
            {
                if (null == _AdvancedServoTypes)
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

                return _AdvancedServoTypes;
            }

            set
            {
                _AdvancedServoTypes = value;
                OnPropertyChanged();
            }
        }

        //private ServoServo.ServoType _servoTypeEnum;
        //public ServoServo.ServoType ServoTypeEnum
        //{
        //    get => _servoTypeEnum;
        //    set
        //    {
        //        if (_servoTypeEnum == value)
        //        {
        //            return;
        //        }

        //        _servoTypeEnum = value;
        //        OnPropertyChanged();
        //    }
        //}

        private VNCPhidgetConfig.AdvancedServo _selectedAdvancedServo;
        public VNCPhidgetConfig.AdvancedServo SelectedAdvancedServo
        {
            get => _selectedAdvancedServo;
            set
            {
                if (_selectedAdvancedServo == value)
                    return;
                _selectedAdvancedServo = value;

                OpenAdvancedServoCommand.RaiseCanExecuteChanged();
                //PlayPerformanceCommand.RaiseCanExecuteChanged();
                //PlaySequenceCommand.RaiseCanExecuteChanged();

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
                    PhidgetDevice = null;
                }

                OnPropertyChanged();
            }
        }

        private int? _servoCount;
        public int? ServoCount
        {
            get => _servoCount;
            set
            {
                if (_servoCount == value)
                    return;
                _servoCount = value;
                OnPropertyChanged();
            }
        }

        private ServoProperties[] _advancedServoProperties = new ServoProperties[8]
        {
            new ServoProperties() { ServoIndex = 0 },
            new ServoProperties() { ServoIndex = 1 },
            new ServoProperties() { ServoIndex = 2 },
            new ServoProperties() { ServoIndex = 3 },
            new ServoProperties() { ServoIndex = 4 },
            new ServoProperties() { ServoIndex = 5 },
            new ServoProperties() { ServoIndex = 6 },
            new ServoProperties() { ServoIndex = 7 },
        };

        public ServoProperties[] AdvancedServoProperties
        {
            get => _advancedServoProperties;
            set
            {
                if (_advancedServoProperties == value)
                    return;
                _advancedServoProperties = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand SayHelloCommand { get; private set; }

        #region Command ConfigFileName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }
        //public DelegateCommand PerformanceFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Message = "ConfigFileName_DoubleClick";
        }

        //private void PerformanceFileName_DoubleClick()
        //{
        //    Message = "PerformanceFileName_DoubleClick";

        //    LoadPerformancesConfig();
        //}

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
                SelectedAdvancedServo.SerialNumber);

            ActiveAdvancedServo.AdvancedServo.Attach += ActiveAdvancedServo_Attach;
            ActiveAdvancedServo.AdvancedServo.Detach += ActiveAdvancedServo_Detach;

            ActiveAdvancedServo.AdvancedServo.CurrentChange += ActiveAdvancedServo_CurrentChange;
            ActiveAdvancedServo.AdvancedServo.PositionChange += ActiveAdvancedServo_PositionChange;
            ActiveAdvancedServo.AdvancedServo.VelocityChange += ActiveAdvancedServo_VelocityChange;

            ActiveAdvancedServo.LogPhidgetEvents = LogPhidgetEvents;

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

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ActiveAdvancedServo_VelocityChange(object sender, VelocityChangeEventArgs e)
        {
            var senderType = sender.GetType();

            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var velocity = e.Velocity;

            if (LogVelocityChangeEvents)
            {
                Log.Trace($"VelocityChange index:{index} velocity:{velocity} position:{servo.servos[index].Position}", Common.LOG_CATEGORY);
            }

            AdvancedServoProperties[e.Index].Velocity = e.Velocity;

            //switch (e.Index)
            //{
            //    case 0:
            //        AdvancedServoProperties[e.Index].Velocity = e.Velocity;
            //        break;

            //    case 1:
            //        Velocity_S1 = e.Velocity;
            //        break;

            //    case 2:
            //        Velocity_S2 = e.Velocity;
            //        break;

            //    case 3:
            //        Velocity_S3 = e.Velocity;
            //        break;

            //    case 4:
            //        Velocity_S4 = e.Velocity;
            //        break;

            //    case 5:
            //        Velocity_S5 = e.Velocity;
            //        break;

            //    case 6:
            //        Velocity_S6 = e.Velocity;
            //        break;

            //    case 7:
            //        Velocity_S7 = e.Velocity;
            //        break;

            //    default:
            //        Log.Trace($"VelocityChange index:{index} value:{velocity}", Common.LOG_CATEGORY);
            //        break;
            //}
        }

        private void ActiveAdvancedServo_PositionChange(object sender, PositionChangeEventArgs e)
        {
            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var position = e.Position;

            if (LogPositionChangeEvents)
            {
                Log.Trace($"PositionChange index:{index} position:{position} velocity:{servo.servos[index].Velocity}", Common.LOG_CATEGORY);
            }

            AdvancedServoProperties[e.Index].Position = e.Position;
            AdvancedServoProperties[e.Index].Stopped = servo.servos[e.Index].Stopped;

            //switch (e.Index)
            //{
            //    case 0:
            //        AdvancedServoProperties[0].Position = e.Position;
            //        AdvancedServoProperties[0].Stopped = servo.servos[e.Index].Stopped;
            //        break;

            //    case 1:
            //        Position_S1 = e.Position;
            //        Stopped_S1 = servo.servos[e.Index].Stopped;
            //        break;

            //    case 2:
            //        Position_S2 = e.Position;
            //        Stopped_S2 = servo.servos[e.Index].Stopped;
            //        break;

            //    case 3:
            //        Position_S3 = e.Position;
            //        Stopped_S3 = servo.servos[e.Index].Stopped;
            //        break;

            //    case 4:
            //        Position_S4 = e.Position;
            //        Stopped_S4 = servo.servos[e.Index].Stopped;
            //        break;

            //    case 5:
            //        Position_S5 = e.Position;
            //        Stopped_S5 = servo.servos[e.Index].Stopped;
            //        break;

            //    case 6:
            //        Position_S6 = e.Position;
            //        Stopped_S6 = servo.servos[e.Index].Stopped;
            //        break;

            //    case 7:
            //        Position_S7 = e.Position;
            //        Stopped_S7 = servo.servos[e.Index].Stopped;
            //        break;

            //    default:
            //        Log.Trace($"PositionChange index:{index} value:{position}", Common.LOG_CATEGORY);
            //        break;
            //}
        }

        private void ActiveAdvancedServo_CurrentChange(object sender, CurrentChangeEventArgs e)
        {
            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var current = e.Current;

            AdvancedServoProperties[e.Index].Current = e.Current;

            //switch (e.Index)
            //{
            //    case 0:
            //        AdvancedServoProperties[0].Current = e.Current;
            //        break;

            //    case 1:
            //        Current_S1 = e.Current;
            //        break;

            //    case 2:
            //        Current_S2 = e.Current;
            //        break;

            //    case 3:
            //        Current_S3 = e.Current;
            //        break;

            //    case 4:
            //        Current_S4 = e.Current;
            //        break;

            //    case 5:
            //        Current_S5 = e.Current;
            //        break;

            //    case 6:
            //        Current_S6 = e.Current;
            //        break;

            //    case 7:
            //        Current_S7 = e.Current;
            //        break;

            //    default:
            //        // NOTE(crhodes)
            //        // Constant stream of this.
            //        // Do we really need to monitor current?

            //        //Log.Trace($"CurrentChange index:{index} value:{current}", Common.LOG_CATEGORY);
            //        break;
            //}
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

        #region RefreshAdvancedServo Command

        public DelegateCommand RefreshAdvancedServoCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> RefreshAdvancedServoCommand { get; set; }
        //public TYPE RefreshAdvancedServoCommandParameter;
        public string RefreshAdvancedServoContent { get; set; } = "Refresh";
        public string RefreshAdvancedServoToolTip { get; set; } = "Refresh ToolTip";

        // Can get fancy and use Resources
        //public string RefreshAdvancedServoContent { get; set; } = "ViewName_RefreshAdvancedServoContent";
        //public string RefreshAdvancedServoToolTip { get; set; } = "ViewName_RefreshAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RefreshAdvancedServoContent">RefreshAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_RefreshAdvancedServoContentToolTip">RefreshAdvancedServo ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void RefreshAdvancedServo(TYPE value)
        public void RefreshAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called RefreshAdvancedServo";

            RefreshAdvancedServoUIProperties();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<RefreshAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<RefreshAdvancedServoEvent>().Publish(
            //      new RefreshAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class RefreshAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<RefreshAdvancedServoEvent>().Subscribe(RefreshAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool RefreshAdvancedServoCanExecute(TYPE value)
        public bool RefreshAdvancedServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #region SetAdvancedServoDefaults Command

        public DelegateCommand<string> SetAdvancedServoDefaultsCommand { get; set; }
        //public string SetAdvancedServoDefaultsCommandParameter;
        public string SetAdvancedServoDefaultsContent { get; set; } = "SetAdvancedServoDefaults";
        public string SetAdvancedServoDefaultsToolTip { get; set; } = "SetAdvancedServoDefaults ToolTip";

        // Can get fancy and use Resources
        //public string SetAdvancedServoDefaultsContent { get; set; } = "ViewName_SetAdvancedServoDefaultsContent";
        //public string SetAdvancedServoDefaultsToolTip { get; set; } = "ViewName_SetAdvancedServoDefaultsContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_SetAdvancedServoDefaultsContent">SetAdvancedServoDefaults</system:String>
        //    <system:String x:Key="ViewName_SetAdvancedServoDefaultsContentToolTip">SetAdvancedServoDefaults ToolTip</system:String>  

        public void SetAdvancedServoDefaults(string servoID)
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = $"Cool, you called SetAdvancedServoDefaults from servo {servoID}";

            AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
            Phidgets.AdvancedServoServo servo = null;

            Int32 servoIndex = Int32.Parse(servoID);
            servo = servos[servoIndex];

            // NOTE(crhodes)
            // Should be safe to get Acceleration, Velocity, and Position here
            // Device is Engaged

            Double? halfRange;
            Double? percent = .20;
            Double? midPoint;

            try
            {
                for (int i = 0; i < ActiveAdvancedServo.AdvancedServo.servos.Count; i++)
                {
                    AdvancedServoProperties[i].Acceleration = AdvancedServoProperties[i].AccelerationMin;
                    AdvancedServoProperties[i].VelocityLimit = AdvancedServoProperties[i].VelocityMin == 0
                        ? 10 : 
                        AdvancedServoProperties[i].VelocityMin;

                    midPoint = (AdvancedServoProperties[i].DevicePositionMax - AdvancedServoProperties[i].DevicePositionMin) / 2;
                    halfRange = midPoint * percent;
                    AdvancedServoProperties[i].PositionMin = midPoint - halfRange;
                    AdvancedServoProperties[i].PositionMax = midPoint + halfRange;
                    AdvancedServoProperties[i].Position = midPoint;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<SetAdvancedServoDefaultsEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<SetAdvancedServoDefaultsEvent>().Publish(
            //      new SetAdvancedServoDefaultsEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class SetAdvancedServoDefaultsEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<SetAdvancedServoDefaultsEvent>().Subscribe(SetAdvancedServoDefaults);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool SetAdvancedServoDefaultsCanExecute(string value)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
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

            DisEngageAllServos();

            // NOTE(crhodes)
            // May need to give device chance to respond

            ActiveAdvancedServo.AdvancedServo.Attach -= ActiveAdvancedServo_Attach;
            ActiveAdvancedServo.AdvancedServo.Detach -= ActiveAdvancedServo_Detach;

            ActiveAdvancedServo.AdvancedServo.CurrentChange -= ActiveAdvancedServo_CurrentChange;
            ActiveAdvancedServo.AdvancedServo.PositionChange -= ActiveAdvancedServo_PositionChange;
            ActiveAdvancedServo.AdvancedServo.VelocityChange -= ActiveAdvancedServo_VelocityChange;
 
            ActiveAdvancedServo.Close();

            DeviceAttached = false;
            UpdateAdvancedServoProperties();

            ActiveAdvancedServo = null;
            //ClearDigitalInputsAndOutputs();

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            RefreshAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();
            SetAdvancedServoDefaultsCommand.RaiseCanExecuteChanged();
            SetPositionRangeCommand.RaiseCanExecuteChanged();

            //PlayPerformanceCommand.RaiseCanExecuteChanged();
            //PlaySequenceCommand.RaiseCanExecuteChanged();

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

        private void DisEngageAllServos()
        {
            for (int i = 0; i < ServoCount; i++)
            {
                var advancedServo = ActiveAdvancedServo.AdvancedServo;
                advancedServo.servos[i].Engaged = false;
            }
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

        #region ConfigureServo Command

        public DelegateCommand ConfigureServoCommand { get; set; }
        public string ConfigureServoContent { get; set; } = "ConfigureServo";
        public string ConfigureServoToolTip { get; set; } = "ConfigureServo ToolTip";

        // Can get fancy and use Resources
        //public string ConfigureServoContent { get; set; } = "ViewName_ConfigureServoContent";
        //public string ConfigureServoToolTip { get; set; } = "ViewName_ConfigureServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_ConfigureServoContent">ConfigureServo</system:String>
        //    <system:String x:Key="ViewName_ConfigureServoContentToolTip">ConfigureServo ToolTip</system:String>  

        public void ConfigureServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called ConfigureServo";

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<ConfigureServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<ConfigureServoEvent>().Publish(
            //      new ConfigureServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class ConfigureServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<ConfigureServoEvent>().Subscribe(ConfigureServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool ConfigureServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region SetPositionRange Command

        public DelegateCommand<string> SetPositionRangeCommand { get; set; }
        //public TYPE SetPositionRangeCommandParameter;
        public string SetPositionRangeContent { get; set; } = "SetPositionRange";
        public string SetPositionRangeToolTip { get; set; } = "SetPositionRange ToolTip";

        // Can get fancy and use Resources
        //public string SetPositionRangeContent { get; set; } = "ViewName_SetPositionRangeContent";
        //public string SetPositionRangeToolTip { get; set; } = "ViewName_SetPositionRangeContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_SetPositionRangeContent">SetPositionRange</system:String>
        //    <system:String x:Key="ViewName_SetPositionRangeContentToolTip">SetPositionRange ToolTip</system:String>  

        public void SetPositionRange(string servoID)
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called SetPositionRange";

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<SetPositionRangeEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<SetPositionRangeEvent>().Publish(
            //      new SetPositionRangeEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class SetPositionRangeEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<SetPositionRangeEvent>().Subscribe(SetPositionRange);

            // End Cut Four


            AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
            Phidgets.AdvancedServoServo servo = null;

            Int32 servoIndex = Int32.Parse(servoID);
            servo = servos[servoIndex];

            // NOTE(crhodes)
            // Should be safe to get Acceleration, Velocity, and Position here
            // Device is Engaged

            Double? halfRange;
            Double? tenPercent;

            try
            {
                AdvancedServoProperties[servoIndex].PositionMin = 
                    AdvancedServoProperties[servoIndex].Position - AdvancedServoProperties[servoIndex].PositionRange;

                AdvancedServoProperties[servoIndex].PositionMax =
                    AdvancedServoProperties[servoIndex].Position + AdvancedServoProperties[servoIndex].PositionRange;

                //switch (servoIndex)
                //{

                //    case 0:
                //        // TODO(crhodes)
                //        // Make this fancier.  Take the smaller of difference
                //        // between Min and Current and Max and Current
                //        // and then divide that by 10

                //        AdvancedServoProperties[0].PositionMin = AdvancedServoProperties[0].Position - AdvancedServoProperties[0].PositionRange;

                //        AdvancedServoProperties[0].PositionMax = AdvancedServoProperties[0].Position + AdvancedServoProperties[0].PositionRange;

                //        //PositionMin_S0 = Position_S0 - PositionRange_S0;

                //        //PositionMax_S0 = Position_S0 + PositionRange_S0;

                //        break;

                //    case 1:
                //        PositionMin_S1 = Position_S1 - PositionRange_S1;

                //        PositionMax_S1 = Position_S1 + PositionRange_S1;

                //        break;

                //    case 2:
                //        PositionMin_S2 = Position_S2 - PositionRange_S2;

                //        PositionMax_S2 = Position_S2 + PositionRange_S2;

                //        break;

                //    case 3:
                //        PositionMin_S3 = Position_S3 - PositionRange_S3;

                //        PositionMax_S3 = Position_S3 + PositionRange_S3;

                //        break;

                //    case 4:
                //        PositionMin_S4 = Position_S4 - PositionRange_S4;

                //        PositionMax_S4 = Position_S4 + PositionRange_S4;

                //        break;

                //    case 5:
                //        PositionMin_S5 = Position_S5 - PositionRange_S5;

                //        PositionMax_S5 = Position_S5 + PositionRange_S5;

                //        break;

                //    case 6:
                //        PositionMin_S6 = Position_S6 - PositionRange_S6;

                //        PositionMax_S6 = Position_S6 + PositionRange_S6;

                //        break;

                //    case 7:
                //        PositionMin_S7 = Position_S7 - PositionRange_S7;

                //        PositionMax_S7 = Position_S7 + PositionRange_S7;

                //        break;

                //    default:
                //        Log.Trace($"UpdateAdvancedServoProperties count:{servos.Count}", Common.LOG_CATEGORY);
                //        break;

                //}
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool SetPositionRangeCanExecute(string value)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #region ConfigureServo2 Command

        //public DelegateCommand ConfigureServo2Command { get; set; }
        public DelegateCommand<string> ConfigureServo2Command { get; set; }
        // If using CommandParameter, figure out TYPE and use second above
        //public DelegateCommand<TYPE> ConfigureServo2CommandParameter;
        public string ConfigureServo2Content { get; set; } = "ConfigureServo2";
        public string ConfigureServo2ToolTip { get; set; } = "ConfigureServo2 ToolTip";

        // Can get fancy and use Resources
        //public string ConfigureServo2Content { get; set; } = "ViewName_ConfigureServo2Content";
        //public string ConfigureServo2ToolTip { get; set; } = "ViewName_ConfigureServo2ContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_ConfigureServo2Content">ConfigureServo2</system:String>
        //    <system:String x:Key="ViewName_ConfigureServo2ContentToolTip">ConfigureServo2 ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        public void ConfigureServo2(string value)
        //public void ConfigureServo2()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = $"Cool, you called ConfigureServo2 and passed: {value}";

            Int32 servoIndex = Int32.Parse(value);

            AdvancedServoServo servo = ActiveAdvancedServo.AdvancedServo.servos[servoIndex];

            try
            {
                servo.setServoParameters(
                    AdvancedServoProperties[0].MinimumPulseWidth,
                    AdvancedServoProperties[0].MaximumPulseWidth, 
                    AdvancedServoProperties[0].Degrees, 
                    (Double)AdvancedServoProperties[0].VelocityMax);
                //servo.setServoParameters(MinimumPulseWidth_S0, MaximumPulseWidth_S0, Degrees_S0, (Double)VelocityMax_S0);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<ConfigureServo2Event>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<ConfigureServo2Event>().Publish(
            //      new ConfigureServo2EventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class ConfigureServo2Event : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<ConfigureServo2Event>().Subscribe(ConfigureServo2);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool ConfigureServo2CanExecute(string value)
        //public bool ConfigureServo2CanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #endregion

        #region Event Handlers

        private void ActiveAdvancedServo_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                //Log.Trace($"ActiveAdvancedServo_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
                
                DeviceAttached = ActiveAdvancedServo.AdvancedServo.Attached;

                AdvancedServoProperties[0].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[1].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[2].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[3].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[4].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[5].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[6].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[7].AdvancedServoEx = ActiveAdvancedServo;

                UpdateAdvancedServoProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            RefreshAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            SetAdvancedServoDefaultsCommand.RaiseCanExecuteChanged();
            SetPositionRangeCommand.RaiseCanExecuteChanged();
        }

        private void UpdateAdvancedServoProperties()
        {
            if ((Boolean)DeviceAttached)
            {
                AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
                Phidgets.AdvancedServoServo servo = null;

                ServoCount = servos.Count;

                try
                {
                    for (int i = 0; i < ServoCount; i++)
                    {
                        servo = servos[i];

                        // NOTE(crhodes)
                        // All the work is now done in Type.UpdateProperties()
                        AdvancedServoProperties[i].Type = servo.Type;
                    }

                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
            else
            {
                DeviceAttached = null;
                InitializAdvancedServoUI();
            }
        }

        private void RefreshAdvancedServoUIProperties()
        {
            if ((Boolean)DeviceAttached)
            {
                AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
                Phidgets.AdvancedServoServo servo = null;

                ServoCount = servos.Count;

                try
                {
                    for (int i = 0; i < ServoCount; i++)
                    {
                        servo = servos[i];

                        // NOTE(crhodes)
                        // Should be safe to get Acceleration, Velocity, and Position here
                        // if Device is Engaged, otherwise set to null

                        AdvancedServoProperties[0].Type = servo.Type;

                        //DevicePositionMin = servo.PositionMin;
                        //DevicePositionMax = servo.PositionMax;

                        AdvancedServoProperties[i].Stopped = servo.Stopped;
                        AdvancedServoProperties[i].Engaged = servo.Engaged;
                        AdvancedServoProperties[i].SpeedRamping = servo.SpeedRamping;

                        AdvancedServoProperties[i].Current = servo.Current;

                        AdvancedServoProperties[i].AccelerationMin = servo.AccelerationMin;
                        AdvancedServoProperties[i].Acceleration = servo.Engaged ? servo.Acceleration : null;
                        AdvancedServoProperties[i].AccelerationMax = servo.AccelerationMax;

                        AdvancedServoProperties[i].VelocityMin = servo.VelocityMin;
                        AdvancedServoProperties[i].Velocity = servo.Velocity;
                        AdvancedServoProperties[i].VelocityLimit = servo.VelocityLimit;
                        AdvancedServoProperties[i].VelocityMax = servo.VelocityMax;

                        AdvancedServoProperties[i].PositionMin = servo.PositionMin;
                        AdvancedServoProperties[i].Position = servo.Engaged ? servo.Position : null;
                        AdvancedServoProperties[i].PositionMax = servo.PositionMax;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
            else
            {
                DeviceAttached = null;
                InitializAdvancedServoUI();
            }
        }

        private void ActiveAdvancedServo_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;

                Log.Trace($"ActiveAdvancedServo_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);

                DeviceAttached = ActiveAdvancedServo.AdvancedServo.Attached;

                for (int i = 0; i < ServoCount; i++)
                {
                    AdvancedServoProperties[i].AdvancedServoEx = null;
                }

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

        #region Protected Methods (none)


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

        private void InitializAdvancedServoUI()
        {
            for (int i = 0; i < 8; i++)
            {
                AdvancedServoProperties[i].InitialProperties();
                //AdvancedServoProperties[i].Stopped = null;
                //AdvancedServoProperties[i].Engaged = null;
                //AdvancedServoProperties[i].SpeedRamping = null;
                //AdvancedServoProperties[i].Current = null;

                //// NOTE(crhodes)
                //// Have to clear Acceleration before Min/Max as UI triggers an update
                //AdvancedServoProperties[i].Acceleration = null;
                //AdvancedServoProperties[i].AccelerationMin = null;
                //AdvancedServoProperties[i].AccelerationMax = null;

                //// NOTE(crhodes)
                //// Handle VelocityLimit same way as Acceleration
                //// Have not confirmed this is an issue
                //AdvancedServoProperties[i].VelocityLimit = null;
                //AdvancedServoProperties[i].VelocityMin = null;
                //AdvancedServoProperties[i].Velocity = null;
                //AdvancedServoProperties[i].VelocityMax = null;

                //AdvancedServoProperties[i].DevicePositionMin = null;
                //AdvancedServoProperties[i].PositionMin = null;
                //AdvancedServoProperties[i].Position = null;
                //AdvancedServoProperties[i].PositionMax = null;
                //AdvancedServoProperties[i].DevicePositionMax = null;

            }         
        }

        #endregion

        private void SetServoValue(Double property, Double? value)
        {
            if (value is not null)
            {
                try
                {
                    property = (double)value;
                }
                catch (PhidgetException pex)
                {
                    Log.Error(pex, Common.LOG_CATEGORY);
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    try
                    {
                        property = (double)value;
                    }
                    catch (PhidgetException pex2)
                    {
                        Log.Error(pex2, Common.LOG_CATEGORY);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, Common.LOG_CATEGORY);
                    }
                }
            }
        }

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
