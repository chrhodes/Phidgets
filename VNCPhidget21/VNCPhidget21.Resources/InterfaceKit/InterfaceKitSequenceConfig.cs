namespace VNCPhidgets21Explorer.Configuration
{
    public class InterfaceKitSequenceConfig
    {
        public InterfaceKitSequence[] InterfaceKitSequences { get; set; } = new[]
        {
            new InterfaceKitSequence
            {
                Host = new Host
                {
                    Name = "localhost",
                    IPAddress = "127.0.0.1",
                    Port = 5001,
                    InterfaceKits = new[]
                    {
                        new InterfaceKit { Name = "InterfaceKit 1", SerialNumber = 124744, Embedded = false, Open = true }
                    },
                },
                Name="localhost_SequenceIK 1",

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                Host = new Host
                {
                    Name = "psbc11",
                    IPAddress = "192.168.150.11",
                    Port = 5001,
                    InterfaceKits = new[]
                    {
                        new InterfaceKit { Name = "psbc11 InterfaceKit", SerialNumber = 46049, Embedded = true, Open = true }
                    },
                },
                Name="psbc11_SequenceIK 1",

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                Host = new Host
                {
                    Name = "psbc21",
                    IPAddress = "192.168.150.21",
                    Port = 5001,
                    InterfaceKits = new[]
                    {
                        new InterfaceKit { Name = "psbc21 InterfaceKit", SerialNumber = 48301, Embedded = true, Open = true }
                    },
                },
                Name="psbc21_SequenceIK 1",
                NextSequence = new PerformanceSequence { Name = "psbc22_SequenceIK 1", SequenceType = "IK", Loops = 1 },

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                Host = new Host
                {
                    Name = "psbc21",
                    IPAddress = "192.168.150.21",
                    Port = 5001,
                    InterfaceKits = new[]
                    {
                        new InterfaceKit { Name = "psbc21 InterfaceKit", SerialNumber = 48301, Embedded = true, Open = true }
                    },
                },
                Name="psbc21_SequenceIK 1 Parallel",
                PlayActionsInParallel = true,

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                Host = new Host
                {
                    Name = "psbc22",
                    IPAddress = "192.168.150.22",
                    Port = 5001,
                    InterfaceKits = new[]
                    {
                        new InterfaceKit { Name = "psbc22 InterfaceKit", SerialNumber = 251831, Embedded = true, Open = true },
                    },
                },
                Name="psbc22_SequenceIK 1",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceIK 1", SequenceType = "IK", Loops = 1 },

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                Host = new Host
                {
                    Name = "psbc23",
                    IPAddress = "192.168.150.23",
                    Port = 5001,
                    InterfaceKits = new[]
                    {
                        new InterfaceKit { Name = "psbc23 InterfaceKit", SerialNumber = 48284, Embedded = true, Open = true }
                    },
                },
                Name="psbc23_SequenceIK 1",

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
        };
    }
}