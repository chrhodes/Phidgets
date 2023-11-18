using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

using Phidgets;
using Phidgets.Events;

using VNCPhidget21.Configuration;

namespace VNC.Phidget
{
    public struct ServoMinMax
    {
        public enum LimitType
        {
            AccelerationMin,
            AccelerationMax,
            PositionMin,
            PositionMax,
            VelocityMin,
            VelocityMax,
        }

        public Double AccelerationMin;
        public Double AccelerationMax;
        public Double PositionMin;
        public Double PositionMax;
        public Double VelocityMin;
        public Double VelocityMax;

    }
    public class AdvancedServoEx : PhidgetEx // AdvancedServo
    {
        #region Constructors, Initialization, and Load

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public AdvancedServoEx(string ipAddress, int port, int serialNumber)
            : base(ipAddress, port, serialNumber)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InitializePhidget();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePhidget()
        {
            AdvancedServo = new Phidgets.AdvancedServo();

            this.AdvancedServo.Attach += Phidget_Attach;
            this.AdvancedServo.Attach += AdvancedServo_Attach;
            this.AdvancedServo.Detach += Phidget_Detach;
            this.AdvancedServo.Error += Phidget_Error;
            this.AdvancedServo.ServerConnect += Phidget_ServerConnect;
            this.AdvancedServo.ServerDisconnect += Phidget_ServerDisconnect;
        }

        private void AdvancedServo_Attach(object sender, AttachEventArgs e)
        {
            SaveInitialServoLimits();
        }

        private void SaveInitialServoLimits()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                AdvancedServoServoCollection servos = AdvancedServo.servos;
                AdvancedServoServo servo = null;

                // Save the device position min/max before any changes are made

