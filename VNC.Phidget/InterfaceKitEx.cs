using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Phidgets;
using Phidgets.Events;

using Prism.Events;

using VNC.Phidget.Events;

using VNCPhidget21.Configuration;

namespace VNC.Phidget
{
    public class InterfaceKitEx : PhidgetEx // InterfaceKit
    {
        #region Constructors, Initialization, and Load

        public readonly IEventAggregator EventAggregator;

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public InterfaceKitEx(string ipAddress, int port, int serialNumber, bool embedded, IEventAggregator eventAggregator) 
            : base(ipAddress, port, serialNumber)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;
            InitializePhidget();

            EventAggregator.GetEvent<InterfaceKitSequenceEvent>().Subscribe(TriggerSequence);

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void TriggerSequence(SequenceEventArgs args)
        {
            Log.EVENT_HANDLER("Called", Common.LOG_CATEGORY);
        }

        private void InitializePhidget()
        {
            InterfaceKit = new Phidgets.InterfaceKit();

            InterfaceKit.Attach += Phidget_Attach;
            InterfaceKit.Detach += Phidget_Detach;
            InterfaceKit.Error += Phidget_Error;
            InterfaceKit.ServerConnect += Phidget_ServerConnect;
            InterfaceKit.ServerDisconnect += Phidget_ServerDisconnect;
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Phidgets.InterfaceKit InterfaceKit = null;

        private bool _logInputChangeEvents;
        public bool LogInputChangeEvents 
        { 
            get => _logInputChangeEvents; 
            set
            {
                if (_logInputChangeEvents == value) return;

                if (_logInputChangeEvents = value)
                {
                    InterfaceKit.InputChange += InterfaceKitk_InputChange;
                }
                else
                {
                    InterfaceKit.InputChange -= InterfaceKitk_InputChange;
                }
            }
        }

        private bool _logOutputChangeEvents;
        public bool LogOutputChangeEvents 
        { 
            get => _logOutputChangeEvents;
            set
            {
                if (_logOutputChangeEvents == value) return;

                if (_logOutputChangeEvents = value)
                {
                    InterfaceKit.OutputChange += InterfaceKitk_OutputChange;
                }
                else
                {
                    InterfaceKit.OutputChange -= InterfaceKitk_OutputChange;
                }
            }
        }

        private bool _logSensorChangeEvents;
        public bool LogSensorChangeEvents 
        {
            get => _logSensorChangeEvents;
            set
            {
                if (_logSensorChangeEvents == value) return;               

                if (_logSensorChangeEvents = value)
                {
                    InterfaceKit.SensorChange += InterfaceKitk_SensorChange;
                }
                else
                {
                    InterfaceKit.SensorChange -= InterfaceKitk_SensorChange;
                }
            }
        }

        public bool LogPerformanceSequence { get; set; }
        public bool LogPerformanceAction { get; set; }

        #endregion

        #region Event Handlers

        private void InterfaceKitk_SensorChange(object sender, SensorChangeEventArgs e)
        {
            try
            {
                Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                Log.EVENT_HANDLER($"SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void InterfaceKitk_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        {
            try
            {
                Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                Log.EVENT_HANDLER($"OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void InterfaceKitk_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        {
            try
            {
                Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                Log.EVENT_HANDLER($"InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        /// <summary>
        /// Open Phidget and waitForAttachment
        /// </summary>
        /// <param name="timeOut">Optionally time out after timeOut(ms)</param>
        public new void Open(Int32? timeOut = null)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                InterfaceKit.open(SerialNumber, Host.IPAddress, Host.Port);

                if (timeOut is not null)
                { 
                    InterfaceKit.waitForAttachment((Int32)timeOut); 
                }
                else 
                { 
                    InterfaceKit.waitForAttachment(); 
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void Close()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                // NOTE(crhodes)
                // We may be logging events.  Remove them before closing.

                if (LogInputChangeEvents) LogInputChangeEvents = false;
                if (LogOutputChangeEvents) LogOutputChangeEvents = false;
                if (LogSensorChangeEvents) LogSensorChangeEvents = false;

                this.InterfaceKit.close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunSequenceLoops(InterfaceKitSequence interfaceKitSequence)
        {
            try
            {
                Int64 startTicks = 0;

                if (LogPerformanceSequence) Log.Trace("Enter", Common.LOG_CATEGORY);


                if (interfaceKitSequence.Actions is not null)
                {
                    for (int sequenceLoop = 0; sequenceLoop < interfaceKitSequence.Loops; sequenceLoop++)
                    {
                        Log.Trace($"Loop:{sequenceLoop + 1}", Common.LOG_CATEGORY);

                        if (interfaceKitSequence.PlayActionsInParallel)
                        {
                            if (LogPerformanceSequence) Log.Trace($"Parallel Actions Loop:{sequenceLoop + 1}", Common.LOG_CATEGORY);

                            await PlaySequenceActionsInParallel(interfaceKitSequence);
                        }
                        else
                        {
                            if (LogPerformanceSequence) Log.Trace($"Sequential Actions Loop:{sequenceLoop + 1}", Common.LOG_CATEGORY);

                            await PlaySequenceActionsInSequence(interfaceKitSequence);
                        }
                    }
                }

                if (LogPerformanceSequence) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)

        // TODO(crhodes)
        // Create Array of DigitalInputs, DigitalOutputs, SensorInputs
        // Follow something like ServoMinMax and InitialServoLimits in AdvancedServoEX

        private async Task PlaySequenceActionsInParallel(InterfaceKitSequence interfaceKitSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Maybe just pass the interfaceKit into Action and get this there

            InterfaceKitDigitalOutputCollection ifkDigitalOutputs = InterfaceKit.outputs;

            Parallel.ForEach(interfaceKitSequence.Actions, action =>
            {
                if (LogPerformanceSequence)
                {
                    Log.Trace($"DigitalOut Index:{action.DigitalOutIndex} DigitalOut:{action.DigitalOut} Duration:{action.Duration}", Common.LOG_CATEGORY);
                }

                try
                {
                    switch (action.DigitalOutIndex)
                    {
                        case 0:
                            PerformAction(ifkDigitalOutputs, action, 0);
                            break;

                        case 1:
                            PerformAction(ifkDigitalOutputs, action, 1);
                            break;

                        case 2:
                            PerformAction(ifkDigitalOutputs, action, 2);
                            break;

                        case 3:
                            PerformAction(ifkDigitalOutputs, action, 30);
                            break;

                        case 4:
                            PerformAction(ifkDigitalOutputs, action, 4);
                            break;

                        case 5:
                            PerformAction(ifkDigitalOutputs, action, 50);
                            break;

                        case 6:
                            PerformAction(ifkDigitalOutputs, action, 60);
                            break;

                        case 7:
                            PerformAction(ifkDigitalOutputs, action, 7);
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

        private async Task PlaySequenceActionsInSequence(InterfaceKitSequence interfaceKitSequence)
        {
            Int64 startTicks = 0;
            if (LogPerformanceSequence) Log.Trace($"Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Maybe just pass the interfaceKit into Action and get this there

            InterfaceKitDigitalOutputCollection ifkDigitalOutputs = InterfaceKit.outputs;

            foreach (InterfaceKitAction action in interfaceKitSequence.Actions)
            {
                if (LogPerformanceSequence)
                {
                    Log.Trace($"DigitalOut Index:{action.DigitalOutIndex} DigitalOut:{action.DigitalOut} Duration:{action.Duration}", Common.LOG_CATEGORY);
                }

                try
                {
                    switch (action.DigitalOutIndex)
                    {
                        case 0:
                            PerformAction(ifkDigitalOutputs, action, 0);
                            break;

                        case 1:
                            PerformAction(ifkDigitalOutputs, action, 1);
                            break;

                        case 2:
                            PerformAction(ifkDigitalOutputs, action, 2);
                            break;

                        case 3:
                            PerformAction(ifkDigitalOutputs, action, 30);
                            break;

                        case 4:
                            PerformAction(ifkDigitalOutputs, action, 4);
                            break;

                        case 5:
                            PerformAction(ifkDigitalOutputs, action, 50);
                            break;

                        case 6:
                            PerformAction(ifkDigitalOutputs, action, 60);
                            break;

                        case 7:
                            PerformAction(ifkDigitalOutputs, action, 7);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            if (LogPerformanceSequence) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void PerformAction(InterfaceKitDigitalOutputCollection ifkDigitalOutputs, InterfaceKitAction action, Int32 index)
        {
            Int64 startTicks = 0;

            StringBuilder actionMessage = new StringBuilder();

            if (LogPerformanceAction)
            {
                startTicks = Log.Trace($"Enter index:{index}", Common.LOG_CATEGORY);
                actionMessage.Append($"index:{index}");
            }

            try
            {
                if (action.DigitalOut is not null)
                { 
                    if (LogPerformanceAction) actionMessage.Append($" digitalOut:{action.DigitalOut}");

                    ifkDigitalOutputs[index] = (Boolean)action.DigitalOut; 
                }

                if (action.Duration > 0)
                {
                    if (LogPerformanceAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
            finally
            {
                if (LogPerformanceAction)
                {
                    Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
                }
            }
        }

        #endregion
    }
}
