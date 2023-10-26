using System;
using System.Collections.Generic;
using System.Linq;

namespace VNCPhidgets21Explorer.Resources
{
    public class Stepper
    {
        public string Name { get; set; } = "STEPPER NAME";
        public int SerialNumber { get; set; } = 12345;
        public bool Enable { get; set; } = true;
        public bool Embedded { get; set; } = false;
    }
}
