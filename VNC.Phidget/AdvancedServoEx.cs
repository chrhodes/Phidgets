using System;
using System.Threading;
using System.Threading.Tasks;

using Phidgets;

using VNCPhidgets21Explorer.Resources;

namespace VNC.Phidget
{
    public class AdvancedServoEx : PhidgetEx // AdvancedServo
    {
        #region Constructors, Initialization, and Load

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public AdvancedServoEx(string ipAddress, int port, int serialNumber)
            : base(ipAddress, port, serialNumber, true, false)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InitializePhidget();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePhidget()
        {
            AdvancedServo = new Phidgets.AdvancedServo();

            this.AdvancedServo.Attach += Phidget_Attach;
            this.AdvancedServo.Detach += Phidget_Detach;
            this.AdvancedServo.Error += Phidget_Error;
            this.AdvancedServo.ServerConnect += Phidget_ServerConnect;
            this.AdvancedServo.ServerDisconnect += Phidget_ServerDisconnect;

            //this.InputChange += AdvancedServo_InputChange;
            //this.OutputChange += AdvancedServo_OutputChange;
            //this.SensorChange += AdvancedServo_SensorChange;
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Phidgets.AdvancedServo AdvancedServo = null;

        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }
        public bool LogPerformanceStep { get; set; }

        #endregion

        #region Event Handlers

        //private void AdvancedServo_SensorChange(object sender, SensorChangeEventArgs e)
        //{
        //    if (LogSensorChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"AdvancedServo_SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AdvancedServo_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        //{
        //    if (LogOutputChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"AdvancedServo_OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AdvancedServo_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        //{
        //    if (LogInputChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"AdvancedServo_InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        public void Open()
        {
            try
            {
                this.AdvancedServo.open(HostSerialNumber, HostIPAddress, HostPort);

                // TDO(crhodes)
                // This will hang if AdvancedServo no attached.
                // How to handle this

                AdvancedServo.waitForAttachment();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        public void Close()
        {
            try
            {
                AdvancedServo.close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }           
        }

        public async Task PlayAdvancedServoSequenceLoops(AdvancedServoEx advancedServo, AdvancedServoSequence advancedServoSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            for (int i = 0; i < advancedServoSequence.Loops; i++)
            {
                Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);

                if (advancedServoSequence.PlayActionsInParallel)
                {
                    PlayAdvancedServoSequenceActionsInParallel(advancedServo, advancedServoSequence);
                }
                else
                {
                    PlayAdvancedServoSequenceActionsInSequence(advancedServo, advancedServoSequence);
                }
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async void PlayAdvancedServoSequenceActionsInParallel(AdvancedServoEx advancedServo, AdvancedServoSequence advancedServoSequence)
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

        private void PlayAdvancedServoSequenceActionsInSequence(AdvancedServoEx advancedServo, AdvancedServoSequence advancedServoSequence)
        {
            Int64 startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            foreach (AdvancedServoServoAction action in advancedServoSequence.AdvancedServoServoActions)
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

        private void PerformAdvancedServoAction(AdvancedServoServo servo, AdvancedServoServoAction action, Int32 index)
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

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)


        #endregion
    }
}
