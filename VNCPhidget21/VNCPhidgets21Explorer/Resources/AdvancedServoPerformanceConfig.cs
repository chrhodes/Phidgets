namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServoPerformanceConfig
    {
        public AdvancedServoPerformance[] AdvancedServoPerformances { get; set; } = new[]
        {
            new AdvancedServoPerformance
            {
                Name = "Performance0",
                Description = "Performance0 Description",
                Loops = 1,
                PlayInParallel = false,
                ContinueWith = "",

                AdvancedServoSequences = new[]
                {
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo0",
                        ContinueWith="SequenceServo1",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = true, TargetPosition = 90, Duration=1000 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = true, TargetPosition = 95, Duration=1000 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = true, TargetPosition = 100, Duration=2000 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = true, TargetPosition = 95, Duration=500 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = true, TargetPosition = 90, Duration=500 }
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo1",
                        ContinueWith="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = true, TargetPosition = 90, Duration=1000 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = true, TargetPosition = 95, Duration=1000 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = true, TargetPosition = 100, Duration=2000 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = true, TargetPosition = 95, Duration=500 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = true, TargetPosition = 90, Duration=500 }
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = true, TargetPosition = 90, Duration=1000 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = true, TargetPosition = 95, Duration=1000 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = true, TargetPosition = 100, Duration=2000 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = true, TargetPosition = 95, Duration=500 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = true, TargetPosition = 90, Duration=500 }
                        }
                    }
                }
            }
        };
    }
}
