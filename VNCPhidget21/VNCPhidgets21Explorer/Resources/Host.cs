namespace VNCPhidgets21Explorer.Resources
{
    public class Host
    {
        public string Name { get; set; } = "HOST NAME";
        public string IPAddress { get; set; } = "192.168.150.1";
        public int Port { get; set; } = 5001;
        public bool Enable { get; set; } = true;

        public InterfaceKit[] InterfaceKits { get; set; } = new[]
        {
            new InterfaceKit { SerialNumber = 1234, Embedded = true, Enable = true },
            new InterfaceKit { SerialNumber = 5678, Embedded = false, Enable = false }
        };
    }
}
