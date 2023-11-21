using System;
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
            //this.Attach += InterfaceKitk_Attach;
            //this.Detach += InterfaceKitk_Detach;
            //this.Error += InterfaceKitk_Error;
            //this.ServerConnect += InterfaceKitk_ServerConnect;
            //this.ServerDisconnect += InterfaceKitk_ServerDisconnect;

            //this.InputChange += InterfaceKitk_InputChange;
            //this.OutputChange += InterfaceKitk_OutputChange;
            //this.SensorChange += InterfaceKitk_SensorChange;

            InterfaceKit = new Phidgets.InterfaceKit();

            this.InterfaceKit.Attach += Phidget_Attach;
            this.InterfaceKit.Detach += Phidget_Detach;
            this.InterfaceKit.Error += Phidget_Error;
            this.InterfaceKit.ServerConnect += Phidget_ServerConnect;
            this.InterfaceKit.ServerDisconnect += Phidget_ServerDisconnect;

            this.InterfaceKit.InputChange += InterfaceKitk_InputChange;
            this.InterfaceKit.OutputChange += InterfaceKitk_OutputChange;
            this.InterfaceKit.SensorChange += InterfaceKitk_SensorChange;
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Phidgets.InterfaceKit InterfaceKit = null;
            
        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }
        public bool LogPerformanceStep { get; set; }

        #endregion

        #region Event Handlers

        private void InterfaceKitk_SensorChange(object sender, SensorChangeEventArgs e)
        {
            if (LogSensorChangeEvents)
            {
                try
                {
                    Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"InterfaceKit_SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void InterfaceKitk_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        {
            if (LogOutputChangeEvents)
            {
                try
                {
                    Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"InterfaceKit_OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void InterfaceKitk_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        {
            if (LogInputChangeEvents)
            {
                try
                {
                    Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"InterfaceKit_InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
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

                if (timeOut is not null) { InterfaceKit.waitForAttachment((Int32)timeOut); }
                else { InterfaceKit.waitForAttachment(); }
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
                this.InterfaceKit.close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public async Task PlaySequenceLoops(InterfaceKitSequence interfaceKitSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            for (int i = 0; i < interfaceKitSequence.Loops; i++)
            {
                Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);

                if (interfaceKitSequence.PlayActionsInParallel)
                {
                    await PlaySequenceActionsInParallel(interfaceKitSequence);
                }
                else
                {
                    await PlaySequenceActionsInSequence(interfaceKitSequence);
                }
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)

        private async Task PlaySequenceActionsInParallel(InterfaceKitSequence interfaceKitSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Maybe just pass the interfaceKit into Action and get this there

            InterfaceKitDigitalOutputCollection ifkDigitalOutputs = InterfaceKit.outputs;

            Parallel.ForEach(interfaceKitSequence.Actions, action =>
            {
                if (LogPerformanceStep)
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
            Int64 startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Maybe just pass the interfaceKit into Action and get this there

            InterfaceKitDigitalOutputCollection ifkDigitalOutputs = InterfaceKit.outputs;

            foreach (InterfaceKitAction action in interfaceKitSequence.Actions)
            {
                if (LogPerformanceStep)
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

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void PerformAction(InterfaceKitDigitalOutputCollection ifkDigitalOutputs, InterfaceKitAction action, Int32 index)
        {
            Int64 startTicks = 0;

            if (LogPerformanceStep)
            {
                startTicks = Log.Trace($"Enter index:{index} digitalOut:{action.DigitalOut}" +
                    $" duration:{action.Duration}", Common.LOG_CATEGORY);
            }

            try
            {
                if (action.DigitalOut is not null) ifkDigitalOutputs[index] = (Boolean)action.DigitalOut;

                if (action.Duration > 0) Thread.Sleep((Int32)action.Duration);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogPerformanceStep)
            {
                Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }
        }

        #endregion
    }
}
