using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;

using VNCPhidget21.Configuration;

namespace VNC.Phidget
{
    public class PerformanceSequencePlayer
    {
        public IEventAggregator EventAggregator { get; set; }

        public PerformanceSequencePlayer(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public bool LogPerformanceSequence { get; set; }
        public bool LogPerformanceAction { get; set; }
        public bool LogActionVerification { get; set; }

        // AdvancedServo events

        public bool LogCurrentChangeEvents { get; set; }
        public bool LogPositionChangeEvents { get; set; }
        public bool LogVelocityChangeEvents { get; set; }

        // InterfaceKit events

        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }

        //public Dictionary<SerialHost, PhidgetDevice> AvailablePhidgets = new Dictionary<SerialHost, PhidgetDevice>();
        public Dictionary<Int32, PhidgetDevice> AvailablePhidgets = new Dictionary<Int32, PhidgetDevice>();

        public Dictionary<string, Performance> AvailablePerformances { get; set; }
        public Dictionary<string, AdvancedServoSequence> AvailableAdvancedServoSequences { get; set; }
        public Dictionary<string, InterfaceKitSequence> AvailableInterfaceKitSequences { get; set; }
        public Dictionary<string, StepperSequence> AvailableStepperSequences { get; set; }

        public AdvancedServoEx ActiveAdvancedServoHost { get; set; }
        public InterfaceKitEx ActiveInterfaceKitHost { get; set; }
        public StepperEx ActiveStepperHost { get; set; }

        public async Task ExecutePerformanceSequenceLoops(PerformanceSequence performanceSequence)
        {
            PerformanceSequence nextPerformanceSequence = null;

            try
            {
                if (LogPerformanceSequence)
                {
                    Log.Trace($"Executing performanceSequence:>{performanceSequence?.Name}< type:>{performanceSequence?.SequenceType}<" +
                        $" loops:>{performanceSequence?.Loops}< duration:>{performanceSequence?.Duration}<" +
                        $" closePhidget:>{performanceSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
                }

                for (int sequenceLoop = 0; sequenceLoop < performanceSequence.Loops; sequenceLoop++)
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
                    Thread.Sleep((Int32)performanceSequence.Duration);
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

            if (AvailableAdvancedServoSequences.ContainsKey(performanceSequence.Name ?? ""))
            {
                var advancedServoSequence = AvailableAdvancedServoSequences[performanceSequence.Name];

                if (advancedServoSequence.SerialNumber is not null)
                {
                    phidgetHost = GetAdvancedServoHost((Int32)advancedServoSequence.SerialNumber);
                }
                else if(ActiveAdvancedServoHost is not null)
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
                    await phidgetHost.RunSequenceLoops(advancedServoSequence);

                    if (advancedServoSequence.CallSequences is not null)
                    {
                        foreach (PerformanceSequence sequence in advancedServoSequence.CallSequences)
                        {
                            ExecutePerformanceSequenceLoops(sequence);
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
            PerformanceSequence nextPerformanceSequence = null;
            InterfaceKitEx phidgetHost = null;

            if (AvailableInterfaceKitSequences.ContainsKey(performanceSequence.Name ?? ""))
            {
                var interfaceKitSequence = AvailableInterfaceKitSequences[performanceSequence.Name];

                if (interfaceKitSequence.SerialNumber is not null)
                {
                    phidgetHost = GetInterfaceKitHost((Int32)interfaceKitSequence.SerialNumber);
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
                    await phidgetHost.RunSequenceLoops(interfaceKitSequence);

                    if (interfaceKitSequence.CallSequences is not null)
                    {
                        foreach (PerformanceSequence sequence in interfaceKitSequence.CallSequences)
                        {
                            ExecutePerformanceSequenceLoops(sequence);
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

            return nextPerformanceSequence;
        }

        private async Task<PerformanceSequence> ExecuteStepperPerformanceSequence(PerformanceSequence performanceSequence)
        {
            PerformanceSequence nextPerformanceSequence = null;
            StepperEx phidgetHost = null;

            if (AvailableStepperSequences.ContainsKey(performanceSequence.Name ?? ""))
            {
                var stepperSequence = AvailableStepperSequences[performanceSequence.Name];

                if (stepperSequence.SerialNumber is not null)
                {
                    phidgetHost = GetStepperHost((Int32)stepperSequence.SerialNumber);
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
                    await phidgetHost.RunSequenceLoops(stepperSequence);

                    if (stepperSequence.CallSequences is not null)
                    {
                        foreach (PerformanceSequence sequence in stepperSequence.CallSequences)
                        {
                            ExecutePerformanceSequenceLoops(sequence);
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

        private AdvancedServoEx GetAdvancedServoHost(Int32 serialNumber)
        {
            PhidgetDevice phidgetDevice = AvailablePhidgets[serialNumber];

            AdvancedServoEx advancedServoHost = null;

            if (phidgetDevice?.PhidgetEx is not null)
            {
                advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

                advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
                advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
                advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

                advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
                advancedServoHost.LogPerformanceAction = LogPerformanceAction;
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
                advancedServoHost.LogPerformanceAction = LogPerformanceAction;
                advancedServoHost.LogActionVerification = LogActionVerification;

                advancedServoHost.Open(Common.PhidgetOpenTimeout);
            }

            // NOTE(crhodes)
            // Save this so we can use it in other commands

            ActiveAdvancedServoHost = advancedServoHost;

            return advancedServoHost;
        }

        private InterfaceKitEx GetInterfaceKitHost(Int32 serialNumber)
        {
            PhidgetDevice phidgetDevice = AvailablePhidgets[serialNumber];

            InterfaceKitEx interfaceKitHost = null;

            if (phidgetDevice?.PhidgetEx is not null)
            {
                interfaceKitHost = (InterfaceKitEx)phidgetDevice.PhidgetEx;

                interfaceKitHost.LogInputChangeEvents = LogInputChangeEvents;
                interfaceKitHost.LogOutputChangeEvents = LogOutputChangeEvents;
                interfaceKitHost.LogSensorChangeEvents = LogSensorChangeEvents;

                interfaceKitHost.LogPerformanceAction = LogPerformanceAction;

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

                interfaceKitHost.LogPerformanceAction = LogPerformanceAction;

                // TODO(crhodes)
                // Should we do open somewhere else?

                interfaceKitHost.Open(Common.PhidgetOpenTimeout);
            }

            ActiveInterfaceKitHost = interfaceKitHost;

            return interfaceKitHost;
        }
        private StepperEx GetStepperHost(Int32 serialNumber)
        {
            PhidgetDevice phidgetDevice = AvailablePhidgets[serialNumber];

            StepperEx stepperHost = null;

            if (phidgetDevice?.PhidgetEx is not null)
            {
                stepperHost = (StepperEx)phidgetDevice.PhidgetEx;

                //stepperHost.LogInputChangeEvents = LogInputChangeEvents;
                //stepperHost.LogOutputChangeEvents = LogOutputChangeEvents;
                //stepperHost.LogSensorChangeEvents = LogSensorChangeEvents;

                //stepperHost.LogPerformanceAction = LogPerformanceAction;

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

                //stepperHost.LogPerformanceAction = LogPerformanceAction;

                // TODO(crhodes)
                // Should we do open somewhere else?

                stepperHost.Open(Common.PhidgetOpenTimeout);
            }

            ActiveStepperHost = stepperHost;

            return stepperHost;
        }

    }
}
