namespace VNCPhidget21.Configuration
{
    public class AdvancedServoSequenceConfig
    {
        public AdvancedServoSequence[] AdvancedServoSequences { get; set; } = new[]
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
                        new AdvancedServo { Name = "AdvancedServo 1", SerialNumber = 99415, Open = true }
                    }
                },
                Name="localhost_SequenceServo0",
                NextSequence = new PerformanceSequence { Name = "localhost_SequenceServo1", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="localhost_SequenceServo1",
                NextSequence = new PerformanceSequence { Name = "localhost_SequenceServo2", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="localhost_SequenceServo2",
                NextSequence = new PerformanceSequence { Name = "localhost_SequenceServoFin", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="localhost_SequenceServoFin",

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Host = new Host
                {
                    Name = "psbc11",
                    IPAddress = "192.168.150.11",
                    Port = 5001,
                    AdvancedServos = new[]
                    {
                        new AdvancedServo { Name = "AdvancedServo 1", SerialNumber = 99415, Open = true }
                    }
                },
                Name="psbc11_SequenceServo0",
                NextSequence = new PerformanceSequence { Name = "psbc11_SequenceServo1", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc11_SequenceServo1",
                NextSequence = new PerformanceSequence { Name = "psbc11_SequenceServo2", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc11_SequenceServo2",
                NextSequence = new PerformanceSequence { Name = "psbc11_SequenceServoFin", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc11_SequenceServoFin",

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Host = new Host
                {
                    Name = "psbc21",
                    IPAddress = "192.168.150.21",
                    Port = 5001, 
                    AdvancedServos = new[] 
                    { 
                        new AdvancedServo { Name = "AdvancedServo 1", SerialNumber = 99415, Open = true }
                    }
                },
                Name="psbc21_SequenceServo0",
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo1", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo1",
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo2", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo2",
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServoFin", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Host = new Host
                {
                    Name = "psbc21",
                    IPAddress = "192.168.150.21",
                    Port = 5001,
                    AdvancedServos = new[]
                    {
                        new AdvancedServo { Name = "AdvancedServo 1", SerialNumber = 99415, Open = true }
                    }
                },
                Name="psbc21_SequenceServo0P Configure and Engage",
                Loops = 5,
                ExecuteActionsInParallel = true,
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo1P", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo1P",
                ExecuteActionsInParallel = true,
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo2P", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },

                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo2P",
                ExecuteActionsInParallel = true,
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo3P", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },

                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo3P",
                ExecuteActionsInParallel = true,
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo4P", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },

                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo4P",
                ExecuteActionsInParallel = true,
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo5P", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo5P",
                ExecuteActionsInParallel = true,
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServoFin", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServoFin",

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Host = new Host
                {
                    Name = "psbc22",
                    IPAddress = "192.168.150.22",
                    Port = 5001,
                    AdvancedServos = new[]
                    {
                        new AdvancedServo { Name = "AdvancedServo 1", SerialNumber = 99415, Open = true }
                    }
                },
                Name="psbc22_SequenceServo0",
                NextSequence = new PerformanceSequence { Name = "psbc22_SequenceServo1", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc22_SequenceServo1",
                NextSequence = new PerformanceSequence { Name = "psbc22_SequenceServo2", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc22_SequenceServo2",
                NextSequence = new PerformanceSequence { Name = "psbc22_SequenceServoFin", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc22_SequenceServoFin",

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Host = new Host
                {
                    Name = "psbc23",
                    IPAddress = "192.168.150.23",
                    Port = 5001,
                    AdvancedServos = new[]
                    {
                        new AdvancedServo { Name = "AdvancedServo 1", SerialNumber = 99415, Open = true }
                    }
                },
                Name="psbc23_SequenceServo0",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceServo1", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc23_SequenceServo1",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceServo2", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc23_SequenceServo2",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceServoFin", SequenceType = "AS", Loops = 1 },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc23_SequenceServoFin",

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            }
        };
    }
}
