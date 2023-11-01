using System;

namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServoStep
    {
        public int ServoIndex { get; set; }

        public bool? Engaged { get; set; }
        public double TargetPosition { get; set; }
        public Int32 Duration { get; set; } = 1000;    // ms
    }
}
