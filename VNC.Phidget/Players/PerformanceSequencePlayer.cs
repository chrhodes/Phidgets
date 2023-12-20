using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;

using VNCPhidget21.Configuration;

namespace VNC.Phidget.Players
{
    // TODO(crhodes)
    // Figure out how to make this singleton
    // Mabe as simple as creating one if needed 
    // in the get of ActivePerformanceSequencePlayer

    public class PerformanceSequencePlayer
    {
        #region Constructors, Initialization, and Load

        public IEventAggregator EventAggregator { get; set; }

        public PerformanceSequencePlayer(IEventAggregator eventAggregator)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;

            ActivePerformanceSequencePlayer = this;

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties (None)

        public static PerformanceSequencePlayer ActivePerformanceSequencePlayer { get; set; }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }
        public bool LogActionVerification { get; set; }

        // AdvancedServo events

        public bool LogCurrentChangeEvents { get; set; }
        public bool LogPositionChangeEvents { get; set; }
        public bool LogVelocityChangeEvents { get; set; }

        // InterfaceKit events

        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }

        //public Dictionary<int, PhidgetDevice> AvailablePhidgets = 
        //    new Dictionary<int, PhidgetDevice>();

        //public Dictionary<string, AdvancedServoSequence> AvailableAdvancedServoSequences { get; set; } =
        //    new Dictionary<string, AdvancedServoSequence>();

        //public Dictionary<string, InterfaceKitSequence> AvailableInterfaceKitSequences { get; set; } = 
        //    new Dictionary<string, InterfaceKitSequence>();

        //public Dictionary<string, StepperSequence> AvailableStepperSequences { get; set; } = 
        //    new Dictionary<string, StepperSequence>();

        public AdvancedServoEx ActiveAdvancedServoHost { get; set; }
        public InterfaceKitEx ActiveInterfaceKitHost { get; set; }
        public StepperEx ActiveStepperHost { get; set; }

        #endregion

        #region Event Handlers (None)


        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        public async Task ExecutePerformanceSequence(PerformanceSequence performanceSequence)
        {
            PerformanceSequence nextPerformanceSequence = null;

            try
            {
                if (LogPerformanceSequence)
                {
                    Log.Trace($"Executing performanceSequence:>{performanceSequence?.Name}< type:>{performanceSequence?.SequenceType}<" +
                        $" loops:>{performanceSequence?.SequenceLoops}< duration:>{performanceSequence?.Duration}<" +
                        $" closePhidget:>{performanceSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
                }

                for (int sequenceLoop = 0; sequenceLoop < performanceSequence.SequenceLoops; sequenceLoop++)
                {
                    if (LogPerformanceSequence) Log.Trace($"Running PerformanceSequence Loop:{sequenceLoop + 1}", Common.LOG_CATEGORY);
                    // TODO(crhodes)
                    // Think about Open/Close more.  Maybe config.
                    // What happens if nextSequence.Host is null    

                    // NOTE(crhodes)
                    // Each loop start back at the initial sequence
                    nextPerformanceSequence = performanceSequence;

                    do
                    {
                        switch (nextPerformanceSequence.SequenceType)
                        {
                            case "AS":
                                nextPerformanceSequence = await ExecuteAdvancedServoPerformanceSequence(nextPerformanceSequence);

                                break;

                            case "IK":
                                nextPerformanceSequence = await ExecuteInterfaceKitPerformanceSequence(nextPerformanceSequence);

                                break;

                            case "ST":
                                nextPerformanceSequence = await ExecuteStepperPerformanceSequence(nextPerformanceSequence);

                                break;

                            default:
                                Log.Error($"Unsupported SequenceType:>{nextPerformanceSequence.SequenceType}<", Common.LOG_CATEGORY);
                                nextPerformanceSequence = null;
                                break;
                        }
                    } while (nextPerformanceSequence is not null);
                }

                if (performanceSequence.Duration is not null)
                {
                    if (LogPerformanceSequence)
                    {
                        Log.Trace($"Zzzzz Sleeping:>{performanceSequence.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((int)performanceSequence.Duration);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            // NOTE(crhodes)
            // I don't think this ever returns anything but null

            //return nextPerformanceSequence;
        }

        private async Task<PerformanceSequence> ExecuteAdvancedServoPerformanceSequence(PerformanceSequence performanceSequence)
        {
            PerformanceSequence nextPerformanceSequence = null;
            AdvancedServoEx phidgetHost = null;

            if (PerformanceLibrary.AvailableAdvancedServoSequences.ContainsKey(performanceSequence.Name ?? ""))
            {
                var advancedServoSequence = PerformanceLibrary.AvailableAdvancedServoSequences[performanceSequence.Name];

                if (advancedServoSequence.SerialNumber is not null)
                {
                    phidgetHost = GetAdvancedServoHost((int)advancedServoSequence.SerialNumber);
                }
                else if (ActiveAdvancedServoHost is not null)
                {
                    phidgetHost = ActiveAdvancedServoHost;
                }
                else
                {
                    Log.Error($"Cannot locate host to execute SerialNumber:{advancedServoSequence.SerialNumber} is null", Common.LOG_CATEGORY);
                    nextPerformanceSequence = null;
                }

                if (phidgetHost is not null)
                {
                    if (advancedServoSequence.BeforeActionLoopSequences is not null)
                    {
                        foreach (PerformanceSequence sequence in advancedServoSequence.BeforeActionLoopSequences)
                        {
                            ExecutePerformanceSequence(sequence);
                        }
                    }

                    await phidgetHost.RunActionLoops(advancedServoSequence);

                    if (advancedServoSequence.AfterActionLoopSequences is not null)
                    {
                        foreach (PerformanceSequence sequence in advancedServoSequence.AfterActionLoopSequences)
                        {
                            ExecutePerformanceSequence(sequence);
                        }
                    }

                    nextPerformanceSequence = advancedServoSequence.NextSequence;
                }
            }
            else
            {
                Log.Error($"Cannot find performanceSequence:>{performanceSequence.Name}<", Common.LOG_CATEGORY);
                nextPerformanceSequence = null;
            }

            return nextPerformanceSequence;
        }

        private async Task<PerformanceSequence> ExecuteInterfaceKitPerformanceSequence(PerformanceSequence performanceSequence)
        {
            Int64 startTicks = 0;
            PerformanceSequence nextPerformanceSequence = null;

            try
            {
                InterfaceKitEx phidgetHost = null;

                if (PerformanceLibrary.AvailableInterfaceKitSequences.ContainsKey(performanceSequence.Name ?? ""))
                {
                    var interfaceKitSequence = PerformanceLibrary.AvailableInterfaceKitSequences[performanceSequence.Name];

                    if (interfaceKitSequence.SerialNumber is not null)
                    {
                        phidgetHost = GetInterfaceKitHost((int)interfaceKitSequence.SerialNumber);
                    }
                    else if (ActiveInterfaceKitHost is not null)
                    {
                        phidgetHost = ActiveInterfaceKitHost;
                    }
                    else
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{interfaceKitSequence.SerialNumber} is null", Common.LOG_CATEGORY);
                        nextPerformanceSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (interfaceKitSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (PerformanceSequence sequence in interfaceKitSequence.BeforeActionLoopSequences)
                            {
                                ExecutePerformanceSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(interfaceKitSequence);

                        if (interfaceKitSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (PerformanceSequence sequence in interfaceKitSequence.AfterActionLoopSequences)
                            {
                                ExecutePerformanceSequence(sequence);
                            }
                        }

                        nextPerformanceSequence = interfaceKitSequence.NextSequence;
                    }
                }
                else
                {
                    Log.Trace($"Cannot find performanceSequence:{performanceSequence.Name}", Common.LOG_CATEGORY);
                    nextPerformanceSequence = null;
                }
            }
            catch (Exception ex)
            {
                var name = performanceSequence.Name;
                var huh = PerformanceLibrary.AvailableInterfaceKitSequences;
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return nextPerformanceSequence;
        }

        private async Task<PerformanceSequence> ExecuteStepperPerformanceSequence(PerformanceSequence performanceSequence)
        {
            PerformanceSequence nextPerformanceSequence = null;
            StepperEx phidgetHost = null;

            if (PerformanceLibrary.AvailableStepperSequences.ContainsKey(performanceSequence.Name ?? ""))
            {
                var stepperSequence = PerformanceLibrary.AvailableStepperSequences[performanceSequence.Name];

                if (stepperSequence.SerialNumber is not null)
                {
                    phidgetHost = GetStepperHost((int)stepperSequence.SerialNumber);
                }
                else if (ActiveInterfaceKitHost is not null)
                {
                    phidgetHost = ActiveStepperHost;
                }
                else
                {
                    Log.Error($"Cannot locate host to execute SerialNumber:{stepperSequence.SerialNumber} is null", Common.LOG_CATEGORY);
                    nextPerformanceSequence = null;
                }

                if (phidgetHost is not null)
                {
                    if (stepperSequence.BeforeActionLoopSequences is not null)
                    {
                        foreach (PerformanceSequence sequence in stepperSequence.BeforeActionLoopSequences)
                        {
                            ExecutePerformanceSequence(sequence);
                        }
                    }

                    await phidgetHost.RunActionLoops(stepperSequence);

                    if (stepperSequence.AfterActionLoopSequences is not null)
                    {
                        foreach (PerformanceSequence sequence in stepperSequence.AfterActionLoopSequences)
                        {
                            ExecutePerformanceSequence(sequence);
                        }
                    }

                    nextPerformanceSequence = stepperSequence.NextSequence;
                }
            }
            else
            {
                Log.Trace($"Cannot find performanceSequence:{performanceSequence.Name}", Common.LOG_CATEGORY);
                nextPerformanceSequence = null;
            }

            return nextPerformanceSequence;
        }

        private AdvancedServoEx GetAdvancedServoHost(int serialNumber)
        {
            PhidgetDevice phidgetDevice = PhidgetLibrary.AvailablePhidgets[serialNumber];

            AdvancedServoEx advancedServoHost = null;

            if (phidgetDevice?.PhidgetEx is not null)
            {
                advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

                advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
                advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
                advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

                advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
                advancedServoHost.LogSequenceAction = LogSequenceAction;
                advancedServoHost.LogActionVerification = LogActionVerification;
            }
            else
            {
                phidgetDevice.PhidgetEx = new AdvancedServoEx(
                    phidgetDevice.IPAddress,
                    phidgetDevice.Port,
                    serialNumber,
                    EventAggregator);

                advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

                advancedServoHost = phidgetDevice.PhidgetEx as AdvancedServoEx;

                advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
                advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
                advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

                advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
                advancedServoHost.LogSequenceAction = LogSequenceAction;
                advancedServoHost.LogActionVerification = LogActionVerification;

                advancedServoHost.Open(Common.PhidgetOpenTimeout);
            }

            // NOTE(crhodes)
            // Save this so we can use it in other commands

            ActiveAdvancedServoHost = advancedServoHost;

            return advancedServoHost;
        }

        private InterfaceKitEx GetInterfaceKitHost(int serialNumber)
        {
            InterfaceKitEx interfaceKitHost = null;

            PhidgetDevice phidgetDevice = PhidgetLibrary.AvailablePhidgets[serialNumber];

            if (phidgetDevice?.PhidgetEx is not null)
            {
                interfaceKitHost = (InterfaceKitEx)phidgetDevice.PhidgetEx;

                interfaceKitHost.LogInputChangeEvents = LogInputChangeEvents;
                interfaceKitHost.LogOutputChangeEvents = LogOutputChangeEvents;
                interfaceKitHost.LogSensorChangeEvents = LogSensorChangeEvents;

                interfaceKitHost.LogPerformanceSequence = LogPerformanceSequence;
                interfaceKitHost.LogSequenceAction = LogSequenceAction;
            }
            else
            {
                phidgetDevice.PhidgetEx = new InterfaceKitEx(
                    phidgetDevice.IPAddress,
                    phidgetDevice.Port,
                    serialNumber,
                    true,
                    EventAggregator);

                interfaceKitHost = (InterfaceKitEx)phidgetDevice.PhidgetEx;

                interfaceKitHost.LogInputChangeEvents = LogInputChangeEvents;
                interfaceKitHost.LogOutputChangeEvents = LogOutputChangeEvents;
                interfaceKitHost.LogSensorChangeEvents = LogSensorChangeEvents;

                interfaceKitHost.LogPerformanceSequence = LogPerformanceSequence;
                interfaceKitHost.LogSequenceAction = LogSequenceAction;

                // TODO(crhodes)
                // Should we do open somewhere else?

                interfaceKitHost.Open(Common.PhidgetOpenTimeout);
            }

            ActiveInterfaceKitHost = interfaceKitHost;

            return interfaceKitHost;
        }
        private StepperEx GetStepperHost(int serialNumber)
        {
            StepperEx stepperHost = null;

            PhidgetDevice phidgetDevice = PhidgetLibrary.AvailablePhidgets[serialNumber];

            if (phidgetDevice?.PhidgetEx is not null)
            {
                stepperHost = (StepperEx)phidgetDevice.PhidgetEx;

                //stepperHost.LogInputChangeEvents = LogInputChangeEvents;
                //stepperHost.LogOutputChangeEvents = LogOutputChangeEvents;
                //stepperHost.LogSensorChangeEvents = LogSensorChangeEvents;

                //stepperHost.LogSequenceAction = LogSequenceAction;

            }
            else
            {
                phidgetDevice.PhidgetEx = new StepperEx(
                    phidgetDevice.IPAddress,
                    phidgetDevice.Port,
                    serialNumber,
                    EventAggregator);

                stepperHost = (StepperEx)phidgetDevice.PhidgetEx;

                //stepperHost.LogInputChangeEvents = LogInputChangeEvents;
                //stepperHost.LogOutputChangeEvents = LogOutputChangeEvents;
                //stepperHost.LogSensorChangeEvents = LogSensorChangeEvents;

                //stepperHost.LogSequenceAction = LogSequenceAction;

                // TODO(crhodes)
                // Should we do open somewhere else?

                stepperHost.Open(Common.PhidgetOpenTimeout);
            }

            ActiveStepperHost = stepperHost;

            return stepperHost;
        }

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)

        //private JsonSerializerOptions GetJsonSerializerOptions()
        //{
        //    var jsonOptions = new JsonSerializerOptions
        //    {
        //        ReadCommentHandling = JsonCommentHandling.Skip,
        //        AllowTrailingCommas = true
        //    };

        //    return jsonOptions;
        //}

        //private IEnumerable<string> GetListOfAdvancedServoConfigFiles()
        //{
        //    // TODO(crhodes)
        //    // Read a directory and return files, perhaps with RegEx name match

        //    List<string> files = new List<string>
        //    {
        //        @"AdvancedServoSequences\AdvancedServoSequenceConfig_99415.json",
        //        @"AdvancedServoSequences\AdvancedServoSequenceConfig_Test.json",
        //        @"AdvancedServoSequences\AdvancedServoSequenceConfig_99220_Skulls.json",
        //    };

        //    return files;
        //}

        //private IEnumerable<string> GetListOfInterfaceKitConfigFiles()
        //{
        //    // TODO(crhodes)
        //    // Read a directory and return files, perhaps with RegEx name match

        //    List<string> files = new List<string>
        //    {
        //        @"InterfaceKitSequences\InterfaceKitSequenceConfig_46049.json",
        //        @"InterfaceKitSequences\InterfaceKitSequenceConfig_48284.json",
        //        @"InterfaceKitSequences\InterfaceKitSequenceConfig_48301.json",
        //        @"InterfaceKitSequences\InterfaceKitSequenceConfig_124744.json",
        //        @"InterfaceKitSequences\InterfaceKitSequenceConfig_251831.json"
        //    };

        //    return files;
        //}

        //private IEnumerable<string> GetListOfStepperConfigFiles()
        //{
        //    // TODO(crhodes)
        //    // Read a directory and return files, perhaps with RegEx name match

        //    List<string> files = new List<string>
        //    {
        //        @"localhost\localhost_StepperSequenceConfig_1.json",
        //    };

        //    return files;
        //}

        #endregion

    }
}
