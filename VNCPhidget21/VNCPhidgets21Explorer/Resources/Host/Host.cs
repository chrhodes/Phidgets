namespace VNCPhidgets21Explorer.Resources
{
    public class Host
    {
        public string Name { get; set; } = "HOST NAME";
        public string IPAddress { get; set; } = "192.168.150.1";
        public int Port { get; set; } = 5001;
        public bool Enable { get; set; } = true;

        public AdvancedServo[] AdvancedServos { get; set; } = new[]
        {
            new AdvancedServo { SerialNumber = 1234, Open = true },
            new AdvancedServo { SerialNumber = 5678, Open = false }
        };

        public InterfaceKit[] InterfaceKits { get; set; } = new[]
        {
            new InterfaceKit { SerialNumber = 1234, Embedded = true, Open = true },
            new InterfaceKit { SerialNumber = 5678, Embedded = false, Open = false }
        };

        public Stepper[] Steppers { get; set; } = new[]
        {
            new Stepper { SerialNumber = 1234, Open = true },
            new Stepper { SerialNumber = 5678, Open = false }
        };
    }
}

