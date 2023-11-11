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
                        Host = new Host
                        {
                            Name = "localhost",
                            IPAddress = "127.0.0.1",
                            Port = 5001,
                            AdvancedServos = new[]
                            {
                                new AdvancedServo
                                {
                                    Name = "AdvancedServo 1",
                                    SerialNumber = 99415,
                                    Open = true
                                }
                            }
                        },
                        Name="SequenceServo0",
                        ContinueWith="SequenceServo1",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo1",
                        ContinueWith="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                        }
                    }
                }
            },
            new AdvancedServoPerformance
            {
                Name = "Performance1",
                Description = "Performance1 Description",
                Loops = 1,
                PlayInParallel = false,
                ContinueWith = "",

                AdvancedServoSequences = new[]
                {
                    new AdvancedServoSequence
                    {
                        Host = new Host
                        {
                            Name = "psbc11",
                            IPAddress = "192.168.150.11",
                            Port = 5001,
                            AdvancedServos = new[]
                            {
                                new AdvancedServo
                                {
                                    Name = "AdvancedServo 1",
                                    SerialNumber = 99415,
                                    Open = true
                                }
                            }
                        },
                        Name="SequenceServo0",
                        ContinueWith="SequenceServo1",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo1",
                        ContinueWith="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                        }
                    }
                }
            },
            new AdvancedServoPerformance
            {
                Name = "Performance2",
                Description = "Performance2 Description",
                Loops = 1,
                PlayInParallel = false,
                ContinueWith = "",

                AdvancedServoSequences = new[]
                {
                    new AdvancedServoSequence
                    {
                        Host = new Host
                        {
                            Name = "psbc21",
                            IPAddress = "192.168.150.21",
                            Port = 5001,
                            AdvancedServos = new[]
                            {
                                new AdvancedServo
                                {
                                    Name = "AdvancedServo 1",
                                    SerialNumber = 99415,
                                    Open = true
                                }
                            }
                        },
                        Name="SequenceServo0",
                        ContinueWith="SequenceServo1",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo1",
                        ContinueWith="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                        }
                    }
                }
            },
            new AdvancedServoPerformance
            {
                Name = "Performance2 Parallel",
                Description = "Performance2 Parallel",
                Loops = 1,
                PlayInParallel = false,
                ContinueWith = "",

                AdvancedServoSequences = new[]
                {
                    new AdvancedServoSequence
                    {
                        Host = new Host
                        {
                            Name = "psbc21",
                            IPAddress = "192.168.150.21",
                            Port = 5001,
                            AdvancedServos = new[]
                            {
                                new AdvancedServo
                                {
                                    Name = "AdvancedServo 1",
                                    SerialNumber = 99415,
                                    Open = true
                                }
                            }
                        },
                        Name="SequenceServo0P",
                        PlayInParallel= true,
                        ContinueWith="SequenceServo1",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo1P",
                        ContinueWith="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo2P",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                        }
                    }
                }
            },
            new AdvancedServoPerformance
            {
                Name = "Performance3",
                Description = "Performance3 Description",
                Loops = 1,
                PlayInParallel = false,
                ContinueWith = "",

                AdvancedServoSequences = new[]
                {
                    new AdvancedServoSequence
                    {
                        Host = new Host
                        {
                            Name = "psbc22",
                            IPAddress = "192.168.150.22",
                            Port = 5001,
                            AdvancedServos = new[]
                            {
                                new AdvancedServo
                                {
                                    Name = "AdvancedServo 1",
                                    SerialNumber = 99415,
                                    Open = true
                                }
                            }
                        },
                        Name="SequenceServo0",
                        ContinueWith="SequenceServo1",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo1",
                        ContinueWith="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                        }
                    }
                }
            },
            new AdvancedServoPerformance
            {
                Name = "Performance4",
                Description = "Performance4 Description",
                Loops = 1,
                PlayInParallel = false,
                ContinueWith = "",

                AdvancedServoSequences = new[]
                {
                    new AdvancedServoSequence
                    {
                        Host = new Host
                        {
                            Name = "psbc23",
                            IPAddress = "192.168.150.23",
                            Port = 5001,
                            AdvancedServos = new[]
                            {
                                new AdvancedServo
                                {
                                    Name = "AdvancedServo 1",
                                    SerialNumber = 99415,
                                    Open = true
                                }
                            }
                        },
                        Name="SequenceServo0",
                        ContinueWith="SequenceServo1",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo1",
                        ContinueWith="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                        }
                    },
                    new AdvancedServoSequence
                    {
                        Name="SequenceServo2",

                        AdvancedServoServoActions = new[]
                        {
                            new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 95 },
                            new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                            new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                        }
                    }
                }
            }
        };
    }
}
