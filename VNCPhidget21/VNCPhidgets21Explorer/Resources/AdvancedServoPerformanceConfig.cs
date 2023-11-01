﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServoPerformanceConfig
    {
        public AdvancedServoPerformance[] AdvancedServoPerformances { get; set; } = new[]
        {
            new AdvancedServoPerformance
            {
                Name="PerformanceServo0",

                AdvancedServoSteps = new[]
                {
                    new AdvancedServoStep { ServoIndex = 0, TargetPosition = 90, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 0, TargetPosition = 95, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 0, TargetPosition = 100, Duration=2000 },
                    new AdvancedServoStep { ServoIndex = 0, TargetPosition = 95, Duration=500 },
                    new AdvancedServoStep { ServoIndex = 0, TargetPosition = 90, Duration=500 }
                }
            },
            new AdvancedServoPerformance
            {
                Name="PerformanceServp1",

                AdvancedServoSteps = new[]
                {
                    new AdvancedServoStep { ServoIndex = 1, TargetPosition = 90, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 1, TargetPosition = 95, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 1, TargetPosition = 100, Duration=2000 },
                    new AdvancedServoStep { ServoIndex = 1, TargetPosition = 95, Duration=500 },
                    new AdvancedServoStep { ServoIndex = 1, TargetPosition = 90, Duration=500 }
                }
            },
            new AdvancedServoPerformance
            {
                Name="PerformanceServo2",

                AdvancedServoSteps = new[]
                {
                    new AdvancedServoStep { ServoIndex = 2, TargetPosition = 90, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 2, TargetPosition = 95, Duration=1000 },
                    new AdvancedServoStep { ServoIndex = 2, TargetPosition = 100, Duration=2000 },
                    new AdvancedServoStep { ServoIndex = 2, TargetPosition = 95, Duration=500 },
                    new AdvancedServoStep { ServoIndex = 2, TargetPosition = 90, Duration=500 }
                }
            }
        };
    }
}