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
    public class AdvancedServoEx : PhidgetEx // AdvancedServo
    {
        #region Constructors, Initialization, and Load

        public IEventAggregator EventAggregator { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the AdvancedServo class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public AdvancedServoEx(string ipAddress, int port, int serialNumber, IEventAggregator eventAggregator)
            : base(ipAddress, port, serialNumber)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;
            InitializePhidget();

            EventAggregator.GetEvent<AdvancedServoSequenceEvent>().Subscribe(TriggerSequence);

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }
        private void TriggerSequence(SequenceEventArgs args)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            var advancedServoSequence = args.AdvancedServoSequence;

            PlaySequenceLoops(advancedServoSequence);


            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePhidget()
        {
            AdvancedServo = new Phidgets.AdvancedServo();

            AdvancedServo.Attach += Phidget_Attach;
            //AdvancedServo.Attach += AdvancedServo_Attach;
            AdvancedServo.Detach += Phidget_Detach;
            AdvancedServo.Error += Phidget_Error;
            AdvancedServo.ServerConnect += Phidget_ServerConnect;
            AdvancedServo.ServerDisconnect += Phidget_ServerDisconnect;
        }

        internal override void Phidget_Attach(object sender, AttachEventArgs e)
        {
            SaveInitialServoLimits();
            base.Phidget_Attach(sender, e);
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

                    Log.Trace($"servo:{i} positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
                    // NOTE(crhodes)
                    // We do not need to save Accleration and Velocity Min,Max,
                    // they cannot change

                    //InitialServoLimits[i].AccelerationMin = servo.AccelerationMin;
                    //InitialServoLimits[i].AccelerationMax = servo.AccelerationMax;
                    InitialServoLimits[i].DevicePositionMin = servo.PositionMin;
                    InitialServoLimits[i].PositionMin = servo.PositionMin;
                    InitialServoLimits[i].PositionMax = servo.PositionMax;
                    InitialServoLimits[i].DevicePositionMax = servo.PositionMax;
                    //InitialServoLimits[i].VelocityMin = servo.VelocityMin;
                    //InitialServoLimits[i].VelocityMax = servo.VelocityMax;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //private Double GetPositionLimit(Int32 index, ServoMinMax.LimitType positionType)
        //{
        //    Double limit;

        //    if (positionType == ServoMinMax.LimitType.PositionMin)
        //    {
        //        limit = InitialServoLimits[index].PositionMin;
        //    }
        //    else
        //    {
        //        limit = InitialServoLimits[index].PositionMax;
        //    }  

        //    return limit;
        //}

        #endregion

        #region Enums (None)


        #endregion

        #region Structures

        public struct ServoMinMax
        {
            public enum LimitType
            {
                //AccelerationMin,
                //AccelerationMax,
                DevicePositionMin,
                PositionMin,
                PositionMax,
                DevicePositionMax
                //VelocityMin,
                //VelocityMax,
            }

            //public Double AccelerationMin;
            //public Double AccelerationMax;
            public Double DevicePositionMin;
            public Double PositionMin;
            public Double PositionMax;
            public Double DevicePositionMax;
            //public Double VelocityMin;
            //public Double VelocityMax;
        }

        #endregion

        #region Fields and Properties

        public Phidgets.AdvancedServo AdvancedServo = null;

        private bool _logPositionChangeEvents;
        public bool LogPositionChangeEvents
        {
            get => _logPositionChangeEvents;
            set
            {
                if (_logPositionChangeEvents == value) return;

                if (_logPositionChangeEvents = value)
                {
                    AdvancedServo.PositionChange += AdvancedServo_PositionChange;
                }
                else
                {
                    AdvancedServo.PositionChange -= AdvancedServo_PositionChange;
                }
            }
        }

        private bool _logVelocityChangeEvents;
        public bool LogVelocityChangeEvents
        {
            get => _logVelocityChangeEvents;
            set
            {
                if (_logVelocityChangeEvents == value) return;

                if (_logVelocityChangeEvents = value)
                {
                    AdvancedServo.VelocityChange += AdvancedServo_VelocityChange;
                }
                else
                {
                    AdvancedServo.VelocityChange -= AdvancedServo_VelocityChange;
                }
            }
        }

        private bool _logCurrentChangeEvents;
        public bool LogCurrentChangeEvents
        {
            get => _logCurrentChangeEvents;
            set
            {
                if (_logCurrentChangeEvents == value) return;

                if (_logCurrentChangeEvents = value)
                {
                    AdvancedServo.CurrentChange += AdvancedServo_CurrentChange;
                }
                else
                {
                    AdvancedServo.CurrentChange -= AdvancedServo_CurrentChange;
                }
            }
        }

        public bool LogPerformanceStep { get; set; }

        public ServoMinMax[] InitialServoLimits { get; set; } = new ServoMinMax[8];

        #endregion

        #region Event Handlers (None)

        private void AdvancedServo_CurrentChange(object sender, CurrentChangeEventArgs e)
        {
            try
            {
                Phidgets.AdvancedServo advancedServo = sender as Phidgets.AdvancedServo;
                Log.EVENT_HANDLER($"CurrentChange {advancedServo.Address},{advancedServo.SerialNumber} - Index:{e.Index} Value:{e.Current}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void AdvancedServo_PositionChange(object sender, PositionChangeEventArgs e)
        {
            try
            {
                Phidgets.AdvancedServo advancedServo = sender as Phidgets.AdvancedServo;
                Log.EVENT_HANDLER($"PositionChange {advancedServo.Address},{advancedServo.SerialNumber} - Index:{e.Index} Value:{e.Position}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void AdvancedServo_VelocityChange(object sender, VelocityChangeEventArgs e)
        {
            try
            {
                Phidgets.AdvancedServo advancedServo = sender as Phidgets.AdvancedServo;
                Log.EVENT_HANDLER($"VelocityChange {advancedServo.Address},{advancedServo.SerialNumber} - Index:{e.Index} Value:{e.Velocity}", Common.LOG_CATEGORY);
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
                // NOTE(crhodes)
                // We may be logging events.  Remove them before closing

                if (LogCurrentChangeEvents) LogCurrentChangeEvents = false;
                if (LogPositionChangeEvents) LogPositionChangeEvents = false;
                if (LogVelocityChangeEvents) LogVelocityChangeEvents = false;

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
                if (LogPerformanceStep) Log.Trace($"Loop:{i + 1}", Common.LOG_CATEGORY);               

                if (advancedServoSequence.PlayActionsInParallel)
                {
                    //PlaySequenceActionsInParallel(advancedServoSequence);
                    Parallel.ForEach(advancedServoSequence.Actions, action =>
                    {
                        try
                        {
                            PerformAction(action);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, Common.LOG_CATEGORY);
                        }
                    });
                }
                else
                {
                    //await PlaySequenceActionsInSequence(advancedServoSequence);
                    foreach (AdvancedServoServoAction action in advancedServoSequence.Actions)
                    {
                        try
                        {
                            await PerformAction(action);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, Common.LOG_CATEGORY);
                        }
                    }
                }
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }


        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)

        //private async void PlaySequenceActionsInParallel(AdvancedServoSequence advancedServoSequence)
        //{
        //    Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

        //    Parallel.ForEach(advancedServoSequence.Actions, action =>
        //    {
        //        try
        //        {
        //            PerformAction(action);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    });

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //private async Task PlaySequenceActionsInSequence(AdvancedServoSequence advancedServoSequence)
        //{
        //    Int64 startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

        //    foreach (AdvancedServoServoAction action in advancedServoSequence.Actions)
        //    {
        //        try
        //        {
        //            PerformAction(action);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        private async Task PerformAction(AdvancedServoServoAction action)
        {             
            Int64 startTicks = 0;

            Int32 index = action.ServoIndex;
            AdvancedServoServo servo = AdvancedServo.servos[index];

            StringBuilder actionMessage = new StringBuilder();

            if (LogPerformanceStep)
            {
                startTicks = Log.Trace($"Enter servo:{index}", Common.LOG_CATEGORY);
                actionMessage.Append($"servo:{index}");
            }
            
            try
            {
                if (action.ServoType is not null)
                {
                    if (LogPerformanceStep) actionMessage.Append($" servoType:>{action.ServoType}<");

                    servo.Type = (Phidgets.ServoServo.ServoType)action.ServoType;
                }

                if (action.Acceleration is not null)
                {
                    if (LogPerformanceStep) actionMessage.Append($" acceleration:>{action.Acceleration}<");

                    SetAcceleration((Double)action.Acceleration, servo);
                }

                if (action.RelativeAcceleration is not null)
                {
                    var newAcceleration = servo.Acceleration += (Double)action.RelativeAcceleration;
                    if (LogPerformanceStep) actionMessage.Append($" relativeAcceleration:>{action.RelativeAcceleration}< ({newAcceleration})");

                    SetAcceleration(newAcceleration, servo);
                    ////servo.Acceleration = newAcceleration;
                    //// TODO(crhodes)
                    //// Do we really need these sleeps?
                    //Thread.Sleep(1);
                }

                if (action.VelocityLimit is not null)
                {
                    if (LogPerformanceStep) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");

                    SetVelocityLimit((Double)action.VelocityLimit, servo);
                }

                if (action.RelativeVelocityLimit is not null)
                {
                    var newVelocityLimit = servo.VelocityLimit += (Double)action.RelativeVelocityLimit;
                    if (LogPerformanceStep) actionMessage.Append($" relativeVelocityLimit:>{action.RelativeVelocityLimit}< ({newVelocityLimit})");

                    SetVelocityLimit(newVelocityLimit, servo);
                    //Thread.Sleep(1);
                }

                if (action.PositionMin is not null)
                {
                    //Double newPositionMin = action.PositionMin < 0 ? InitialServoLimits[index].PositionMin : (Double)action.PositionMin;
                    if (LogPerformanceStep) actionMessage.Append($" positionMin:>{action.PositionMin}<");

                    SetPositionMin((Double)action.PositionMin, servo, index);
                }

                if (action.PositionMax is not null)
                {
                    //Double newPositionMax = action.PositionMax < 0 ? InitialServoLimits[index].PositionMax : (Double)action.PositionMax;
                    if (LogPerformanceStep) actionMessage.Append($" positionMax:>{action.PositionMax}<");

                    SetPositionMax((Double)action.PositionMax, servo, index);
                }

                if (action.Engaged is not null)
                {
                    if (LogPerformanceStep) actionMessage.Append($" engaged:>{action.Engaged}<");

                    servo.Engaged = (Boolean)action.Engaged;

                    if ((Boolean)action.Engaged) VerifyServoEngaged(index, servo);
                }

                if (action.TargetPosition is not null)
                {
                    if (LogPerformanceStep) actionMessage.Append($" targetPosition:>{action.TargetPosition}<");
                    Double targetPosition = (Double)action.TargetPosition;

                    if (targetPosition < 0)
                    {
                        if (action.TargetPosition == -1)
                        { 
                            targetPosition = InitialServoLimits[index].PositionMin; 
                        }
                        else if (action.TargetPosition == -2)
                        { 
                            targetPosition = InitialServoLimits[index].PositionMax;
                        }
                    }
                  
                    VerifyNewPositionAchieved(index, servo, SetPosition(targetPosition, servo));
                }

                if (action.RelativePosition is not null)
                {
                    var newPosition = servo.Position + (Double)action.RelativePosition;
                    if (LogPerformanceStep) actionMessage.Append($" relativePosition:>{action.RelativePosition}< ({newPosition})");

                    VerifyNewPositionAchieved(index, servo, SetPosition(newPosition, servo));                
                }

                if (action.Duration > 0)
                {
                    if (LogPerformanceStep) actionMessage.Append($" duration:>{action.Duration}<");

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
        public void SetAcceleration(Double acceleration, AdvancedServoServo servo)
        {
            try
            {
                if (acceleration < servo.AccelerationMin) servo.Acceleration = servo.AccelerationMin;
                else if (acceleration > servo.AccelerationMax) servo.Acceleration = servo.AccelerationMax;
                else servo.Acceleration = acceleration;
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set velocity
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="servo"></param>
        public void SetVelocityLimit(Double velocity, AdvancedServoServo servo)
        {
            try
            {
                if (velocity < servo.VelocityMin) servo.VelocityLimit = servo.VelocityMin;
                else if (velocity > servo.VelocityMax) servo.VelocityLimit = servo.VelocityMax;
                else servo.VelocityLimit = velocity;
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="servo"></param>
        public void SetPositionMin(Double position, AdvancedServoServo servo, Int32 index)
        {
            Log.Trace($"Begin index:{index} position:{position} servo.PositionMin:{servo.PositionMin} servo.PositionMax:{servo.PositionMax}" +
                $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin} DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);

            try
            {
                //if (position < servo.PositionMin) servo.Position = servo.PositionMin;
                //else if (position > servo.PositionMax) servo.Position = servo.PositionMax;
                //else servo.PositionMin = position;
                if (position < 0)
                {
                    servo.PositionMin = InitialServoLimits[index].DevicePositionMin;
                }
                else if (position < InitialServoLimits[index].DevicePositionMin)
                {
                    servo.PositionMin = InitialServoLimits[index].DevicePositionMin;
                }
                else if (position > servo.PositionMax)
                {
                    servo.PositionMin = servo.PositionMax;
                }
                else
                {
                    servo.PositionMin = position;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace($"End index:{index} servo.PositionMin:{servo.PositionMin} servo.PositionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="servo"></param>
        public Double SetPosition(Double position, AdvancedServoServo servo)
        {
            try
            {
                if (position < servo.PositionMin) position = servo.PositionMin;
                else if (position > servo.PositionMax) position = servo.PositionMax;
                    
                servo.Position = position;
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return position;
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="servo"></param>
        public void SetPositionMax(Double position, AdvancedServoServo servo, Int32 index)
        {
            Log.Trace($"Begin index:{index} position:{position} servo.PositionMin:{servo.PositionMin} servo.PositionMax:{servo.PositionMax}" +
                $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin} DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);

            try
            {
                //if (position < servo.PositionMin) servo.Position = servo.PositionMin;
                //else if (position > servo.PositionMax) servo.Position = servo.PositionMax;
                //else servo.PositionMax = position;
                if (position < 0)
                {
                    servo.PositionMax = InitialServoLimits[index].DevicePositionMax;
                }
                else if (position < servo.PositionMin)
                { 
                    servo.PositionMax = servo.PositionMin; 
                }
                else if (position > InitialServoLimits[index].DevicePositionMax)
                { 
                    servo.PositionMax = InitialServoLimits[index].DevicePositionMax; 
                }
                else
                {
                    servo.PositionMax = position; 
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
            Log.Trace($"End index:{index} servo.PositionMin:{servo.PositionMin} servo.PositionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
        }

        private void VerifyServoEngaged(Int32 index, AdvancedServoServo servo)
        {
            Int64 startTicks = 0;
            var msSleep = 0;

            try
            {
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
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void VerifyNewPositionAchieved(Int32 index, AdvancedServoServo servo, double targetPosition)
        {
            Int64 startTicks = 0;
            var msSleep = 0;

            try
            {
                if (LogPerformanceStep)
                {
                    startTicks = Log.Trace($"Enter index:{index} targetPosition:{targetPosition}", Common.LOG_CATEGORY);
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
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion
    }
}
