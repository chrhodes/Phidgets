namespace VNCPhidgets21Explorer.Resources
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
                        new AdvancedServo
                        {
                            Name = "AdvancedServo 1",
                            SerialNumber = 99415,
                            Open = true
                        }
                    }
                },
                Name="localhost_SequenceServo0",
                ContinueWith="localhost_SequenceServo1",

                AdvancedServoServoActions = new[]
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
                ContinueWith="localhost_SequenceServo2",

                AdvancedServoServoActions = new[]
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
                ContinueWith = "localhost_SequenceServoFin",

                AdvancedServoServoActions = new[]
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

                AdvancedServoServoActions = new[]
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
                        new AdvancedServo
                        {
                            Name = "AdvancedServo 1",
                            SerialNumber = 99415,
                            Open = true
                        }
                    }
                },
                Name="psbc11_SequenceServo0",
                ContinueWith="psbc11_SequenceServo1",

                AdvancedServoServoActions = new[]
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
                ContinueWith="psbc11_SequenceServo2",

                AdvancedServoServoActions = new[]
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
                ContinueWith="psbc11_SequenceServoFin",

                AdvancedServoServoActions = new[]
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

                AdvancedServoServoActions = new[]
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
                        new AdvancedServo
                        {
                            Name = "AdvancedServo 1",
                            SerialNumber = 99415,
                            Open = true
                        }
                    }
                },
                Name="psbc21_SequenceServo0",
                ContinueWith="psbc21_SequenceServo1",

                AdvancedServoServoActions = new[]
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
                ContinueWith="psbc21_SequenceServo2",

                AdvancedServoServoActions = new[]
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
                ContinueWith="psbc21_SequenceServoFin",

                AdvancedServoServoActions = new[]
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
                        new AdvancedServo
                        {
                            Name = "AdvancedServo 1",
                            SerialNumber = 99415,
                            Open = true
                        }
                    }
                },
                Name="psbc21_SequenceServo0P Configure and Engage",
                Loops = 5,
                PlayInParallel = true,
                ContinueWith = "psbc21_SequenceServo1P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo1P",
                PlayInParallel = true,
                ContinueWith="psbc21_SequenceServo2P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },

                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo2P",
                PlayInParallel = true,
                ContinueWith="psbc21_SequenceServo3P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },

                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo3P",
                PlayInParallel = true,
                ContinueWith="psbc21_SequenceServo4P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },

                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo4P",
                PlayInParallel = true,
                ContinueWith="psbc21_SequenceServo5P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo5P",
                PlayInParallel = true,
                ContinueWith="psbc21_SequenceServoFin",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServoFin",

                AdvancedServoServoActions = new[]
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
                        new AdvancedServo
                        {
                            Name = "AdvancedServo 1",
                            SerialNumber = 99415,
                            Open = true
                        }
                    }
                },
                Name="psbc22_SequenceServo0",
                ContinueWith="psbc22_SequenceServo1",

                AdvancedServoServoActions = new[]
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
                ContinueWith="psbc22_SequenceServo2",

                AdvancedServoServoActions = new[]
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
                ContinueWith = "psbc22_SequenceServoFin",

                AdvancedServoServoActions = new[]
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

                AdvancedServoServoActions = new[]
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
                        new AdvancedServo
                        {
                            Name = "AdvancedServo 1",
                            SerialNumber = 99415,
                            Open = true
                        }
                    }
                },
                Name="psbc23_SequenceServo0",
                ContinueWith="psbc23_SequenceServo1",

                AdvancedServoServoActions = new[]
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
                ContinueWith="psbc23_SequenceServo2",

                AdvancedServoServoActions = new[]
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
                ContinueWith = "psbc23_SequenceServoFin",

                AdvancedServoServoActions = new[]
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

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            }
        };
    }
}
