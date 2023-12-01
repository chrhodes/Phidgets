﻿using System;
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

        public Dictionary<SerialHost, PhidgetDevice> AvailablePhidgets = new Dictionary<SerialHost, PhidgetDevice>();

        public Dictionary<string, Performance> AvailablePerformances { get; set; }
        public Dictionary<string, AdvancedServoSequence> AvailableAdvancedServoSequences { get; set; }
        public Dictionary<string, InterfaceKitSequence> AvailableInterfaceKitSequences { get; set; }
        public Dictionary<string, StepperSequence> AvailableStepperSequences { get; set; }

        public AdvancedServoEx ActiveAdvancedServoHost { get; set; }

        public async Task<PerformanceSequence> ExecutePerformanceSequenceLoops(PerformanceSequence performanceSequence)
        {
            var nextPerformanceSequence = performanceSequence;

            try
            {
                if (LogPerformanceSequence)
                {
                    Log.Trace($"Executing performanceSequence:>{performanceSequence?.Name}< type:>{performanceSequence?.SequenceType}<" +
                        $" loops:>{performanceSequence?.Loops}< duration:>{performanceSequence?.Duration}<" +
                        $" closePhidget:>{performanceSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
                }

                // TODO(crhodes)
                // Think about Open/Close more.  Maybe config.
                // What happens if nextSequence.Host is null    

                if (performanceSequence is not null)
                {
                    switch (performanceSequence.SequenceType)
                    {
                        case "AS":
                            nextPerformanceSequence = await ExecuteAdvancedServoPerformanceSequence(performanceSequence);

                            break;

                        case "IK":
                            nextPerformanceSequence = await ExecuteInterfaceKitPerformanceSequence(performanceSequence);

                            break;

                        case "ST":
                            nextPerformanceSequence = await ExecuteStepperPerformanceSequence(performanceSequence);

                            break;

                        default:
                            Log.Error($"Unsupported SequenceType:>{nextPerformanceSequence.SequenceType}<", Common.LOG_CATEGORY);
                            nextPerformanceSequence = null;
                            break;
                    }
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
            // I don't think this ever returns anything but nuyll

            return nextPerformanceSequence;
        }

        private async Task<PerformanceSequence> ExecuteAdvancedServoPerformanceSequence(PerformanceSequence performanceSequence)
        {
            PerformanceSequence nextPerformanceSequence;

            if (AvailableAdvancedServoSequences.ContainsKey(performanceSequence.Name ?? ""))
            {
                var advancedServoSequence = AvailableAdvancedServoSequences[performanceSequence.Name];

                if (advancedServoSequence.Host is not null)
                {
                    var advancedServoHost = OpenAdvancedServoHost(advancedServoSequence.Host);

                    await advancedServoHost.RunSequenceLoops(advancedServoSequence);

                    nextPerformanceSequence = advancedServoSequence.NextSequence;

                    // NOTE(crhodes)
                    // This should handle continuations without a Host.  
                    // TODO(crhodes)
                    // Do we need to handle continuations that have a Host?  I think so.
                    // Play AS sequence on one Host and then a different AS sequence on a different host.
                    // This would be dialog back and forth across hosts.

                    while (nextPerformanceSequence is not null)
                    {
                        if (LogPerformanceSequence)
                        {
                            Log.Trace($"Executing sequence:>{nextPerformanceSequence?.Name}< type:>{nextPerformanceSequence?.SequenceType}<" +
                                $" loops:>{nextPerformanceSequence?.Loops}< closePhidget:>{nextPerformanceSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
                        }

                        if (nextPerformanceSequence.SequenceType == "AS")
                        {
                            if (AvailableAdvancedServoSequences.ContainsKey(nextPerformanceSequence.Name ?? ""))
                            {
                                advancedServoSequence = AvailableAdvancedServoSequences[nextPerformanceSequence.Name];

                                await advancedServoHost.RunSequenceLoops(advancedServoSequence);

                                nextPerformanceSequence = advancedServoSequence.NextSequence;
                            }
                            else
                            {
                                Log.Error($"Cannot find performanceSequence:>{nextPerformanceSequence.Name}<", Common.LOG_CATEGORY);
                                nextPerformanceSequence = null;
                            }
                        }
                        else
                        {
                            // TODO(crhodes)
                            // 
                            // Seems like this can just recursively call
                            //ExecutePerformanceSequenceLoops(nextPerformanceSequence);
                        }

                    }

                    if (performanceSequence.ClosePhidget)
                    {
                        //advancedServoHost.LogCurrentChangeEvents = false;
                        //advancedServoHost.LogPositionChangeEvents = false;
                        //advancedServoHost.LogVelocityChangeEvents = false;

                        //advancedServoHost.LogPerformanceStep = false;

                        //advancedServoHost.Close();
                    }
                }
                else
                {
                    Log.Error($"Host is null", Common.LOG_CATEGORY);
                    nextPerformanceSequence = null;
                }
            }
            else
            {
                Log.Error($"Cannot find performanceSequence:>{performanceSequence.Name}<", Common.LOG_CATEGORY);
                nextPerformanceSequence = null;
            }

            // TODO(crhodes)
            // Why do we need to return this.  It will be null

            return nextPerformanceSequence;
        }



        private async Task<PerformanceSequence> ExecuteInterfaceKitPerformanceSequence(PerformanceSequence performanceSequence)
        {
            PerformanceSequence? nextPerformanceSequence;

            if (AvailableInterfaceKitSequences.ContainsKey(performanceSequence.Name ?? ""))
            {
                var interfaceKitSequence = AvailableInterfaceKitSequences[performanceSequence.Name];

                if (interfaceKitSequence.Host is not null)
                {
                    var interfaceKitHost = OpenInterfaceKitHost(interfaceKitSequence.Host);

                    await interfaceKitHost.RunSequenceLoops(interfaceKitSequence);

                    nextPerformanceSequence = interfaceKitSequence.NextSequence;

                    // NOTE(crhodes)
                    // This should handle continuations without a Host.  
                    // TODO(crhodes)
                    // Do we need to handle continuations that have a Host?  I think so.
                    // Play AS sequence on one Host and then a different AS sequence on a different host.
                    // This would be dialog back and forth across hosts.

                    while (nextPerformanceSequence is not null && nextPerformanceSequence.SequenceType == "AS")
                    {
                        if (LogPerformanceSequence)
                        {
                            Log.Trace($"Executing sequence:>{nextPerformanceSequence?.Name}< type:>{nextPerformanceSequence?.SequenceType}<" +
                                $" loops:>{nextPerformanceSequence?.Loops}< closePhidget:>{nextPerformanceSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
                        }

                        interfaceKitSequence = AvailableInterfaceKitSequences[nextPerformanceSequence.Name];

                        await interfaceKitHost.RunSequenceLoops(interfaceKitSequence);

                        nextPerformanceSequence = interfaceKitSequence.NextSequence;
                    }

                    if (performanceSequence.ClosePhidget)
                    {
                        interfaceKitHost.Close();
                    }
                }
                else
                {
                    Log.Trace($"Host is null", Common.LOG_CATEGORY);
                    nextPerformanceSequence = null;
                }
            }
            else
            {
                Log.Trace($"Cannot find sequence:{performanceSequence.Name}", Common.LOG_CATEGORY);
                nextPerformanceSequence = null;
            }

            // TODO(crhodes)
            // Why do we need to return this.  It will be null

            return nextPerformanceSequence;
        }

        private async Task<PerformanceSequence> ExecuteStepperPerformanceSequence(PerformanceSequence performanceSequence)
        {
            PerformanceSequence nextPerformanceSequence = null;

            if (AvailableStepperSequences.ContainsKey(performanceSequence.Name ?? ""))
            {
                var stepperSequence = AvailableStepperSequences[performanceSequence.Name];
            }

            // TODO(crhodes)
            // Why do we need to return this.  It will be null

            return nextPerformanceSequence;
        }

        private AdvancedServoEx OpenAdvancedServoHost(VNCPhidget21.Configuration.Host host)
        {
            SerialHost serialHost = new SerialHost { IPAddress = host.IPAddress, SerialNumber = host.AdvancedServos[0].SerialNumber };

            PhidgetDevice phidgetDevice = AvailablePhidgets[serialHost];

            AdvancedServoEx advancedServoHost;

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
                    host.IPAddress,
                    host.Port,
                    host.AdvancedServos[0].SerialNumber,
                    EventAggregator);

                advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

                advancedServoHost = phidgetDevice.PhidgetEx as AdvancedServoEx;

                advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
                advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
                advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

                advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
                advancedServoHost.LogPerformanceAction = LogPerformanceAction;
                advancedServoHost.LogActionVerification = LogActionVerification;

                advancedServoHost.Open();
            }

            //advancedServoHost = new AdvancedServoEx(
            //    host.IPAddress,
            //    host.Port,
            //    host.AdvancedServos[0].SerialNumber,
            //    EventAggregator);

            //advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

            //advancedServoHost = phidgetDevice.PhidgetEx as AdvancedServoEx;

            //advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
            //advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //advancedServoHost.LogPerformanceStep = LogPerformanceStep;

            //advancedServoHost.Open();

            // NOTE(crhodes)
            // Save this so we can use it in other commands

            ActiveAdvancedServoHost = advancedServoHost;

            return advancedServoHost;
        }

        private InterfaceKitEx OpenInterfaceKitHost(VNCPhidget21.Configuration.Host host)
        {
            InterfaceKitEx interfaceKitHost;

            interfaceKitHost = new InterfaceKitEx(
                host.IPAddress,
                host.Port,
                host.InterfaceKits[0].SerialNumber, true,
                EventAggregator);

            interfaceKitHost.LogInputChangeEvents = LogInputChangeEvents;
            interfaceKitHost.LogOutputChangeEvents = LogOutputChangeEvents;
            interfaceKitHost.LogSensorChangeEvents = LogSensorChangeEvents;

            interfaceKitHost.LogPerformanceAction = LogPerformanceAction;

            interfaceKitHost.Open();

            return interfaceKitHost;
        }

    }
}