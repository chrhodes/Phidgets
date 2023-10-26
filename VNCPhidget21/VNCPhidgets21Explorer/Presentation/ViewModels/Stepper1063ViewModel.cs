using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

using DevExpress.CodeParser;
using Phidgets;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Events;
using VNC.Core.Mvvm;
using VNC.Core.Services;
using VNC.Phidget;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class Stepper1063ViewModel : EventViewModelBase, IStepper1063ViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public Stepper1063ViewModel(
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

            // Turn off logging of PropertyChanged from VNC.Core
            // We display the logging in 
            //LogOnPropertyChanged = false;

            // TODO(crhodes)
            //

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);
            OpenStepperCommand = new DelegateCommand(OpenStepper, OpenStepperCanExecute);
            CloseStepperCommand = new DelegateCommand(CloseStepper, CloseStepperCanExecute);

            // TODO(crhodes)
            // For now just hard code this.  Can have UI let us choose later.

            ConfigFileName = "phidgetconfig.json";
            LoadUIConfig();

            SayHelloCommand = new DelegateCommand(
                SayHello, SayHelloCanExecute);
                
            Message = "Stepper1063ViewModel says hello";           

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
                Steppers = _selectedHost.Steppers.ToList<Resources.Stepper>();
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.Stepper> _Steppers;
        public IEnumerable<Resources.Stepper> Steppers
        {
            get
            {
                if (null == _Steppers)
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

                return _Steppers;
            }

            set
            {
                _Steppers = value;
                OnPropertyChanged();
            }
        }

        private Resources.Stepper _selectedStepper;
        public Resources.Stepper SelectedStepper
        {
            get => _selectedStepper;
            set
            {
                if (_selectedStepper == value)
                    return;
                _selectedStepper = value;

                OpenStepperCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        private StepperEx _activeStepper;
        public StepperEx ActiveStepper
        {
            get => _activeStepper;
            set
            {
                if (_activeStepper == value)
                    return;
                _activeStepper = value;

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

        private string _sAddress;
        public string StepperAddress
        {
            get => _sAddress;
            set
            {
                if (_sAddress == value)
                    return;
                _sAddress = value;
                OnPropertyChanged();
            }
        }

        private bool? _sAttached;
        public bool? StepperAttached
        {
            get => _sAttached;
            set
            {
                if (_sAttached == value)
                    return;
                _sAttached = value;
                OnPropertyChanged();
            }
        }

        private bool? _sAttachedToServer;
        public bool? StepperAttachedToServer
        {
            get => _sAttachedToServer;
            set
            {
                if (_sAttachedToServer == value)
                    return;
                _sAttachedToServer = value;
                OnPropertyChanged();
            }
        }

        private string _sClass;

        public string StepperClass
        {
            get => _sClass;
            set
            {
                if (_sClass == value)
                    return;
                _sClass = value;
                OnPropertyChanged();
            }
        }

        private string _sID;
        public string StepperID
        {
            get => _sID;
            set
            {
                if (_sID == value)
                    return;
                _sID = value;
                OnPropertyChanged();
            }
        }


        private string _sLabel;
        public string StepperLabel
        {
            get => _sLabel;
            set
            {
                if (_sLabel == value)
                    return;
                _sLabel = value;
                OnPropertyChanged();
            }
        }

        private string _sLibraryVersion;
        public string StepperLibraryVersion
        {
            get => _sLibraryVersion;
            set
            {
                if (_sLibraryVersion == value)
                    return;
                _sLibraryVersion = value;
                OnPropertyChanged();
            }
        }

        private string _sName;
        public string StepperName
        {
            get => _sName;
            set
            {
                if (_sName == value)
                    return;
                _sName = value;
                OnPropertyChanged();
            }
        }

        private int? _sPort;
        public int? StepperPort
        {
            get => _sPort;
            set
            {
                if (_sPort == value)
                    return;
                _sPort = value;
                OnPropertyChanged();
            }
        }


        private int? _sSerialNumber;

        public int? StepperSerialNumber
        {
            get => _sSerialNumber;
            set
            {
                if (_sSerialNumber == value)
                    return;
                _sSerialNumber = value;
                OnPropertyChanged();
            }
        }


        private string _sServerID;
        public string StepperServerID
        {
            get => _sServerID;
            set
            {
                if (_sServerID == value)
                    return;
                _sServerID = value;
                OnPropertyChanged();
            }
        }


        private string _sType;
        public string StepperType
        {
            get => _sType;
            set
            {
                if (_sType == value)
                    return;
                _sType = value;
                OnPropertyChanged();
            }
        }

        private int? _sVersion;
        public int? StepperVersion
        {
            get => _sVersion;
            set
            {
                if (_sVersion == value)
                    return;
                _sVersion = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        #region Command ConfigFIleName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Message = "ConfigFileName_DoubleClick";
        }

        #endregion

        #region OpenStepper Command

        public DelegateCommand OpenStepperCommand { get; set; }
        public string OpenStepperContent { get; set; } = "Open";
        public string OpenStepperToolTip { get; set; } = "OpenStepper ToolTip";

        // Can get fancy and use Resources
        //public string OpenStepperContent { get; set; } = "ViewName_OpenStepperContent";
        //public string OpenStepperToolTip { get; set; } = "ViewName_OpenStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenStepperContent">OpenStepper</system:String>
        //    <system:String x:Key="ViewName_OpenStepperContentToolTip">OpenStepper ToolTip</system:String>  

        public void OpenStepper()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenStepper";

            ActiveStepper = new StepperEx(
                SelectedHost.IPAddress,
                SelectedHost.Port,
                SelectedStepper.SerialNumber,
                SelectedStepper.Enable,
                SelectedStepper.Embedded);

            ActiveStepper.Attach += ActiveStepper_Attach;
            ActiveStepper.Detach += ActiveStepper_Detach;

            // NOTE(crhodes)
            // Capture Digital Input and Output changes so we can update the UI
            // The StepperEx attaches to these events also.
            // Itlogs the changes if xxx is set to true.

            //ActiveStepper.OutputChange += ActiveStepper_OutputChange;
            //ActiveStepper.InputChange += ActiveStepper_InputChange;

            //// NOTE(crhodes)
            //// Let's do see if we can watch some analog data stream in.

            //ActiveStepper.SensorChange += ActiveStepper_SensorChange;
            ActiveStepper.Open();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenStepperEvent>().Publish(
            //      new OpenStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenStepperEvent>().Subscribe(OpenStepper);

            // End Cut Four

            //OpenStepperCommand.RaiseCanExecuteChanged();
            //CloseStepperCommand.RaiseCanExecuteChanged();

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool OpenStepperCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedStepper is not null)
            {
                if (StepperAttached is not null)
                    return !(Boolean)StepperAttached;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CloseStepper Command

        public DelegateCommand CloseStepperCommand { get; set; }
        public string CloseStepperContent { get; set; } = "Close";
        public string CloseStepperToolTip { get; set; } = "CloseStepper ToolTip";

        // Can get fancy and use Resources
        //public string CloseStepperContent { get; set; } = "ViewName_CloseStepperContent";
        //public string CloseStepperToolTip { get; set; } = "ViewName_CloseStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseStepperContent">CloseStepper</system:String>
        //    <system:String x:Key="ViewName_CloseStepperContentToolTip">CloseStepper ToolTip</system:String>  

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

        public void CloseStepper()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseStepper";

            ActiveStepper.Close();
            UpdateStepperProperties();
            ActiveStepper = null;
            ClearDigitalInputsAndOutputs();

            //OpenStepperCommand.RaiseCanExecuteChanged();
            //CloseStepperCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseStepperEvent>().Publish(
            //      new CloseStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseStepperEvent>().Subscribe(CloseStepper);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool CloseStepperCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (StepperAttached is not null)
                return (Boolean)StepperAttached;
            else
                return false;
        }

        #endregion

        #endregion

        #region Event Handlers

        //private void PopulateSensorValues(StepperAnalogSensor interfaceKitAnalogSensor)
        //{

        //}

        //private void ActiveStepper_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        //{
        //    Phidgets.Stepper ifk = (Phidgets.Stepper)sender;

        //    StepperAnalogSensor sensor = ifk.sensors[0];

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

        //private void ActiveStepper_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        //{
        //    Phidgets.Stepper ifk = (Phidgets.Stepper)sender;

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


        //private void ActiveStepper_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        //{
        //    Phidgets.Stepper ifk = (Phidgets.Stepper)sender;
        //    var outputs = ifk.outputs;
        //    StepperDigitalOutputCollection doc = outputs;

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

        private void ActiveStepper_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.Stepper ifk = (Phidgets.Stepper)sender;
                //Phidget device = (Phidget)e.Device;
                //var b = e.GetType();
                Log.Trace($"ActiveStepper_Attach {ifk.Address},{ifk.Port} S#:{ifk.SerialNumber}", Common.LOG_CATEGORY);
                // TODO(crhodes)
                // This is where properties should be grabbed
                UpdateStepperProperties();

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void UpdateStepperProperties()
        {
            if (ActiveStepper.Attached)
            {
                StepperAddress = ActiveStepper.Address;
                StepperAttached = ActiveStepper.Attached;
                StepperAttachedToServer = ActiveStepper.AttachedToServer;
                StepperClass = ActiveStepper.Class.ToString();
                StepperID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveStepper.ID);
                StepperLabel = ActiveStepper.Label;
                StepperLibraryVersion = Phidget.LibraryVersion;  // This is a static field
                StepperName = ActiveStepper.Name;
                StepperPort = ActiveStepper.Port;
                StepperSerialNumber = ActiveStepper.SerialNumber; // This throws exception
                //SServerID = ActiveStepper.ServerID;
                StepperType = ActiveStepper.Type;
                StepperVersion = ActiveStepper.Version;

                //var sensors = ActiveStepper.sensors;
                //StepperAnalogSensor sensor = null;

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
                // NOTE(crhodes)
                // Commented out properties throw exceptions when Phidget not attached
                // Just clear field

                //SAddress = ActiveStepper.Address;
                StepperAddress = "";
                StepperAttached = ActiveStepper.Attached;
                //SAttachedToServer = ActiveStepper.AttachedToServer;
                StepperAttachedToServer = false;
                // This doesn't throw exception but let's clear anyway
                //SClass = ActiveStepper.Class.ToString();
                StepperClass = "";
                //SID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveStepper.ID);
                StepperID = "";
                //SLabel = ActiveStepper.Label;
                StepperLabel = "";
                //SLibraryVersion = ActiveStepper.LibraryVersion;
                StepperLibraryVersion = Phidget.LibraryVersion;
                //SName = ActiveStepper.Name;
                StepperName = "";
                //SSerialNumber = ActiveStepper.SerialNumber;
                StepperSerialNumber = null;
                //SServerID = ActiveStepper.ServerID;
                StepperServerID = "";
                //SType = ActiveStepper.Type;
                StepperType = "";
                //SVersion = ActiveStepper.Version;
                StepperVersion = null;
            }

            OpenStepperCommand.RaiseCanExecuteChanged();
            CloseStepperCommand.RaiseCanExecuteChanged();
        }

        private void ActiveStepper_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                Phidgets.Stepper ifk = (Phidgets.Stepper)sender;
                var a = e;
                var b = e.GetType();
                Log.Trace($"ActiveStepper_Detach {ifk.Address},{ifk.SerialNumber}", Common.LOG_CATEGORY);

                // TODO(crhodes)
                // What kind of cleanup?  Maybe set ActiveStepper to null.  Clear UI
                UpdateStepperProperties();
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
