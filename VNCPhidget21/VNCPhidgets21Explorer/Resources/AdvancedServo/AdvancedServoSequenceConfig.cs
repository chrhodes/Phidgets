

namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServoSequenceConfig
    {
        public AdvancedServoSequence[] AdvancedServoSequences = new[]
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
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
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
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
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
                    new AdvancedServoServoAction { ServoIndex = 4, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo1",
                ContinueWith="SequenceServo2",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 5, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo2",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 6, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, Engaged = false },
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
                Name="SequenceServo0",
                ContinueWith="SequenceServo1",

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
                Name="SequenceServo1",
                ContinueWith="SequenceServo2",

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
                Name="SequenceServo2",

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
                    new AdvancedServoServoAction { ServoIndex = 4, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo1",
                ContinueWith="SequenceServo2",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 5, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo2",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 6, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, Engaged = false },
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
                Name="SequenceServo0",
                ContinueWith="SequenceServo1",

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
                Name="SequenceServo1",
                ContinueWith="SequenceServo2",

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
                Name="SequenceServo2",

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
                Name="SequenceServo0",
                ContinueWith="SequenceServo1",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 4, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo1",
                ContinueWith="SequenceServo2",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 5, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, Engaged = false },
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
                Name="SequenceServo0P Configure and Engage",
                PlayInParallel = true,
                ContinueWith = "SequenceServo1P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo1P",
                PlayInParallel = true,
                ContinueWith="SequenceServo2P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },

                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo2P",
                PlayInParallel = true,
                ContinueWith="SequenceServo3P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },

                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo3P",
                PlayInParallel = true,
                ContinueWith="SequenceServo4P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },

                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo4P",
                PlayInParallel = true,
                ContinueWith="SequenceServo5P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo5P",
                PlayInParallel = true,
                ContinueWith="SequenceServoFin",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServoFin",

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
                Name="SequenceServo0P Configure and Engage",
                PlayInParallel = true,
                ContinueWith = "SequenceServo1P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 4, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 5, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 6, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo1P",
                PlayInParallel = true,
                ContinueWith="SequenceServo2P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },

                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo2P",
                PlayInParallel = true,
                ContinueWith="SequenceServo3P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },

                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo3P",
                PlayInParallel = true,
                ContinueWith="SequenceServo4P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 110 },

                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo4P",
                PlayInParallel = true,
                ContinueWith="SequenceServo5P",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo5P",
                PlayInParallel = true,
                ContinueWith="SequenceServoFin",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServoFin",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 4, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 5, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 6, Engaged = false },
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
                Name="SequenceServo0",
                ContinueWith="SequenceServo1",

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
                Name="SequenceServo1",
                ContinueWith="SequenceServo2",

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
                Name="SequenceServo2",

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
                    new AdvancedServoServoAction { ServoIndex = 4, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo1",
                ContinueWith="SequenceServo2",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 5, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo2",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 6, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, Engaged = false },
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
                Name="SequenceServo0",
                ContinueWith="SequenceServo1",

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
                Name="SequenceServo1",
                ContinueWith="SequenceServo2",

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
                Name="SequenceServo2",

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
                    new AdvancedServoServoAction { ServoIndex = 4, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 4, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 4, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo1",
                ContinueWith="SequenceServo2",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 5, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 5, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 5, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="SequenceServo2",

                AdvancedServoServoActions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 6, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 6, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 6, Engaged = false },
                }
            }
        };
    }
}
