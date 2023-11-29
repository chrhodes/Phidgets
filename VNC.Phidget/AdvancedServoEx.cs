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

            RunSequenceLoops(advancedServoSequence);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePhidget()
        {
            AdvancedServo = new Phidgets.AdvancedServo();

            AdvancedServo.Attach += Phidget_Attach;
            AdvancedServo.Detach += Phidget_Detach;
            AdvancedServo.Error += Phidget_Error;
            AdvancedServo.ServerConnect += Phidget_ServerConnect;
            AdvancedServo.ServerDisconnect += Phidget_ServerDisconnect;
        }

        internal override void Phidget_Attach(object sender, AttachEventArgs e)
        {
            SetInitialServoType();
            base.Phidget_Attach(sender, e);
        }

        private void SetInitialServoType()
        {
            try
            {
                AdvancedServoServoCollection servos = AdvancedServo.servos;
                AdvancedServoServo servo = null;

                // Save the device position min/max before any changes are made

                for (int i = 0; i < servos.Count; i++)
                {
                    servo = servos[i];

                    if (LogPerformanceAction)
                    {
                        Log.Trace($"servo:{i} type:{servo.Type} positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY); 
                    }

                    // NOTE(crhodes)
                    // Force the initial Servo Type to avoid opening something that has
                    // been set to an unexpected Type, e.g. RAW_us_MODE

                    servo.Type = ServoServo.ServoType.DEFAULT;

                    // NOTE(crhodes)
                    // We do not need to save Accleration and Velocity Min,Max,
                    // they cannot change

                    InitialServoLimits[i].AccelerationMin = servo.AccelerationMin;
                    InitialServoLimits[i].AccelerationMax = servo.AccelerationMax;
                    InitialServoLimits[i].DevicePositionMin = servo.PositionMin;
                    //InitialServoLimits[i].PositionMin = servo.PositionMin;
                    //InitialServoLimits[i].PositionMax = servo.PositionMax;
                    InitialServoLimits[i].DevicePositionMax = servo.PositionMax;
                    InitialServoLimits[i].VelocityMin = servo.VelocityMin + 1; // 0 won't move
                    InitialServoLimits[i].VelocityMax = servo.VelocityMax;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void SaveServoLimits(AdvancedServoServo servo, Int32 index)
        {
            if (LogPerformanceAction)
            {
                Log.Trace($"servo:{index} positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
            }
            // NOTE(crhodes)
            // We do not need to save Accleration and Velocity Min,Max,
            // they cannot change, but,
            // Useful when setting Acceleration/VelocityLimit to Min/Max in PerformAction

            InitialServoLimits[index].AccelerationMin = servo.AccelerationMin;
            InitialServoLimits[index].AccelerationMax = servo.AccelerationMax;
            InitialServoLimits[index].DevicePositionMin = servo.PositionMin;
            //InitialServoLimits[i].PositionMin = servo.PositionMin;
            //InitialServoLimits[i].PositionMax = servo.PositionMax;
            InitialServoLimits[index].DevicePositionMax = servo.PositionMax;
            InitialServoLimits[index].VelocityMin = servo.VelocityMin + 1; // 0 won't move
            InitialServoLimits[index].VelocityMax = servo.VelocityMax;
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures

        public struct ServoMinMax
        {
            //public enum LimitType
            //{
            //    //AccelerationMin,
            //    //AccelerationMax,
            //    DevicePositionMin,
            //    PositionMin,
            //    PositionMax,
            //    DevicePositionMax
            //    //VelocityMin,
            //    //VelocityMax,
            //}

            public Double AccelerationMin;
            public Double AccelerationMax;
            public Double DevicePositionMin;
            //public Double PositionMin;
            //public Double PositionMax;
            public Double DevicePositionMax;
            public Double VelocityMin;
            public Double VelocityMax;
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

        public bool LogPerformanceSequence { get; set; }
        public bool LogPerformanceAction { get; set; }
        public bool LogActionVerification { get; set; }

        public ServoMinMax[] InitialServoLimits { get; set; } = new ServoMinMax[8];

        #endregion

        #region Event Handlers

        private void AdvancedServo_CurrentChange(object sender, CurrentChangeEventArgs e)
        {
            try
            {
                Phidgets.AdvancedServo advancedServo = sender as Phidgets.AdvancedServo;
                Phidgets.AdvancedServoServo servo = advancedServo.servos[e.Index];
                Log.EVENT_HANDLER($"CurrentChange {advancedServo.Address},{advancedServo.SerialNumber},servo:{e.Index}" +
                    $" - current:{e.Current:00.000} - stopped:{servo.Stopped}", Common.LOG_CATEGORY);
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
                Phidgets.AdvancedServoServo servo = advancedServo.servos[e.Index];
                Log.EVENT_HANDLER($"PositionChange {advancedServo.Address},{advancedServo.SerialNumber},servo:{e.Index}" +
                    $" - velocity:{servo.Velocity,8:0.000} position:{e.Position,7:0.000} current:{servo.Current:00.000}" +
                    $" - stopped:{servo.Stopped} ", Common.LOG_CATEGORY);
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
                Phidgets.AdvancedServoServo servo = advancedServo.servos[e.Index];
                Log.EVENT_HANDLER($"VelocityChange {advancedServo.Address},{advancedServo.SerialNumber},servo:{e.Index}" +
                    $" - velocity:{e.Velocity,8:0.000} position:{servo.Position,7:0.000} current:{servo.Current:00.000}" +
                    $" - stopped:{servo.Stopped}", Common.LOG_CATEGORY);
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

        public async Task RunSequenceLoops(AdvancedServoSequence advancedServoSequence)
        {
            try
            {
                Int64 startTicks = 0;

                if (LogPerformanceSequence) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

                if (advancedServoSequence.Actions is not null)
                {
                    for (int sequenceLoop = 0; sequenceLoop < advancedServoSequence.Loops; sequenceLoop++)
                    {
                        //if (LogPerformanceSequence) Log.Trace($"Loop:{loop + 1}", Common.LOG_CATEGORY);

                        if (advancedServoSequence.ExecuteActionsInParallel)
                        {
                            if (LogPerformanceSequence) Log.Trace($"Parallel Actions Loop:{sequenceLoop + 1}", Common.LOG_CATEGORY);

                            Parallel.ForEach(advancedServoSequence.Actions, async action =>
                            {
                                await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogPerformanceSequence) Log.Trace($"Sequential Actions Loop:{sequenceLoop + 1}", Common.LOG_CATEGORY);

                            foreach (AdvancedServoServoAction action in advancedServoSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }
                    }
                }

                if (advancedServoSequence.Duration is not null)
                {
                    if (LogPerformanceSequence)
                    {
                        Log.Trace($"Zzzzz Sleeping:>{advancedServoSequence.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((Int32)advancedServoSequence.Duration);
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

         private async Task PerformAction(AdvancedServoServoAction action)
        {             
            Int64 startTicks = 0;

            Int32 index = action.ServoIndex;
            AdvancedServoServo servo = AdvancedServo.servos[index];

            StringBuilder actionMessage = new StringBuilder();

            if (LogPerformanceAction)
            {
                startTicks = Log.Trace($"Enter servo:{index}", Common.LOG_CATEGORY);
                actionMessage.Append($"servo:{index}");
            }
            
            try
            {
                if (action.ServoType is not null)
                {
                    if (LogPerformanceAction) actionMessage.Append($" servoType:>{action.ServoType}<");

                    servo.Type = (Phidgets.ServoServo.ServoType)action.ServoType;

                    // Save the refreshed values
                    SaveServoLimits(servo, index);
                }

                // NOTE(crhodes)
                // These can be performed without the servo being engaged.  This helps address
                // previous values that get applied when servo engaged.
                // The servo still snaps to last postion when enabled.  No known way to address that.

                if (action.Acceleration is not null)
                {
                    if (LogPerformanceAction) actionMessage.Append($" acceleration:>{action.Acceleration}<");
                    var acceleration = action.Acceleration;

                    if (acceleration < 0)
                    {
                        if (acceleration == -1)        // -1 is magic number for AccelerationMin :)
                        {
                            acceleration = InitialServoLimits[index].AccelerationMin;
                        }
                        else if (acceleration == -2)   // -2 is magic number for AccelerationMax :)
                        {
                            acceleration = InitialServoLimits[index].AccelerationMax;
                        }
                    }

                    SetAcceleration((Double)acceleration, servo, index);
                }

                if (action.VelocityLimit is not null)
                {
                    if (LogPerformanceAction) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");
                    var velocityLimit = action.VelocityLimit;

                    if (velocityLimit < 0)
                    {
                        if (velocityLimit == -1)        // -1 is magic number for VelocityMin :)
                        {
                            velocityLimit = InitialServoLimits[index].VelocityMin;
                        }
                        else if (velocityLimit == -2)   // -2 is magic number for VelocityMax :)
                        {
                            velocityLimit = InitialServoLimits[index].VelocityMax;
                        }
                    }

                    SetVelocityLimit((Double)velocityLimit, servo, index);
                }

                if (action.PositionMin is not null)
                {
                    if (LogPerformanceAction) actionMessage.Append($" positionMin:>{action.PositionMin}<");

                    SetPositionMin((Double)action.PositionMin, servo, index);
                }

                if (action.PositionMax is not null)
                {
                    if (LogPerformanceAction) actionMessage.Append($" positionMax:>{action.PositionMax}<");

                    SetPositionMax((Double)action.PositionMax, servo, index);
                }

                // NOTE(crhodes)
                // Engage the servo before doing other actions as some,
                // e.g. TargetPosition, requires servo to be engaged.

                if (action.Engaged is not null)
                {
                    if (LogPerformanceAction) actionMessage.Append($" engaged:>{action.Engaged}<");

                    servo.Engaged = (Boolean)action.Engaged;

                    if ((Boolean)action.Engaged) VerifyServoEngaged(servo, index);
                }

                //if (action.Acceleration is not null)
                //{
                //    if (LogPerformanceAction) actionMessage.Append($" acceleration:>{action.Acceleration}<");

                //    SetAcceleration((Double)action.Acceleration, servo, index);
                //}

                if (action.RelativeAcceleration is not null)
                {
                    var newAcceleration = servo.Acceleration + (Double)action.RelativeAcceleration;
                    if (LogPerformanceAction) actionMessage.Append($" relativeAcceleration:>{action.RelativeAcceleration}< ({newAcceleration})");

                    SetAcceleration(newAcceleration, servo, index);
                }

                //if (action.VelocityLimit is not null)
                //{
                //    if (LogPerformanceAction) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");

                //    SetVelocityLimit((Double)action.VelocityLimit, servo, index);
                //}

                if (action.RelativeVelocityLimit is not null)
                {
                    var newVelocityLimit = servo.VelocityLimit + (Double)action.RelativeVelocityLimit;
                    if (LogPerformanceAction) actionMessage.Append($" relativeVelocityLimit:>{action.RelativeVelocityLimit}< ({newVelocityLimit})");

                    SetVelocityLimit(newVelocityLimit, servo, index);
                }

                if (action.TargetPosition is not null)
                {
                    if (LogPerformanceAction) actionMessage.Append($" targetPosition:>{action.TargetPosition}<");
                    Double targetPosition = (Double)action.TargetPosition;

                    if (targetPosition < 0)
                    {
                        if (action.TargetPosition == -1)        // -1 is magic number for DevicePostionMin :)
                        { 
                            targetPosition = InitialServoLimits[index].DevicePositionMin; 
                        }
                        else if (action.TargetPosition == -2)   // -2 is magic number for DevicePostionMax :)
                        { 
                            targetPosition = InitialServoLimits[index].DevicePositionMax;
                        }
                    }
                  
                    VerifyNewPositionAchieved(servo, index, SetPosition(targetPosition, servo, index));
                }

                if (action.RelativePosition is not null)
                {
                    var newPosition = servo.Position + (Double)action.RelativePosition;
                    if (LogPerformanceAction) actionMessage.Append($" relativePosition:>{action.RelativePosition}< ({newPosition})");

                    VerifyNewPositionAchieved(servo, index, SetPosition(newPosition, servo, index));                
                }

                if (action.Duration > 0)
                {
                    if (LogPerformanceAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
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

        /// <summary>
        /// Bounds check and set acceleration
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="servo"></param>
        public void SetAcceleration(Double acceleration, AdvancedServoServo servo, Int32 index)
        {
            try
            {
                if (LogPerformanceAction)
                {
                    Log.Trace($"Begin index:{index} acceleration:{acceleration}" +
                        $" accelerationMin:{servo.AccelerationMin}" +
                        //$" acceleration:{servo.Acceleration}" + // Can't check this as it may not have been set yet
                        $" accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
                }

                if (acceleration < servo.AccelerationMin) 
                { 
                    servo.Acceleration = servo.AccelerationMin; 
                }
                else if (acceleration > servo.AccelerationMax) 
                { 
                    servo.Acceleration = servo.AccelerationMax; 
                }
                else 
                { 
                    servo.Acceleration = acceleration; 
                }

                if (LogPerformanceAction)
                {
                    Log.Trace($"End index:{index} servoAcceleration:{servo.Acceleration}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"index:{index} acceleration:{acceleration}" +
                    $" accelerationMin:{servo.AccelerationMin}" +
                    //$" acceleration:{servo.Acceleration}" + // Can't check this as it may not have been set yet
                    $" accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
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
        public void SetVelocityLimit(Double velocity, AdvancedServoServo servo, Int32 index)
        {
            try
            {
                if (LogPerformanceAction)
                {
                    Log.Trace($"Begin index:{index}" +
                        $" velocity:{velocity}" +
                        $" velocityMin:{servo.VelocityMin}" +
                        $" velocityLimit:{servo.VelocityLimit}" +
                        $" velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
                }

                if (velocity < servo.VelocityMin)
                {
                    servo.VelocityLimit = servo.VelocityMin; 
                }
                else if (velocity > servo.VelocityMax) 
                { 
                    servo.VelocityLimit = servo.VelocityMax; 
                }
                else 
                { 
                    servo.VelocityLimit = velocity;
                }

                if (LogPerformanceAction)
                {
                    Log.Trace($"End index:{index} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"index:{index}" +
                    $" velocity:{velocity}" +
                    $" velocityMin:{servo.VelocityMin}" +
                    $" velocityLimit:{servo.VelocityLimit}" +
                    $" velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
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
            try
            {
                if (LogPerformanceAction)
                {
                    Log.Trace($"Begin index:{index} position:{position}" +
                        $" servo.PositionMin:{servo.PositionMin}" +
                        $" servo.PositionMax:{servo.PositionMax}" +
                        $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                        $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                if (position < 0)
                {
                    position = InitialServoLimits[index].DevicePositionMin;
                }
                else if (position < InitialServoLimits[index].DevicePositionMin)
                {
                    position = InitialServoLimits[index].DevicePositionMin;
                }
                else if (position > servo.PositionMax)
                {
                    position = servo.PositionMax;
                }

                if (servo.PositionMin != position) servo.PositionMin = position;

                if (LogPerformanceAction)
                {
                    Log.Trace($"End index:{index} servo.PositionMin:{servo.PositionMin}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{index} {pex.Description} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"index:{index} position:{position}" +
                    $" servo.PositionMin:{servo.PositionMin}" +
                    $" servo.PositionMax:{servo.PositionMax}" +
                    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
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
        public Double SetPosition(Double position, AdvancedServoServo servo, Int32 index)
        {
            try
            {
                if (LogPerformanceAction)
                {
                    Log.Trace($"Begin servo:{index} position:{position}" +
                        $" servo.PositionMin:{servo.PositionMin}" +
                        $" servo.PositionMax:{servo.PositionMax}" +
                        $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                        $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                if (position < servo.PositionMin)
                {
                    position = servo.PositionMin;
                }
                else if (position > servo.PositionMax)
                {
                    position = servo.PositionMax;
                }

                // TODO(crhodes)
                // Maybe save last postion set and not bother checking servo.Position is same
                if (servo.Position != position) servo.Position = position;

                if (LogPerformanceAction)
                {
                    Log.Trace($"End servo:{index} servo.Position:{position}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"servo:{index} position:{position}" +
                    $" servo.PositionMin:{servo.PositionMin}" +
                    $" servo.PositionMax:{servo.PositionMax}" +
                    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
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
            try
            {
                if (LogPerformanceAction)
                {
                    Log.Trace($"Begin servo:{index} position:{position}" +
                        $" servo.PositionMin:{servo.PositionMin}" +
                        $" servo.PositionMax:{servo.PositionMax}" +
                        $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                        $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                if (position < 0)
                {
                    position = InitialServoLimits[index].DevicePositionMax;
                }
                else if (position < servo.PositionMin)
                {
                    position = servo.PositionMin;
                }
                else if (position > InitialServoLimits[index].DevicePositionMax)
                {
                    position = InitialServoLimits[index].DevicePositionMax;
                }

                if (servo.PositionMax != position) servo.PositionMax = position;

                if (LogPerformanceAction)
                {
                    Log.Trace($"End servo:{index} position:{position} servo.PositionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void VerifyServoEngaged(AdvancedServoServo servo, Int32 index)
        {
            Int64 startTicks = 0;
            var msSleep = 0;

            try
            {
                if (LogActionVerification)
                {
                    startTicks = Log.Trace($"Enter servo:{index} engaged:{servo.Engaged}", Common.LOG_CATEGORY);
                }

                do
                {
                    Thread.Sleep(1);
                    msSleep++;
                } while (servo.Engaged != true);

                if (LogActionVerification)
                {
                    Log.Trace($"Exit servo:{index} engaged:{servo.Engaged} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void VerifyNewPositionAchieved(AdvancedServoServo servo, Int32 index, double targetPosition)
        {
            Int64 startTicks = 0;
            var msSleep = 0;

            try
            {
                if (LogPerformanceAction)
                {
                    startTicks = Log.Trace($"Enter servo:{index} targetPosition:{targetPosition}", Common.LOG_CATEGORY);
                }

                //while (servo.Position != targetPosition)
                //{
                //    Thread.Sleep(1);
                //    msSleep++;
                //}

                // NOTE(crhodes)
                // Maybe poll velocity != 0
                do
                {
                    if (LogActionVerification) Log.Trace($"servo:{index}" +
                        $" - velocity:{servo.Velocity,8:0.000} position:{servo.Position,7:0.000}" +
                        $" - stopped:{servo.Stopped}", Common.LOG_CATEGORY);
                    Thread.Sleep(1);
                    msSleep++;
                }
                while (servo.Position != targetPosition);
                // NOTE(crhodes)
                // Stopped does not mean we got there.
                //while (! servo.Stopped ) ;

                if (LogActionVerification)
                {
                    Log.Trace($"Exit servo:{index} servoPosition:{servo.Position,7:0.000} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion
    }
}
