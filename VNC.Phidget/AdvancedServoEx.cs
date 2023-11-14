using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.SqlServer.Server;

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
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Phidgets.AdvancedServo AdvancedServo = null;

        public bool LogPerformanceStep { get; set; }

        #endregion

        #region Event Handlers

       
        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        public void Open()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

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

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void Close()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                AdvancedServo.close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public async Task PlaySequenceLoops(AdvancedServoSequence advancedServoSequence)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            for (int i = 0; i < advancedServoSequence.Loops; i++)
            {
                Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);

                if (advancedServoSequence.PlayActionsInParallel)
                {
                    PlaySequenceActionsInParallel(advancedServoSequence);
                }
                else
                {
                    PlaySequenceActionsInSequence(advancedServoSequence);
                }
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }


        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)

        private async void PlaySequenceActionsInParallel(AdvancedServoSequence advancedServoSequence)
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
                            PerformAction(AdvancedServo.servos[0], action, 0);
                            break;

                        case 1:
                            PerformAction(AdvancedServo.servos[1], action, 1);
                            break;

                        case 2:
                            PerformAction(AdvancedServo.servos[2], action, 2);
                            break;

                        case 3:
                            PerformAction(AdvancedServo.servos[3], action, 3);
                            break;

                        case 4:
                            PerformAction(AdvancedServo.servos[4], action, 4);
                            break;

                        case 5:
                            PerformAction(AdvancedServo.servos[5], action, 5);
                            break;

                        case 6:
                            PerformAction(AdvancedServo.servos[6], action, 6);
                            break;

                        case 7:
                            PerformAction(AdvancedServo.servos[7], action, 7);
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

        private void PlaySequenceActionsInSequence(AdvancedServoSequence advancedServoSequence)
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
                            PerformAction(AdvancedServo.servos[0], action, 0);
                            break;

                        case 1:
                            PerformAction(AdvancedServo.servos[1], action, 1);
                            break;

                        case 2:
                            PerformAction(AdvancedServo.servos[2], action, 2);
                            break;

                        case 3:
                            PerformAction(AdvancedServo.servos[3], action, 3);
                            break;

                        case 4:
                            PerformAction(AdvancedServo.servos[4], action, 4);
                            break;

                        case 5:
                            PerformAction(AdvancedServo.servos[5], action, 5);
                            break;

                        case 6:
                            PerformAction(AdvancedServo.servos[6], action, 6);
                            break;

                        case 7:
                            PerformAction(AdvancedServo.servos[7], action, 7);
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

        private void PerformAction(AdvancedServoServo servo, AdvancedServoServoAction action, Int32 index)
        {
            Int64 startTicks = 0;

            if (LogPerformanceStep)
            {
                startTicks = Log.Trace($"Enter servo:{index} engaged:{action.Engaged}"
                    + $" acceleration:{action.Acceleration} velocityLimit:{action.VelocityLimit}"
                    + $" postionMax:{action.PositionMax} positionMin:{action.PositionMin} targetPosition:{action.TargetPosition}"
                    + $" duration:{action.Duration}", Common.LOG_CATEGORY);
            }
            
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

            if (LogPerformanceStep)
            {
                Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }            
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
    }
}
