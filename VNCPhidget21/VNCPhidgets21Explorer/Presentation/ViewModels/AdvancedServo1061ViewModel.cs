using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using DevExpress.CodeParser;
using DevExpress.Mvvm.Native;
using DevExpress.Utils.Serializing;
using DevExpress.XtraRichEdit.Commands;

using Phidgets;
using Phidgets.Events;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

using VNCPhidgets21Explorer.Resources;

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
            PerformanceFileName_DoubleClick_Command = new DelegateCommand(PerformanceFileName_DoubleClick);

            OpenAdvancedServoCommand = new DelegateCommand(OpenAdvancedServo, OpenAdvancedServoCanExecute);
            RefreshAdvancedServoCommand = new DelegateCommand(RefreshAdvancedServo, RefreshAdvancedServoCanExecute);
            SetAdvancedServoDefaultsCommand = new DelegateCommand<string>(SetAdvancedServoDefaults, SetAdvancedServoDefaultsCanExecute);
            CloseAdvancedServoCommand = new DelegateCommand(CloseAdvancedServo, CloseAdvancedServoCanExecute);

            //ConfigureServoCommand = new DelegateCommand(ConfigureServo, ConfigureServoCanExecute);

            ConfigureServo2Command = new DelegateCommand<string>(ConfigureServo2, ConfigureServo2CanExecute);

            SetPositionRangeCommand = new DelegateCommand<string>(SetPositionRange, SetPositionRangeCanExecute);

            PlayPerformanceCommand = new DelegateCommand<string>(PlayPerformance, PlayPerformanceCanExecute);


            // TODO(crhodes)
            // For now just hard code this.  Can have UI let us choose later.

            ConfigFileName = "phidgetconfig.json";
            PerformanceConfigFileName = "advancedservoperformancesconfig.json";

            LoadUIConfig();
            LoadPerformancesConfig();

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

            Resources.PhidgetConfig? phidgetConfig 
                = JsonSerializer.Deserialize<Resources.PhidgetConfig>(jsonString, jsonOptions);

            this.Hosts = phidgetConfig.Hosts.ToList();

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadPerformancesConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            var jsonOptions = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };

            string jsonString = File.ReadAllText(PerformanceConfigFileName);

            Resources.AdvancedServoPerformanceConfig? performancesConfig
                = JsonSerializer.Deserialize<Resources.AdvancedServoPerformanceConfig>(jsonString, jsonOptions);

            this.AdvancedServoPerformances = performancesConfig.AdvancedServoPerformances.ToList();

            AvailableAdvancedServoPerformances =
                performancesConfig.AdvancedServoPerformances
                .ToDictionary(k => k.Name, v => v);

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
            get => _Hosts;
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

        private Resources.AdvancedServoPerformanceConfig _advancedServoPerformanceConfig;
        public Resources.AdvancedServoPerformanceConfig AdvancedServoPerformanceConfig
        {
            get => _advancedServoPerformanceConfig;
            set
            {
                if (_advancedServoPerformanceConfig == value)
                    return;
                _advancedServoPerformanceConfig = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.AdvancedServoPerformance> _advancedServoPerformances;
        public IEnumerable<Resources.AdvancedServoPerformance> AdvancedServoPerformances
        {
            get => _advancedServoPerformances;
            set
            {
                _advancedServoPerformances = value;
                OnPropertyChanged();
            }
        }

        private Resources.AdvancedServoPerformance? _selectedAdvancedServoPerformance;
        public Resources.AdvancedServoPerformance? SelectedAdvancedServoPerformance
        {
            get => _selectedAdvancedServoPerformance;
            set
            {
                if (_selectedAdvancedServoPerformance == value) return;

                _selectedAdvancedServoPerformance = value;
                OnPropertyChanged();

                PlayPerformanceCommand.RaiseCanExecuteChanged();
            }
        }

        private Dictionary<string, Resources.AdvancedServoPerformance> _availableAdvancedServoPerformances;
        public Dictionary<string, Resources.AdvancedServoPerformance> AvailableAdvancedServoPerformances
        {
            get => _availableAdvancedServoPerformances;
            set
            {
                _availableAdvancedServoPerformances = value;
                OnPropertyChanged();
            }
        }

        //private List<AdvancedServoPerformance> _advancedServoPerformancesL;
        //public List<AdvancedServoPerformance> AdvancedServoPerformancesL
        //{
        //    get => _advancedServoPerformancesL;
        //    set
        //    {
        //        _advancedServoPerformancesL = value;
        //        OnPropertyChanged();
        //    }
        //}

        private List<AdvancedServoPerformance> _selectedPerformances;
        public List<AdvancedServoPerformance> SelectedPerformances
        {
            get => _selectedPerformances;
            set
            {
                if (_selectedPerformances == value)
                {
                    return;
                }

                _selectedPerformances = value;
                OnPropertyChanged();

                PlayPerformanceCommand.RaiseCanExecuteChanged();
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

        private bool _logPerformanceStep = false;
        public bool LogPerformanceStep
        {
            get => _logPerformanceStep;
            set
            {
                if (_logPerformanceStep == value)
                    return;
                _logPerformanceStep = value;
                OnPropertyChanged();
            }
        }

        #region AdvancedServo Properties

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

        //public Phidgets.ServoServo.ServoType ServoTypeEnum;

        private IEnumerable<Resources.AdvancedServo> _AdvancedServoTypes;
        public IEnumerable<Resources.AdvancedServo> AdvancedServoTypes
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
                PlayPerformanceCommand.RaiseCanExecuteChanged();

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

        #region Servo S0

        private double _minimumPulseWidthS0 = 1000;
        public double MinimumPulseWidth_S0
        {
            get => _minimumPulseWidthS0;
            set
            {
                if (_minimumPulseWidthS0 == value)
                    return;
                _minimumPulseWidthS0 = value;
                OnPropertyChanged();
            }
        }

        private double _maximumPulseWidthS0 = 1001;
        public double MaximumPulseWidth_S0
        {
            get => _maximumPulseWidthS0;
            set
            {
                if (_maximumPulseWidthS0 == value)
                    return;
                _maximumPulseWidthS0 = value;
                OnPropertyChanged();
            }
        }

        private double _degreesS0;
        public double Degrees_S0
        {
            get => _degreesS0;
            set
            {
                if (_degreesS0 == value)
                    return;
                _degreesS0 = value;
                OnPropertyChanged();
            }
        }

        private Phidgets.ServoServo.ServoType _typeS0;
        public Phidgets.ServoServo.ServoType Type_S0
        {
            get => _typeS0;
            set
            {
                if (_typeS0 == value)
                    return;
                _typeS0 = value;
                OnPropertyChanged();

                if (ActiveAdvancedServo is not null)
                {
                    if (ActiveAdvancedServo.AdvancedServo is not null)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[0].Type = value;
                        // HACK(crhodes)
                        // This is a bit much as only one servo changed type
                        // TODO(crhodes)
                        // Break UpdateAdvancedServoProperties into pieces
                        // that can be called from here.
                        UpdateAdvancedServoProperties();
                    }
                }
            }
        }

        private Double? _currentS0;
        public Double? Current_S0
        {
            get => _currentS0;
            set
            {
                if (_currentS0 == value)
                    return;
                _currentS0 = value;
                OnPropertyChanged();
            }
        }

        private Double? _devicePositionMax_S0;
        public Double? DevicePositionMax_S0
        {
            get => _devicePositionMax_S0;
            set
            {
                if (_devicePositionMax_S0 == value)
                    return;
                _devicePositionMax_S0 = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMax_S0;
        public Double? PositionMax_S0
        {
            get => _positionMax_S0;
            set
            {
                if (_positionMax_S0 == value)
                    return;
                _positionMax_S0 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[0].PositionMax != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[0].PositionMax = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[0].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionS0;
        public Double? Position_S0
        {
            get => _positionS0;
            set
            {
                if (_positionS0 == value)
                    return;
                _positionS0 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[0].Engaged)
                    {
                        // Do not set position until servo is engaged.
                        try
                        {
                            if (ActiveAdvancedServo.AdvancedServo.servos[0].Position != value)
                            {
                                ActiveAdvancedServo.AdvancedServo.servos[0].Position = (double)value;
                            }
                        }
                        catch (PhidgetException pex)
                        {
                            Log.Error(pex, Common.LOG_CATEGORY);
                            ActiveAdvancedServo.AdvancedServo.servos[0].Position = (double)value;
                        }
                    }
                }
            }
        }

        private Double? _positionMin_S0;
        public Double? PositionMin_S0
        {
            get => _positionMin_S0;
            set
            {
                if (_positionMin_S0 == value)
                    return;
                _positionMin_S0 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[0].PositionMin != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[0].PositionMin = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[0].Position = (double)value;
                    }
                }
            }
        }

        private Double? _devicePositionMin_S0;
        public Double? DevicePositionMin_S0
        {
            get => _devicePositionMin_S0;
            set
            {
                if (_devicePositionMin_S0 == value)
                    return;
                _devicePositionMin_S0 = value;
                OnPropertyChanged();
            }
        }

        private int _positionRangeS0 = 10;
        public int PositionRange_S0
        {
            get => _positionRangeS0;
            set
            {
                if (_positionRangeS0 == value)
                    return;
                _positionRangeS0 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityMinS0;
        public Double? VelocityMin_S0
        {
            get => _velocityMinS0;
            set
            {
                if (_velocityMinS0 == value)
                    return;
                _velocityMinS0 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityS0;
        public Double? Velocity_S0
        {
            get => _velocityS0;
            set
            {
                if (_velocityS0 == value)
                    return;
                _velocityS0 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityLimitS0;
        public Double? VelocityLimit_S0
        {
            get => _velocityLimitS0;
            set
            {
                if (_velocityLimitS0 == value)
                    return;
                _velocityLimitS0 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[0].VelocityLimit = (double)value;
                }
            }
        }

        private Double? _velocityMaxS0;
        public Double? VelocityMax_S0
        {
            get => _velocityMaxS0;
            set
            {
                if (_velocityMaxS0 == value)
                    return;
                _velocityMaxS0 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationMinS0;
        public Double? AccelerationMin_S0
        {
            get => _accelerationMinS0;
            set
            {
                if (_accelerationMinS0 == value)
                    return;
                _accelerationMinS0 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationS0;
        public Double? Acceleration_S0
        {
            get => _accelerationS0;
            set
            {
                if (_accelerationS0 == value)
                    return;
                _accelerationS0 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[0].Acceleration = (double)value;
                }
            }
        }

        private Double? _accelerationMaxS0;
        public Double? AccelerationMax_S0
        {
            get => _accelerationMaxS0;
            set
            {
                if (_accelerationMaxS0 == value)
                    return;
                _accelerationMaxS0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS0;
        public bool? Engaged_S0
        {
            get => _engagedS0;
            set
            {
                if (_engagedS0 == value)
                    return;
                _engagedS0 = value;

                if (value is not null) ActiveAdvancedServo.AdvancedServo.servos[0].Engaged = (Boolean)value;

                OnPropertyChanged();
            }
        }

        private bool? _speedRampingS0;
        public bool? SpeedRamping_S0
        {
            get => _speedRampingS0;
            set
            {
                if (_speedRampingS0 == value)
                    return;
                _speedRampingS0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS0;
        public bool? Stopped_S0
        {
            get => _stoppedS0;
            set
            {
                if (_stoppedS0 == value)
                    return;
                _stoppedS0 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S1

        private Phidgets.ServoServo.ServoType _typeS1;
        public Phidgets.ServoServo.ServoType Type_S1
        {
            get => _typeS1;
            set
            {
                if (_typeS1 == value)
                    return;
                _typeS1 = value;
                OnPropertyChanged();

                if (ActiveAdvancedServo is not null)
                {
                    if (ActiveAdvancedServo.AdvancedServo is not null)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[1].Type = value;
                        // HACK(crhodes)
                        // This is a bit much as only one servo changed type
                        // TODO(crhodes)
                        // Break UpdateAdvancedServoProperties into pieces
                        // that can be called from here.
                        UpdateAdvancedServoProperties();
                    }
                }
            }
        }

        private Double? _currentS1;
        public Double? Current_S1
        {
            get => _currentS1;
            set
            {
                if (_currentS1 == value)
                    return;
                _currentS1 = value;
                OnPropertyChanged();
            }
        }

        private Double? _devicePositionMax_S1;
        public Double? DevicePositionMax_S1
        {
            get => _devicePositionMax_S1;
            set
            {
                if (_devicePositionMax_S1 == value)
                    return;
                _devicePositionMax_S1 = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMax_S1;
        public Double? PositionMax_S1
        {
            get => _positionMax_S1;
            set
            {
                if (_positionMax_S1 == value)
                    return;
                _positionMax_S1 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[1].PositionMax != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[1].PositionMax = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[1].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionS1;
        public Double? Position_S1
        {
            get => _positionS1;
            set
            {
                if (_positionS1 == value)
                    return;
                _positionS1 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[1].Position != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[1].Position = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[1].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionMin_S1;
        public Double? PositionMin_S1
        {
            get => _positionMin_S1;
            set
            {
                if (_positionMin_S1 == value)
                    return;
                _positionMin_S1 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[1].PositionMin != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[1].PositionMin = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[1].Position = (double)value;
                    }
                }
            }
        }

        private Double? _devicePositionMin_S1;
        public Double? DevicePositionMin_S1
        {
            get => _devicePositionMin_S1;
            set
            {
                if (_devicePositionMin_S1 == value)
                    return;
                _devicePositionMin_S1 = value;
                OnPropertyChanged();
            }
        }

        private int _positionRangeS1 = 10;
        public int PositionRange_S1
        {
            get => _positionRangeS1;
            set
            {
                if (_positionRangeS1 == value)
                    return;
                _positionRangeS1 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityMinS1;
        public Double? VelocityMin_S1
        {
            get => _velocityMinS1;
            set
            {
                if (_velocityMinS1 == value)
                    return;
                _velocityMinS1 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityS1;
        public Double? Velocity_S1
        {
            get => _velocityS1;
            set
            {
                if (_velocityS1 == value)
                    return;
                _velocityS1 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityLimitS1;
        public Double? VelocityLimit_S1
        {
            get => _velocityLimitS1;
            set
            {
                if (_velocityLimitS1 == value)
                    return;
                _velocityLimitS1 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[1].VelocityLimit = (double)value;
                }
            }
        }

        private Double? _velocityMaxS1;
        public Double? VelocityMax_S1
        {
            get => _velocityMaxS1;
            set
            {
                if (_velocityMaxS1 == value)
                    return;
                _velocityMaxS1 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationMinS1;
        public Double? AccelerationMin_S1
        {
            get => _accelerationMinS1;
            set
            {
                if (_accelerationMinS1 == value)
                    return;
                _accelerationMinS1 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationS1;
        public Double? Acceleration_S1
        {
            get => _accelerationS1;
            set
            {
                if (_accelerationS1 == value)
                    return;
                _accelerationS1 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[1].Acceleration = (double)value;
                }
            }
        }

        private Double? _accelerationMaxS1;
        public Double? AccelerationMax_S1
        {
            get => _accelerationMaxS1;
            set
            {
                if (_accelerationMaxS1 == value)
                    return;
                _accelerationMaxS1 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS1;
        public bool? Engaged_S1
        {
            get => _engagedS1;
            set
            {
                if (_engagedS1 == value)
                    return;
                _engagedS1 = value;
                OnPropertyChanged();

                if (value is not null) ActiveAdvancedServo.AdvancedServo.servos[1].Engaged = (Boolean)value;

            }
        }

        private bool? _speedRampingS1;
        public bool? SpeedRamping_S1
        {
            get => _speedRampingS1;
            set
            {
                if (_speedRampingS1 == value)
                    return;
                _speedRampingS1 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS1;
        public bool? Stopped_S1
        {
            get => _stoppedS1;
            set
            {
                if (_stoppedS1 == value)
                    return;
                _stoppedS1 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S2

        private Phidgets.ServoServo.ServoType _typeS2;
        public Phidgets.ServoServo.ServoType Type_S2
        {
            get => _typeS2;
            set
            {
                if (_typeS2 == value)
                    return;
                _typeS2 = value;
                OnPropertyChanged();

                if (ActiveAdvancedServo is not null)
                {
                    if (ActiveAdvancedServo.AdvancedServo is not null)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[2].Type = value;
                        // HACK(crhodes)
                        // This is a bit much as only one servo changed type
                        // TODO(crhodes)
                        // Break UpdateAdvancedServoProperties into pieces
                        // that can be called from here.
                        UpdateAdvancedServoProperties();
                    }
                }
            }
        }

        private Double? _currentS2;
        public Double? Current_S2
        {
            get => _currentS2;
            set
            {
                if (_currentS2 == value)
                    return;
                _currentS2 = value;
                OnPropertyChanged();
            }
        }

        private Double? _devicePositionMax_S2;
        public Double? DevicePositionMax_S2
        {
            get => _devicePositionMax_S2;
            set
            {
                if (_devicePositionMax_S2 == value)
                    return;
                _devicePositionMax_S2 = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMax_S2;
        public Double? PositionMax_S2
        {
            get => _positionMax_S2;
            set
            {
                if (_positionMax_S2 == value)
                    return;
                _positionMax_S2 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[2].PositionMax != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[2].PositionMax = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[2].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionS2;
        public Double? Position_S2
        {
            get => _positionS2;
            set
            {
                if (_positionS2 == value)
                    return;
                _positionS2 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[2].Position != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[2].Position = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[2].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionMin_S2;
        public Double? PositionMin_S2
        {
            get => _positionMin_S2;
            set
            {
                if (_positionMin_S2 == value)
                    return;
                _positionMin_S2 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[2].PositionMin != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[2].PositionMin = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[2].Position = (double)value;
                    }
                }
            }
        }

        private Double? _devicePositionMin_S2;
        public Double? DevicePositionMin_S2
        {
            get => _devicePositionMin_S2;
            set
            {
                if (_devicePositionMin_S2 == value)
                    return;
                _devicePositionMin_S2 = value;
                OnPropertyChanged();
            }
        }

        private int _positionRangeS2 = 10;
        public int PositionRange_S2
        {
            get => _positionRangeS2;
            set
            {
                if (_positionRangeS2 == value)
                    return;
                _positionRangeS2 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityMinS2;
        public Double? VelocityMin_S2
        {
            get => _velocityMinS2;
            set
            {
                if (_velocityMinS2 == value)
                    return;
                _velocityMinS2 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityS2;
        public Double? Velocity_S2
        {
            get => _velocityS2;
            set
            {
                if (_velocityS2 == value)
                    return;
                _velocityS2 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityLimitS2;
        public Double? VelocityLimit_S2
        {
            get => _velocityLimitS2;
            set
            {
                if (_velocityLimitS2 == value)
                    return;
                _velocityLimitS2 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[2].VelocityLimit = (double)value;
                }
            }
        }

        private Double? _velocityMaxS2;
        public Double? VelocityMax_S2
        {
            get => _velocityMaxS2;
            set
            {
                if (_velocityMaxS2 == value)
                    return;
                _velocityMaxS2 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationMinS2;
        public Double? AccelerationMin_S2
        {
            get => _accelerationMinS2;
            set
            {
                if (_accelerationMinS2 == value)
                    return;
                _accelerationMinS2 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationS2;
        public Double? Acceleration_S2
        {
            get => _accelerationS2;
            set
            {
                if (_accelerationS2 == value)
                    return;
                _accelerationS2 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[2].Acceleration = (double)value;
                }
            }
        }

        private Double? _accelerationMaxS2;
        public Double? AccelerationMax_S2
        {
            get => _accelerationMaxS2;
            set
            {
                if (_accelerationMaxS2 == value)
                    return;
                _accelerationMaxS2 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS2;
        public bool? Engaged_S2
        {
            get => _engagedS2;
            set
            {
                if (_engagedS2 == value)
                    return;
                _engagedS2 = value;
                OnPropertyChanged();

                if (value is not null) ActiveAdvancedServo.AdvancedServo.servos[2].Engaged = (Boolean)value;
            }
        }

        private bool? _speedRampingS2;
        public bool? SpeedRamping_S2
        {
            get => _speedRampingS2;
            set
            {
                if (_speedRampingS2 == value)
                    return;
                _speedRampingS2 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS2;
        public bool? Stopped_S2
        {
            get => _stoppedS2;
            set
            {
                if (_stoppedS2 == value)
                    return;
                _stoppedS2 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S3

        private Phidgets.ServoServo.ServoType _typeS3;
        public Phidgets.ServoServo.ServoType Type_S3
        {
            get => _typeS3;
            set
            {
                if (_typeS3 == value)
                    return;
                _typeS3 = value;
                OnPropertyChanged();

                if (ActiveAdvancedServo is not null)
                {
                    if (ActiveAdvancedServo.AdvancedServo is not null)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[3].Type = value;
                        // HACK(crhodes)
                        // This is a bit much as only one servo changed type
                        // TODO(crhodes)
                        // Break UpdateAdvancedServoProperties into pieces
                        // that can be called from here.
                        UpdateAdvancedServoProperties();
                    }
                }
            }
        }

        private Double? _currentS3;
        public Double? Current_S3
        {
            get => _currentS3;
            set
            {
                if (_currentS3 == value)
                    return;
                _currentS3 = value;
                OnPropertyChanged();
            }
        }

        private Double? _devicePositionMax_S3;
        public Double? DevicePositionMax_S3
        {
            get => _devicePositionMax_S3;
            set
            {
                if (_devicePositionMax_S3 == value)
                    return;
                _devicePositionMax_S3 = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMax_S3;
        public Double? PositionMax_S3
        {
            get => _positionMax_S3;
            set
            {
                if (_positionMax_S3 == value)
                    return;
                _positionMax_S3 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[3].PositionMax != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[3].PositionMax = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[3].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionS3;
        public Double? Position_S3
        {
            get => _positionS3;
            set
            {
                if (_positionS3 == value)
                    return;
                _positionS3 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[3].Position != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[3].Position = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[3].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionMin_S3;
        public Double? PositionMin_S3
        {
            get => _positionMin_S3;
            set
            {
                if (_positionMin_S3 == value)
                    return;
                _positionMin_S3 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[3].PositionMin != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[3].PositionMin = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[3].Position = (double)value;
                    }
                }
            }
        }

        private Double? _devicePositionMin_S3;
        public Double? DevicePositionMin_S3
        {
            get => _devicePositionMin_S3;
            set
            {
                if (_devicePositionMin_S3 == value)
                    return;
                _devicePositionMin_S3 = value;
                OnPropertyChanged();
            }
        }

        private int _positionRangeS3 = 10;
        public int PositionRange_S3
        {
            get => _positionRangeS3;
            set
            {
                if (_positionRangeS3 == value)
                    return;
                _positionRangeS3 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityMinS3;
        public Double? VelocityMin_S3
        {
            get => _velocityMinS3;
            set
            {
                if (_velocityMinS3 == value)
                    return;
                _velocityMinS3 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityS3;
        public Double? Velocity_S3
        {
            get => _velocityS3;
            set
            {
                if (_velocityS3 == value)
                    return;
                _velocityS3 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityLimitS3;
        public Double? VelocityLimit_S3
        {
            get => _velocityLimitS3;
            set
            {
                if (_velocityLimitS3 == value)
                    return;
                _velocityLimitS3 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[3].VelocityLimit = (double)value;
                }
            }
        }

        private Double? _velocityMaxS3;
        public Double? VelocityMax_S3
        {
            get => _velocityMaxS3;
            set
            {
                if (_velocityMaxS3 == value)
                    return;
                _velocityMaxS3 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationMinS3;
        public Double? AccelerationMin_S3
        {
            get => _accelerationMinS3;
            set
            {
                if (_accelerationMinS3 == value)
                    return;
                _accelerationMinS3 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationS3;
        public Double? Acceleration_S3
        {
            get => _accelerationS3;
            set
            {
                if (_accelerationS3 == value)
                    return;
                _accelerationS3 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[3].Acceleration = (double)value;
                }
            }
        }

        private Double? _accelerationMaxS3;
        public Double? AccelerationMax_S3
        {
            get => _accelerationMaxS3;
            set
            {
                if (_accelerationMaxS3 == value)
                    return;
                _accelerationMaxS3 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS3;
        public bool? Engaged_S3
        {
            get => _engagedS3;
            set
            {
                if (_engagedS3 == value)
                    return;
                _engagedS3 = value;

                OnPropertyChanged();

                if (value is not null) ActiveAdvancedServo.AdvancedServo.servos[3].Engaged = (Boolean)value;
            }
        }

        private bool? _speedRampingS3;
        public bool? SpeedRamping_S3
        {
            get => _speedRampingS3;
            set
            {
                if (_speedRampingS3 == value)
                    return;
                _speedRampingS3 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS3;
        public bool? Stopped_S3
        {
            get => _stoppedS3;
            set
            {
                if (_stoppedS3 == value)
                    return;
                _stoppedS3 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S4

        private Phidgets.ServoServo.ServoType _typeS4;
        public Phidgets.ServoServo.ServoType Type_S4
        {
            get => _typeS4;
            set
            {
                if (_typeS4 == value)
                    return;
                _typeS4 = value;
                OnPropertyChanged();

                if (ActiveAdvancedServo is not null)
                {
                    if (ActiveAdvancedServo.AdvancedServo is not null)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[4].Type = value;
                        // HACK(crhodes)
                        // This is a bit much as only one servo changed type
                        // TODO(crhodes)
                        // Break UpdateAdvancedServoProperties into pieces
                        // that can be called from here.
                        UpdateAdvancedServoProperties();
                    }
                }
            }
        }

        private Double? _currentS4;
        public Double? Current_S4
        {
            get => _currentS4;
            set
            {
                if (_currentS4 == value)
                    return;
                _currentS4 = value;
                OnPropertyChanged();
            }
        }

        private Double? _devicePositionMax_S4;
        public Double? DevicePositionMax_S4
        {
            get => _devicePositionMax_S4;
            set
            {
                if (_devicePositionMax_S4 == value)
                    return;
                _devicePositionMax_S4 = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMax_S4;
        public Double? PositionMax_S4
        {
            get => _positionMax_S4;
            set
            {
                if (_positionMax_S4 == value)
                    return;
                _positionMax_S4 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[4].PositionMax != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[4].PositionMax = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[4].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionS4;
        public Double? Position_S4
        {
            get => _positionS4;
            set
            {
                if (_positionS4 == value)
                    return;
                _positionS4 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[4].Position != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[4].Position = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[4].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionMin_S4;
        public Double? PositionMin_S4
        {
            get => _positionMin_S4;
            set
            {
                if (_positionMin_S4 == value)
                    return;
                _positionMin_S4 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[4].PositionMin != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[4].PositionMin = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[4].Position = (double)value;
                    }
                }
            }
        }

        private Double? _devicePositionMin_S4;
        public Double? DevicePositionMin_S4
        {
            get => _devicePositionMin_S4;
            set
            {
                if (_devicePositionMin_S4 == value)
                    return;
                _devicePositionMin_S4 = value;
                OnPropertyChanged();
            }
        }

        private int _positionRangeS4 = 10;
        public int PositionRange_S4
        {
            get => _positionRangeS4;
            set
            {
                if (_positionRangeS4 == value)
                    return;
                _positionRangeS4 = value;
                OnPropertyChanged();
            }
        }        

        private Double? _velocityMinS4;
        public Double? VelocityMin_S4
        {
            get => _velocityMinS4;
            set
            {
                if (_velocityMinS4 == value)
                    return;
                _velocityMinS4 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityS4;
        public Double? Velocity_S4
        {
            get => _velocityS4;
            set
            {
                if (_velocityS4 == value)
                    return;
                _velocityS4 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityLimitS4;
        public Double? VelocityLimit_S4
        {
            get => _velocityLimitS4;
            set
            {
                if (_velocityLimitS4 == value)
                    return;
                _velocityLimitS4 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[4].VelocityLimit = (double)value;
                }
            }
        }

        private Double? _velocityMaxS4;
        public Double? VelocityMax_S4
        {
            get => _velocityMaxS4;
            set
            {
                if (_velocityMaxS4 == value)
                    return;
                _velocityMaxS4 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationMinS4;
        public Double? AccelerationMin_S4
        {
            get => _accelerationMinS4;
            set
            {
                if (_accelerationMinS4 == value)
                    return;
                _accelerationMinS4 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationS4;
        public Double? Acceleration_S4
        {
            get => _accelerationS4;
            set
            {
                if (_accelerationS4 == value)
                    return;
                _accelerationS4 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[4].Acceleration = (double)value;
                }
            }
        }

        private Double? _accelerationMaxS4;
        public Double? AccelerationMax_S4
        {
            get => _accelerationMaxS4;
            set
            {
                if (_accelerationMaxS4 == value)
                    return;
                _accelerationMaxS4 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS4;
        public bool? Engaged_S4
        {
            get => _engagedS4;
            set
            {
                if (_engagedS4 == value)
                    return;
                _engagedS4 = value;
                OnPropertyChanged();

                if (value is not null) ActiveAdvancedServo.AdvancedServo.servos[4].Engaged = (Boolean)value;
            }
        }

        private bool? _speedRampingS4;
        public bool? SpeedRamping_S4
        {
            get => _speedRampingS4;
            set
            {
                if (_speedRampingS4 == value)
                    return;
                _speedRampingS4 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS4;
        public bool? Stopped_S4
        {
            get => _stoppedS4;
            set
            {
                if (_stoppedS4 == value)
                    return;
                _stoppedS4 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S5

        private Phidgets.ServoServo.ServoType _typeS5;
        public Phidgets.ServoServo.ServoType Type_S5
        {
            get => _typeS5;
            set
            {
                if (_typeS5 == value)
                    return;
                _typeS5 = value;
                OnPropertyChanged();

                if (ActiveAdvancedServo is not null)
                {
                    if (ActiveAdvancedServo.AdvancedServo is not null)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[5].Type = value;
                        // HACK(crhodes)
                        // This is a bit much as only one servo changed type
                        // TODO(crhodes)
                        // Break UpdateAdvancedServoProperties into pieces
                        // that can be called from here.
                        UpdateAdvancedServoProperties();
                    }
                }
            }
        }

        private Double? _currentS5;
        public Double? Current_S5
        {
            get => _currentS5;
            set
            {
                if (_currentS5 == value)
                    return;
                _currentS5 = value;
                OnPropertyChanged();
            }
        }

        private Double? _devicePositionMax_S5;
        public Double? DevicePositionMax_S5
        {
            get => _devicePositionMax_S5;
            set
            {
                if (_devicePositionMax_S5 == value)
                    return;
                _devicePositionMax_S5 = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMax_S5;
        public Double? PositionMax_S5
        {
            get => _positionMax_S5;
            set
            {
                if (_positionMax_S5 == value)
                    return;
                _positionMax_S5 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[5].PositionMax != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[5].PositionMax = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[5].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionS5;
        public Double? Position_S5
        {
            get => _positionS5;
            set
            {
                if (_positionS5 == value)
                    return;
                _positionS5 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[5].Position != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[5].Position = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[5].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionMin_S5;
        public Double? PositionMin_S5
        {
            get => _positionMin_S5;
            set
            {
                if (_positionMin_S5 == value)
                    return;
                _positionMin_S5 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[5].PositionMin != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[5].PositionMin = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[5].Position = (double)value;
                    }
                }
            }
        }

        private Double? _devicePositionMin_S5;
        public Double? DevicePositionMin_S5
        {
            get => _devicePositionMin_S5;
            set
            {
                if (_devicePositionMin_S5 == value)
                    return;
                _devicePositionMin_S5 = value;
                OnPropertyChanged();
            }
        }

        private int _positionRangeS5 = 10;
        public int PositionRange_S5
        {
            get => _positionRangeS5;
            set
            {
                if (_positionRangeS5 == value)
                    return;
                _positionRangeS5 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityMinS5;
        public Double? VelocityMin_S5
        {
            get => _velocityMinS5;
            set
            {
                if (_velocityMinS5 == value)
                    return;
                _velocityMinS5 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityS5;
        public Double? Velocity_S5
        {
            get => _velocityS5;
            set
            {
                if (_velocityS5 == value)
                    return;
                _velocityS5 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityLimitS5;
        public Double? VelocityLimit_S5
        {
            get => _velocityLimitS5;
            set
            {
                if (_velocityLimitS5 == value)
                    return;
                _velocityLimitS5 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[5].VelocityLimit = (double)value;
                }
            }
        }

        private Double? _velocityMaxS5;
        public Double? VelocityMax_S5
        {
            get => _velocityMaxS5;
            set
            {
                if (_velocityMaxS5 == value)
                    return;
                _velocityMaxS5 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationMinS5;
        public Double? AccelerationMin_S5
        {
            get => _accelerationMinS5;
            set
            {
                if (_accelerationMinS5 == value)
                    return;
                _accelerationMinS5 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationS5;
        public Double? Acceleration_S5
        {
            get => _accelerationS5;
            set
            {
                if (_accelerationS5 == value)
                    return;
                _accelerationS5 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[5].Acceleration = (double)value;
                }
            }
        }

        private Double? _accelerationMaxS5;
        public Double? AccelerationMax_S5
        {
            get => _accelerationMaxS5;
            set
            {
                if (_accelerationMaxS5 == value)
                    return;
                _accelerationMaxS5 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS5;
        public bool? Engaged_S5
        {
            get => _engagedS5;
            set
            {
                if (_engagedS5 == value)
                    return;
                _engagedS5 = value;
                OnPropertyChanged();

                if (value is not null) ActiveAdvancedServo.AdvancedServo.servos[5].Engaged = (Boolean)value;
            }
        }

        private bool? _speedRampingS5;
        public bool? SpeedRamping_S5
        {
            get => _speedRampingS5;
            set
            {
                if (_speedRampingS5 == value)
                    return;
                _speedRampingS5 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS5;
        public bool? Stopped_S5
        {
            get => _stoppedS5;
            set
            {
                if (_stoppedS5 == value)
                    return;
                _stoppedS5 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S6

        private Phidgets.ServoServo.ServoType _typeS6;
        public Phidgets.ServoServo.ServoType Type_S6
        {
            get => _typeS6;
            set
            {
                if (_typeS6 == value)
                    return;
                _typeS6 = value;
                OnPropertyChanged();

                if (ActiveAdvancedServo is not null)
                {
                    if (ActiveAdvancedServo.AdvancedServo is not null)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[6].Type = value;
                        // HACK(crhodes)
                        // This is a bit much as only one servo changed type
                        // TODO(crhodes)
                        // Break UpdateAdvancedServoProperties into pieces
                        // that can be called from here.
                        UpdateAdvancedServoProperties();
                    }
                }
            }
        }

        private Double? _currentS6;
        public Double? Current_S6
        {
            get => _currentS6;
            set
            {
                if (_currentS6 == value)
                    return;
                _currentS6 = value;
                OnPropertyChanged();
            }
        }

        private Double? _devicePositionMax_S6;
        public Double? DevicePositionMax_S6
        {
            get => _devicePositionMax_S6;
            set
            {
                if (_devicePositionMax_S6 == value)
                    return;
                _devicePositionMax_S6 = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMax_S6;
        public Double? PositionMax_S6
        {
            get => _positionMax_S6;
            set
            {
                if (_positionMax_S6 == value)
                    return;
                _positionMax_S6 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[6].PositionMax != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[6].PositionMax = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[6].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionS6;
        public Double? Position_S6
        {
            get => _positionS6;
            set
            {
                if (_positionS6 == value)
                    return;
                _positionS6 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[6].Position != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[6].Position = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[6].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionMin_S6;
        public Double? PositionMin_S6
        {
            get => _positionMin_S6;
            set
            {
                if (_positionMin_S6 == value)
                    return;
                _positionMin_S6 = value;
                OnPropertyChanged();
            }
        }

        private Double? _devicePositionMin_S6;
        public Double? DevicePositionMin_S6
        {
            get => _devicePositionMin_S6;
            set
            {
                if (_devicePositionMin_S6 == value)
                    return;
                _devicePositionMin_S6 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[6].PositionMin != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[6].PositionMin = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[6].Position = (double)value;
                    }
                }
            }
        }

        private int _positionRangeS6 = 10;
        public int PositionRange_S6
        {
            get => _positionRangeS6;
            set
            {
                if (_positionRangeS6 == value)
                    return;
                _positionRangeS6 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityMinS6;
        public Double? VelocityMin_S6
        {
            get => _velocityMinS6;
            set
            {
                if (_velocityMinS6 == value)
                    return;
                _velocityMinS6 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityS6;
        public Double? Velocity_S6
        {
            get => _velocityS6;
            set
            {
                if (_velocityS6 == value)
                    return;
                _velocityS6 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityLimitS6;
        public Double? VelocityLimit_S6
        {
            get => _velocityLimitS6;
            set
            {
                if (_velocityLimitS6 == value)
                    return;
                _velocityLimitS6 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[0].VelocityLimit = (double)value;
                }
            }
        }

        private Double? _velocityMaxS6;
        public Double? VelocityMax_S6
        {
            get => _velocityMaxS6;
            set
            {
                if (_velocityMaxS6 == value)
                    return;
                _velocityMaxS6 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationMinS6;
        public Double? AccelerationMin_S6
        {
            get => _accelerationMinS6;
            set
            {
                if (_accelerationMinS6 == value)
                    return;
                _accelerationMinS6 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationS6;
        public Double? Acceleration_S6
        {
            get => _accelerationS6;
            set
            {
                if (_accelerationS6 == value)
                    return;
                _accelerationS6 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[6].Acceleration = (double)value;
                }
            }
        }

        private Double? _accelerationMaxS6;
        public Double? AccelerationMax_S6
        {
            get => _accelerationMaxS6;
            set
            {
                if (_accelerationMaxS6 == value)
                    return;
                _accelerationMaxS6 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS6;
        public bool? Engaged_S6
        {
            get => _engagedS6;
            set
            {
                if (_engagedS6 == value)
                    return;
                _engagedS6 = value;
                OnPropertyChanged();

                if (value is not null) ActiveAdvancedServo.AdvancedServo.servos[6].Engaged = (Boolean)value;
            }
        }

        private bool? _speedRampingS6;
        public bool? SpeedRamping_S6
        {
            get => _speedRampingS6;
            set
            {
                if (_speedRampingS6 == value)
                    return;
                _speedRampingS6 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS6;
        public bool? Stopped_S6
        {
            get => _stoppedS6;
            set
            {
                if (_stoppedS6 == value)
                    return;
                _stoppedS6 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S7

        private Phidgets.ServoServo.ServoType _typeS7;
        public Phidgets.ServoServo.ServoType Type_S7
        {
            get => _typeS7;
            set
            {
                if (_typeS7 == value)
                    return;
                _typeS7 = value;
                OnPropertyChanged();

                if (ActiveAdvancedServo is not null)
                {
                    if (ActiveAdvancedServo.AdvancedServo is not null)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[7].Type = value;
                        // HACK(crhodes)
                        // This is a bit much as only one servo changed type
                        // TODO(crhodes)
                        // Break UpdateAdvancedServoProperties into pieces
                        // that can be called from here.
                        UpdateAdvancedServoProperties();
                    }
                }
            }
        }

        private Double? _currentS7;
        public Double? Current_S7
        {
            get => _currentS7;
            set
            {
                if (_currentS7 == value)
                    return;
                _currentS7 = value;
                OnPropertyChanged();
            }
        }

        private Double? _devicePositionMax_S7;
        public Double? DevicePositionMax_S7
        {
            get => _devicePositionMax_S7;
            set
            {
                if (_devicePositionMax_S7 == value)
                    return;
                _devicePositionMax_S7 = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMax_S7;
        public Double? PositionMax_S7
        {
            get => _positionMax_S7;
            set
            {
                if (_positionMax_S7 == value)
                    return;
                _positionMax_S7 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[7].PositionMax != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[7].PositionMax = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[7].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionS7;
        public Double? Position_S7
        {
            get => _positionS7;
            set
            {
                if (_positionS7 == value)
                    return;
                _positionS7 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[7].Position != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[7].Position = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[7].Position = (double)value;
                    }
                }
            }
        }

        private Double? _positionMin_S7;
        public Double? PositionMin_S7
        {
            get => _positionMin_S7;
            set
            {
                if (_positionMin_S7 == value)
                    return;
                _positionMin_S7 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (ActiveAdvancedServo.AdvancedServo.servos[7].PositionMin != value)
                        {
                            ActiveAdvancedServo.AdvancedServo.servos[7].PositionMin = (double)value;
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        ActiveAdvancedServo.AdvancedServo.servos[7].Position = (double)value;
                    }
                }
            }
        }

        private Double? _devicePositionMin_S7;
        public Double? DevicePositionMin_S7
        {
            get => _devicePositionMin_S7;
            set
            {
                if (_devicePositionMin_S7 == value)
                    return;
                _devicePositionMin_S7 = value;
                OnPropertyChanged();
            }
        }

        private int _positionRangeS7 = 10;
        public int PositionRange_S7
        {
            get => _positionRangeS7;
            set
            {
                if (_positionRangeS7 == value)
                    return;
                _positionRangeS7 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityMinS7;
        public Double? VelocityMin_S7
        {
            get => _velocityMinS7;
            set
            {
                if (_velocityMinS7 == value)
                    return;
                _velocityMinS7 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityS7;
        public Double? Velocity_S7
        {
            get => _velocityS7;
            set
            {
                if (_velocityS7 == value)
                    return;
                _velocityS7 = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityLimitS7;
        public Double? VelocityLimit_S7
        {
            get => _velocityLimitS7;
            set
            {
                if (_velocityLimitS7 == value)
                    return;
                _velocityLimitS7 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[7].VelocityLimit = (double)value;
                }
            }
        }

        private Double? _velocityMaxS7;
        public Double? VelocityMax_S7
        {
            get => _velocityMaxS7;
            set
            {
                if (_velocityMaxS7 == value)
                    return;
                _velocityMaxS7 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationMinS7;
        public Double? AccelerationMin_S7
        {
            get => _accelerationMinS7;
            set
            {
                if (_accelerationMinS7 == value)
                    return;
                _accelerationMinS7 = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationS7;
        public Double? Acceleration_S7
        {
            get => _accelerationS7;
            set
            {
                if (_accelerationS7 == value)
                    return;
                _accelerationS7 = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[7].Acceleration = (double)value;
                }
            }
        }

        private Double? _accelerationMaxS7;
        public Double? AccelerationMax_S7
        {
            get => _accelerationMaxS7;
            set
            {
                if (_accelerationMaxS7 == value)
                    return;
                _accelerationMaxS7 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS7;
        public bool? Engaged_S7
        {
            get => _engagedS7;
            set
            {
                if (_engagedS7 == value)
                    return;
                _engagedS7 = value;
                OnPropertyChanged();

                if (value is not null) ActiveAdvancedServo.AdvancedServo.servos[7].Engaged = (Boolean)value;
            }
        }

        private bool? _speedRampingS7;
        public bool? SpeedRamping_S7
        {
            get => _speedRampingS7;
            set
            {
                if (_speedRampingS7 == value)
                    return;
                _speedRampingS7 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS7;
        public bool? Stopped_S7
        {
            get => _stoppedS7;
            set
            {
                if (_stoppedS7 == value)
                    return;
                _stoppedS7 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Commands
        
        public ICommand SayHelloCommand { get; private set; }

        #region Command ConfigFileName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }
        public DelegateCommand PerformanceFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Message = "ConfigFileName_DoubleClick";
        }

        private void PerformanceFileName_DoubleClick()
        {
            Message = "PerformanceFileName_DoubleClick";

            LoadPerformancesConfig();
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

            ActiveAdvancedServo.AdvancedServo.CurrentChange += ActiveAdvancedServo_CurrentChange;
            ActiveAdvancedServo.AdvancedServo.PositionChange += ActiveAdvancedServo_PositionChange;
            ActiveAdvancedServo.AdvancedServo.VelocityChange += ActiveAdvancedServo_VelocityChange;

            PlayPerformanceCommand.RaiseCanExecuteChanged();

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

            switch (e.Index)
            {
                case 0:
                    Velocity_S0 = e.Velocity;
                    break;

                case 1:
                    Velocity_S1 = e.Velocity;
                    break;

                case 2:
                    Velocity_S2 = e.Velocity;
                    break;

                case 3:
                    Velocity_S3 = e.Velocity;
                    break;

                case 4:
                    Velocity_S4 = e.Velocity;
                    break;

                case 5:
                    Velocity_S5 = e.Velocity;
                    break;

                case 60:
                    Velocity_S6 = e.Velocity;
                    break;

                case 7:
                    Velocity_S7 = e.Velocity;
                    break;

                default:
                    Log.Trace($"VelocityChange index:{index} value:{velocity}", Common.LOG_CATEGORY);
                    break;
            }
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

            switch (e.Index)
            {
                case 0:
                    Position_S0 = e.Position;
                    Stopped_S0 = servo.servos[e.Index].Stopped;
                    break;

                case 1:
                    Position_S1 = e.Position;
                    Stopped_S1 = servo.servos[e.Index].Stopped;
                    break;

                case 2:
                    Position_S2 = e.Position;
                    Stopped_S2 = servo.servos[e.Index].Stopped;
                    break;

                case 3:
                    Position_S3 = e.Position;
                    Stopped_S3 = servo.servos[e.Index].Stopped;
                    break;

                case 4:
                    Position_S4 = e.Position;
                    Stopped_S4 = servo.servos[e.Index].Stopped;
                    break;

                case 5:
                    Position_S5 = e.Position;
                    Stopped_S5 = servo.servos[e.Index].Stopped;
                    break;

                case 6:
                    Position_S6 = e.Position;
                    Stopped_S6 = servo.servos[e.Index].Stopped;
                    break;

                case 7:
                    Position_S7 = e.Position;
                    Stopped_S7 = servo.servos[e.Index].Stopped;
                    break;

                default:
                    Log.Trace($"PositionChange index:{index} value:{position}", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void ActiveAdvancedServo_CurrentChange(object sender, CurrentChangeEventArgs e)
        {
            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var current = e.Current;

            switch (e.Index)
            {
                case 0:
                    Current_S0 = e.Current;
                    break;

                case 1:
                    Current_S1 = e.Current;
                    break;

                case 2:
                    Current_S2 = e.Current;
                    break;

                case 3:
                    Current_S3 = e.Current;
                    break;

                case 4:
                    Current_S4 = e.Current;
                    break;

                case 5:
                    Current_S5 = e.Current;
                    break;

                case 60:
                    Current_S6 = e.Current;
                    break;

                case 7:
                    Current_S7 = e.Current;
                    break;

                default:
                    // NOTE(crhodes)
                    // Constant stream of this.
                    // Do we really need to monitor current?

                    //Log.Trace($"CurrentChange index:{index} value:{current}", Common.LOG_CATEGORY);
                    break;
            }
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
            Double? tenPercent;

            try
            {
                switch (servoIndex)
                {
                    case 0:
                        Acceleration_S0 = AccelerationMin_S0;
                        VelocityLimit_S0 = VelocityMin_S0 == 0 ? 10 : VelocityMin_S0;
                        halfRange = (PositionMax_S0 - PositionMin_S0) / 2;
                        tenPercent = halfRange / 10;
                        PositionMin_S0 = halfRange - tenPercent;
                        Position_S0 = halfRange;
                        PositionMax_S0 = halfRange + tenPercent;

                        break;

                    case 1:
                        Acceleration_S1 = AccelerationMin_S1;
                        VelocityLimit_S1 = VelocityMin_S1 == 0 ? 10 : VelocityMin_S1;
                        halfRange = (PositionMax_S1 - PositionMin_S1) / 2;
                        tenPercent = halfRange / 10;
                        PositionMin_S1 = halfRange - tenPercent;
                        Position_S1 = halfRange;
                        PositionMax_S1 = halfRange + tenPercent;

                        break;

                    case 2:
                        Acceleration_S2 = AccelerationMin_S2;
                        VelocityLimit_S2 = VelocityMin_S2 == 0 ? 10 : VelocityMin_S2;
                        halfRange = (PositionMax_S2 - PositionMin_S2) / 2;
                        tenPercent = halfRange / 10;
                        PositionMin_S2 = halfRange - tenPercent;
                        Position_S2 = halfRange;
                        PositionMax_S2 = halfRange + tenPercent;

                        break;

                    case 3:
                        Acceleration_S3 = AccelerationMin_S3;
                        VelocityLimit_S3 = VelocityMin_S3 == 0 ? 10 : VelocityMin_S3;
                        halfRange = (PositionMax_S3 - PositionMin_S3) / 2;
                        tenPercent = halfRange / 10;
                        PositionMin_S3 = halfRange - tenPercent; ;
                        Position_S3 = halfRange;
                        PositionMax_S3 = halfRange + tenPercent;

                        break;

                    case 4:
                        Acceleration_S4 = AccelerationMin_S4;
                        VelocityLimit_S4 = VelocityMin_S4 == 0 ? 10 : VelocityMin_S4;
                        halfRange = (PositionMax_S4 - PositionMin_S4) / 2;
                        tenPercent = halfRange / 10;
                        PositionMin_S4 = halfRange - tenPercent;
                        Position_S4 = halfRange;
                        PositionMax_S4 = halfRange + tenPercent;

                        break;

                    case 5:
                        Acceleration_S5 = AccelerationMin_S5;
                        VelocityLimit_S5 = VelocityMin_S5 == 0 ? 10 : VelocityMin_S5;
                        halfRange = (PositionMax_S5 - PositionMin_S5) / 2;
                        tenPercent = halfRange / 10;
                        PositionMin_S5 = halfRange - tenPercent;
                        Position_S5 = halfRange;
                        PositionMax_S5 = halfRange + tenPercent;

                        break;

                    case 6:
                        Acceleration_S6 = AccelerationMin_S6;
                        VelocityLimit_S6 = VelocityMin_S6 == 0 ? 10 : VelocityMin_S6;
                        halfRange = (PositionMax_S6 - PositionMin_S6) / 2;
                        tenPercent = halfRange / 10;
                        PositionMin_S6 = halfRange - tenPercent;
                        Position_S6 = halfRange;
                        PositionMax_S6 = halfRange + tenPercent;

                        break;

                    case 7:
                        Acceleration_S7 = AccelerationMin_S7;
                        VelocityLimit_S7 = VelocityMin_S7 == 0 ? 10 : VelocityMin_S7;
                        halfRange = (PositionMax_S7 - PositionMin_S7) / 2;
                        tenPercent = halfRange / 10;
                        PositionMin_S7 = halfRange - tenPercent;
                        Position_S7 = halfRange;
                        PositionMax_S7 = halfRange + tenPercent;

                        break;

                    default:
                        Log.Trace($"UpdateAdvancedServoProperties count:{servos.Count}", Common.LOG_CATEGORY);
                        break;

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

            PlayPerformanceCommand.RaiseCanExecuteChanged();
            
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
                switch (servoIndex)
                {
                    case 0:
                        // TODO(crhodes)
                        // Make this fancier.  Take the smaller of difference
                        // between Min and Current and Max and Current
                        // and then divide that by 10

                        PositionMin_S0 = Position_S0 - PositionRange_S0;

                        PositionMax_S0 = Position_S0 + PositionRange_S0;

                        break;

                    case 1:
                        PositionMin_S1 = Position_S1 - PositionRange_S1;

                        PositionMax_S1 = Position_S1 + PositionRange_S1;

                        break;

                    case 2:
                        PositionMin_S2 = Position_S2 - PositionRange_S2;

                        PositionMax_S2 = Position_S2 + PositionRange_S2;

                        break;

                    case 3:
                        PositionMin_S3 = Position_S3 - PositionRange_S3;

                        PositionMax_S3 = Position_S3 + PositionRange_S3;

                        break;

                    case 4:
                        PositionMin_S4 = Position_S4 - PositionRange_S4;

                        PositionMax_S4 = Position_S4 + PositionRange_S4;

                        break;

                    case 5:
                        PositionMin_S5 = Position_S5 - PositionRange_S5;

                        PositionMax_S5 = Position_S5 + PositionRange_S5;

                        break;

                    case 6:
                        PositionMin_S6 = Position_S6 - PositionRange_S6;

                        PositionMax_S6 = Position_S6 + PositionRange_S6;

                        break;

                    case 7:
                        PositionMin_S7 = Position_S7 - PositionRange_S7;

                        PositionMax_S7 = Position_S7 + PositionRange_S7;

                        break;

                    default:
                        Log.Trace($"UpdateAdvancedServoProperties count:{servos.Count}", Common.LOG_CATEGORY);
                        break;

                }
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

        // End Cut One

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
                servo.setServoParameters(MinimumPulseWidth_S0, MaximumPulseWidth_S0, Degrees_S0, (Double)VelocityMax_S0);
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

        #region PlayPerformance Command

        public DelegateCommand<string> PlayPerformanceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlayPerformanceCommand { get; set; }
        //public TYPE PlayPerformanceCommandParameter;
        public string PlayPerformanceContent { get; set; } = "PlayPerformance";
        public string PlayPerformanceToolTip { get; set; } = "PlayPerformance ToolTip";

        // Can get fancy and use Resources
        //public string PlayPerformanceContent { get; set; } = "ViewName_PlayPerformanceContent";
        //public string PlayPerformanceToolTip { get; set; } = "ViewName_PlayPerformanceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlayPerformanceContent">PlayPerformance</system:String>
        //    <system:String x:Key="ViewName_PlayPerformanceContentToolTip">PlayPerformance ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlayPerformance(TYPE value)
        public async void PlayPerformance(string playAll)
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlayPerformance";

            if (Boolean.Parse(playAll))
            {
                foreach (AdvancedServoPerformance performance in AdvancedServoPerformances)
                {
                    await PlayPerformanceLoops(performance);
                }
            }
            else
            {
                var runAllThese = SelectedPerformances;
                var allPerformances = AvailableAdvancedServoPerformances;

                //var runAllTheseD = SelectedAdvancedServoPerformanceD;

                //// TODO(crhodes)
                //// Figure out how to handle loops
                //var nextPerformance = SelectedAdvancedServoPerformance;

                foreach (AdvancedServoPerformance performance in SelectedPerformances)
                {
                    Log.Trace($"Running performance:{performance.Name}", Common.LOG_CATEGORY);

                    var nextPerformance = performance;

                    string name = "";
                    string? continueWith = "";

                    do
                    {
                        name = nextPerformance.Name;
                        continueWith = nextPerformance.ContinueWith;
                        Log.Trace($"  Playing performance:{name} continueWidth:{continueWith}", Common.LOG_CATEGORY);

                        await PlayPerformanceLoops(nextPerformance);

                        if (AvailableAdvancedServoPerformances.ContainsKey(continueWith ?? ""))
                        {
                            nextPerformance = AvailableAdvancedServoPerformances[continueWith];
                        }
                        else
                        {
                            continueWith = "";
                        }

                    } while (!string.IsNullOrEmpty(continueWith));
                }

            }

            // HACK(crhodes)
            // Lets try calling another performance.  If this works, move to a dictionary.



            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<PlayPerformanceEvent>().Publish(
            //      new PlayPerformanceEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class PlayPerformanceEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Subscribe(PlayPerformance);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async Task PlayPerformanceLoops(AdvancedServoPerformance advancedServoPerformance)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            for (int i = 0; i < advancedServoPerformance.Loops; i++)
            {
                Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);

                if (advancedServoPerformance.PlayInParallel)
                {
                    PlayPerformanceInParallel(advancedServoPerformance);
                }
                else
                {
                    PlayPerformanceInSequence(advancedServoPerformance);
                }
                
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async void PlayPerformanceInParallel(AdvancedServoPerformance advancedServoPerformance)
        {
            // TODO(crhodes)
            // Figure out how to use Parallel.Foreach

            Parallel.ForEach(advancedServoPerformance.AdvancedServoSteps, step =>
            {
                if (LogPerformanceStep)
                {
                    Log.Trace($"Servo:{step.ServoIndex} Acceleration:{step.Acceleration} VelocityLimit:{step.VelocityLimit}" +
                        $" Engaged:{step.Engaged} TargetPosition:{step.TargetPosition} Duration:{step.Duration}", Common.LOG_CATEGORY);
                }

                try
                {
                    switch (step.ServoIndex)
                    {
                        case 0:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[0], step, 0);
                            break;

                        case 1:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[1], step, 1);
                            break;

                        case 2:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[2], step, 2);
                            break;

                        case 3:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[3], step, 3);
                            break;

                        case 4:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[4], step, 4);
                            break;

                        case 5:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[5], step, 5);
                            break;

                        case 6:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[6], step, 6);
                            break;

                        case 7:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[7], step, 7);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            });

            //foreach (AdvancedServoStep step in advancedServoPerformance.AdvancedServoSteps)
            //{
            //    if (LogPerformanceStep)
            //    {
            //        Log.Trace($"Servo:{step.ServoIndex} Acceleration:{step.Acceleration} VelocityLimit:{step.VelocityLimit}" +
            //            $" Engaged:{step.Engaged} TargetPosition:{step.TargetPosition} Duration:{step.Duration}", Common.LOG_CATEGORY);
            //    }

            //    try
            //    {
            //        switch (step.ServoIndex)
            //        {
            //            case 0:
            //                PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[0], step, 0);
            //                break;

            //            case 1:
            //                PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[1], step, 1);
            //                break;

            //            case 2:
            //                PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[2], step, 2);
            //                break;

            //            case 3:
            //                // HACK(crhodes)
            //                // This is early POC
            //                await Task.Run(() =>
            //                {
            //                    Parallel.Invoke(
            //                        () => PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[0], step, 0),
            //                        () => PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[1], step, 1),
            //                        () => PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[2], step, 2)
            //                    );
            //                });
            //                //PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[3], step, 3);
            //                break;

            //            case 4:
            //                PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[4], step, 4);
            //                break;

            //            case 5:
            //                PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[5], step, 5);
            //                break;

            //            case 6:
            //                PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[6], step, 6);
            //                break;

            //            case 7:
            //                PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[7], step, 7);
            //                break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error(ex, Common.LOG_CATEGORY);
            //    }
            //}
        }

        private void PlayPerformanceInSequence(AdvancedServoPerformance advancedServoPerformance)
        {
            foreach (AdvancedServoStep step in advancedServoPerformance.AdvancedServoSteps)
            {
                if (LogPerformanceStep)
                {
                    Log.Trace($"Servo:{step.ServoIndex} Acceleration:{step.Acceleration} VelocityLimit:{step.VelocityLimit}" +
                        $" Engaged:{step.Engaged} TargetPosition:{step.TargetPosition} Duration:{step.Duration}", Common.LOG_CATEGORY);
                }

                try
                {
                    switch (step.ServoIndex)
                    {
                        case 0:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[0], step, 0);
                            break;

                        case 1:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[1], step, 1);
                            break;

                        case 2:
                             PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[2], step, 2);
                            break;

                        case 3:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[3], step, 3);
                            break;

                        case 4:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[4], step, 4);
                            break;

                        case 5:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[5], step, 5);
                            break;

                        case 6:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[6], step, 6);
                            break;

                        case 7:
                            PerformServoStep(ActiveAdvancedServo.AdvancedServo.servos[7], step, 7);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void PerformServoStep(AdvancedServoServo servo, AdvancedServoStep step, Int32 index)
        {
            Int64 startTicks = Log.Trace($"Enter servo:{index}", Common.LOG_CATEGORY);

            try
            {
                if (step.Acceleration is not null) servo.Acceleration = (Double)step.Acceleration;
                if (step.VelocityLimit is not null) servo.VelocityLimit = (Double)step.VelocityLimit;
                if (step.Engaged is not null) servo.Engaged = (Boolean)step.Engaged;

                // TODO(crhodes)
                // Maybe wait for servo Engaged to complete if not currently engaged
                // View logs and see how often exceptions thrown.

                if (step.TargetPosition is not null)
                {
                    servo.Position = (Double)step.TargetPosition;
                    Thread.Sleep(1);

                    VerifyNewPositionAchieved(servo, (Double)step.TargetPosition);
                     
                    if (step.Duration > 0) Thread.Sleep((Int32)step.Duration);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void VerifyNewPositionAchieved(AdvancedServoServo servo, double targetPosition)
        {
            while (servo.Position != targetPosition) { Thread.Sleep(1); }
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool PlayPerformanceCanExecute(TYPE value)
        public bool PlayPerformanceCanExecute(string playAll)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (ActiveAdvancedServo is not null && SelectedPerformances?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        private void ActiveAdvancedServo_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                Log.Trace($"ActiveAdvancedServo_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
                
                DeviceAttached = ActiveAdvancedServo.AdvancedServo.Attached;
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
                        // Avoid setting defaults here.
                        // Put those in a performance or behind a button

                        switch (i)
                        {
                            case 0:
                                Type_S0 = servo.Type;
                                // Get the device position min/max before any changes are made
                                DevicePositionMin_S0 = servo.PositionMin;
                                DevicePositionMax_S0 = servo.PositionMax;

                                Stopped_S0 = servo.Stopped;
                                Engaged_S0 = servo.Engaged;
                                SpeedRamping_S0 = servo.SpeedRamping;

                                Current_S0 = servo.Current;

                                AccelerationMin_S0 = servo.AccelerationMin;
                                //Acceleration_S0 = servo.Acceleration;// = servo.AccelerationMin;// = initialAcceleration;
                                AccelerationMax_S0 = servo.AccelerationMax;

                                VelocityMin_S0 = servo.VelocityMin;
                                Velocity_S0 = servo.Velocity;
                                VelocityLimit_S0 = servo.VelocityLimit = servo.VelocityMin;
                                VelocityMax_S0 = servo.VelocityMax;

                                PositionMin_S0 = servo.PositionMin;
                                //Position_S0 = 90;
                                PositionMax_S0 = servo.PositionMax;

                                break;

                            case 1:
                                Type_S1 = servo.Type;
                                // Get the device position min/max before any changes are made
                                DevicePositionMin_S1 = servo.PositionMin;
                                DevicePositionMax_S1 = servo.PositionMax;

                                Stopped_S1 = servo.Stopped;
                                Engaged_S1 = servo.Engaged;
                                SpeedRamping_S1 = servo.SpeedRamping;

                                Current_S1 = servo.Current;

                                AccelerationMin_S1 = servo.AccelerationMin;
                                //Acceleration_S1 = servo.Acceleration;// = servo.AccelerationMin;// = initialAcceleration;
                                AccelerationMax_S1 = servo.AccelerationMax;

                                VelocityMin_S1 = servo.VelocityMin;
                                Velocity_S1 = servo.Velocity;
                                VelocityLimit_S1 = servo.VelocityLimit = servo.VelocityMin;
                                VelocityMax_S1 = servo.VelocityMax;

                                PositionMin_S1 = servo.PositionMin;
                                //Position_S1 = 90;
                                PositionMax_S1 = servo.PositionMax;

                                break;

                            case 2:
                                Type_S2 = servo.Type;
                                // Get the device position min/max before any changes are made
                                DevicePositionMin_S2 = servo.PositionMin;
                                DevicePositionMax_S2 = servo.PositionMax;

                                Stopped_S2 = servo.Stopped;
                                Engaged_S2 = servo.Engaged;
                                SpeedRamping_S2 = servo.SpeedRamping;

                                Current_S2 = servo.Current;

                                AccelerationMin_S2 = servo.AccelerationMin;
                                //Acceleration_S2 = servo.Acceleration;// = servo.AccelerationMin;// = initialAcceleration;
                                AccelerationMax_S2 = servo.AccelerationMax;

                                VelocityMin_S2 = servo.VelocityMin;
                                Velocity_S2 = servo.Velocity;
                                VelocityLimit_S2 = servo.VelocityLimit = servo.VelocityMin;
                                VelocityMax_S2 = servo.VelocityMax;

                                PositionMin_S2 = servo.PositionMin;
                                //Position_S2 = 90;
                                PositionMax_S2 = servo.PositionMax;

                                break;

                            case 3:
                                Type_S3 = servo.Type;
                                // Get the device position min/max before any changes are made
                                DevicePositionMin_S3 = servo.PositionMin;
                                DevicePositionMax_S3 = servo.PositionMax;

                                Stopped_S3 = servo.Stopped;
                                Engaged_S3 = servo.Engaged;
                                SpeedRamping_S3 = servo.SpeedRamping;

                                Current_S3 = servo.Current;

                                AccelerationMin_S3 = servo.AccelerationMin;
                                //Acceleration_S3 = servo.Acceleration;// = servo.AccelerationMin;// = initialAcceleration;
                                AccelerationMax_S3 = servo.AccelerationMax;

                                VelocityMin_S3 = servo.VelocityMin;
                                Velocity_S3 = servo.Velocity;
                                VelocityLimit_S3 = servo.VelocityLimit = servo.VelocityMin;
                                VelocityMax_S3 = servo.VelocityMax;

                                PositionMin_S3 = servo.PositionMin;
                                //Position_S3 = 90;
                                PositionMax_S3 = servo.PositionMax;

                                break;

                            case 4:
                                Type_S4 = servo.Type;
                                // Get the device position min/max before any changes are made
                                DevicePositionMin_S4 = servo.PositionMin;
                                DevicePositionMax_S4 = servo.PositionMax;

                                Stopped_S4 = servo.Stopped;
                                Engaged_S4 = servo.Engaged;
                                SpeedRamping_S4 = servo.SpeedRamping;

                                Current_S4 = servo.Current;

                                AccelerationMin_S4 = servo.AccelerationMin;
                                //Acceleration_S4 = servo.Acceleration = servo.AccelerationMin;// = initialAcceleration;
                                AccelerationMax_S4 = servo.AccelerationMax;

                                VelocityMin_S4 = servo.VelocityMin;
                                Velocity_S4 = servo.Velocity;
                                VelocityLimit_S4 = servo.VelocityLimit = servo.VelocityMin;
                                VelocityMax_S4 = servo.VelocityMax;

                                PositionMin_S4 = servo.PositionMin;
                                //Position_S4 = 90;
                                PositionMax_S4 = servo.PositionMax;

                                break;

                            case 5:
                                Type_S5 = servo.Type;
                                // Get the device position min/max before any changes are made
                                DevicePositionMin_S5 = servo.PositionMin;
                                DevicePositionMax_S5 = servo.PositionMax;

                                Stopped_S5 = servo.Stopped;
                                Engaged_S5 = servo.Engaged;
                                SpeedRamping_S5 = servo.SpeedRamping;

                                Current_S5 = servo.Current;

                                AccelerationMin_S5 = servo.AccelerationMin;
                                //Acceleration_S5 = servo.Acceleration;// = servo.AccelerationMin;// = initialAcceleration;
                                AccelerationMax_S5 = servo.AccelerationMax;

                                VelocityMin_S5 = servo.VelocityMin;
                                Velocity_S5 = servo.Velocity;
                                VelocityLimit_S5 = servo.VelocityLimit = servo.VelocityMin;
                                VelocityMax_S5 = servo.VelocityMax;

                                PositionMin_S5 = servo.PositionMin;
                                //Position_S5 = 90;
                                PositionMax_S5 = servo.PositionMax;

                                break;

                            case 6:
                                Type_S6 = servo.Type;
                                // Get the device position min/max before any changes are made
                                DevicePositionMin_S6 = servo.PositionMin;
                                DevicePositionMax_S6 = servo.PositionMax;

                                Stopped_S6 = servo.Stopped;
                                Engaged_S6 = servo.Engaged;
                                SpeedRamping_S6 = servo.SpeedRamping;

                                Current_S6 = servo.Current;

                                AccelerationMin_S6 = servo.AccelerationMin;
                                //Acceleration_S6 = servo.Acceleration;// = servo.AccelerationMin;// = initialAcceleration;
                                AccelerationMax_S6 = servo.AccelerationMax;

                                VelocityMin_S6 = servo.VelocityMin;
                                Velocity_S6 = servo.Velocity;
                                VelocityLimit_S6 = servo.VelocityLimit = servo.VelocityMin;
                                VelocityMax_S6 = servo.VelocityMax;

                                PositionMin_S6 = servo.PositionMin;
                                //Position_S6 = 90;
                                PositionMax_S6 = servo.PositionMax;

                                break;

                            case 7:
                                Type_S7 = servo.Type;
                                // Get the device position min/max before any changes are made
                                DevicePositionMin_S7 = servo.PositionMin;
                                DevicePositionMax_S7 = servo.PositionMax;

                                Stopped_S7 = servo.Stopped;
                                Engaged_S7 = servo.Engaged;
                                SpeedRamping_S7 = servo.SpeedRamping;

                                Current_S7 = servo.Current;

                                AccelerationMin_S7 = servo.AccelerationMin;
                                //Acceleration_S7 = servo.Acceleration;// = servo.AccelerationMin;// = initialAcceleration;
                                AccelerationMax_S7 = servo.AccelerationMax;

                                VelocityMin_S7 = servo.VelocityMin;
                                Velocity_S7 = servo.Velocity;
                                VelocityLimit_S7 = servo.VelocityLimit = servo.VelocityMin;
                                VelocityMax_S7 = servo.VelocityMax;

                                PositionMin_S7 = servo.PositionMin;
                                //Position_S7 = 90;
                                PositionMax_S7 = servo.PositionMax;

                                break;

                            default:
                                Log.Trace($"UpdateAdvancedServoProperties count:{servos.Count}", Common.LOG_CATEGORY);
                                break;

                        }
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
                        // Device is Engaged

                        switch (i)
                        {
                            case 0:
                                Type_S0 = servo.Type;

                                //DevicePositionMin_S0 = servo.PositionMin;
                                //DevicePositionMax_S0 = servo.PositionMax;

                                Stopped_S0 = servo.Stopped;
                                Engaged_S0 = servo.Engaged;
                                SpeedRamping_S0 = servo.SpeedRamping;

                                Current_S0 = servo.Current;

                                AccelerationMin_S0 = servo.AccelerationMin;
                                Acceleration_S0 = servo.Acceleration;
                                AccelerationMax_S0 = servo.AccelerationMax;

                                VelocityMin_S0 = servo.VelocityMin;
                                Velocity_S0 = servo.Velocity;
                                VelocityLimit_S0 = servo.VelocityLimit;
                                VelocityMax_S0 = servo.VelocityMax;

                                PositionMin_S0 = servo.PositionMin;
                                Position_S0 = servo.Position;
                                PositionMax_S0 = servo.PositionMax;

                                break;

                            case 1:
                                Type_S1 = servo.Type;

                                //DevicePositionMin_S1 = servo.PositionMin;
                                //DevicePositionMax_S1 = servo.PositionMax;

                                Stopped_S1 = servo.Stopped;
                                Engaged_S1 = servo.Engaged;
                                SpeedRamping_S1 = servo.SpeedRamping;

                                Current_S1 = servo.Current;

                                AccelerationMin_S1 = servo.AccelerationMin;
                                Acceleration_S1 = servo.Acceleration;
                                AccelerationMax_S1 = servo.AccelerationMax;

                                VelocityMin_S1 = servo.VelocityMin;
                                Velocity_S1 = servo.Velocity;
                                VelocityLimit_S1 = servo.VelocityLimit;
                                VelocityMax_S1 = servo.VelocityMax;

                                PositionMin_S1 = servo.PositionMin;
                                Position_S1 = servo.Position;
                                PositionMax_S1 = servo.PositionMax;

                                break;

                            case 2:
                                Type_S2 = servo.Type;

                                //DevicePositionMin_S2 = servo.PositionMin;
                                //DevicePositionMax_S2 = servo.PositionMax;

                                Stopped_S2 = servo.Stopped;
                                Engaged_S2 = servo.Engaged;
                                SpeedRamping_S2 = servo.SpeedRamping;

                                Current_S2 = servo.Current;

                                AccelerationMin_S2 = servo.AccelerationMin;
                                Acceleration_S2 = servo.Acceleration;
                                AccelerationMax_S2 = servo.AccelerationMax;

                                VelocityMin_S2 = servo.VelocityMin;
                                Velocity_S2 = servo.Velocity;
                                VelocityLimit_S2 = servo.VelocityLimit;
                                VelocityMax_S2 = servo.VelocityMax;

                                PositionMin_S2 = servo.PositionMin;
                                Position_S2 = servo.Position;
                                PositionMax_S2 = servo.PositionMax;

                                break;

                            case 3:
                                Type_S3 = servo.Type;

                                //DevicePositionMin_S3 = servo.PositionMin;
                                //DevicePositionMax_S3 = servo.PositionMax;

                                Stopped_S3 = servo.Stopped;
                                Engaged_S3 = servo.Engaged;
                                SpeedRamping_S3 = servo.SpeedRamping;

                                Current_S3 = servo.Current;

                                AccelerationMin_S3 = servo.AccelerationMin;
                                Acceleration_S3 = servo.Acceleration;
                                AccelerationMax_S3 = servo.AccelerationMax;

                                VelocityMin_S3 = servo.VelocityMin;
                                Velocity_S3 = servo.Velocity;
                                VelocityLimit_S3 = servo.VelocityLimit;
                                VelocityMax_S3 = servo.VelocityMax;

                                PositionMin_S3 = servo.PositionMin;
                                Position_S3 = servo.Position;
                                PositionMax_S3 = servo.PositionMax;

                                break;

                            case 4:
                                Type_S4 = servo.Type;

                                DevicePositionMin_S4 = servo.PositionMin;
                                DevicePositionMax_S4 = servo.PositionMax;

                                Stopped_S4 = servo.Stopped;
                                Engaged_S4 = servo.Engaged;
                                SpeedRamping_S4 = servo.SpeedRamping;

                                Current_S4 = servo.Current;

                                AccelerationMin_S4 = servo.AccelerationMin;
                                Acceleration_S4 = servo.Acceleration;
                                AccelerationMax_S4 = servo.AccelerationMax;

                                VelocityMin_S4 = servo.VelocityMin;
                                Velocity_S4 = servo.Velocity;
                                VelocityLimit_S4 = servo.VelocityLimit;
                                VelocityMax_S4 = servo.VelocityMax;

                                PositionMin_S4 = servo.PositionMin;
                                Position_S4 = servo.Position;
                                PositionMax_S4 = servo.PositionMax;

                                break;

                            case 5:
                                Type_S5 = servo.Type;

                                DevicePositionMin_S5 = servo.PositionMin;
                                DevicePositionMax_S5 = servo.PositionMax;

                                Stopped_S5 = servo.Stopped;
                                Engaged_S5 = servo.Engaged;
                                SpeedRamping_S5 = servo.SpeedRamping;

                                Current_S5 = servo.Current;

                                AccelerationMin_S5 = servo.AccelerationMin;
                                Acceleration_S5 = servo.Acceleration;
                                AccelerationMax_S5 = servo.AccelerationMax;

                                VelocityMin_S5 = servo.VelocityMin;
                                Velocity_S5 = servo.Velocity;
                                VelocityLimit_S5 = servo.VelocityLimit;
                                VelocityMax_S5 = servo.VelocityMax;

                                PositionMin_S5 = servo.PositionMin;
                                Position_S5 = servo.Position;
                                PositionMax_S5 = servo.PositionMax;

                                break;

                            case 6:
                                Type_S6 = servo.Type;

                                DevicePositionMin_S6 = servo.PositionMin;
                                DevicePositionMax_S6 = servo.PositionMax;

                                Stopped_S6 = servo.Stopped;
                                Engaged_S6 = servo.Engaged;
                                SpeedRamping_S6 = servo.SpeedRamping;

                                Current_S6 = servo.Current;

                                AccelerationMin_S6 = servo.AccelerationMin;
                                Acceleration_S6 = servo.Acceleration;
                                AccelerationMax_S6 = servo.AccelerationMax;

                                VelocityMin_S6 = servo.VelocityMin;
                                Velocity_S6 = servo.Velocity;
                                VelocityLimit_S6 = servo.VelocityLimit;
                                VelocityMax_S6 = servo.VelocityMax;

                                PositionMin_S6 = servo.PositionMin;
                                Position_S6 = servo.Position;
                                PositionMax_S6 = servo.PositionMax;

                                break;

                            case 7:
                                Type_S7 = servo.Type;

                                DevicePositionMin_S7 = servo.PositionMin;
                                DevicePositionMax_S7 = servo.PositionMax;

                                Stopped_S7 = servo.Stopped;
                                Engaged_S7 = servo.Engaged;
                                SpeedRamping_S7 = servo.SpeedRamping;

                                Current_S7 = servo.Current;

                                AccelerationMin_S7 = servo.AccelerationMin;
                                Acceleration_S7 = servo.Acceleration;
                                AccelerationMax_S7 = servo.AccelerationMax;

                                VelocityMin_S7 = servo.VelocityMin;
                                Velocity_S7 = servo.Velocity;
                                VelocityLimit_S7 = servo.VelocityLimit;
                                VelocityMax_S7 = servo.VelocityMax;

                                PositionMin_S7 = servo.PositionMin;
                                Position_S7 = servo.Position;
                                PositionMax_S7 = servo.PositionMax;

                                break;

                            default:
                                Log.Trace($"UpdateAdvancedServoProperties count:{servos.Count}", Common.LOG_CATEGORY);
                                break;

                        }
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

        private void InitializAdvancedServoUI()
        {
            Stopped_S0 = Stopped_S1 = Stopped_S2 = Stopped_S3 = null;
            Stopped_S4 = Stopped_S5 = Stopped_S6 = Stopped_S7 = null;
            Engaged_S0 = Engaged_S1 = Engaged_S2 = Engaged_S3 = null;
            Engaged_S4 = Engaged_S5 = Engaged_S6 = Engaged_S7 = null;
            SpeedRamping_S0 = SpeedRamping_S1 = SpeedRamping_S2 = SpeedRamping_S3 = null;
            SpeedRamping_S4 = SpeedRamping_S5 = SpeedRamping_S6 = SpeedRamping_S7 = null;
            Current_S0 = Current_S1 = Current_S2 = Current_S3 = null;
            Current_S4 = Current_S5 = Current_S6 = Current_S7 = null;

            // NOTE(crhodes)
            // Have to clear Acceleration before Min/Max as UI triggers an update
            Acceleration_S0 = Acceleration_S1 = Acceleration_S2 = Acceleration_S3 = null;
            Acceleration_S4 = Acceleration_S5 = Acceleration_S6 = Acceleration_S7 = null;

            AccelerationMin_S0 = AccelerationMin_S1 = AccelerationMin_S2 = AccelerationMin_S3 = null;
            AccelerationMin_S4 = AccelerationMin_S5 = AccelerationMin_S6 = AccelerationMin_S7 = null;
            AccelerationMax_S0 = AccelerationMax_S1 = AccelerationMax_S2 = AccelerationMax_S3 = null;
            AccelerationMax_S4 = AccelerationMax_S5 = AccelerationMax_S6 = AccelerationMax_S7 = null;

            // NOTE(crhodes)
            // Handle VelocityLimit same way as Acceleration
            // Have not confirmed this is an issue
            VelocityLimit_S0 = VelocityLimit_S1 = VelocityLimit_S2 = VelocityLimit_S3 = null;
            VelocityLimit_S4 = VelocityLimit_S5 = VelocityLimit_S6 = VelocityLimit_S7 = null;

            VelocityMin_S0 = VelocityMin_S1 = VelocityMin_S2 = VelocityMin_S3 = null;
            VelocityMin_S4 = VelocityMin_S5 = VelocityMin_S6 = VelocityMin_S7 = null;
            Velocity_S0 = Velocity_S1 = Velocity_S2 = Velocity_S3 = null;
            Velocity_S4 = Velocity_S5 = Velocity_S6 = Velocity_S7 = null;
            VelocityMax_S0 = VelocityMax_S1 = VelocityMax_S2 = VelocityMax_S3 = null;
            VelocityMax_S4 = VelocityMax_S5 = VelocityMax_S6 = VelocityMax_S7 = null;

            DevicePositionMin_S0 = DevicePositionMin_S1 = DevicePositionMin_S2 = DevicePositionMin_S3 = null;
            DevicePositionMin_S4 = DevicePositionMin_S5 = DevicePositionMin_S6 = DevicePositionMin_S7 = null;
            PositionMin_S0 = PositionMin_S1 = PositionMin_S2 = PositionMin_S3 = null;
            PositionMin_S4 = PositionMin_S5 = PositionMin_S6 = PositionMin_S7 = null;
            Position_S0 = Position_S1 = Position_S2 = Position_S3 = null;
            Position_S4 = Position_S5 = Position_S6 = Position_S7 = null;
            PositionMax_S0 = PositionMax_S1 = PositionMax_S2 = PositionMax_S3 = null;
            PositionMax_S4 = PositionMax_S5 = PositionMax_S6 = PositionMax_S7 = null;
            DevicePositionMax_S0 = DevicePositionMax_S1 = DevicePositionMax_S2 = DevicePositionMax_S3 = null;
            DevicePositionMax_S4 = DevicePositionMax_S5 = DevicePositionMax_S6 = DevicePositionMax_S7 = null;
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
