﻿using Prism.Events;

namespace VNC.Phidget
{
    public class Common
    {
        public const string APPLICATION_NAME = "VNCPhidget";
        public const string LOG_CATEGORY = "VNCPhidget";

        public static int PhidgetOpenTimeout { get; set; } = 1000; // ms
    }
}
