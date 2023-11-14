using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

using DevExpress.Xpf.Editors.DateNavigator;

using Phidgets;

using Prism.Commands;

using Unity.Interception.Utilities;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

//using VNCPhidgets21Explorer.Resources;
using VNCPhidgets21Explorer;
using VNCPhidgets21Explorer.Resources;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class HackAroundViewModel : ViewModelBase, IMainViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public HackAroundViewModel()
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

        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            Button1Command = new DelegateCommand(Button1Execute);
            //Button2Command = new DelegateCommand(Button2Execute);
            //Button3Command = new DelegateCommand(Button3Execute);

            PlayPerformanceCommand = new DelegateCommand (PlayPerformance, PlayPerformanceCanExecute);
            PlayAdvancedServoSequenceCommand = new DelegateCommand(PlayAdvancedServoSequence, PlayAdvancedServoSequenceCanExecute);
            PlayInterfaceKitSequenceCommand = new DelegateCommand(PlayInterfaceKitSequence, PlayInterfaceKitSequenceCanExecute);

            PerformanceConfigFileName = "performanceconfig.json";

            AdvancedServoConfigFileName = "advancedservosequenceconfig.json";
            InterfaceKitConfigFileName = "interfacekitsequenceconfig.json";
            StepperConfigFileName = "steppersequenceconfig.json";

            LoadPerformancesConfig();

            // Turn on logging of PropertyChanged from VNC.Core
            LogOnPropertyChanged = true;

            Message = "HackAroundViewModel says hello";

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadPerformancesConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            var jsonOptions = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };
            string jsonString = File.ReadAllText(PerformanceConfigFileName);

            Resources.PerformanceConfig? performanceConfig
                = JsonSerializer.Deserialize<Resources.PerformanceConfig>(jsonString, jsonOptions);

            Performances = performanceConfig.Performances.ToList();

            AvailablePerformances =
                performanceConfig.Performances
                .ToDictionary(k => k.Name, v => v);

            LoadAdvanceServoConfig(jsonOptions);
            LoadInterfaceKitConfig(jsonOptions);
            LoadStepperConfig(jsonOptions);

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadAdvanceServoConfig(JsonSerializerOptions jsonOptions)
        {
            string jsonString = File.ReadAllText(AdvancedServoConfigFileName);

            Resources.AdvancedServoSequenceConfig? sequenceConfig
                = JsonSerializer.Deserialize<Resources.AdvancedServoSequenceConfig>(jsonString, jsonOptions);

            this.AdvancedServoSequences = sequenceConfig.AdvancedServoSequences.ToList();

            AvailableAdvancedServoSequences =
                sequenceConfig.AdvancedServoSequences
                .ToDictionary(k => k.Name, v => v);
        }

        private void LoadInterfaceKitConfig(JsonSerializerOptions jsonOptions)
        {
            string jsonString = File.ReadAllText(InterfaceKitConfigFileName);

            Resources.InterfaceKitSequenceConfig? sequenceConfig
                = JsonSerializer.Deserialize<Resources.InterfaceKitSequenceConfig>(jsonString, jsonOptions);

            InterfaceKitSequences = sequenceConfig.InterfaceKitSequences.ToList();

            AvailableInterfaceKitSequences =
                sequenceConfig.InterfaceKitSequences
                .ToDictionary(k => k.Name, v => v);
        }

        private void LoadStepperConfig(JsonSerializerOptions jsonOptions)
        {
            string jsonString = File.ReadAllText(StepperConfigFileName);

            Resources.StepperSequenceConfig? sequenceConfig
                = JsonSerializer.Deserialize<Resources.StepperSequenceConfig>(jsonString, jsonOptions);

            StepperSequences = sequenceConfig.StepperSequences.ToList();

            AvailableStepperSequences =
                sequenceConfig.StepperSequences
                .ToDictionary(k => k.Name, v => v);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties


        public ICommand Button1Command { get; private set; }
        public ICommand Button2Command { get; private set; }
        public ICommand Button3Command { get; private set; }

        private string _title = "VNCPhidgets21Explorer - Main";

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value)
                    return;
                _title = value;
                OnPropertyChanged();
            }
        }

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

        private int _numerator = 10;
        public int Numerator
        {
            get => _numerator;
            set
            {
                if (_numerator == value)
                    return;
                _numerator = value;
                OnPropertyChanged();
            }
        }

        private int _denominator = 2;
        public int Denominator
        {
            get => _denominator;
            set
            {
                if (_denominator == value)
                    return;
                _denominator = value;
                OnPropertyChanged();
            }
        }

        private string _answer = "???";
        public string Answer
        {
            get => _answer;
            set
            {
                if (_answer == value)
                    return;
                _answer = value;
                OnPropertyChanged();
            }
        }

        private bool _displayInputChangeEvents = false;

        public bool DisplayInputChangeEvents
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

        public bool DisplayOutputChangeEvents
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

        public bool DisplaySensorChangeEvents
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

        #region Performances

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

        //private Resources.PerformanceConfig _performanceConfig;
        //public Resources.PerformanceConfig PerformanceConfig
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

        private IEnumerable<Resources.Performance> _performances;
        public IEnumerable<Resources.Performance> Performances
        {
            get => _performances;
            set
            {
                _performances = value;
                OnPropertyChanged();
            }
        }

        private Resources.Performance? _selectedPerformance;
        public Resources.Performance? SelectedPerformance
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
                PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private Dictionary<string, Resources.Performance> _availablePerformances;
        public Dictionary<string, Resources.Performance> AvailablePerformances
        {
            get => _availablePerformances;
            set
            {
                _availablePerformances = value;
                OnPropertyChanged();
            }
        }

        private List<Resources.Performance> _selectedPerformances;
        public List<Resources.Performance> SelectedPerformances
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

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
                PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region AdvancedServo

        private string _advancedServoConfigFileName;
        public string AdvancedServoConfigFileName
        {
            get => _advancedServoConfigFileName;
            set
            {
                if (_advancedServoConfigFileName == value) return;
                _advancedServoConfigFileName = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.AdvancedServoSequence> _advancedServoSequences;
        public IEnumerable<Resources.AdvancedServoSequence> AdvancedServoSequences
        {
            get => _advancedServoSequences;
            set
            {
                _advancedServoSequences = value;
                OnPropertyChanged();
            }
        }

        private Resources.AdvancedServoSequence? _selectedAdvancedServoSequence;
        public Resources.AdvancedServoSequence? SelectedAdvancedServoSequence
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

        private Dictionary<string, Resources.AdvancedServoSequence> _availableAdvancedServoSequences;
        public Dictionary<string, Resources.AdvancedServoSequence> AvailableAdvancedServoSequences
        {
            get => _availableAdvancedServoSequences;
            set
            {
                _availableAdvancedServoSequences = value;
                OnPropertyChanged();
            }
        }

        private List<Resources.AdvancedServoSequence> _selectedAdvancedServoSequences;
        public List<Resources.AdvancedServoSequence> SelectedAdvancedServoSequences
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

        private string _interfaceKitConfigFileName;
        public string InterfaceKitConfigFileName
        {
            get => _interfaceKitConfigFileName;
            set
            {
                if (_interfaceKitConfigFileName == value) return;
                _interfaceKitConfigFileName = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.InterfaceKitSequence> _interfaceKitSequences;
        public IEnumerable<Resources.InterfaceKitSequence> InterfaceKitSequences
        {
            get => _interfaceKitSequences;
            set
            {
                _interfaceKitSequences = value;
                OnPropertyChanged();
            }
        }

        private Resources.InterfaceKitSequence? _selectedInterfaceKitSequence;
        public Resources.InterfaceKitSequence? SelectedInterfaceKitSequence
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

        private Dictionary<string, Resources.InterfaceKitSequence> _availableInterfaceKitSequences;
        public Dictionary<string, Resources.InterfaceKitSequence> AvailableInterfaceKitSequences
        {
            get => _availableInterfaceKitSequences;
            set
            {
                _availableInterfaceKitSequences = value;
                OnPropertyChanged();
            }
        }

        private List<Resources.InterfaceKitSequence> _selectedInterfaceKitSequences;
        public List<Resources.InterfaceKitSequence> SelectedInterfaceKitSequences
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

        private string _stepperConfigFileName;
        public string StepperConfigFileName
        {
            get => _stepperConfigFileName;
            set
            {
                if (_stepperConfigFileName == value) return;
                _stepperConfigFileName = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.StepperSequence> _stepperSequences;
        public IEnumerable<Resources.StepperSequence> StepperSequences
        {
            get => _stepperSequences;
            set
            {
                _stepperSequences = value;
                OnPropertyChanged();
            }
        }

        private Resources.StepperSequence? _selectedStepperSequence;
        public Resources.StepperSequence? SelectedStepperSequence
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

        private Dictionary<string, Resources.StepperSequence> _availableStepperSequences;
        public Dictionary<string, Resources.StepperSequence> AvailableStepperSequences
        {
            get => _availableStepperSequences;
            set
            {
                _availableStepperSequences = value;
                OnPropertyChanged();
            }
        }

        private List<Resources.StepperSequence> _selectedStepperSequences;
        public List<Resources.StepperSequence> SelectedStepperSequences
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

        #endregion

        #region Commands

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
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlayPerformance";

            foreach (Resources.Performance performance in SelectedPerformances)
            {
                Log.Trace($"Running performance:{performance.Name} description:{performance.Description}", Common.LOG_CATEGORY);

                PlayPerformanceLoops(performance);

                // TODO(crhodes)
                // I think this is where we look for NextPerformance is not null and call that
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

        private async Task PlayPerformanceLoops(Resources.Performance performance)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            for (int i = 0; i < performance.Loops; i++)
            {
                Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);

                if (performance.PlayInParallel)
                {
                    PlayPerformanceSequencesInParallel(performance.PerformanceSequences);
                }
                else
                {
                    PlayPerformanceSequencesInSequence(performance.PerformanceSequences);
                }
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }
        private async void PlayPerformanceSequencesInParallel(PerformanceSequence[] performanceSequences)
        {
            // TODO(crhodes)
            // Figure out what this should do

            Parallel.ForEach(performanceSequences, sequence =>
            {
                if (LogPerformanceStep)
                {
                    Log.Trace($"Running sequence:{sequence.Name} type:{sequence.SequenceType}", Common.LOG_CATEGORY);
                }

                try
                {

                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            });
        }

        private async void PlayPerformanceSequencesInSequence(PerformanceSequence[] performanceSequences)
        {
            foreach (Resources.PerformanceSequence sequence in performanceSequences)
            {
                if (LogPerformanceStep)
                {
                    Log.Trace($"Running sequence:{sequence.Name} type:{sequence.SequenceType}", Common.LOG_CATEGORY);
                }

                try
                {
                    //    string name = "";
                    //    PerformanceSequence? nextSequence = sequence;

                    //    do
                    //    {
                    //        Log.Trace($"  Playing Sequence:{nextSequence.Name} NextSequence:{nextSequence?.Name}", Common.LOG_CATEGORY);

                    //        switch (nextSequence.SequenceType)
                    //        {
                    //            case "AS":
                    //                var advancedServoSequence = AvailableAdvancedServoSequences[nextSequence.Name];
                    //                var advancedServoHost = advancedServoSequence.Host;
                    //                string? asContinueWith = "";

                    //                // TODO(crhodes)
                    //                // Think about Open/Close.  Do we do it heare or inside Loops

                    //                var advancedServo = OpenAdvancedServoHost(advancedServoHost);
                    //                AdvancedServoSequence asNextSequence = advancedServoSequence;

                    //                do
                    //                {
                    //                    name = nextSequence.Name;
                    //                    asContinueWith = advancedServoSequence.ContinueWith;
                    //                    Log.Trace($"  Playing sequence:{name} NextSequence:{advancedServoSequence.ContinueWith?.Name}", Common.LOG_CATEGORY);

                    //                    await advancedServo.PlayAdvancedServoSequenceLoops(advancedServo, asNextSequence);

                    //                    // NOTE(crhodes)
                    //                    // If there is a NextSequence, figure out how to play it

                    //                    asNextSequence.NextSequence.Name;

                    //                    if (AvailableAdvancedServoSequences.ContainsKey(asNextSequence.NextSequence.Name ?? ""))
                    //                    {
                    //                        nextSequence = AvailableAdvancedServoSequences[asNextSequence.NextSequence.Name].NextSequence;
                    //                    }
                    //                    else
                    //                    {
                    //                        nextSequence = null;
                    //                    }

                    //                } while (!string.IsNullOrEmpty(asContinueWith));

                    //                // TODO(crhodes)
                    //                // Think about Open/Close.  Do we do it heare or inside Loops

                    //                advancedServo.Close();

                    //                break;

                    //            case "IK":
                    //                var interfaceKitSequence = AvailableInterfaceKitSequences[nextSequence.Name];
                    //                var ikHost = interfaceKitSequence.Host;
                    //                string? ikContinueWith = "";

                    //                // TODO(crhodes)
                    //                // Think about Open/Close.  Do we do it heare or inside Loops

                    //                var interfaceKit = OpenInterfaceKitHost(ikHost);
                    //                InterfaceKitSequence ikNextSequence = interfaceKitSequence;

                    //                do
                    //                {
                    //                    name = ikNextSequence.Name;
                    //                    ikContinueWith = ikNextSequence.ContinueWith;
                    //                    Log.Trace($"  Playing sequence:{name} NextPerformance:{nextSequence.NextPerformance?.Name}", Common.LOG_CATEGORY);

                    //                    await PlayInterfaceKitSequenceLoops(interfaceKit, ikNextSequence);

                    //                    if (AvailableInterfaceKitSequences.ContainsKey(ikContinueWith ?? ""))
                    //                    {
                    //                        ikNextSequence = AvailableInterfaceKitSequences[ikContinueWith];
                    //                    }
                    //                    else
                    //                    {
                    //                        ikContinueWith = "";
                    //                    }

                    //                } while (!string.IsNullOrEmpty(ikContinueWith));

                    //                // TODO(crhodes)
                    //                // Think about Open/Close.  Do we do it heare or inside Loops

                    //                interfaceKit.Close();
                    //                break;

                    //            case "ST":

                    //                break;

                    //            default:
                    //                Log.Trace($"Unexpected SequenceType:{nextPerformance.SequenceType}", Common.LOG_CATEGORY);
                    //                break;
                    //        }

                    //        nextPerformance = nextPerformance.NextPerformance;
                    //        //await PlayPerformanceLoops(nextPerformance);

                    //        //if (AvailableAdvancedServoPerformances.ContainsKey(continueWith.Name ?? ""))
                    //        //{
                    //        //    nextPerformance = AvailableAdvancedServoPerformances[continueWith];
                    //        //}
                    //        //else
                    //        //{
                    //        //    continueWith = "";
                    //        //}

                    //    } while (nextPerformance is not null);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        //private void PerformServoAction(AdvancedServoServo servo, AdvancedServoServoAction action, Int32 index)
        //{
        //    Int64 startTicks = Log.Trace($"Enter servo:{index}", Common.LOG_CATEGORY);

        //    try
        //    {
        //        if (action.Acceleration is not null) servo.Acceleration = (Double)action.Acceleration;
        //        if (action.VelocityLimit is not null) servo.VelocityLimit = (Double)action.VelocityLimit;
        //        if (action.PositionMin is not null) servo.PositionMin = (Double)action.PositionMin;
        //        if (action.PositionMax is not null) servo.PositionMax = (Double)action.PositionMax;
        //        if (action.Engaged is not null) servo.Engaged = (Boolean)action.Engaged;

        //        // TODO(crhodes)
        //        // Maybe wait for servo Engaged to complete if not currently engaged
        //        // View logs and see how often exceptions thrown.

        //        if (action.TargetPosition is not null)
        //        {
        //            servo.Position = (Double)action.TargetPosition;
        //            Thread.Sleep(1);

        //            VerifyNewPositionAchieved(servo, (Double)action.TargetPosition);

        //            if (action.Duration > 0) Thread.Sleep((Int32)action.Duration);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //private void VerifyNewPositionAchieved(AdvancedServoServo servo, double targetPosition)
        //{
        //    while (servo.Position != targetPosition) { Thread.Sleep(1); }
        //}

        // If using CommandParameter, figure out TYPE and fix above
        //public bool PlayPerformanceCanExecute(TYPE value)
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
        public string PlayAdvancedServoSequenceContent { get; set; } = "PlayAdvancedServoSequence";
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

            //var runAllThese = SelectedAdvancedServoSequences;
            //var allSequences = AvailableAdvancedServoSequences;

            foreach (AdvancedServoSequence sequence in SelectedAdvancedServoSequences)
            {
                Log.Trace($"Running sequence:{sequence.Name}", Common.LOG_CATEGORY);

                try
                {
                    PerformanceSequence? nextPerformanceSequence = new PerformanceSequence
                    {
                        Name = sequence.Name,
                        SequenceType = "AS",
                        Loops = sequence.Loops
                    };

                    do
                    {
                        nextPerformanceSequence = await ExecutePerformanceSequence(nextPerformanceSequence);
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

        private async Task<PerformanceSequence?> ExecutePerformanceSequence(PerformanceSequence? nextPerformanceSequence)
        {
            Log.Trace($"  Playing sequence:{nextPerformanceSequence?.Name} type:{nextPerformanceSequence?.SequenceType}", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Think about Open/Close more.  Maybe config.
            // What happens if nextSequence.Host is null    

            if (nextPerformanceSequence is not null)
            {
                switch (nextPerformanceSequence.SequenceType)
                {
                    case "AS":
                        if (AvailableAdvancedServoSequences.ContainsKey(nextPerformanceSequence.Name ?? ""))
                        {
                            var advancedServoSequence = AvailableAdvancedServoSequences[nextPerformanceSequence.Name];

                            if (advancedServoSequence.Host is not null)
                            {
                                var advancedServoHost = OpenAdvancedServoHost(advancedServoSequence.Host);

                                //nextPerformanceSequence = await advancedServo.PlayAdvancedServoSequenceLoops(advancedServoSequence);
                                await advancedServoHost.PlaySequenceLoops(advancedServoSequence);

                                nextPerformanceSequence = advancedServoSequence.NextSequence;

                                // NOTE(crhodes)
                                // This should handle continuations without a Host.  
                                // TODO(crhodes)
                                // Do we need to handle continuations that have a Host?  I think so.
                                // Play AS sequence on one Host and then a different AS sequence on a different host.
                                // This would be dialog back and forth across hosts.

                                while (nextPerformanceSequence is not null && nextPerformanceSequence.SequenceType == "AS")
                                {
                                    advancedServoSequence = AvailableAdvancedServoSequences[nextPerformanceSequence.Name];
   
                                    await advancedServoHost.PlaySequenceLoops(advancedServoSequence);

                                    nextPerformanceSequence = advancedServoSequence.NextSequence;
                                }

                                advancedServoHost.Close();
                            }
                            else
                            {
                                Log.Trace($"    Host is null", Common.LOG_CATEGORY);
                                nextPerformanceSequence = null;
                            }
                        }
                        else
                        {
                            Log.Trace($"    Cannot find sequence:{nextPerformanceSequence.Name}", Common.LOG_CATEGORY);
                            nextPerformanceSequence = null;
                        }

                        break;

                    case "IK":
                        if (AvailableInterfaceKitSequences.ContainsKey(nextPerformanceSequence.Name ?? ""))
                        {
                            var interfaceKitSequence = AvailableInterfaceKitSequences[nextPerformanceSequence.Name];

                            if (interfaceKitSequence.Host is not null)
                            {
                                var interfaceKitHost = OpenInterfaceKitHost(interfaceKitSequence.Host);

                                //nextPerformanceSequence = await advancedServo.PlayAdvancedServoSequenceLoops(advancedServoSequence);
                                await interfaceKitHost.PlaySequenceLoops(interfaceKitSequence);

                                nextPerformanceSequence = interfaceKitSequence.NextSequence;

                                // NOTE(crhodes)
                                // This should handle continuations without a Host.  
                                // TODO(crhodes)
                                // Do we need to handle continuations that have a Host?  I think so.
                                // Play AS sequence on one Host and then a different AS sequence on a different host.
                                // This would be dialog back and forth across hosts.

                                while (nextPerformanceSequence is not null && nextPerformanceSequence.SequenceType == "AS")
                                {
                                    interfaceKitSequence = AvailableInterfaceKitSequences[nextPerformanceSequence.Name];

                                    await interfaceKitHost.PlaySequenceLoops(interfaceKitSequence);

                                    nextPerformanceSequence = interfaceKitSequence.NextSequence;
                                }

                                interfaceKitHost.Close();
                            }
                            else
                            {
                                Log.Trace($"    Host is null", Common.LOG_CATEGORY);
                                nextPerformanceSequence = null;
                            }
                        }
                        else
                        {
                            Log.Trace($"    Cannot find sequence:{nextPerformanceSequence.Name}", Common.LOG_CATEGORY);
                            nextPerformanceSequence = null;
                        }

                        break;

                    case "ST":
                        if (AvailableStepperSequences.ContainsKey(nextPerformanceSequence.Name ?? ""))
                        {
                            var stepperSequence = AvailableStepperSequences[nextPerformanceSequence.Name];
                            //var stepper = OpenStepperHost(stepperSequence.Host);
                            //await advancedServo.PlayAdvancedServoSequenceLoops(advancedServo, stepperSequence);
                        }

                        break;

                    default:

                        break;

                }
            }

            return nextPerformanceSequence;
        }

        private AdvancedServoEx OpenAdvancedServoHost(Resources.Host host)
        { 
            AdvancedServoEx advancedServo;

            advancedServo = new AdvancedServoEx(
                host.IPAddress,
                host.Port,
                host.AdvancedServos[0].SerialNumber);

            advancedServo.Open();

            return advancedServo;
        }

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

        #region PlayInterfaceKitSequence Command

        public DelegateCommand PlayInterfaceKitSequenceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlaySequenceCommand { get; set; }
        //public TYPE PlaySequenceCommandParameter;
        public string PlayInterfaceKitSequenceContent { get; set; } = "PlayInterfaceKitSequence";
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

            //var runAllThese = SelectedInterfaceKitSequences;
            //var allSequences = AvailableInterfaceKitSequences;

            foreach (InterfaceKitSequence sequence in SelectedInterfaceKitSequences)
            {
                Log.Trace($"Running sequence:{sequence.Name}", Common.LOG_CATEGORY);

                try
                {
                    var nextSequence = sequence;

                    PerformanceSequence? nextPerformanceSequence = new PerformanceSequence
                    {
                        Name = sequence.Name,
                        SequenceType = "IK",
                        Loops = sequence.Loops
                    };

                    do
                    {
                        nextPerformanceSequence = await ExecutePerformanceSequence(nextPerformanceSequence);
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

        private InterfaceKitEx OpenInterfaceKitHost(Resources.Host host)
        {
            InterfaceKitEx interfaceKit;

            interfaceKit = new InterfaceKitEx(
                host.IPAddress,
                host.Port,
                host.InterfaceKits[0].SerialNumber, true);

            interfaceKit.Open();

            return interfaceKit;
        }

        //private async Task PlayInterfaceKitSequenceLoops(InterfaceKitEx interfaceKit, Resources.InterfaceKitSequence interfaceKitSequence)
        //{
        //    Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

        //    for (int i = 0; i < interfaceKitSequence.Loops; i++)
        //    {
        //        Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);

        //        if (interfaceKitSequence.PlayActionsInParallel)
        //        {
        //            PlayInterfaceKitSequenceActionsInParallel(interfaceKit, interfaceKitSequence);
        //        }
        //        else
        //        {
        //            PlayInterfaceKitSequenceActionsInSequence(interfaceKit, interfaceKitSequence);
        //        }
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //private async void PlayInterfaceKitSequenceActionsInParallel(InterfaceKitEx interfaceKit, Resources.InterfaceKitSequence interfaceKitSequence)
        //{
        //    Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

        //    // TODO(crhodes)
        //    // Maybe just pass the interfaceKit into Action and get this there

        //    InterfaceKitDigitalOutputCollection ifkDigitalOutputs = interfaceKit.InterfaceKit.outputs;

        //    Parallel.ForEach(interfaceKitSequence.InterfaceKitActions, action =>
        //    {
        //        if (LogPerformanceStep)
        //        {
        //            Log.Trace($"DigitalOut Index:{action.DigitalOutIndex} DigitalOut:{action.DigitalOut} Duration:{action.Duration}", Common.LOG_CATEGORY);
        //        }

        //        try
        //        {
        //            switch (action.DigitalOutIndex)
        //            {
        //                case 0:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 0);
        //                    break;

        //                case 1:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 1);
        //                    break;

        //                case 2:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 2);
        //                    break;

        //                case 3:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 30);
        //                    break;

        //                case 4:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 4);
        //                    break;

        //                case 5:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 50);
        //                    break;

        //                case 6:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 60);
        //                    break;

        //                case 7:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 7);
        //                    break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    });

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //private void PlayInterfaceKitSequenceActionsInSequence(InterfaceKitEx interfaceKit, Resources.InterfaceKitSequence interfaceKitSequence)
        //{
        //    Int64 startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

        //    // TODO(crhodes)
        //    // Maybe just pass the interfaceKit into Action and get this there

        //    InterfaceKitDigitalOutputCollection ifkDigitalOutputs = interfaceKit.InterfaceKit.outputs;

        //    foreach (Resources.InterfaceKitAction action in interfaceKitSequence.InterfaceKitActions)
        //    {
        //        if (LogPerformanceStep)
        //        {
        //            Log.Trace($"DigitalOut Index:{action.DigitalOutIndex} DigitalOut:{action.DigitalOut} Duration:{action.Duration}", Common.LOG_CATEGORY);
        //        }

        //        try
        //        {
        //            switch (action.DigitalOutIndex)
        //            {
        //                case 0:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 0);
        //                    break;

        //                case 1:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 1);
        //                    break;

        //                case 2:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 2);
        //                    break;

        //                case 3:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 30);
        //                    break;

        //                case 4:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 4);
        //                    break;

        //                case 5:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 50);
        //                    break;

        //                case 6:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 60);
        //                    break;

        //                case 7:
        //                    PerformInterfaceKitAction(ifkDigitalOutputs, action, 7);
        //                    break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //private void PerformInterfaceKitAction(InterfaceKitDigitalOutputCollection ifkDigitalOutputs, Resources.InterfaceKitAction action, Int32 index)
        //{
        //    Int64 startTicks = Log.Trace($"Enter servo:{index}", Common.LOG_CATEGORY);

        //    try
        //    {
        //        if (action.DigitalOut is not null) ifkDigitalOutputs[index] = (Boolean)action.DigitalOut;

        //        if (action.Duration > 0) Thread.Sleep((Int32)action.Duration);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
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

        private async Task OpenSBCInterfaceKit()
        {

            InterfaceKitEx ifkEx11 = new InterfaceKitEx("192.168.150.11", 5001, sbc11SerialNumber, embedded: true);
            InterfaceKitEx ifkEx21 = new InterfaceKitEx("192.168.150.21", 5001, sbc21SerialNumber, embedded: true);
            InterfaceKitEx ifkEx22 = new InterfaceKitEx("192.168.150.22", 5001, sbc22SerialNumber, embedded: true);
            InterfaceKitEx ifkEx23 = new InterfaceKitEx("192.168.150.23", 5001, sbc23SerialNumber, embedded: true);

            try
            {
                ifkEx11.Open();
                ifkEx21.Open();
                ifkEx22.Open();
                ifkEx23.Open();

                ifkEx11.InterfaceKit.OutputChange += Ifk_OutputChange;
                ifkEx21.InterfaceKit.OutputChange += Ifk_OutputChange;
                ifkEx22.InterfaceKit.OutputChange += Ifk_OutputChange;
                ifkEx23.InterfaceKit.OutputChange += Ifk_OutputChange;

                await Task.Run(() =>
                {
                    Parallel.Invoke(
                         () => InterfaceKitParty2(ifkEx21, 500, 5 * Repeats),
                         () => InterfaceKitParty2(ifkEx22, 250, 10 * Repeats),
                         () => InterfaceKitParty2(ifkEx23, 125, 20 * Repeats),
                         () => InterfaceKitParty2(ifkEx11, 333, 8 * Repeats)
                     );
                });

                ifkEx11.InterfaceKit.OutputChange -= Ifk_OutputChange;
                ifkEx21.InterfaceKit.OutputChange -= Ifk_OutputChange;
                ifkEx22.InterfaceKit.OutputChange -= Ifk_OutputChange;
                ifkEx23.InterfaceKit.OutputChange -= Ifk_OutputChange;

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

        private void InterfaceKitParty(Int32 serialNumber, string hostName, Int32 port, Int32 sleep, Int32 loops)
        {
            try
            {
                Log.Debug($"InterfaceKitParty {hostName},{port} {serialNumber} sleep:{sleep} loops:{loops}", Common.LOG_CATEGORY);

                VNC.Phidget.InterfaceKitEx ifkEx = new VNC.Phidget.InterfaceKitEx(hostName, port, serialNumber, embedded: true);

                ifkEx.Open();

                //ifk.Attach += Ifk_Attach;
                //ifk.Detach += Ifk_Detach;
                //ifk.Error += Ifk_Error;
                //ifk.InputChange += Ifk_InputChange;

                ifkEx.InterfaceKit.OutputChange += Ifk_OutputChange;
                //ifkEx.OutputChange += Ifk_OutputChange;

                //ifk.SensorChange += Ifk_SensorChange;
                //ifk.ServerConnect += Ifk_ServerConnect;
                //ifk.ServerDisconnect += Ifk_ServerDisconnect;

                //ifk.open(serialNumber, hostName, port);
                //ifk.waitForAttachment();

                InterfaceKitDigitalOutputCollection ifkdoc = ifkEx.InterfaceKit.outputs;
                //InterfaceKitDigitalOutputCollection ifkdoc = ifkEx.outputs;

                for (int i = 0; i < loops; i++)
                {
                    ifkdoc[0] = true;
                    Thread.Sleep(sleep);
                    ifkdoc[0] = false;
                    Thread.Sleep(sleep);
                }

                ifkEx.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void InterfaceKitParty2(VNC.Phidget.InterfaceKitEx ifkEx, Int32 sleep, Int32 loops)
        {
            try
            {
                Log.Debug($"InterfaceKitParty2 {ifkEx.HostIPAddress},{ifkEx.HostPort} {ifkEx.HostSerialNumber} sleep:{sleep} loops:{loops}", Common.LOG_CATEGORY);

                //ifkEx.InterfaceKit.OutputChange += Ifk_OutputChange;

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

                ifkEx.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_ServerDisconnect(object sender, Phidgets.Events.ServerDisconnectEventArgs e)
        {
            try
            {
                var a = e;
                var b = e.GetType();
                Log.Trace("Ifk_ServerDisconnect", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_ServerConnect(object sender, Phidgets.Events.ServerConnectEventArgs e)
        {
            try
            {
                Phidget device = (Phidget)e.Device;
                //var b = e.GetType();
                //Log.Trace($"Ifk_ServerConnect {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
                Log.Trace($"Ifk_ServerConnect {device.Address},{device.Port}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {
            if (DisplaySensorChangeEvents)
            {
                try
                {
                    Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"Ifk_SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void Ifk_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        {
            if (DisplayOutputChangeEvents)
            {
                try
                {
                    Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"Ifk_OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void Ifk_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        {
            if (DisplayInputChangeEvents)
            {
                try
                {
                    Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"Ifk_InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void Ifk_Error(object sender, Phidgets.Events.ErrorEventArgs e)
        {
            try
            {
                Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                var a = e;
                var b = e.GetType();
                Log.Trace($"Ifk_Error {ifk.Address},{ifk.Attached} - {e.Type} {e.Code} {e.Description}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                var a = e;
                var b = e.GetType();
                Log.Trace($"Ifk_Detach {ifk.Address},{ifk.SerialNumber}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                //Phidget device = (Phidget)e.Device;
                //var b = e.GetType();
                Log.Trace($"Ifk_Attach {ifk.Address},{ifk.Port} S#:{ifk.SerialNumber}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void PhidgetManager_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            var a = e;
            var b = e.GetType();
            Log.Trace("PhidgetManager_Detach", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            var a = e;
            var b = e.GetType();
            Log.Trace("PhidgetManager_Attach", Common.LOG_CATEGORY);
        }

        private void Button1Execute()
        {
            Int64 startTicks = Log.Info("Enter", "WHOISTHIS");

            Message = "Button1 Clicked";

            OpenSBCInterfaceKit();

            Log.Info("End", "WHOISTHIS", startTicks);
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
