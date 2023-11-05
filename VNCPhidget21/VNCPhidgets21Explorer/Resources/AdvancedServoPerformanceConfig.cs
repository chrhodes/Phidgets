namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServoPerformanceConfig
    {
        public AdvancedServoPerformance[] AdvancedServoPerformances { get; set; } = new[]
        {
            new AdvancedServoPerformance
            {
                Name="PerformanceServo0",
                ContinueWith="PerformanceServo1",

                AdvancedServoSteps = new[]
                {
                    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 90, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 95, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 100, Duration=2000 },
                    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 95, Duration=500 },
                    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 90, Duration=500 }
                }
            },
            new AdvancedServoPerformance
            {
                Name="PerformanceServo1",
                ContinueWith="PerformanceServo2",

                AdvancedServoSteps = new[]
                {
                    new AdvancedServoStep { ServoIndex = 1, Engaged = true, TargetPosition = 90, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 1, Engaged = true, TargetPosition = 95, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 1, Engaged = true, TargetPosition = 100, Duration=2000 },
                    new AdvancedServoStep { ServoIndex = 1, Engaged = true, TargetPosition = 95, Duration=500 },
                    new AdvancedServoStep { ServoIndex = 1, Engaged = true, TargetPosition = 90, Duration=500 }
                }
            },
            new AdvancedServoPerformance
            {
                Name="PerformanceServo2",

                AdvancedServoSteps = new[]
                {
                    new AdvancedServoStep { ServoIndex = 2, Engaged = true, TargetPosition = 90, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 2, Engaged = true, TargetPosition = 95, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 2, Engaged = true, TargetPosition = 100, Duration=2000 },
                    new AdvancedServoStep { ServoIndex = 2, Engaged = true, TargetPosition = 95, Duration=500 },
                    new AdvancedServoStep { ServoIndex = 2, Engaged = true, TargetPosition = 90, Duration=500 }
                }
            }
        };
    }
}
