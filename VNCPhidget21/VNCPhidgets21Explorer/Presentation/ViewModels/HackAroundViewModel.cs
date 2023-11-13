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
            Button2Command = new DelegateCommand(Button2Execute);
            Button3Command = new DelegateCommand(Button3Execute);

            PlayPerformanceCommand = new DelegateCommand<string>(PlayPerformance, PlayPerformanceCanExecute);
            PlayAdvancedServoSequenceCommand = new DelegateCommand<string>(PlayAdvancedServoSequence, PlayAdvancedServoSequenceCanExecute);
            PlayInterfaceKitSequenceCommand = new DelegateCommand<string>(PlayInterfaceKitSequence, PlayInterfaceKitSequenceCanExecute);

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

            Resources.PerformanceConfig? sequenceConfig
                = JsonSerializer.Deserialize<Resources.PerformanceConfig>(jsonString, jsonOptions);

            this.PerformanceSequences = sequenceConfig.PerformanceSequences.ToList();

            AvailablePerformanceSequences =
                sequenceConfig.PerformanceSequences
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
            string jsonString = File.ReadAllText(AdvancedServoConfigFileName);

            Resources.InterfaceKitSequenceConfig? sequenceConfig
                = JsonSerializer.Deserialize<Resources.InterfaceKitSequenceConfig>(jsonString, jsonOptions);

            this.InterfaceKitSequences = sequenceConfig.InterfaceKitSequences.ToList();

            AvailableInterfaceKitSequences =
                sequenceConfig.InterfaceKitSequences
                .ToDictionary(k => k.Name, v => v);
        }

        private void LoadStepperConfig(JsonSerializerOptions jsonOptions)
        {
            string jsonString = File.ReadAllText(StepperConfigFileName);

            Resources.StepperSequenceConfig? sequenceConfig
                = JsonSerializer.Deserialize<Resources.StepperSequenceConfig>(jsonString, jsonOptions);

            this.StepperSequences = sequenceConfig.StepperSequences.ToList();

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

        private Resources.PerformanceConfig _performanceConfig;
        public Resources.PerformanceConfig PerformanceConfig
        {
            get => _performanceConfig;
            set
            {
                if (_performanceConfig == value)
                    return;
                _performanceConfig = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.PerformanceSequence> _performanceSequences;
        public IEnumerable<Resources.PerformanceSequence> PerformanceSequences
        {
            get => _performanceSequences;
            set
            {
                _performanceSequences = value;
                OnPropertyChanged();
            }
        }

        private Resources.PerformanceSequence? _selectedPerformanceSequence;
        public Resources.PerformanceSequence? SelectedPerformanceSequence
        {
            get => _selectedPerformanceSequence;
            set
            {
                if (_selectedPerformanceSequence == value)
                {
                    return;
                }

                _selectedPerformanceSequence = value;
                OnPropertyChanged();

                PlayPerformanceCommand.RaiseCanExecuteChanged();
                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private Dictionary<string, Resources.PerformanceSequence> _availablePerformanceSequences;
        public Dictionary<string, Resources.PerformanceSequence> AvailablePerformanceSequences
        {
            get => _availablePerformanceSequences;
            set
            {
                _availablePerformanceSequences = value;
                OnPropertyChanged();
            }
        }

        private List<Resources.PerformanceSequence> _selectedPerformanceSequences;
        public List<Resources.PerformanceSequence> SelectedPerformanceSequences
        {
            get => _selectedPerformanceSequences;
            set
            {
                if (_selectedPerformanceSequences == value)
                {
                    return;
                }

                _selectedPerformanceSequences = value;
                OnPropertyChanged();

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
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

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
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

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
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
                foreach (Resources.PerformanceSequence performance in PerformanceSequences)
                {
                    await PlayPerformanceLoops(performance);
                }
            }
            else
            {
                var runAllThese = SelectedPerformanceSequences;
                //var allPerformances = A;

                //var runAllTheseD = SelectedAdvancedServoPerformanceD;

                //// TODO(crhodes)
                //// Figure out how to handle loops
                //var nextPerformance = SelectedAdvancedServoPerformance;

                foreach (Resources.PerformanceSequence performance in SelectedPerformanceSequences)
                {
                    Log.Trace($"Running performance:{performance.Name}", Common.LOG_CATEGORY);

                    string name = "";
                    PerformanceSequence? nextPerformance = performance;

                    do
                    {
                        name = nextPerformance.Name;

                        Log.Trace($"  Playing performance:{name} continueWidth:{nextPerformance?.Name}", Common.LOG_CATEGORY);

                        switch (nextPerformance.SequenceType)
                        {
                            case "AS":
                                var advancedServoSequence = AvailableAdvancedServoSequences[nextPerformance.Name];
                                var advancedServoHost = advancedServoSequence.Host;
                                string? continueWith = "";

                                var advancedServo = OpenAdvancedServoHost(advancedServoHost);
                                AdvancedServoSequence nextSequence = advancedServoSequence;

                                do
                                {
                                    name = nextSequence.Name;
                                    continueWith = nextSequence.ContinueWith;
                                    Log.Trace($"  Playing sequence:{name} NextPerformance:{nextPerformance.NextPerformance?.Name}", Common.LOG_CATEGORY);

                                    await PlayAdvancedServoSequenceLoops(advancedServo, nextSequence);

                                    if (AvailableAdvancedServoSequences.ContainsKey(continueWith ?? ""))
                                    {
                                        nextSequence = AvailableAdvancedServoSequences[continueWith];
                                    }
                                    else
                                    {
                                        continueWith = "";
                                    }

                                } while (!string.IsNullOrEmpty(continueWith));

                                advancedServo.Close();

                                break;

                            case "IK":

                                break;

                            case "ST":

                                break;

                            default:
                                Log.Trace($"Unexpected SequenceType:{nextPerformance.SequenceType}", Common.LOG_CATEGORY);
                                break;
                        }

                        nextPerformance = nextPerformance.NextPerformance;
                        //await PlayPerformanceLoops(nextPerformance);

                        //if (AvailableAdvancedServoPerformances.ContainsKey(continueWith.Name ?? ""))
                        //{
                        //    nextPerformance = AvailableAdvancedServoPerformances[continueWith];
                        //}
                        //else
                        //{
                        //    continueWith = "";
                        //}

                    } while (nextPerformance is not null);
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

        private async Task PlayPerformanceLoops(Resources.PerformanceSequence performanceSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            //for (int i = 0; i < advancedServoPerformance.Loops; i++)
            //{
            //    Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);

            //    if (advancedServoPerformance.PlayInParallel)
            //    {
            //        PlayPerformanceInParallel(advancedServoPerformance);
            //    }
            //    else
            //    {
            //        PlayPerformanceInSequence(advancedServoPerformance);
            //    }

            //}

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async void PlayPerformanceInParallel(Resources.PerformanceSequence performance)
        {
            // TODO(crhodes)
            // Figure out what this should do

            //Parallel.ForEach(advancedServoSequence.AdvancedServoServoActions, action =>
            //{
            //    if (LogPerformanceStep)
            //    {
            //        Log.Trace($"Servo:{action.ServoIndex} Acceleration:{action.Acceleration} VelocityLimit:{action.VelocityLimit}" +
            //            $" Engaged:{action.Engaged} TargetPosition:{action.TargetPosition} Duration:{action.Duration}", Common.LOG_CATEGORY);
            //    }

            //    try
            //    {
            //        switch (action.ServoIndex)
            //        {
            //            case 0:
            //                PerformServoAction(ActiveAdvancedServo.AdvancedServo.servos[0], action, 0);
            //                break;

            //            case 1:
            //                PerformServoAction(ActiveAdvancedServo.AdvancedServo.servos[1], action, 1);
            //                break;

            //            case 2:
            //                PerformServoAction(ActiveAdvancedServo.AdvancedServo.servos[2], action, 2);
            //                break;

            //            case 3:
            //                PerformServoAction(ActiveAdvancedServo.AdvancedServo.servos[3], action, 3);
            //                break;

            //            case 4:
            //                PerformServoAction(ActiveAdvancedServo.AdvancedServo.servos[4], action, 4);
            //                break;

            //            case 5:
            //                PerformServoAction(ActiveAdvancedServo.AdvancedServo.servos[5], action, 5);
            //                break;

            //            case 6:
            //                PerformServoAction(ActiveAdvancedServo.AdvancedServo.servos[6], action, 6);
            //                break;

            //            case 7:
            //                PerformServoAction(ActiveAdvancedServo.AdvancedServo.servos[7], action, 7);
            //                break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error(ex, Common.LOG_CATEGORY);
            //    }
            //});
        }

        private async void PlayPerformanceInSequence(Resources.PerformanceSequence performance)
        {
            //foreach (Resources.AdvancedServoSequence sequence in advancedServoPerformance.AdvancedServoSequences)
            //{
            //    if (LogPerformanceStep)
            //    {
            //        Log.Trace($"Running sequence:{sequence.Name} Description:{sequence.Description} " +
            //            $"PlayInParallel:{sequence.PlayInParallel} ContinueWith:{sequence.ContinueWith}", Common.LOG_CATEGORY);
            //    }

            //    try
            //    {
            //        var advancedServo = OpenHost(sequence.Host);

            //        var nextSequence = sequence;

            //        string name = "";
            //        string? continueWith = "";

            //        do
            //        {
            //            name = nextSequence.Name;
            //            continueWith = nextSequence.ContinueWith;
            //            Log.Trace($"  Playing sequence:{name} continueWidth:{continueWith}", Common.LOG_CATEGORY);

            //            await PlaySequenceLoops(advancedServo, nextSequence);

            //            if (AvailableAdvancedServoSequences.ContainsKey(continueWith ?? ""))
            //            {
            //                nextSequence = AvailableAdvancedServoSequences[continueWith];
            //            }
            //            else
            //            {
            //                continueWith = "";
            //            }

            //        } while (!string.IsNullOrEmpty(continueWith));

            //        advancedServo.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error(ex, Common.LOG_CATEGORY);
            //    }
            //}
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
        public bool PlayPerformanceCanExecute(string playAll)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedPerformanceSequences?.Count > 0)
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

        public DelegateCommand<string> PlayAdvancedServoSequenceCommand { get; set; }
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
        public async void PlayAdvancedServoSequence(string playAll)
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlaySequence";

            if (Boolean.Parse(playAll))
            {
                // TODO(crhodes)
                // Figure out how to get Host

                //var advancedServo = OpenHost(sequence.Host);

                //foreach (Resources.AdvancedServoSequence sequence in AdvancedServoSequences)
                //{
                //    await PlaySequenceLoops(sequence);
                //}
            }
            else
            {
                var runAllThese = SelectedAdvancedServoSequences;
                var allSequences = AvailableAdvancedServoSequences;

                foreach (Resources.AdvancedServoSequence sequence in SelectedAdvancedServoSequences)
                {
                    Log.Trace($"Running sequence:{sequence.Name}", Common.LOG_CATEGORY);

                    try
                    {
                        var advancedServo = OpenAdvancedServoHost(sequence.Host);

                        var nextSequence = sequence;

                        string name = "";
                        string? continueWith = "";

                        do
                        {
                            name = nextSequence.Name;
                            continueWith = nextSequence.ContinueWith;
                            Log.Trace($"  Playing sequence:{name} continueWidth:{continueWith}", Common.LOG_CATEGORY);

                            await PlayAdvancedServoSequenceLoops(advancedServo, nextSequence);

                            if (AvailableAdvancedServoSequences.ContainsKey(continueWith ?? ""))
                            {
                                nextSequence = AvailableAdvancedServoSequences[continueWith];
                            }
                            else
                            {
                                continueWith = "";
                            }

                        } while (!string.IsNullOrEmpty(continueWith));
                        
                        advancedServo.Close();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, Common.LOG_CATEGORY);
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

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
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

        private async Task PlayAdvancedServoSequenceLoops(AdvancedServoEx advancedServo, Resources.AdvancedServoSequence advancedServoSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            for (int i = 0; i < advancedServoSequence.Loops; i++)
            {
                Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);

                if (advancedServoSequence.PlayInParallel)
                {
                    PlayAdvancedServoSequenceInParallel(advancedServo, advancedServoSequence);
                }
                else
                {
                    PlayAdvancedServoSequenceInSequence(advancedServo, advancedServoSequence);
                }
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async void PlayAdvancedServoSequenceInParallel(AdvancedServoEx advancedServo, Resources.AdvancedServoSequence advancedServoSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            Parallel.ForEach(advancedServoSequence.AdvancedServoServoActions, action =>
            {
                if (LogPerformanceStep)
                {
                    Log.Trace($"Servo:{action.ServoIndex} Acceleration:{action.Acceleration} VelocityLimit:{action.VelocityLimit}" +
                        $" Engaged:{action.Engaged} TargetPosition:{action.TargetPosition} Duration:{action.Duration}", Common.LOG_CATEGORY);
                }

                try
                {
                    switch (action.ServoIndex)
                    {
                        case 0:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[0], action, 0);
                            break;

                        case 1:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[1], action, 1);
                            break;

                        case 2:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[2], action, 2);
                            break;

                        case 3:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[3], action, 3);
                            break;

                        case 4:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[4], action, 4);
                            break;

                        case 5:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[5], action, 5);
                            break;

                        case 6:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[6], action, 6);
                            break;

                        case 7:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[7], action, 7);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            });

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void PlayAdvancedServoSequenceInSequence(AdvancedServoEx advancedServo, Resources.AdvancedServoSequence advancedServoSequence)
        {
            Int64 startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            foreach (Resources.AdvancedServoServoAction action in advancedServoSequence.AdvancedServoServoActions)
            {
                if (LogPerformanceStep)
                {
                    Log.Trace($"Servo:{action.ServoIndex} Acceleration:{action.Acceleration} VelocityLimit:{action.VelocityLimit}" +
                        $" Engaged:{action.Engaged} TargetPosition:{action.TargetPosition} Duration:{action.Duration}", Common.LOG_CATEGORY);
                }

                try
                {
                    switch (action.ServoIndex)
                    {
                        case 0:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[0], action, 0);
                            break;

                        case 1:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[1], action, 1);
                            break;

                        case 2:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[2], action, 2);
                            break;

                        case 3:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[3], action, 3);
                            break;

                        case 4:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[4], action, 4);
                            break;

                        case 5:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[5], action, 5);
                            break;

                        case 6:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[6], action, 6);
                            break;

                        case 7:
                            PerformAdvancedServoAction(advancedServo.AdvancedServo.servos[7], action, 7);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void PerformAdvancedServoAction(AdvancedServoServo servo, Resources.AdvancedServoServoAction action, Int32 index)
        {
            Int64 startTicks = Log.Trace($"Enter servo:{index}", Common.LOG_CATEGORY);

            try
            {
                if (action.Acceleration is not null) servo.Acceleration = (Double)action.Acceleration;
                if (action.VelocityLimit is not null) servo.VelocityLimit = (Double)action.VelocityLimit;
                if (action.PositionMin is not null) servo.PositionMin = (Double)action.PositionMin;
                if (action.PositionMax is not null) servo.PositionMax = (Double)action.PositionMax;
                if (action.Engaged is not null) servo.Engaged = (Boolean)action.Engaged;

                // TODO(crhodes)
                // Maybe wait for servo Engaged to complete if not currently engaged
                // View logs and see how often exceptions thrown.

                if (action?.Engaged ?? false)
                {
                    VerifyServoEngaged(servo);
                }             

                if (action.TargetPosition is not null)
                {
                    servo.Position = (Double)action.TargetPosition;
                    Thread.Sleep(1);

                    VerifyNewPositionAchieved(servo, (Double)action.TargetPosition);

                    if (action.Duration > 0) Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }
        private void VerifyServoEngaged(AdvancedServoServo servo)
        {
            while (servo.Engaged != true) { Thread.Sleep(1); }
        }

        private void VerifyNewPositionAchieved(AdvancedServoServo servo, double targetPosition)
        {
            while (servo.Position != targetPosition) { Thread.Sleep(1); }
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool PlayPerformanceCanExecute(TYPE value)
        public bool PlayAdvancedServoSequenceCanExecute(string playAll)
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

        public DelegateCommand<string> PlayInterfaceKitSequenceCommand { get; set; }
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
        public async void PlayInterfaceKitSequence(string playAll)
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlaySequence";

            if (Boolean.Parse(playAll))
            {
                // TODO(crhodes)
                // Figure out how to get Host

                //var advancedServo = OpenHost(sequence.Host);

                //foreach (Resources.InterfaceKitSequence sequence in InterfaceKitSequences)
                //{
                //    await PlaySequenceLoops(sequence);
                //}
            }
            else
            {
                var runAllThese = SelectedInterfaceKitSequences;
                var allSequences = AvailableInterfaceKitSequences;

                foreach (Resources.InterfaceKitSequence sequence in SelectedInterfaceKitSequences)
                {
                    Log.Trace($"Running sequence:{sequence.Name}", Common.LOG_CATEGORY);

                    try
                    {
                        var interfaceKit = OpenInterfaceKitHost(sequence.Host);

                        var nextSequence = sequence;

                        string name = "";
                        string? continueWith = "";

                        do
                        {
                            name = nextSequence.Name;
                            continueWith = nextSequence.ContinueWith;
                            Log.Trace($"  Playing sequence:{name} continueWidth:{continueWith}", Common.LOG_CATEGORY);

                            await PlayInterfaceKitSequenceLoops(interfaceKit, nextSequence);

                            if (AvailableInterfaceKitSequences.ContainsKey(continueWith ?? ""))
                            {
                                nextSequence = AvailableInterfaceKitSequences[continueWith];
                            }
                            else
                            {
                                continueWith = "";
                            }

                        } while (!string.IsNullOrEmpty(continueWith));

                        interfaceKit.Close();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, Common.LOG_CATEGORY);
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

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private InterfaceKitEx OpenInterfaceKitHost(Resources.Host host)
        {
            InterfaceKitEx interfaceKit;

            interfaceKit = new InterfaceKitEx(
                host.IPAddress,
                host.Port,
                host.InterfaceKits[0].SerialNumber,true);

            interfaceKit.Open();

            return interfaceKit;
        }

        private async Task PlayInterfaceKitSequenceLoops(InterfaceKitEx interfaceKit, Resources.InterfaceKitSequence interfaceKitSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            for (int i = 0; i < interfaceKitSequence.Loops; i++)
            {
                Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);

                if (interfaceKitSequence.PlayInParallel)
                {
                    PlayInterfaceKitSequenceInParallel(interfaceKit, interfaceKitSequence);
                }
                else
                {
                    PlayInterfaceKitSequenceInSequence(interfaceKit, interfaceKitSequence);
                }
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async void PlayInterfaceKitSequenceInParallel(InterfaceKitEx interfaceKit, Resources.InterfaceKitSequence interfaceKitSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            Parallel.ForEach(interfaceKitSequence.InterfaceKitActions, action =>
            {
                if (LogPerformanceStep)
                {
                    Log.Trace($"Servo:{action.ServoIndex} Acceleration:{action.Acceleration} VelocityLimit:{action.VelocityLimit}" +
                        $" Engaged:{action.Engaged} TargetPosition:{action.TargetPosition} Duration:{action.Duration}", Common.LOG_CATEGORY);
                }

                try
                {
                    //switch (action.ServoIndex)
                    //{
                    //    case 0:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[0], action, 0);
                    //        break;

                    //    case 1:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[1], action, 1);
                    //        break;

                    //    case 2:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[2], action, 2);
                    //        break;

                    //    case 3:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[3], action, 3);
                    //        break;

                    //    case 4:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[4], action, 4);
                    //        break;

                    //    case 5:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[5], action, 5);
                    //        break;

                    //    case 6:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[6], action, 6);
                    //        break;

                    //    case 7:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[7], action, 7);
                    //        break;
                    //}
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            });

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void PlayInterfaceKitSequenceInSequence(InterfaceKitEx interfaceKit, Resources.InterfaceKitSequence interfaceKitSequence)
        {
            Int64 startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            foreach (Resources.InterfaceKitAction action in interfaceKitSequence.InterfaceKitActions)
            {
                if (LogPerformanceStep)
                {
                    Log.Trace($"Servo:{action.ServoIndex} Acceleration:{action.Acceleration} VelocityLimit:{action.VelocityLimit}" +
                        $" Engaged:{action.Engaged} TargetPosition:{action.TargetPosition} Duration:{action.Duration}", Common.LOG_CATEGORY);
                }

                try
                {
                    //switch (action.ServoIndex)
                    //{
                    //    case 0:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[0], action, 0);
                    //        break;

                    //    case 1:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[1], action, 1);
                    //        break;

                    //    case 2:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[2], action, 2);
                    //        break;

                    //    case 3:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[3], action, 3);
                    //        break;

                    //    case 4:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[4], action, 4);
                    //        break;

                    //    case 5:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[5], action, 5);
                    //        break;

                    //    case 6:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[6], action, 6);
                    //        break;

                    //    case 7:
                    //        PerformInterfaceKitAction(interfaceKit.InterfaceKit.servos[7], action, 7);
                    //        break;
                    //}
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //private void PerformInterfaceKitAction(InterfaceKitServo servo, Resources.InterfaceKitAction action, Int32 index)
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

        //        if (action?.Engaged ?? false)
        //        {
        //            VerifyServoEngaged(servo);
        //        }

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

        //private void VerifyServoEngaged(InterfaceKitServo servo)
        //{
        //    while (servo.Engaged != true) { Thread.Sleep(1); }
        //}

        //private void VerifyNewPositionAchieved(InterfaceKitServo servo, double targetPosition)
        //{
        //    while (servo.Position != targetPosition) { Thread.Sleep(1); }
        //}

        // If using CommandParameter, figure out TYPE and fix above
        //public bool PlayPerformanceCanExecute(TYPE value)
        public bool PlayInterfaceKitSequenceCanExecute(string playAll)
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

        //private void OpenPhidget()
        //{
        //    //Net.ServerAdded += Net_ServerAdded;
        //    //ph22.Net.ServerRemoved += Net_ServerRemoved;

        //    ////ph22.Net.EnableServerDiscovery(ph22.ServerType.SBC);
        //    //ph22.Net.AddServer("phsbc11", "192.168.150.11", 5001, "", 0);
        //    //ph22.Net.AddServer("phsbc21", "192.168.150.21", 5001, "", 0);
        //    //ph22.Net.AddServer("phsbc22", "192.168.150.22", 5001, "", 0);
        //    //ph22.Net.AddServer("phsbc23", "192.168.150.23", 5001, "", 0);

        //    //// NOTE(crhodes)
        //    //// Passing null throws exception

        //    ////ph22.Net.AddServer("phsbc11", "192.168.150.11", 5001, null, 0);
        //    ////ph22.Net.AddServer("phsbc21", "192.168.150.21", 5001, null, 0);
        //    ////ph22.Net.AddServer("phsbc22", "192.168.150.22", 5001, null, 0);
        //    ////ph22.Net.AddServer("phsbc23", "192.168.150.23", 5001, null, 0);

        //    //ph22.Phidget phidget = new ph22.Phidget();

        //    //phidget.Attach += Phidget_Attach;
        //    //phidget.Detach += Phidget_Detach;

        //    //ph22.DigitalOutput digitalOutput;

        //    //digitalOutput = new ph22.DigitalOutput();

        //    //digitalOutput.Attach += DigitalOutput_Attach;
        //    //digitalOutput.Detach += DigitalOutput_Detach;

        //    //digitalOutput.Channel = 0;
        //    //digitalOutput.IsRemote = true;
        //    //digitalOutput.DeviceSerialNumber = sbc22SerialNumber;
        //    //digitalOutput.Open(5000);

        //    //digitalOutput.DutyCycle = 1;
        //    //digitalOutput.DutyCycle = 0;

        //    //phidget.IsHubPortDevice = true;

        //    //phidget.Channel = 0;
        //    //phidget.DeviceSerialNumber = sbc21SerialNumber;

        //    //phidget.Open();
        //}

        //private void DigitalOutput_Detach(object sender, ph22E.DetachEventArgs e)
        //{
        //    var a = e;
        //    var b = e.GetType();

        //}

        //private void DigitalOutput_Attach(object sender, ph22E.AttachEventArgs e)
        //{
        //    var a = e;
        //    var b = e.GetType();
        //}

        //private void Net_ServerRemoved(ph22E.NetServerRemovedEventArgs e)
        //{
        //    var a = e;
        //    var b = e.GetType();
        //}

        //private void Net_ServerAdded(ph22E.NetServerAddedEventArgs e)
        //{
        //    var a = e;
        //    var server = e.Server;
        //    var b = e.GetType();
        //}

        //private void Phidget_Detach(object sender, ph22E.DetachEventArgs e)
        //{
        //    var a = e;
        //    var b = e.GetType();
        //}

        //private void Phidget_Attach(object sender, ph22E.AttachEventArgs e)
        //{
        //    var a = e;
        //    var b = e.GetType();
        //}

        private void OpenPhidgetManager()
        {
            Manager phidgetManager = new Manager();

            phidgetManager.Attach += PhidgetManager_Attach;
            phidgetManager.Detach += PhidgetManager_Detach;

            phidgetManager.open();

        }

        private async Task OpenSBCInterfaceKit()
        {
            //InterfaceKit ifk11 = new InterfaceKit();

            //InterfaceKit ifk21 = new InterfaceKit();
            //InterfaceKit ifk22 = new InterfaceKit();
            //InterfaceKit ifk23 = new InterfaceKit();

            VNC.Phidget.InterfaceKitEx ifkEx11 = new VNC.Phidget.InterfaceKitEx("192.168.150.11", 5001, sbc11SerialNumber, embedded: true);
            VNC.Phidget.InterfaceKitEx ifkEx21 = new VNC.Phidget.InterfaceKitEx("192.168.150.21", 5001, sbc21SerialNumber, embedded: true);
            VNC.Phidget.InterfaceKitEx ifkEx22 = new VNC.Phidget.InterfaceKitEx("192.168.150.22", 5001, sbc22SerialNumber, embedded: true);
            VNC.Phidget.InterfaceKitEx ifkEx23 = new VNC.Phidget.InterfaceKitEx("192.168.150.23", 5001, sbc23SerialNumber, embedded: true);

            try
            {
                //await Task.Run(() =>
                //{
                //    Parallel.Invoke(
                //         () => InterfaceKitParty(sbc11SerialNumber, "192.168.150.11", 5001, 500, 5 * Repeats),
                //         () => InterfaceKitParty(sbc21SerialNumber, "192.168.150.21", 5001, 250, 10 * Repeats),
                //         () => InterfaceKitParty(sbc22SerialNumber, "192.168.150.22", 5001, 125, 20 * Repeats),
                //         () => InterfaceKitParty(sbc23SerialNumber, "192.168.150.23", 5001, 333, 8 * Repeats)
                //     );
                //});

                ifkEx11.Open();
                ifkEx21.Open();
                ifkEx22.Open();
                ifkEx23.Open();

                await Task.Run(() =>
                {
                    Parallel.Invoke(
                         () => InterfaceKitParty2(ifkEx21, 500, 5 * Repeats),
                         () => InterfaceKitParty2(ifkEx22, 250, 10 * Repeats),
                         () => InterfaceKitParty2(ifkEx23, 125, 20 * Repeats),
                         () => InterfaceKitParty2(ifkEx11, 333, 8 * Repeats)
                     );
                });

                ifkEx11.Close();
                ifkEx21.Close();
                ifkEx22.Close();
                ifkEx23.Close();

                //InterfaceKitParty(sbc21SerialNumber, "192.168.150.21", 5001, 250, 10);
                //InterfaceKitParty(ifk0, sbc21SerialNumber, "192.168.150.21", 5001, 250);
                //InterfaceKitParty(ifk1, sbc22SerialNumber, "192.168.150.22", 5001, 125);
                //InterfaceKitParty(ifk2, sbc23SerialNumber, "192.168.150.23", 5001, 333);
                //InterfaceKitParty(ifk1);
                //InterfaceKitParty(ifk2);

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

                //VNC.Phidget.InterfaceKitEx ifkEx = new VNC.Phidget.InterfaceKitEx(hostName, port, serialNumber, enable: true, embedded: true);

                //ifkEx.Open();

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

                InterfaceKitDigitalOutputCollection ifkDigitalOut = ifkEx.InterfaceKit.outputs;
                //InterfaceKitDigitalOutputCollection ifkDigitalOut = ifkEx.outputs;

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

            //OpenPhidgetManager();
            OpenSBCInterfaceKit();
            //OpenPhidget();
            //LightAction1();

            //ph22.DigitalOutput digitalOutput = new ph22.DigitalOutput();
            //digitalOutput.Open(5000);
            //digitalOutput.DutyCycle = 0;
            //Console.ReadLine();
            //digitalOutput.Close();
            //digitalOutput.Dispose();

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



        private void Button2Execute()
        {
            Int64 startTicks = Log.Debug("Enter", Common.LOG_CATEGORY);

            Message = "Button2 Clicked";

            //for (int i = 0; i < 10; i++)
            //{
            //    digitalOutput0.DutyCycle = 1;
            //    Thread.Sleep(125);
            //    digitalOutput0.DutyCycle = 0;
            //    Thread.Sleep(125);
            //    digitalOutput2.DutyCycle = 1;
            //    Thread.Sleep(250);
            //    digitalOutput2.DutyCycle = 0;
            //    Thread.Sleep(250);
            //}
            //ph22.DigitalOutput digitalOutput = new ph22.DigitalOutput();
            //digitalOutput.Open(5000);
            //digitalOutput.DutyCycle = 1;
            //Console.ReadLine();
            //digitalOutput.Close();
            //digitalOutput.Dispose();

            Log.Debug("End", Common.LOG_CATEGORY, startTicks);
        }

        private void Button3Execute()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            Message = "Button3 Clicked";

            try
            {
                method1();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
                Log.Error(ex, "BOOM");
            }

            Log.Trace("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method1()
        {
            Int64 startTicks = Log.Trace1("Enter", Common.LOG_CATEGORY);

            method2();

            Log.Trace1("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method2()
        {
            Int64 startTicks = Log.Trace2("Enter", Common.LOG_CATEGORY);

            method3();

            Log.Trace2("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method3()
        {
            Int64 startTicks = Log.Trace3("Enter", Common.LOG_CATEGORY);

            method4();

            Log.Trace3("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method4()
        {
            Int64 startTicks = Log.Trace4("Enter", Common.LOG_CATEGORY);

            method5();

            Log.Trace4("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method5()
        {
            Int64 startTicks = Log.Trace5("Enter", Common.LOG_CATEGORY);


            int answer = Numerator / Denominator;

            Answer = answer.ToString();

            Log.Trace5("End", Common.LOG_CATEGORY, startTicks);
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
