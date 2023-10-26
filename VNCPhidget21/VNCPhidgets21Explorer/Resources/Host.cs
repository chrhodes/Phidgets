namespace VNCPhidgets21Explorer.Resources
{
    public class Host
    {
        // TODO(crhodes)
        // Think through whether we need any initializaion values here

        public string Name { get; set; } = "HOST NAME";
        public string IPAddress { get; set; } = "192.168.150.1";
        public int Port { get; set; } = 5001;
        public bool Enable { get; set; } = true;

        public AdvancedServo[] AdvancedServos { get; set; } = new[]
        {
            new AdvancedServo { SerialNumber = 1234, Embedded = true, Enable = true },
            new AdvancedServo { SerialNumber = 5678, Embedded = false, Enable = false }
        };

        public InterfaceKit[] InterfaceKits { get; set; } = new[]
        {
            new InterfaceKit { SerialNumber = 1234, Embedded = true, Enable = true },
            new InterfaceKit { SerialNumber = 5678, Embedded = false, Enable = false }
        };

        public Stepper[] Steppers { get; set; } = new[]
        {
            new Stepper { SerialNumber = 1234, Embedded = true, Enable = true },
            new Stepper { SerialNumber = 5678, Embedded = false, Enable = false }
        };
    }
}
