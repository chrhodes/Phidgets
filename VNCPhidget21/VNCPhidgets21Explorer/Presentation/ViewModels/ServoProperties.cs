using System;

using Phidgets;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public partial class AdvancedServo1061ViewModel
    {
        public class ServoProperties : INPCBase
        {
            public AdvancedServoEx AdvancedServoEx { get; set; }

            public int ServoIndex { get; set; }

            private double _minimumPulseWidth = 1000;
            public double MinimumPulseWidth
            {
                get => _minimumPulseWidth;
                set
                {
                    if (_minimumPulseWidth == value) return;
                    _minimumPulseWidth = value;
                    OnPropertyChanged();
                }
            }

            private double _maximumPulseWidth = 1001;
            public double MaximumPulseWidth
            {
                get => _maximumPulseWidth;
                set
                {
                    if (_maximumPulseWidth == value) return;
                    _maximumPulseWidth = value;
                    OnPropertyChanged();
                }
            }

            private double _degrees;
            public double Degrees
            {
                get => _degrees;
                set
                {
                    if (_degrees == value) return;
                    _degrees = value;
                    OnPropertyChanged();
                }
            }

            private Phidgets.ServoServo.ServoType? _servoType;
            public Phidgets.ServoServo.ServoType? ServoType
            {
                get => _servoType;
                set
                {
                    // NOTE(crhodes)
                    // We always want to call UpdateProperties
                    //if (_servoType == value) return;
                    _servoType = value;
                    OnPropertyChanged();

                    if (AdvancedServoEx is not null)
                    {
                        if (AdvancedServoEx.AdvancedServo is not null)
                        {
                            AdvancedServoEx.AdvancedServo.servos[ServoIndex].Type = (Phidgets.ServoServo.ServoType)value;

                            // Need to update all the properties since the type changed
                            UpdateProperties();
                        }
                    }
                }
            }

            private void UpdateProperties()
            {
                Int64 startTicks = Log.Trace($"Enter servoIndex:{ServoIndex}", Common.LOG_CATEGORY);

                try
                {
                    var servo = AdvancedServoEx.AdvancedServo.servos[ServoIndex];

                    Stopped = servo.Stopped;
                    Engaged = servo.Engaged;
                    SpeedRamping = servo.SpeedRamping;
                    Current = servo.Current;

                    AccelerationMin = servo.AccelerationMin;
                    Acceleration = AccelerationMin;
                    AccelerationMax = servo.AccelerationMax;
                    VelocityMin = servo.VelocityMin;
                    Velocity = servo.Velocity;
                    // Make it possible to move servo without using UI to set non-zero velocity
                    VelocityLimit = servo.Velocity == 0 ? 10 : servo.Velocity;
                    VelocityMax = servo.VelocityMax;
                    PositionMin = DevicePositionMin = servo.PositionMin;
                    PositionMax = DevicePositionMax = servo.PositionMax;

                    Double? halfRange;
                    Double? percent = 0.20;
                    Double? midPoint;

                    midPoint = (DevicePositionMax - DevicePositionMin) / 2;
                    halfRange = midPoint * percent;
                    PositionMin = midPoint - halfRange;
                    PositionMax = midPoint + halfRange;
                    Position = midPoint;
                }
                catch (PhidgetException pex)
                {
                    Log.Error(pex, Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }

                Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }

            public void SetInitialProperties()
            {
                Int64 startTicks = Log.Trace($"Enter servoIndex:{ServoIndex}", Common.LOG_CATEGORY);

                try
                {
                    //var servo = AdvancedServoEx.AdvancedServo.servos[ServoIndex];
                    //ServoType = Phidgets.ServoServo.ServoType.DE;

                    Stopped = null;
                    Engaged = null;
                    SpeedRamping = null;
                    Current = null;

                    // NOTE(crhodes)
                    // Have to clear Acceleration before Min/Max as UI triggers an update
                    Acceleration = null;
                    AccelerationMin = null;
                    AccelerationMax = null;
                    // NOTE(crhodes)
                    // Handle VelocityLimit same way as Acceleration
                    // Have not confirmed this is an issue
                    VelocityLimit = null;
                    VelocityMin = null;
                    Velocity = null;
                    VelocityMax = null;

                    DevicePositionMin = null;
                    PositionMin = null;
                    Position = null;
                    PositionMax = null;
                    DevicePositionMax = null;
                }
                catch (PhidgetException pex)
                {
                    Log.Error(pex, Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }

                Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }

            private Double? _current;
            public Double? Current
            {
                get => _current;
                set
                {
                    if (_current == value) return;
                    _current = value;
                    OnPropertyChanged();
                }
            }

            private Double? _devicePositionMax;
            public Double? DevicePositionMax
            {
                get => _devicePositionMax;
                set
                {
                    if (_devicePositionMax == value) return;
                    _devicePositionMax = value;
                    OnPropertyChanged();
                }
            }

            private Double? _positionMax;
            public Double? PositionMax
            {
                get => _positionMax;
                set
                {
                    if (_positionMax == value) return;
                    _positionMax = value;
                    OnPropertyChanged();

                    if (value is not null)
                    {
                        try
                        {
                            if (AdvancedServoEx.AdvancedServo.servos[ServoIndex].PositionMax != value)
                            {
                                AdvancedServoEx.SetPositionMax(
                                    (Double)value,
                                    AdvancedServoEx.AdvancedServo.servos[ServoIndex], ServoIndex);
                            }
                        }
                        catch (PhidgetException pex)
                        {
                            Log.Error(pex, Common.LOG_CATEGORY);
                            AdvancedServoEx.AdvancedServo.servos[ServoIndex].Position = (double)value;
                        }
                    }
                }
            }

            private Double? _position;
            public Double? Position
            {
                get => _position;
                set
                {
                    if (_position == value)
                        return;

                    //if (value < PositionMin_ || value > PositionMax_)
                    //{
                    //    return;
                    //}

                    _position = value;
                    OnPropertyChanged();

                    if (value is not null)
                    {
                        if (AdvancedServoEx.AdvancedServo.servos[ServoIndex].Engaged)
                        {
                            // Do not set position until servo is engaged.
                            try
                            {
                                if (AdvancedServoEx.AdvancedServo.servos[ServoIndex].Position != value)
                                {
                                    AdvancedServoEx.SetPosition(
                                        (Double)value,
                                        AdvancedServoEx.AdvancedServo.servos[ServoIndex]);
                                }
                            }
                            catch (PhidgetException pex)
                            {
                                Log.Error(pex, Common.LOG_CATEGORY);
                                AdvancedServoEx.AdvancedServo.servos[ServoIndex].Position = (double)value;
                            }
                        }
                    }
                }
            }

            private Double? _positionMin;
            public Double? PositionMin
            {
                get => _positionMin;
                set
                {
                    if (_positionMin == value) return;
                    _positionMin = value;
                    OnPropertyChanged();

                    if (value is not null)
                    {
                        try
                        {
                            if (AdvancedServoEx.AdvancedServo.servos[ServoIndex].PositionMin != value)
                            {
                                AdvancedServoEx.SetPositionMin(
                                    (Double)value,
                                    AdvancedServoEx.AdvancedServo.servos[ServoIndex], ServoIndex);
                            }
                        }
                        catch (PhidgetException pex)
                        {
                            Log.Error(pex, Common.LOG_CATEGORY);
                            AdvancedServoEx.AdvancedServo.servos[ServoIndex].Position = (double)value;
                        }
                    }
                }
            }

            private Double? _devicePositionMin;
            public Double? DevicePositionMin
            {
                get => _devicePositionMin;
                set
                {
                    if (_devicePositionMin == value) return;
                    _devicePositionMin = value;
                    OnPropertyChanged();
                }
            }

            private int _positionRange = 10;
            public int PositionRange
            {
                get => _positionRange;
                set
                {
                    if (_positionRange == value) return;
                    _positionRange = value;
                    OnPropertyChanged();
                }
            }

            private Double? _velocityMin;
            public Double? VelocityMin
            {
                get => _velocityMin;
                set
                {
                    if (_velocityMin == value) return;
                    _velocityMin = value;
                    OnPropertyChanged();
                }
            }

            private Double? _velocity;
            public Double? Velocity
            {
                get => _velocity;
                set
                {
                    if (_velocity == value) return;
                    _velocity = value;
                    OnPropertyChanged();
                }
            }

            private Double? _velocityLimit;
            public Double? VelocityLimit
            {
                get => _velocityLimit;
                set
                {
                    if (_velocityLimit == value) return;
                    _velocityLimit = value;
                    OnPropertyChanged();

                    if (value is not null)
                    {
                        AdvancedServoEx.SetVelocityLimit(
                            (Double)value,
                            AdvancedServoEx.AdvancedServo.servos[ServoIndex]);
                    }
                }
            }

            private Double? _velocityMax;
            public Double? VelocityMax
            {
                get => _velocityMax;
                set
                {
                    if (_velocityMax == value) return;
                    _velocityMax = value;
                    OnPropertyChanged();
                }
            }

            private Double? _accelerationMin;
            public Double? AccelerationMin
            {
                get => _accelerationMin;
                set
                {
                    if (_accelerationMin == value) return;
                    _accelerationMin = value;
                    OnPropertyChanged();
                }
            }

            private Double? _acceleration;
            public Double? Acceleration
            {
                get => _acceleration;
                set
                {
                    if (_acceleration == value) return;
                    _acceleration = value;
                    OnPropertyChanged();

                    if (value is not null)
                    {
                        AdvancedServoEx.SetAcceleration(
                            (Double)value,
                            AdvancedServoEx.AdvancedServo.servos[ServoIndex]);
                    }
                }
            }

            private Double? _accelerationMax;
            public Double? AccelerationMax
            {
                get => _accelerationMax;
                set
                {
                    if (_accelerationMax == value) return; 
                    _accelerationMax = value;
                    OnPropertyChanged();
                }
            }

            private bool? _engaged;
            public bool? Engaged
            {
                get => _engaged;
                set
                {
                    if (_engaged == value) return;
                    _engaged = value;
                    OnPropertyChanged();

                    if (value is not null) AdvancedServoEx.AdvancedServo.servos[ServoIndex].Engaged = (Boolean)value;
                }
            }

            private bool? _speedRamping;
            public bool? SpeedRamping
            {
                get => _speedRamping;
                set
                {
                    if (_speedRamping == value) return;
                    _speedRamping = value;
                    OnPropertyChanged();
                }
            }

            private bool? _stopped;
            public bool? Stopped
            {
                get => _stopped;
                set
                {
                    if (_stopped == value) return;
                    _stopped = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
