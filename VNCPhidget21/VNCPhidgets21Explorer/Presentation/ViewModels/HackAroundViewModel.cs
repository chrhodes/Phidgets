using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Phidgets;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;
using VNC.Phidget.Events;

using VNCPhidgetConfig = VNCPhidget21.Configuration;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class HackAroundViewModel 
        : EventViewModelBase, IMainViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public HackAroundViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //DigitalOutput digitalOutput0;
        //ph22.DigitalOutput digitalOutput2;

        const Int32 sbc11SerialNumber = 46049;

        const Int32 sbc21SerialNumber = 48301;
        const Int32 sbc22SerialNumber = 251831;
        const Int32 sbc23SerialNumber = 48284;

        private IEnumerable<string> GetListOfPerformanceConfigFiles()
        {
            // TODO(crhodes)
            // Read a directory and return files, perhaps with RegEx name match

            List<string> files = new List<string>
            {
                @"Performances\PerformanceConfig_1.json",  
                @"Performances\PerformanceConfig_2.json",
                @"Performances\PerformanceConfig_3.json",

                @"Performances\PerformanceConfig_Skulls.json",
            };

            return files;
        }

        private IEnumerable<string> GetListOfAdvancedServoConfigFiles()
        {
            // TODO(crhodes)
            // Read a directory and return files, perhaps with RegEx name match

            List<string> files = new List<string>
            { 
                @"localhost\localhost_AdvancedServoSequenceConfig_1.json", 
                @"psbc11\psbc11_AdvancedServoSequenceConfig_1.json",
                @"psbc21\psbc21_AdvancedServoSequenceConfig_1.json",
                @"psbc22\psbc22_AdvancedServoSequenceConfig_1.json",
                @"psbc23\psbc23_AdvancedServoSequenceConfig_1.json",

                @"localhost\localhost_AdvancedServoSequence_Skulls.json",
                @"psbc11\psbc11_AdvancedServoSequence_Skulls.json",
                @"psbc21\psbc21_AdvancedServoSequence_Skulls.json",
                @"psbc22\psbc22_AdvancedServoSequence_Skulls.json",
                @"psbc23\psbc23_AdvancedServoSequence_Skulls.json"
            };

            return files;
        }

        private IEnumerable<string> GetListOfInterfaceKitConfigFiles()
        {
            // TODO(crhodes)
            // Read a directory and return files, perhaps with RegEx name match

            List<string> files = new List<string>
            { 
                @"localhost\localhost_InterfaceKitSequenceConfig_1.json", 
                @"psbc11\psbc11_InterfaceKitSequenceConfig_1.json",
                @"psbc21\psbc21_InterfaceKitSequenceConfig_1.json",
                @"psbc22\psbc22_InterfaceKitSequenceConfig_1.json",
                @"psbc23\psbc23_InterfaceKitSequenceConfig_1.json"
            };

            return files;
        }

        private IEnumerable<string> GetListOfStepperConfigFiles()
        {
            // TODO(crhodes)
            // Read a directory and return files, perhaps with RegEx name match

            List<string> files = new List<string>
            { 
                @"localhost\localhost_StepperSequenceConfig_1.json", 
                @"psbc11\psbc11_StepperSequenceConfig_1.json",
                @"psbc21\psbc21_StepperSequenceConfig_1.json",
                @"psbc22\psbc22_StepperSequenceConfig_1.json",
                @"psbc23\psbc23_StepperSequenceConfig_1.json" 
            };

            return files;
        }


        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            Button1Command = new DelegateCommand(Button1Execute);
            Button2Command = new DelegateCommand(Button2Execute);
            Button3Command = new DelegateCommand(Button3Execute);
            Button4Command = new DelegateCommand(Button4Execute);
            Button5Command = new DelegateCommand(Button5Execute);


            ReloadPerformanceConfigFilesCommand = new DelegateCommand(ReloadPerformanceConfigFiles);
            ReloadAdvancedServoSequenceConfigFilesCommand = new DelegateCommand(ReloadAdvancedServoSequenceConfigFiles);
            ReloadInterfaceKitSequenceConfigFilesCommand = new DelegateCommand(ReloadInterfaceKitSequenceConfigFiles);
            ReloadStepperSequenceConfigFilesCommand = new DelegateCommand(ReloadStepperSequenceConfigFiles);

            PlayPerformanceCommand = new DelegateCommand (PlayPerformance, PlayPerformanceCanExecute);
            PlayAdvancedServoSequenceCommand = new DelegateCommand(PlayAdvancedServoSequence, PlayAdvancedServoSequenceCanExecute);
            EngageAndCenterCommand = new DelegateCommand(EngageAndCenter, EngageAndCenterCanExecute);
            ResetLimitsCommand = new DelegateCommand(ResetLimits);
            SetMotionParametersCommand = new DelegateCommand<string>(SetMotionParameters);
            RelativeAccelerationCommand = new DelegateCommand<Int32?>(RelativeAcceleration);
            RelativeVelocityLimitCommand = new DelegateCommand<Int32?>(RelativeVelocityLimit);

            PlayInterfaceKitSequenceCommand = new DelegateCommand(PlayInterfaceKitSequence, PlayInterfaceKitSequenceCanExecute);

            HostConfigFileName = "hostconfig.json";

            PerformanceConfigFiles = GetListOfPerformanceConfigFiles();
            AdvancedServoSequenceConfigFiles = GetListOfAdvancedServoConfigFiles();
            InterfaceKitSequenceConfigFiles = GetListOfInterfaceKitConfigFiles();
            StepperSequenceConfigFiles = GetListOfStepperConfigFiles();

            //PerformanceConfigFileName = "performanceconfig.json";

            //AdvancedServoConfigFileName = "advancedservosequenceconfig.json";
            //InterfaceKitConfigFileName = "interfacekitsequenceconfig.json";
            //StepperConfigFileName = "steppersequenceconfig.json";

            LoadHostConfig();
            BuildPhidgetDeviceDictionary();
            //LoadPerformancesConfig();
            //LoadAdvanceServoConfig();
            //LoadInterfaceKitConfig();
            //LoadStepperConfig();

            // Turn on logging of PropertyChanged from VNC.Core
            LogOnPropertyChanged = false;

            Message = "HackAroundViewModel says hello";

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }


        private void BuildPhidgetDeviceDictionary()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            foreach (VNCPhidgetConfig.Host host in Hosts)
            {
                if (host.AdvancedServos is not null)
                {
                    foreach (VNCPhidgetConfig.AdvancedServo advancedServo in host.AdvancedServos)
                    {
                        AvailablePhidgets.Add(
                            new SerialHost { IPAddress = host.IPAddress, SerialNumber = advancedServo.SerialNumber},
                            new PhidgetDevice(
                                host.IPAddress, host.Port,
                                Phidget.PhidgetClass.ADVANCEDSERVO, advancedServo.SerialNumber));
                    }
                }

                if (host.InterfaceKits is not null)
                {
                    foreach (VNCPhidgetConfig.InterfaceKit interfaceKit in host.InterfaceKits)
                    {
                        AvailablePhidgets.Add(
                            new SerialHost { IPAddress = host.IPAddress, SerialNumber = interfaceKit.SerialNumber },
                            new PhidgetDevice(
                                host.IPAddress, host.Port,
                                Phidget.PhidgetClass.INTERFACEKIT, interfaceKit.SerialNumber));
                    }
                }

                if (host.Steppers is not null)
                {
                    foreach (VNCPhidgetConfig.Stepper stepper in host.Steppers)
                    {
                        AvailablePhidgets.Add(
                            new SerialHost { IPAddress = host.IPAddress, SerialNumber = stepper.SerialNumber },
                            new PhidgetDevice(
                                host.IPAddress, host.Port,
                                Phidget.PhidgetClass.STEPPER, stepper.SerialNumber));
                    }
                }
            }

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        Dictionary<SerialHost, PhidgetDevice> AvailablePhidgets = new Dictionary<SerialHost, PhidgetDevice>();

        private void LoadHostConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            var jsonOptions = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };

            string jsonString = File.ReadAllText(HostConfigFileName);

            VNCPhidgetConfig.HostConfig? hostConfig
                = JsonSerializer.Deserialize<VNCPhidgetConfig.HostConfig>(jsonString, jsonOptions);

            Hosts = hostConfig.Hosts.ToList();

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadPerformancesConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(PerformanceConfigFileName);

            VNCPhidgetConfig.PerformanceConfig? performanceConfig
                = JsonSerializer.Deserialize<VNCPhidgetConfig.PerformanceConfig>
                (jsonString, GetJsonSerializerOptions());

            Performances = performanceConfig.Performances.ToList();

            AvailablePerformances =
                performanceConfig.Performances
                .ToDictionary(k => k.Name, v => v);

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadAdvanceServoConfig()
        {
            string jsonString = File.ReadAllText(AdvancedServoSequenceConfigFileName);

            VNCPhidgetConfig.AdvancedServoSequenceConfig? sequenceConfig
                = JsonSerializer.Deserialize<VNCPhidgetConfig.AdvancedServoSequenceConfig>
                (jsonString, GetJsonSerializerOptions());

            AdvancedServoSequences = sequenceConfig.AdvancedServoSequences.ToList();

            AvailableAdvancedServoSequences =
                sequenceConfig.AdvancedServoSequences
                .ToDictionary(k => k.Name, v => v);
        }

        private void LoadInterfaceKitConfig()
        {
            string jsonString = File.ReadAllText(InterfaceKitSequenceConfigFileName);

            VNCPhidgetConfig.InterfaceKitSequenceConfig? sequenceConfig
                = JsonSerializer.Deserialize<VNCPhidgetConfig.InterfaceKitSequenceConfig>
                (jsonString, GetJsonSerializerOptions());

            InterfaceKitSequences = sequenceConfig.InterfaceKitSequences.ToList();

            AvailableInterfaceKitSequences =
                sequenceConfig.InterfaceKitSequences
                .ToDictionary(k => k.Name, v => v);
        }

        private void LoadStepperConfig()
        {
            string jsonString = File.ReadAllText(StepperSequenceConfigFileName);

            VNCPhidgetConfig.StepperSequenceConfig? sequenceConfig
                = JsonSerializer.Deserialize<VNCPhidgetConfig.StepperSequenceConfig>
                (jsonString, GetJsonSerializerOptions());

            StepperSequences = sequenceConfig.StepperSequences.ToList();

            AvailableStepperSequences =
                sequenceConfig.StepperSequences
                .ToDictionary(k => k.Name, v => v);
        }

        JsonSerializerOptions GetJsonSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions
            { ReadCommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true };

            return jsonOptions;
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        //private string _title = "VNCPhidgets21Explorer - Main";

        //public string Title
        //{
        //    get => _title;
        //    set
        //    {
        //        if (_title == value)
        //            return;
        //        _title = value;
        //        OnPropertyChanged();
        //    }
        //}

        private string _message = "Initial Message";

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

        private int _repeats = 1;
        public int Repeats
        {
            get => _repeats;
            set
            {
                if (_repeats == value)
                    return;
                _repeats = value;
                OnPropertyChanged();
            }
        }

        private string _hostConfigFileName;
        public string HostConfigFileName
        {
            get => _hostConfigFileName;
            set
            {
                if (_hostConfigFileName == value) return;
                _hostConfigFileName = value;
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
                OnPropertyChanged();
            }
        }

        private bool _logPerformance = false;
        public bool LogPerformance
        {
            get => _logPerformance;
            set
            {
                if (_logPerformance == value)
                    return;
                _logPerformance = value;
                OnPropertyChanged();
            }
        }

        private bool _logPerformanceSequence = false;
        public bool LogPerformanceSequence
        {
            get => _logPerformanceSequence;
            set
            {
                if (_logPerformanceSequence == value)
                    return;
                _logPerformanceSequence = value;
                OnPropertyChanged();
            }
        }

        private bool _logPerformanceAction = false;
        public bool LogPerformanceAction
        {
            get => _logPerformanceAction;
            set
            {
                if (_logPerformanceAction == value)
                    return;
                _logPerformanceAction = value;
                OnPropertyChanged();
            }
        }

        private bool _logActionVerification = false;
        public bool LogActionVerification
        {
            get => _logActionVerification;
            set
            {
                if (_logActionVerification == value)
                    return;
                _logActionVerification = value;
                OnPropertyChanged();
            }
        }
        #region Performances

        private IEnumerable<string> _performanceConfigFiles;
        public IEnumerable<string> PerformanceConfigFiles
        {
            get => _performanceConfigFiles;
            set
            {
                _performanceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string _performanceConfigFileName;
        public string PerformanceConfigFileName
        {
            get => _performanceConfigFileName;
            set
            {
                if (_performanceConfigFileName == value) return;
                _performanceConfigFileName = value;
                OnPropertyChanged();

                LoadPerformancesConfig();
            }
        }

        public string PerformanceFileNameToolTip { get; set; } = "DoubleClick to select new file";

        //private VNCPhidgetConfig.PerformanceConfig _performanceConfig;
        //public VNCPhidgetConfig.PerformanceConfig PerformanceConfig
        //{
        //    get => _performanceConfig;
        //    set
        //    {
        //        if (_performanceConfig == value)
        //            return;
        //        _performanceConfig = value;
        //        OnPropertyChanged();
        //    }
        //}

        private IEnumerable<VNCPhidgetConfig.Performance> _performances;
        public IEnumerable<VNCPhidgetConfig.Performance> Performances
        {
            get => _performances;
            set
            {
                _performances = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.Performance? _selectedPerformance;
        public VNCPhidgetConfig.Performance? SelectedPerformance
        {
            get => _selectedPerformance;
            set
            {
                if (_selectedPerformance == value)
                {
                    return;
                }

                _selectedPerformance = value;
                OnPropertyChanged();

                PlayPerformanceCommand.RaiseCanExecuteChanged();
                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
                EngageAndCenterCommand.RaiseCanExecuteChanged();
                PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private Dictionary<string, VNCPhidgetConfig.Performance> _availablePerformances;
        public Dictionary<string, VNCPhidgetConfig.Performance> AvailablePerformances
        {
            get => _availablePerformances;
            set
            {
                _availablePerformances = value;
                OnPropertyChanged();
            }
        }

        private List<VNCPhidgetConfig.Performance> _selectedPerformances;
        public List<VNCPhidgetConfig.Performance> SelectedPerformances
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

                //PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
                //EngageAndCenterCommand.RaiseCanExecuteChanged();
                //PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private PerformanceSequencePlayer ActivePerformanceSequencePlayer { get; set; }

        #endregion

        #region AdvancedServo

        //public AdvancedServoEx ActiveAdvancedServoHost { get; set; }

        private bool _logCurrentChangeEvents = false;
        public bool LogCurrentChangeEvents
        {
            get => _logCurrentChangeEvents;
            set
            {
                if (_logCurrentChangeEvents == value)
                    return;
                _logCurrentChangeEvents = value;
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

        public List<Int32> RelativeAccelerationAdjustment { get; } = new List<Int32>
        {                
            -1000,
            -500,
            -100,
            -50,
            50,
            100,
            500,
            1000
        };

        public List<Int32> RelativeVelocityLimitAdjustment { get; } = new List<Int32>
        {
            -500,
            -100,
            -50,
            -10,
            10,
            50,
            100,
            500
        };

        private IEnumerable<string> _advancedServoSequenceConfigFiles;
        public IEnumerable<string> AdvancedServoSequenceConfigFiles
        {
            get => _advancedServoSequenceConfigFiles;
            set
            {
                _advancedServoSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string _advancedServoSequenceConfigFileName;
        public string AdvancedServoSequenceConfigFileName
        {
            get => _advancedServoSequenceConfigFileName;
            set
            {
                if (_advancedServoSequenceConfigFileName == value) return;
                _advancedServoSequenceConfigFileName = value;
                OnPropertyChanged();

                LoadAdvanceServoConfig();

                EngageAndCenterCommand.RaiseCanExecuteChanged();
            }
        }

        private IEnumerable<VNCPhidgetConfig.AdvancedServoSequence> _advancedServoSequences;
        public IEnumerable<VNCPhidgetConfig.AdvancedServoSequence> AdvancedServoSequences
        {
            get => _advancedServoSequences;
            set
            {
                _advancedServoSequences = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.AdvancedServoSequence? _selectedAdvancedServoSequence;
        public VNCPhidgetConfig.AdvancedServoSequence? SelectedAdvancedServoSequence
        {
            get => _selectedAdvancedServoSequence;
            set
            {
                if (_selectedAdvancedServoSequence == value) return;

                _selectedAdvancedServoSequence = value;
                OnPropertyChanged();

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private Dictionary<string, VNCPhidgetConfig.AdvancedServoSequence> _availableAdvancedServoSequences;
        public Dictionary<string, VNCPhidgetConfig.AdvancedServoSequence> AvailableAdvancedServoSequences
        {
            get => _availableAdvancedServoSequences;
            set
            {
                _availableAdvancedServoSequences = value;
                OnPropertyChanged();
            }
        }

        private List<VNCPhidgetConfig.AdvancedServoSequence> _selectedAdvancedServoSequences;
        public List<VNCPhidgetConfig.AdvancedServoSequence> SelectedAdvancedServoSequences
        {
            get => _selectedAdvancedServoSequences;
            set
            {
                if (_selectedAdvancedServoSequences == value)
                {
                    return;
                }

                _selectedAdvancedServoSequences = value;
                OnPropertyChanged();

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region InterfaceKit

        private bool _displayInputChangeEvents = false;

        public bool LogInputChangeEvents
        {
            get => _displayInputChangeEvents;
            set
            {
                if (_displayInputChangeEvents == value)
                    return;
                _displayInputChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private bool _displayOutputChangeEvents = false;

        public bool LogOutputChangeEvents
        {
            get => _displayOutputChangeEvents;
            set
            {
                if (_displayOutputChangeEvents == value)
                    return;
                _displayOutputChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private bool _sensorChangeEvents = false;

        public bool LogSensorChangeEvents
        {
            get => _sensorChangeEvents;
            set
            {
                if (_sensorChangeEvents == value)
                    return;
                _sensorChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string> _interfaceKitSequenceConfigFiles;
        public IEnumerable<string> InterfaceKitSequenceConfigFiles
        {
            get => _interfaceKitSequenceConfigFiles;
            set
            {
                _interfaceKitSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string _interfaceKitSequenceConfigFileName;
        public string InterfaceKitSequenceConfigFileName
        {
            get => _interfaceKitSequenceConfigFileName;
            set
            {
                if (_interfaceKitSequenceConfigFileName == value) return;
                _interfaceKitSequenceConfigFileName = value;
                OnPropertyChanged();

                LoadInterfaceKitConfig();
            }
        }

        private IEnumerable<VNCPhidgetConfig.InterfaceKitSequence> _interfaceKitSequences;
        public IEnumerable<VNCPhidgetConfig.InterfaceKitSequence> InterfaceKitSequences
        {
            get => _interfaceKitSequences;
            set
            {
                _interfaceKitSequences = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.InterfaceKitSequence? _selectedInterfaceKitSequence;
        public VNCPhidgetConfig.InterfaceKitSequence? SelectedInterfaceKitSequence
        {
            get => _selectedInterfaceKitSequence;
            set
            {
                if (_selectedInterfaceKitSequence == value) return;

                _selectedInterfaceKitSequence = value;
                OnPropertyChanged();

                PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private Dictionary<string, VNCPhidgetConfig.InterfaceKitSequence> _availableInterfaceKitSequences;
        public Dictionary<string, VNCPhidgetConfig.InterfaceKitSequence> AvailableInterfaceKitSequences
        {
            get => _availableInterfaceKitSequences;
            set
            {
                _availableInterfaceKitSequences = value;
                OnPropertyChanged();
            }
        }

        private List<VNCPhidgetConfig.InterfaceKitSequence> _selectedInterfaceKitSequences;
        public List<VNCPhidgetConfig.InterfaceKitSequence> SelectedInterfaceKitSequences
        {
            get => _selectedInterfaceKitSequences;
            set
            {
                if (_selectedInterfaceKitSequences == value)
                {
                    return;
                }

                _selectedInterfaceKitSequences = value;
                OnPropertyChanged();

                PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Stepper

        private IEnumerable<string> _stepperSequenceConfigFiles;
        public IEnumerable<string> StepperSequenceConfigFiles
        {
            get => _stepperSequenceConfigFiles;
            set
            {
                _stepperSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string _stepperSequenceConfigFileName;
        public string StepperSequenceConfigFileName
        {
            get => _stepperSequenceConfigFileName;
            set
            {
                if (_stepperSequenceConfigFileName == value) return;
                _stepperSequenceConfigFileName = value;
                OnPropertyChanged();

                LoadStepperConfig();
            }
        }

        private IEnumerable<VNCPhidgetConfig.StepperSequence> _stepperSequences;
        public IEnumerable<VNCPhidgetConfig.StepperSequence> StepperSequences
        {
            get => _stepperSequences;
            set
            {
                _stepperSequences = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.StepperSequence? _selectedStepperSequence;
        public VNCPhidgetConfig.StepperSequence? SelectedStepperSequence
        {
            get => _selectedStepperSequence;
            set
            {
                if (_selectedStepperSequence == value) return;

                _selectedStepperSequence = value;
                OnPropertyChanged();

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private Dictionary<string, VNCPhidgetConfig.StepperSequence> _availableStepperSequences;
        public Dictionary<string, VNCPhidgetConfig.StepperSequence> AvailableStepperSequences
        {
            get => _availableStepperSequences;
            set
            {
                _availableStepperSequences = value;
                OnPropertyChanged();
            }
        }

        private List<VNCPhidgetConfig.StepperSequence> _selectedStepperSequences;
        public List<VNCPhidgetConfig.StepperSequence> SelectedStepperSequences
        {
            get => _selectedStepperSequences;
            set
            {
                if (_selectedStepperSequences == value)
                {
                    return;
                }

                _selectedStepperSequences = value;
                OnPropertyChanged();

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand Button1Command { get; private set; }
        public ICommand Button2Command { get; private set; }
        public ICommand Button3Command { get; private set; }
        public ICommand Button4Command { get; private set; }
        public ICommand Button5Command { get; private set; }

        private void Button1Execute()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "Button1 Clicked";

            PlayParty();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private async void Button2Execute()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "Button2 Clicked - Opening PhidgetManager";

            Phidgets.Manager phidgetManager = new Phidgets.Manager();

            phidgetManager.Attach += PhidgetManager_Attach;
            phidgetManager.Detach += PhidgetManager_Detach;
            phidgetManager.ServerConnect += PhidgetManager_ServerConnect;
            phidgetManager.ServerDisconnect += PhidgetManager_ServerDisconnect;
            phidgetManager.Error += PhidgetManager_Error;

            phidgetManager.open();
            //phidgetManager.open("192.168.150.21", 5001);

            phidgetManager.Attach -= PhidgetManager_Attach;
            phidgetManager.Detach -= PhidgetManager_Detach;
            phidgetManager.ServerConnect -= PhidgetManager_ServerConnect;
            phidgetManager.ServerDisconnect -= PhidgetManager_ServerDisconnect;
            phidgetManager.Error -= PhidgetManager_Error;

            phidgetManager.close();

            //InterfaceKitEx ifkEx21 = new InterfaceKitEx("192.168.150.21", 5001, sbc21SerialNumber, embedded: true, EventAggregator);

            //ifkEx21.Open();

            ////ifk.Attach += Ifk_Attach;
            ////ifk.Detach += Ifk_Detach;
            ////ifk.Error += Ifk_Error;
            ////ifk.InputChange += Ifk_InputChange;

            //ifkEx21.InterfaceKit.OutputChange += Ifk_OutputChange;
            ////ifkEx.OutputChange += Ifk_OutputChange;

            ////ifk.SensorChange += Ifk_SensorChange;
            ////ifk.ServerConnect += Ifk_ServerConnect;
            ////ifk.ServerDisconnect += Ifk_ServerDisconnect;

            ////ifk.open(serialNumber, hostName, port);
            ////ifk.waitForAttachment();

            //InterfaceKitDigitalOutputCollection ifkdoc = ifkEx21.InterfaceKit.outputs;
            ////InterfaceKitDigitalOutputCollection ifkdoc = ifkEx.outputs;

            //await Task.Run(() =>
            //{
            //    Parallel.Invoke(() =>
            //    {
            //        for (int i = 0; i < 5; i++)
            //        {
            //            ifkdoc[0] = true;
            //            Thread.Sleep(500);
            //            ifkdoc[0] = false;
            //            Thread.Sleep(500);
            //        }
            //    });
            //});

            //ifkEx21.Close();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private void Button3Execute()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "Button3 Clicked - Loading PhidgetDevices";


            //InterfaceKitEx ifkEx21 = new InterfaceKitEx("192.168.150.21", 5001, sbc21SerialNumber, embedded: true, EventAggregator);

            //ifkEx21.Open();

            ////ifkEx21.InterfaceKit.OutputChange += Ifk_OutputChange;

            ////InterfaceKitDigitalOutputCollection ifkdoc = ifkEx21.InterfaceKit.outputs;
            ////InterfaceKitDigitalOutputCollection ifkdoc = ifkEx.outputs;

            //Task.Run(() =>
            //{
            //    InterfaceKitDigitalOutputCollection ifkdoc = ifkEx21.InterfaceKit.outputs;

            //    for (int i = 0; i < 5; i++)
            //    {
            //        ifkdoc[0] = true;
            //        Thread.Sleep(500);
            //        ifkdoc[0] = false;
            //        Thread.Sleep(500);
            //    }
            //    //Parallel.Invoke(
            //    //    () => InterfaceKitParty2(ifkEx21, 500, 5 * Repeats)
            //    //);
            //});

            //ifkEx21.Close();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private void Button4Execute()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "Button4 Clicked";

            SequenceEventArgs sequenceEventArgs = new SequenceEventArgs();

            sequenceEventArgs.AdvancedServoSequence = new VNCPhidgetConfig.AdvancedServoSequence
            {
                Host = new VNCPhidgetConfig.Host
                {
                    Name = "psbc21",
                    IPAddress = "192.168.150.21",
                    Port = 5001,
                    AdvancedServos = new[]
                    {
                        new VNCPhidgetConfig.AdvancedServo { Name = "AdvancedServo 1", SerialNumber = 99415, Open = true }
                    }
                },
                Name = "psbc21_SequenceServo0",

                Actions = new[]
                {
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                }
            };

            EventAggregator.GetEvent<VNC.Phidget.Events.AdvancedServoSequenceEvent>().Publish(sequenceEventArgs);

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }


        #region Reload Config Files

        public ICommand ReloadPerformanceConfigFilesCommand { get; private set; }
        public ICommand ReloadAdvancedServoSequenceConfigFilesCommand { get; private set; }
        public ICommand ReloadInterfaceKitSequenceConfigFilesCommand { get; private set; }
        public ICommand ReloadStepperSequenceConfigFilesCommand { get; private set; }

        private void ReloadPerformanceConfigFiles()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "ReloadPerformanceConfigFiles Clicked";

            LoadPerformancesConfig();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadAdvancedServoSequenceConfigFiles()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "ReloadAdvancedServoSequenceConfigFiles Clicked";

            LoadAdvanceServoConfig();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadInterfaceKitSequenceConfigFiles()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "ReloadInterfaceKitSequenceConfigFiles Clicked";

            LoadInterfaceKitConfig();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadStepperSequenceConfigFiles()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "ReloadStepperSequenceConfigFiles Clicked";

            LoadStepperConfig();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region PerformanceFileName DoubleClick

        public DelegateCommand PerformanceFileName_DoubleClick_Command { get; set; }

        //private void PerformanceFileName_DoubleClick()
        //{
        //    Message = "PerformanceFileName_DoubleClick";

        //    LoadPerformancesConfig();
        //}

        #endregion

        #region PlayPerformance Command

        public DelegateCommand PlayPerformanceCommand { get; set; }
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

        public async void PlayPerformance()
        {
            //Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlayPerformance";

            //PerformancePlayer performancePlayer = new PerformancePlayer();
            PerformancePlayer performancePlayer = GetPerformancePlayer();

            foreach (VNCPhidgetConfig.Performance performance in SelectedPerformances)
            {
                if (LogPerformance)
                {
                    Log.Trace($"Playing performance:{performance.Name} description:{performance.Description}" +
                        $" loops:{performance.Loops} playSequencesInParallel:{performance.PlaySequencesInParallel}" +
                        $" performanceSequences:{performance.PerformanceSequences?.Count()}" +
                        $" callPerformances:{performance.CallPerformances?.Count()}" +
                        $" nextPerformance:{performance.NextPerformance}", Common.LOG_CATEGORY);
                }

                VNCPhidgetConfig.Performance? nextPerformance = performance;

                // NOTE(crhodes)
                // Why would we need to check given UI brought us here.
                // Might be useful generally

                //if (AvailablePerformances.ContainsKey(nextPerformance.Name ?? ""))
                //{ 

                //}

                await performancePlayer.RunPerformanceLoops(nextPerformance);

                //foreach (VNCPhidgetConfig.Performance callPerformance in performance.CallPerformances)
                //{
                //    if (AvailablePerformances.ContainsKey(callPerformance.Name ?? ""))
                //    {
                //        nextPerformance = AvailablePerformances[callPerformance.Name];

                //        await performancePlayer.RunPerformanceLoops(nextPerformance);

                //        // TODO(crhodes)
                //        // Should we process Next Performance if exists.  Recursive implications need to be considered.
                //        // May have to detect loops.

                //        nextPerformance = nextPerformance?.NextPerformance;
                //    }
                //    else
                //    {
                //        Log.Error($"Cannot find performance:>{nextPerformance.Name}<", Common.LOG_CATEGORY);
                //        nextPerformance = null;
                //    }
                //}

                nextPerformance = nextPerformance?.NextPerformance;

                while (nextPerformance is not null)
                {
                    if (LogPerformance)
                    {
                        Log.Trace($"Playing performance:{performance.Name} description:{performance.Description}" +
                            $" loops:{performance.Loops} playSequencesInParallel:{performance.PlaySequencesInParallel}" +
                            $" performanceSequences:{performance.PerformanceSequences?.Count()}" +
                            $" callPerformances:{performance.CallPerformances?.Count()}" +
                            $" nextPerformance:{performance.NextPerformance}", Common.LOG_CATEGORY);
                    }

                    if (AvailablePerformances.ContainsKey(nextPerformance.Name ?? ""))
                    {
                        nextPerformance = AvailablePerformances[nextPerformance.Name];

                        await performancePlayer.RunPerformanceLoops(nextPerformance);

                        nextPerformance = nextPerformance?.NextPerformance;
                    }
                    else
                    {
                        Log.Error($"Cannot find performance:>{nextPerformance.Name}<", Common.LOG_CATEGORY);
                        nextPerformance = null;
                    }
                }
            }

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

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }


        //private async Task RunPerformanceLoops(VNCPhidgetConfig.Performance performance)
        //{
        //    Int64 startTicks = 0;

        //    if (LogPerformance) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);
        //    PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

        //    for (int performanceLoop = 0; performanceLoop < performance.Loops; performanceLoop++)
        //    {
        //        // NOTE(crhodes)
        //        // First execute PerformanceSequences if any

        //        if (performance.PerformanceSequences is not null)
        //        {
        //            if (performance.PlaySequencesInParallel)
        //            {
        //                if (LogPerformance) Log.Trace($"Parallel Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

        //                Parallel.ForEach(performance.PerformanceSequences, async sequence =>
        //                {
        //                    await performanceSequencePlayer.ExecutePerformanceSequence(sequence);
        //                });
        //            }
        //            else
        //            {
        //                if (LogPerformance) Log.Trace($"Sequential Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

        //                foreach (VNCPhidgetConfig.PerformanceSequence sequence in performance.PerformanceSequences)
        //                {
        //                    for (int sequenceLoop = 0; sequenceLoop < sequence.Loops; sequenceLoop++)
        //                    {
        //                        await performanceSequencePlayer.ExecutePerformanceSequence(sequence);
        //                    }
        //                }
        //            }
        //        }

        //        // NOTE(crhodes)
        //        // Then execute CallPerformances if any

        //        if (performance.CallPerformances is not null)
        //        {
        //            //foreach (VNCPhidgetConfig.Performance callPerformance in performance.CallPerformances)
        //            //{

        //            //    if (AvailablePerformances.ContainsKey(callPerformance.Name ?? ""))
        //            //    {
        //            //        nextPerformance = AvailablePerformances[callPerformance.Name];

        //            //        await RunPerformanceLoops(nextPerformance);

        //            //        // TODO(crhodes)
        //            //        // Should we process Next Performance if exists.  Recursive implications need to be considered.
        //            //        // May have to detect loops.

        //            //        nextPerformance = nextPerformance?.NextPerformance;
        //            //    }
        //            //    else
        //            //    {
        //            //        Log.Error($"Cannot find performance:>{nextPerformance.Name}<", Common.LOG_CATEGORY);
        //            //        nextPerformance = null;
        //            //    }
        //            //}
        //        }

        //        // NOTE(crhodes)
        //        // Then sleep if necessary before next loop

        //        if (performance.Duration is not null)
        //        {
        //            if (LogPerformance)
        //            {
        //                Log.Trace($"Zzzzz End of Performance Sleeping:>{performance.Duration}<", Common.LOG_CATEGORY);
        //            }
        //            Thread.Sleep((Int32)performance.Duration);
        //        }
        //    }

        //    if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        public bool PlayPerformanceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedPerformances?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region PlayAdvancedServoSequence Command

        public DelegateCommand PlayAdvancedServoSequenceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlaySequenceCommand { get; set; }
        //public TYPE PlaySequenceCommandParameter;
        public string PlayAdvancedServoSequenceContent { get; set; } = "Play Sequence";
        public string PlayAdvancedServoSequenceToolTip { get; set; } = "PlayAdvancedServoSequence ToolTip";

        // Can get fancy and use Resources
        //public string PlaySequenceContent { get; set; } = "ViewName_PlaySequenceContent";
        //public string PlaySequenceToolTip { get; set; } = "ViewName_PlaySequenceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlaySequenceContent">PlaySequence</system:String>
        //    <system:String x:Key="ViewName_PlaySequenceContentToolTip">PlaySequence ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlaySequence(TYPE value)
        public async void PlayAdvancedServoSequence()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);

            Message = "Cool, you called PlayAdvancedServoSequence";

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            foreach (VNCPhidgetConfig.AdvancedServoSequence sequence in SelectedAdvancedServoSequences)
            {
                if (LogPerformanceSequence) Log.Trace($"Playing sequence:{sequence.Name}", Common.LOG_CATEGORY);

                try
                {
                    VNCPhidgetConfig.PerformanceSequence? nextPerformanceSequence = 
                        new VNCPhidgetConfig.PerformanceSequence
                        {
                            Name = sequence.Name,
                            SequenceType = "AS",
                            Loops = sequence.Loops
                        };

                    //for (int i = 0; i < sequence.Loops; i++)
                    //{
                    //    do
                    //    {
                    // NOTE(crhodes)
                    // Run on another thread to keep UI active
                    await Task.Run(async () =>
                            {
                                if (LogPerformanceSequence) Log.Trace($"Executing sequence:{nextPerformanceSequence.Name}", Common.LOG_CATEGORY);
                                //nextPerformanceSequence = await performanceSequencePlayer.ExecutePerformanceSequenceLoops(nextPerformanceSequence);
                                await performanceSequencePlayer.ExecutePerformanceSequenceLoops(nextPerformanceSequence);
                            });
                        //} while (nextPerformanceSequence is not null);
                    //}
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

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

        //private async Task<VNCPhidgetConfig.PerformanceSequence?> ExecutePerformanceSequence(VNCPhidgetConfig.PerformanceSequence? nextPerformanceSequence)
        //{
        //    try
        //    {
        //        if (LogPerformanceSequence)
        //        {
        //            Log.Trace($"Executing performanceSequence:>{nextPerformanceSequence?.Name}< type:>{nextPerformanceSequence?.SequenceType}<" +
        //                $" loops:>{nextPerformanceSequence?.Loops}< duration:>{nextPerformanceSequence?.Duration}< closePhidget:>{nextPerformanceSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
        //        }

        //        // TODO(crhodes)
        //        // Think about Open/Close more.  Maybe config.
        //        // What happens if nextSequence.Host is null    

        //        var startingPerformanceSequence = nextPerformanceSequence;

        //        if (nextPerformanceSequence is not null)
        //        {
        //            switch (nextPerformanceSequence.SequenceType)
        //            {
        //                case "AS":
        //                    if (AvailableAdvancedServoSequences.ContainsKey(nextPerformanceSequence.Name ?? ""))
        //                    {
        //                        var advancedServoSequence = AvailableAdvancedServoSequences[nextPerformanceSequence.Name];

        //                        if (advancedServoSequence.Host is not null)
        //                        {
        //                            var advancedServoHost = OpenAdvancedServoHost(advancedServoSequence.Host);

        //                            //nextPerformanceSequence = await advancedServo.PlayAdvancedServoSequenceLoops(advancedServoSequence);
        //                            await advancedServoHost.RunSequenceLoops(advancedServoSequence);

        //                            nextPerformanceSequence = advancedServoSequence.NextSequence;

        //                            // NOTE(crhodes)
        //                            // This should handle continuations without a Host.  
        //                            // TODO(crhodes)
        //                            // Do we need to handle continuations that have a Host?  I think so.
        //                            // Play AS sequence on one Host and then a different AS sequence on a different host.
        //                            // This would be dialog back and forth across hosts.

        //                            while (nextPerformanceSequence is not null && nextPerformanceSequence.SequenceType == "AS")
        //                            {
        //                                if (LogPerformanceSequence)
        //                                {
        //                                    Log.Trace($"Executing sequence:>{nextPerformanceSequence?.Name}< type:>{nextPerformanceSequence?.SequenceType}<" +
        //                                        $" loops:>{nextPerformanceSequence?.Loops}< closePhidget:>{nextPerformanceSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
        //                                }

        //                                if (AvailableAdvancedServoSequences.ContainsKey(nextPerformanceSequence.Name ?? ""))
        //                                {
        //                                    advancedServoSequence = AvailableAdvancedServoSequences[nextPerformanceSequence.Name];

        //                                    await advancedServoHost.RunSequenceLoops(advancedServoSequence);

        //                                    nextPerformanceSequence = advancedServoSequence.NextSequence;
        //                                }
        //                                else
        //                                {
        //                                    Log.Error($"Cannot find performanceSequence:>{nextPerformanceSequence.Name}<", Common.LOG_CATEGORY);
        //                                    nextPerformanceSequence = null;
        //                                }
        //                            }

        //                            if (startingPerformanceSequence.ClosePhidget)
        //                            {
        //                                //advancedServoHost.LogCurrentChangeEvents = false;
        //                                //advancedServoHost.LogPositionChangeEvents = false;
        //                                //advancedServoHost.LogVelocityChangeEvents = false;

        //                                //advancedServoHost.LogPerformanceStep = false;

        //                                //advancedServoHost.Close();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Log.Error($"Host is null", Common.LOG_CATEGORY);
        //                            nextPerformanceSequence = null;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Log.Error($"Cannot find performanceSequence:>{nextPerformanceSequence.Name}<", Common.LOG_CATEGORY);
        //                        nextPerformanceSequence = null;
        //                    }

        //                    break;

        //                case "IK":
        //                    if (AvailableInterfaceKitSequences.ContainsKey(nextPerformanceSequence.Name ?? ""))
        //                    {
        //                        var interfaceKitSequence = AvailableInterfaceKitSequences[nextPerformanceSequence.Name];

        //                        if (interfaceKitSequence.Host is not null)
        //                        {
        //                            var interfaceKitHost = OpenInterfaceKitHost(interfaceKitSequence.Host);

        //                            await interfaceKitHost.RunSequenceLoops(interfaceKitSequence);

        //                            nextPerformanceSequence = interfaceKitSequence.NextSequence;

        //                            // NOTE(crhodes)
        //                            // This should handle continuations without a Host.  
        //                            // TODO(crhodes)
        //                            // Do we need to handle continuations that have a Host?  I think so.
        //                            // Play AS sequence on one Host and then a different AS sequence on a different host.
        //                            // This would be dialog back and forth across hosts.

        //                            while (nextPerformanceSequence is not null && nextPerformanceSequence.SequenceType == "AS")
        //                            {
        //                                if (LogPerformanceSequence)
        //                                {
        //                                    Log.Trace($"Executing sequence:>{nextPerformanceSequence?.Name}< type:>{nextPerformanceSequence?.SequenceType}<" +
        //                                        $" loops:>{nextPerformanceSequence?.Loops}< closePhidget:>{nextPerformanceSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
        //                                }

        //                                interfaceKitSequence = AvailableInterfaceKitSequences[nextPerformanceSequence.Name];

        //                                await interfaceKitHost.RunSequenceLoops(interfaceKitSequence);

        //                                nextPerformanceSequence = interfaceKitSequence.NextSequence;
        //                            }

        //                            if (startingPerformanceSequence.ClosePhidget)
        //                            {
        //                                interfaceKitHost.Close();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Log.Trace($"Host is null", Common.LOG_CATEGORY);
        //                            nextPerformanceSequence = null;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Log.Trace($"Cannot find sequence:{nextPerformanceSequence.Name}", Common.LOG_CATEGORY);
        //                        nextPerformanceSequence = null;
        //                    }

        //                    break;

        //                case "ST":
        //                    if (AvailableStepperSequences.ContainsKey(nextPerformanceSequence.Name ?? ""))
        //                    {
        //                        var stepperSequence = AvailableStepperSequences[nextPerformanceSequence.Name];
        //                    }

        //                    break;

        //                default:
        //                    Log.Error($"Unsupported SequenceType:>{nextPerformanceSequence.SequenceType}<", Common.LOG_CATEGORY);
        //                    nextPerformanceSequence = null;
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    return nextPerformanceSequence;
        //}

        //private AdvancedServoEx OpenAdvancedServoHost(VNCPhidgetConfig.Host host)
        //{
        //    SerialHost serialHost = new SerialHost { IPAddress = host.IPAddress, SerialNumber = host.AdvancedServos[0].SerialNumber };

        //    PhidgetDevice phidgetDevice = AvailablePhidgets[serialHost];

        //    AdvancedServoEx advancedServoHost;

        //    if (phidgetDevice?.PhidgetEx is not null)
        //    {
        //        advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

        //        advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
        //        advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
        //        advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

        //        advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
        //        advancedServoHost.LogPerformanceAction = LogPerformanceAction;
        //        advancedServoHost.LogActionVerification = LogActionVerification;
        //    }
        //    else
        //    {
        //        phidgetDevice.PhidgetEx = new AdvancedServoEx(
        //            host.IPAddress,
        //            host.Port,
        //            host.AdvancedServos[0].SerialNumber,
        //            EventAggregator);

        //        advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

        //        advancedServoHost = phidgetDevice.PhidgetEx as AdvancedServoEx;

        //        advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
        //        advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
        //        advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

        //        advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
        //        advancedServoHost.LogPerformanceAction = LogPerformanceAction;
        //        advancedServoHost.LogActionVerification = LogActionVerification;

        //        advancedServoHost.Open();
        //    }

        //    //advancedServoHost = new AdvancedServoEx(
        //    //    host.IPAddress,
        //    //    host.Port,
        //    //    host.AdvancedServos[0].SerialNumber,
        //    //    EventAggregator);

        //    //advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

        //    //advancedServoHost = phidgetDevice.PhidgetEx as AdvancedServoEx;

        //    //advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
        //    //advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
        //    //advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

        //    //advancedServoHost.LogPerformanceStep = LogPerformanceStep;

        //    //advancedServoHost.Open();

        //    // NOTE(crhodes)
        //    // Save this so we can use it in other commands

        //    ActiveAdvancedServoHost = advancedServoHost;

        //    return advancedServoHost;
        //}

        // If using CommandParameter, figure out TYPE and fix above
        //public bool PlayPerformanceCanExecute(TYPE value)
        public bool PlayAdvancedServoSequenceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedAdvancedServoSequences?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region AdvancedServo Individual Commands

        #region EngageAndCenter Command

        public DelegateCommand EngageAndCenterCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> EngageAndCenterCommand { get; set; }
        //public TYPE EngageAndCenterCommandParameter;
        public string EngageAndCenterContent { get; set; } = "EngageAndCenter";
        public string EngageAndCenterToolTip { get; set; } = "EngageAndCenter ToolTip";

        // Can get fancy and use Resources
        //public string EngageAndCenterContent { get; set; } = "ViewName_EngageAndCenterContent";
        //public string EngageAndCenterToolTip { get; set; } = "ViewName_EngageAndCenterContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_EngageAndCenterContent">EngageAndCenter</system:String>
        //    <system:String x:Key="ViewName_EngageAndCenterContentToolTip">EngageAndCenter ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void EngageAndCenter(TYPE value)
        public async void EngageAndCenter()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called EngageAndCenter";

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            VNCPhidgetConfig.PerformanceSequence? advancedServoSequence = 
                new VNCPhidgetConfig.PerformanceSequence
                {
                    Name = "Engage and Center Servos",
                    SequenceType = "AS"
                };

            await performanceSequencePlayer.ExecutePerformanceSequenceLoops(advancedServoSequence);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<EngageAndCenterEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<EngageAndCenterEvent>().Publish(
            //      new EngageAndCenterEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class EngageAndCenterEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<EngageAndCenterEvent>().Subscribe(EngageAndCenter);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool EngageAndCenterCanExecute(TYPE value)
        public bool EngageAndCenterCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (AdvancedServoSequenceConfigFileName is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region SetMotionParameters Command

        public DelegateCommand<string> SetMotionParametersCommand { get; set; }

        public async void SetMotionParameters(string speed)
        {
            //Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called SetMotionParameters";

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            VNCPhidgetConfig.PerformanceSequence? advancedServoSequence = null;

            switch (speed)
            {
                case "Fast":

                    advancedServoSequence =
                        new VNCPhidgetConfig.PerformanceSequence
                        {
                            Name = "Acceleration(5000) VelocityLimit(1000)",
                            SequenceType = "AS"
                        };
                    break;

                case "Slow":

                    advancedServoSequence =
                        new VNCPhidgetConfig.PerformanceSequence
                        {
                            Name = "Acceleration(500) VelocityLimit(100)",
                            SequenceType = "AS"
                        };
                    break;
            }


            await performanceSequencePlayer.ExecutePerformanceSequenceLoops(advancedServoSequence);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<SetMotionParametersEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<SetMotionParametersEvent>().Publish(
            //      new SetMotionParametersEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region ResetLimits Command

        public DelegateCommand ResetLimitsCommand { get; set; }

        public async void ResetLimits()
        {
            //Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = $"Cool, you called ResetLimits";

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            VNCPhidgetConfig.PerformanceSequence? advancedServoSequence =
                new VNCPhidgetConfig.PerformanceSequence
                {
                    Name = "Reset Position Limits (RPL)",
                    SequenceType = "AS"
                };

            await performanceSequencePlayer.ExecutePerformanceSequenceLoops(advancedServoSequence);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<SetMotionParametersEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<SetMotionParametersEvent>().Publish(
            //      new SetMotionParametersEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region RelativeAccelerationCommand Command

        public DelegateCommand<Int32?> RelativeAccelerationCommand { get; set; }

        public async void RelativeAcceleration(Int32? relativeAcceleration)
        {
            //Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = $"Cool, you called RelativeAcceleration {relativeAcceleration}";

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            VNCPhidgetConfig.AdvancedServoSequence advancedServoSequence = new VNCPhidgetConfig.AdvancedServoSequence
            {
                Actions = new[]
                {
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 1, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 2, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 3, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 4, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 5, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 6, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 7, RelativeAcceleration = relativeAcceleration}
                }
            };

            await performanceSequencePlayer.ActiveAdvancedServoHost.RunSequenceLoops(advancedServoSequence);

            //VNCPhidgetConfig.PerformanceSequence? nextPerformanceSequence = 
            //     new PerformanceSequence 
            //     { 
            //         Name = $"Acceleraion {relativeAcceleration}", 
            //         SequenceType = "AS"
            //     };

            // await ExecutePerformanceSequence(nextPerformanceSequence);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<SetMotionParametersEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<SetMotionParametersEvent>().Publish(
            //      new SetMotionParametersEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region RelativeVelocityLimitCommand Command

        public DelegateCommand<Int32?> RelativeVelocityLimitCommand { get; set; }

        public async void RelativeVelocityLimit(Int32? relativeVelocityLimit)
        {
            //Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = $"Cool, you called RelativeVelocityLimit {relativeVelocityLimit}";

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            VNCPhidgetConfig.AdvancedServoSequence advancedServoSequence = new VNCPhidgetConfig.AdvancedServoSequence
            { 
                Actions = new[]
                {
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 1, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 2, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 3, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 4, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 5, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 6, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 7, RelativeVelocityLimit = relativeVelocityLimit}
                }
                
            };

            await performanceSequencePlayer.ActiveAdvancedServoHost.RunSequenceLoops(advancedServoSequence);

            //VNCPhidgetConfig.PerformanceSequence? nextPerformanceSequence =
            //     new PerformanceSequence
            //     {
            //         Name = $"VelocityLimit {relativeVelocityLimit}",
            //         SequenceType = "AS"
            //     };

            //await ExecutePerformanceSequence(nextPerformanceSequence);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<SetMotionParametersEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<SetMotionParametersEvent>().Publish(
            //      new SetMotionParametersEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #endregion
        
        #region PlayInterfaceKitSequence Command

        public DelegateCommand PlayInterfaceKitSequenceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlaySequenceCommand { get; set; }
        //public TYPE PlaySequenceCommandParameter;
        public string PlayInterfaceKitSequenceContent { get; set; } = "Play Sequence";
        public string PlayInterfaceKitSequenceToolTip { get; set; } = "PlayInterfaceKitSequence ToolTip";

        // Can get fancy and use Resources
        //public string PlaySequenceContent { get; set; } = "ViewName_PlaySequenceContent";
        //public string PlaySequenceToolTip { get; set; } = "ViewName_PlaySequenceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlaySequenceContent">PlaySequence</system:String>
        //    <system:String x:Key="ViewName_PlaySequenceContentToolTip">PlaySequence ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlaySequence(TYPE value)
        public async void PlayInterfaceKitSequence()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);

            Message = "Cool, you called PlayInterfaceKitSequence";

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            //var runAllThese = SelectedInterfaceKitSequences;
            //var allSequences = AvailableInterfaceKitSequences;

            foreach (VNCPhidgetConfig.InterfaceKitSequence sequence in SelectedInterfaceKitSequences)
            {
                Log.Trace($"Running sequence:{sequence.Name}", Common.LOG_CATEGORY);

                try
                {
                    var nextSequence = sequence;

                    VNCPhidgetConfig.PerformanceSequence? nextPerformanceSequence = new VNCPhidgetConfig.PerformanceSequence
                    {
                        Name = sequence.Name,
                        SequenceType = "IK",
                        Loops = sequence.Loops
                    };

                    do
                    {
                        await performanceSequencePlayer.ExecutePerformanceSequenceLoops(nextPerformanceSequence);
                    } while (nextPerformanceSequence is not null);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

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

        //private InterfaceKitEx OpenInterfaceKitHost(VNCPhidgetConfig.Host host)
        //{
        //    InterfaceKitEx interfaceKitHost;

        //    interfaceKitHost = new InterfaceKitEx(
        //        host.IPAddress,
        //        host.Port,
        //        host.InterfaceKits[0].SerialNumber, true,
        //        EventAggregator);

        //    interfaceKitHost.LogInputChangeEvents = LogInputChangeEvents;
        //    interfaceKitHost.LogOutputChangeEvents = LogOutputChangeEvents;
        //    interfaceKitHost.LogSensorChangeEvents = LogSensorChangeEvents;

        //    interfaceKitHost.LogPerformanceAction = LogPerformanceAction;

        //    interfaceKitHost.Open();

        //    return interfaceKitHost;
        //}

        // If using CommandParameter, figure out TYPE and fix above
        //public bool PlayPerformanceCanExecute(TYPE value)
        public bool PlayInterfaceKitSequenceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedInterfaceKitSequences?.Count > 0)
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

        #region Event Handlers (none)


        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods

        private PerformancePlayer GetPerformancePlayer()
        {
            PerformancePlayer performancePlayer = new PerformancePlayer();
            performancePlayer.AvailablePerformances = AvailablePerformances;
            performancePlayer.PerformanceSequencePlayer = GetPerformanceSequencePlayer();
            performancePlayer.LogPerformance = LogPerformance;

            return performancePlayer;
        }

        private PerformanceSequencePlayer GetPerformanceSequencePlayer()
        {
            if (ActivePerformanceSequencePlayer == null)
            {
                ActivePerformanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);
            }

            //PerformanceSequencePlayer performanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);

            ActivePerformanceSequencePlayer.AvailablePhidgets = AvailablePhidgets;

            ActivePerformanceSequencePlayer.AvailablePerformances = AvailablePerformances;
            ActivePerformanceSequencePlayer.AvailableAdvancedServoSequences = AvailableAdvancedServoSequences;
            ActivePerformanceSequencePlayer.AvailableInterfaceKitSequences = AvailableInterfaceKitSequences;
            ActivePerformanceSequencePlayer.AvailableStepperSequences = AvailableStepperSequences;

            ActivePerformanceSequencePlayer.LogPerformanceSequence = LogPerformanceSequence;
            ActivePerformanceSequencePlayer.LogPerformanceAction = LogPerformanceAction;
            ActivePerformanceSequencePlayer.LogActionVerification = LogActionVerification;

            ActivePerformanceSequencePlayer.LogCurrentChangeEvents = LogCurrentChangeEvents;
            ActivePerformanceSequencePlayer.LogPositionChangeEvents = LogPositionChangeEvents;
            ActivePerformanceSequencePlayer.LogVelocityChangeEvents = LogVelocityChangeEvents;

            ActivePerformanceSequencePlayer.LogInputChangeEvents = LogInputChangeEvents;
            ActivePerformanceSequencePlayer.LogOutputChangeEvents = LogOutputChangeEvents;
            ActivePerformanceSequencePlayer.LogSensorChangeEvents = LogSensorChangeEvents;

            return ActivePerformanceSequencePlayer;
        }

        private async Task PlayParty()
        {
            InterfaceKitEx ifkEx11 = new InterfaceKitEx("192.168.150.11", 5001, sbc11SerialNumber, embedded: true, EventAggregator);
            InterfaceKitEx ifkEx21 = new InterfaceKitEx("192.168.150.21", 5001, sbc21SerialNumber, embedded: true, EventAggregator);
            InterfaceKitEx ifkEx22 = new InterfaceKitEx("192.168.150.22", 5001, sbc22SerialNumber, embedded: true, EventAggregator);
            InterfaceKitEx ifkEx23 = new InterfaceKitEx("192.168.150.23", 5001, sbc23SerialNumber, embedded: true, EventAggregator);

            try
            {
                ifkEx11.Open();
                ifkEx21.Open();
                ifkEx22.Open();
                ifkEx23.Open();

                //ifkEx11.InterfaceKit.OutputChange += Ifk_OutputChange;
                //ifkEx21.InterfaceKit.OutputChange += Ifk_OutputChange;
                //ifkEx22.InterfaceKit.OutputChange += Ifk_OutputChange;
                //ifkEx23.InterfaceKit.OutputChange += Ifk_OutputChange;

                await Task.Run(() =>
                {
                    //Parallel.Invoke(
                    //     () => InterfaceKitParty2(ifkEx21, 500, 5 * Repeats),
                    //     () => InterfaceKitParty2(ifkEx22, 250, 10 * Repeats),
                    //     () => InterfaceKitParty2(ifkEx23, 125, 20 * Repeats),
                    //     () => InterfaceKitParty2(ifkEx11, 333, 8 * Repeats)
                    Parallel.Invoke(
                         () => InterfaceKitParty2(ifkEx21, 10, 5 * Repeats),
                         () => InterfaceKitParty2(ifkEx22, 10, 10 * Repeats),
                         () => InterfaceKitParty2(ifkEx23, 10, 20 * Repeats),
                         () => InterfaceKitParty2(ifkEx11, 10, 8 * Repeats)
                     );
                });

                //ifkEx11.InterfaceKit.OutputChange -= Ifk_OutputChange;
                //ifkEx21.InterfaceKit.OutputChange -= Ifk_OutputChange;
                //ifkEx22.InterfaceKit.OutputChange -= Ifk_OutputChange;
                //ifkEx23.InterfaceKit.OutputChange -= Ifk_OutputChange;

                ifkEx11.Close();
                ifkEx21.Close();
                ifkEx22.Close();
                ifkEx23.Close();
            }
            catch (PhidgetException pe)
            {
                switch (pe.Type)
                {
                    case Phidgets.PhidgetException.ErrorType.PHIDGET_ERR_TIMEOUT:
                        //System.Diagnostics.Debug.WriteLine(
                        //    string.Format("TimeOut Error.  InterfaceKit {0} not attached.  Disable in ConfigFile or attach",
                        //        ifk.SerialNumber));
                        break;

                    default:
                        //System.Diagnostics.Debug.WriteLine(
                        //    string.Format("{0}\nInterface Kit {0}",
                        //pe.ToString(),
                        //        ifk.SerialNumber));
                        break;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        //private void InterfaceKitParty(Int32 serialNumber, string hostName, Int32 port, Int32 sleep, Int32 loops)
        //{
        //    try
        //    {
        //        Log.Debug($"InterfaceKitParty {hostName},{port} {serialNumber} sleep:{sleep} loops:{loops}", Common.LOG_CATEGORY);

        //        VNC.Phidget.InterfaceKitEx ifkEx = new VNC.Phidget.InterfaceKitEx(hostName, port, serialNumber, embedded: true, EventAggregator);

        //        ifkEx.Open();

        //        //ifk.Attach += Ifk_Attach;
        //        //ifk.Detach += Ifk_Detach;
        //        //ifk.Error += Ifk_Error;
        //        //ifk.InputChange += Ifk_InputChange;

        //        ifkEx.InterfaceKit.OutputChange += Ifk_OutputChange;
        //        //ifkEx.OutputChange += Ifk_OutputChange;

        //        //ifk.SensorChange += Ifk_SensorChange;
        //        //ifk.ServerConnect += Ifk_ServerConnect;
        //        //ifk.ServerDisconnect += Ifk_ServerDisconnect;

        //        //ifk.open(serialNumber, hostName, port);
        //        //ifk.waitForAttachment();

        //        InterfaceKitDigitalOutputCollection ifkdoc = ifkEx.InterfaceKit.outputs;
        //        //InterfaceKitDigitalOutputCollection ifkdoc = ifkEx.outputs;

        //        for (int i = 0; i < loops; i++)
        //        {
        //            ifkdoc[0] = true;
        //            Thread.Sleep(sleep);
        //            ifkdoc[0] = false;
        //            Thread.Sleep(sleep);
        //        }

        //        ifkEx.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        private void InterfaceKitParty2(VNC.Phidget.InterfaceKitEx ifkEx, Int32 sleep, Int32 loops)
        {
            try
            {
                Log.Debug($"InterfaceKitParty2 {ifkEx.Host.IPAddress},{ifkEx.Host.Port} {ifkEx.SerialNumber} " +
                    $"sleep:{sleep} loops:{loops}", Common.LOG_CATEGORY);

                InterfaceKitDigitalOutputCollection ifkDigitalOut = ifkEx.InterfaceKit.outputs;

                for (int i = 0; i < loops; i++)
                {
                    ifkDigitalOut[0] = true;
                    Thread.Sleep(sleep);
                    ifkDigitalOut[1] = true;
                    Thread.Sleep(sleep);
                    ifkDigitalOut[2] = true;
                    Thread.Sleep(sleep);

                    ifkDigitalOut[0] = false;
                    Thread.Sleep(sleep);
                    ifkDigitalOut[1] = false;
                    Thread.Sleep(sleep);
                    ifkDigitalOut[2] = false;
                    Thread.Sleep(sleep);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void PhidgetManager_Error(object sender, Phidgets.Events.ErrorEventArgs e)
        {
            Log.Trace($"Error {e.Type} {e.Code}", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_ServerDisconnect(object sender, Phidgets.Events.ServerDisconnectEventArgs e)
        {
            Log.Trace($"ServerDisconnect {e.Device}", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_ServerConnect(object sender, Phidgets.Events.ServerConnectEventArgs e)
        {
            Log.Trace($"ServerConnect {e.Device}", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            Log.Trace($"Detach {e.Device.Name} {e.Device.Address} {e.Device.ID} {e.Device.SerialNumber}", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            Log.Trace($"Attach {e.Device.Name} {e.Device.Address} {e.Device.ID} {e.Device.SerialNumber}", Common.LOG_CATEGORY);
        }


        private void Button5Execute()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "Button5 Clicked";

            EventAggregator.GetEvent<VNC.Phidget.Events.InterfaceKitSequenceEvent>().Publish(new VNC.Phidget.Events.SequenceEventArgs());

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private void LightAction1()
        {
            //for (int i = 0; i < 10; i++)
            //{
                Parallel.Invoke(() => LightAction1A(), () => LightAction1B());
                //await LightAction1A();
                //await LightAction1B();
            //}
        }

        private void LightAction1A()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    digitalOutput0.DutyCycle = 1;
            //    Thread.Sleep(500);
            //    digitalOutput0.DutyCycle = 0;
            //    Thread.Sleep(500);
            //}
        }

        private void LightAction1B()
        {
            //for (int i = 0; i < 50; i++)
            //{
            //    digitalOutput2.DutyCycle = 1;
            //    Thread.Sleep(100);
            //    digitalOutput2.DutyCycle = 0;
            //    Thread.Sleep(100);
            //}
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