                for (int i = 0; i < servos.Count; i++)
                {
                    servo = servos[i];

                    // NOTE(crhodes)
                    // Not sure we need to save Accleration and Velocity, they cannot change

                    InitialServoLimits[i].AccelerationMin = servo.AccelerationMin;
                    InitialServoLimits[i].AccelerationMax = servo.AccelerationMax;
                    InitialServoLimits[i].PositionMin = servo.PositionMin;
                    InitialServoLimits[i].PositionMax = servo.PositionMax;
                    InitialServoLimits[i].VelocityMin = servo.VelocityMin;
                    InitialServoLimits[i].VelocityMax = servo.VelocityMax;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private Double GetPositionLimit(Int32 index, ServoMinMax.LimitType positionType)
        {
            Double limit;

            if (positionType== ServoMinMax.LimitType.VelocityMin)
            {
                limit = InitialServoLimits[index].PositionMin;
            }
            else
            {
                limit = InitialServoLimits[index].PositionMax;
            }  

            return limit;
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Phidgets.AdvancedServo AdvancedServo = null;

        public bool LogPerformanceStep { get; set; }


        public ServoMinMax[] InitialServoLimits { get; set; } = new ServoMinMax[8];

        #endregion

        #region Event Handlers


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
                // HACK(crhodes)
                // Trying to get new RCC0004_0 AdvancedServo board to work with
                // Legacy SBC1 or SBC2.  No luck.
                // Only seems to work it directly attached, sigh.
                //AdvancedServo.open(SerialNumber);
                //AdvancedServo.open(-1);

                AdvancedServo.open(SerialNumber, Host.IPAddress, Host.Port);

                if (timeOut is not null)
                {
                    AdvancedServo.waitForAttachment((Int32)timeOut); 
                }
                else 
                { 
                    AdvancedServo.waitForAttachment();
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
            Int64 startTicks = Log.Trace($"Enter loops:{advancedServoSequence.Loops}", Common.LOG_CATEGORY);

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

            Parallel.ForEach(advancedServoSequence.Actions, action =>
            {
                //if (LogPerformanceStep)
                //{
                //    Log.Trace($"Servo:{action.ServoIndex} Acceleration:{action.Acceleration} VelocityLimit:{action.VelocityLimit}" +
                //        $" Engaged:{action.Engaged} TargetPosition:{action.TargetPosition} Duration:{action.Duration}", Common.LOG_CATEGORY);
                //}

                try
                {
                    PerformAction(action);
                    //switch (action.ServoIndex)
                    //{
                    //    case 0:
                    //        PerformAction(AdvancedServo.servos[0], action, 0);
                    //        break;

                    //    case 1:
                    //        PerformAction(AdvancedServo.servos[1], action, 1);
                    //        break;

                    //    case 2:
                    //        PerformAction(AdvancedServo.servos[2], action, 2);
                    //        break;

                    //    case 3:
                    //        PerformAction(AdvancedServo.servos[3], action, 3);
                    //        break;

                    //    case 4:
                    //        PerformAction(AdvancedServo.servos[4], action, 4);
                    //        break;

                    //    case 5:
                    //        PerformAction(AdvancedServo.servos[5], action, 5);
                    //        break;

                    //    case 6:
                    //        PerformAction(AdvancedServo.servos[6], action, 6);
                    //        break;

                    //    case 7:
                    //        PerformAction(AdvancedServo.servos[7], action, 7);
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

        private void PlaySequenceActionsInSequence(AdvancedServoSequence advancedServoSequence)
        {
            Int64 startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            foreach (AdvancedServoServoAction action in advancedServoSequence.Actions)
            {

                //if (LogPerformanceStep)
                //{
                //    Log.Trace($"Servo:{action.ServoIndex} Acceleration:{action.Acceleration} VelocityLimit:{action.VelocityLimit}" +
                //        $" Engaged:{action.Engaged} TargetPosition:{action.TargetPosition} Duration:{action.Duration}", Common.LOG_CATEGORY);
                //}

                try
                {
                    PerformAction(action);


                    //switch (action.ServoIndex)
                    //{
                    //    case 0:
                    //        PerformAction(AdvancedServo.servos[0], action, 0);
                    //        break;

                    //    case 1:
                    //        PerformAction(AdvancedServo.servos[1], action, 1);
                    //        break;

                    //    case 2:
                    //        PerformAction(AdvancedServo.servos[2], action, 2);
                    //        break;

                    //    case 3:
                    //        PerformAction(AdvancedServo.servos[3], action, 3);
                    //        break;

                    //    case 4:
                    //        PerformAction(AdvancedServo.servos[4], action, 4);
                    //        break;

                    //    case 5:
                    //        PerformAction(AdvancedServo.servos[5], action, 5);
                    //        break;

                    //    case 6:
                    //        PerformAction(AdvancedServo.servos[6], action, 6);
                    //        break;

                    //    case 7:
                    //        PerformAction(AdvancedServo.servos[7], action, 7);
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

        private void PerformAction(AdvancedServoServoAction action)
        {
            Int64 startTicks = 0;

            Int32 index = action.ServoIndex;
            AdvancedServoServo servo = AdvancedServo.servos[index];

            StringBuilder actionMessage = new StringBuilder();

            if (LogPerformanceStep)
            {
                startTicks = Log.Trace($"Enter servo:{index}", Common.LOG_CATEGORY);
                actionMessage.Append($"servo:{index}");
                    //+ $" acceleration:>{action.Acceleration}< relativeAcceleration:>{action.RelativeAcceleration}<"
                    //+ $" velocityLimit:>{action.VelocityLimit}< relativeVelocityLimit:>{action.RelativeVelocityLimit}<"
                    //+ $" postionMax:>{action.PositionMax}< positionMin:>{action.PositionMin}<"
                    //+ $" targetPosition:>{action.TargetPosition}< relativePosition:>{action.RelativePosition}<"
                    //+ $" duration:>{action.Duration}<", Common.LOG_CATEGORY);
            }
            
            try
            {
                if (action.Acceleration is not null)
                {
                    actionMessage.Append($" acceleration:>{action.Acceleration}<");
                    SetAcceleration((Double)action.Acceleration, servo);
                    //servo.Acceleration = (Double)action.Acceleration;
                }

                if (action.RelativeAcceleration is not null)
                {
                    var newAcceleration = servo.Acceleration += (Double)action.RelativeAcceleration;
                    actionMessage.Append($" relativeAcceleration:>{action.RelativeAcceleration}< ({newAcceleration})");

                    SetAcceleration(newAcceleration, servo);
                    //servo.Acceleration = newAcceleration;
                    // TODO(crhodes)
                    // Do we really need these sleeps?
                    Thread.Sleep(1);
                }

                if (action.VelocityLimit is not null)
                {
                    actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");

                    SetVelocity((Double)action.VelocityLimit, servo);
                }

                if (action.RelativeVelocityLimit is not null)
                {
                    var newVelocityLimit = servo.VelocityLimit += (Double)action.RelativeVelocityLimit;
                    actionMessage.Append($" relativeVelocityLimit:>{action.RelativeVelocityLimit}< ({newVelocityLimit})");

                    SetVelocity(newVelocityLimit, servo);
                    Thread.Sleep(1);
                }

                if (action.PositionMin is not null)
                {
                    // TODO(crhodes)
                    // Need to save initial low limit based on ServoType
                    // Hard code for now
                    Double newPositionMin = action.PositionMin < 0 ? InitialServoLimits[index].PositionMin : (Double)action.PositionMin;
                    actionMessage.Append($" positionMin:>{action.PositionMin}<");

                    SetPositionMin(newPositionMin, servo);
                }

                if (action.PositionMax is not null)
                {
                    // TODO(crhodes)
                    // Need to save initial upper limit based on ServoType
                    // Hard code for now

                    Double newPositionMax = action.PositionMax < 0 ? InitialServoLimits[index].PositionMax : (Double)action.PositionMax;
                    actionMessage.Append($" positionMax:>{action.PositionMax}<");

                    SetPositionMax(newPositionMax, servo);
                }

                if (action.Engaged is not null)
                {
                    actionMessage.Append($" engaged:>{action.Engaged}<");

                    servo.Engaged = (Boolean)action.Engaged;

                    VerifyServoEngaged(index, servo);
                }

                if (action.TargetPosition is not null)
                {
                    actionMessage.Append($" targetPosition:>{action.TargetPosition}<");

                    SetPosition((Double)action.TargetPosition, servo);
                    //servo.Position = newPosition;

                    VerifyNewPositionAchieved(index, servo, (Double)action.TargetPosition);
                }

                if (action.RelativePosition is not null)
                {
                    var newPosition = servo.Position += (Double)action.RelativePosition;
                    actionMessage.Append($" relativePosition:>{action.RelativePosition}< ({newPosition})");

                    SetPosition(newPosition, servo);
                    //servo.Position = newPosition;

                    VerifyNewPositionAchieved(index, servo, newPosition);                
                }

                if (action.Duration > 0)
                {
                    actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
            finally
            {
                if (LogPerformanceStep)
                {
                    Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
                }
            }   
        }

        /// <summary>
        /// Bounds check and set acceleration
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="servo"></param>
        private static void SetAcceleration(Double acceleration, AdvancedServoServo servo)
        {
            if (acceleration < servo.AccelerationMin) servo.Acceleration = servo.AccelerationMin;
            else if (acceleration > servo.AccelerationMax) servo.Acceleration = servo.AccelerationMax;
            else servo.Acceleration = acceleration;
        }

        /// <summary>
        /// Bounds check and set velocity
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="servo"></param>
        private static void SetVelocity(Double velocity, AdvancedServoServo servo)
        {
            if (velocity < servo.VelocityMin) servo.VelocityLimit = servo.VelocityMin;
            else if(velocity > servo.VelocityMax) servo.VelocityLimit = servo.VelocityMax;
            else servo.VelocityLimit = velocity;
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="servo"></param>
        private static void SetPositionMin(Double position, AdvancedServoServo servo)
        {
            if (position < servo.PositionMin) servo.Position = servo.PositionMin;
            else if (position > servo.PositionMax) servo.Position = servo.PositionMax;
            else servo.PositionMin = position;
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="servo"></param>
        private static void SetPosition(Double position, AdvancedServoServo servo)
        {
            if (position < servo.PositionMin) servo.Position = servo.PositionMin;
            else if (position > servo.PositionMax) servo.Position = servo.PositionMax;
            else servo.Position = position;
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="servo"></param>
        private static void SetPositionMax(Double position, AdvancedServoServo servo)
        {
            if (position < servo.PositionMin) servo.Position = servo.PositionMin;
            else if (position > servo.PositionMax) servo.Position = servo.PositionMax;
            else servo.PositionMax = position;
        }

        private void VerifyServoEngaged(Int32 index, AdvancedServoServo servo)
        {
            Int64 startTicks = 0;
            var msSleep = 0;

            if (LogPerformanceStep)
            {
                startTicks = Log.Trace($"Enter index:{index}", Common.LOG_CATEGORY);
            }



            while (servo.Engaged != true)
            {
                Thread.Sleep(1);
                msSleep++;
            }

            if (LogPerformanceStep)
            {
                Log.Trace($"Exit index:{index} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
            }
        }

        private void VerifyNewPositionAchieved(Int32 index, AdvancedServoServo servo, double targetPosition)
        {
            Int64 startTicks = 0;
            var msSleep = 0;

            if (LogPerformanceStep)
            {
                startTicks = Log.Trace($"Enter index:{index}", Common.LOG_CATEGORY);
            }

            while (servo.Position != targetPosition)
            {
                Thread.Sleep(1);
                msSleep++;
            }

            if (LogPerformanceStep)
            {
                Log.Trace($"Exit index:{index} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
            }
        }

        #endregion
    }
}
