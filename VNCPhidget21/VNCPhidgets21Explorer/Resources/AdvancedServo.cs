namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServo
    {
        public string Name { get; set; } = "ADVANCEDSERVO NAME";
        public int SerialNumber { get; set; } = 12345;
        // NOTE(crhodes)
        // How do we use Enable?  If just like open, remove
        //public bool? Enable { get; set; } = true;
        public bool Open { get; set; } = true;
    }
}
