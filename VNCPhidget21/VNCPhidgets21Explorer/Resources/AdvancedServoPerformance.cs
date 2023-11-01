using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServoPerformance
    {
        public string Name { get; set; } = "PERFORMANCE NAME";

        public AdvancedServoStep[] AdvancedServoSteps { get; set; } = new[]
        {
            new AdvancedServoStep { ServoIndex = 0, TargetPosition = 90, Duration=1000 },
            new AdvancedServoStep { ServoIndex = 0, TargetPosition = 95, Duration=1000 },
            new AdvancedServoStep { ServoIndex = 0, TargetPosition = 100, Duration=2000 },
            new AdvancedServoStep { ServoIndex = 0, TargetPosition = 95, Duration=500 },
            new AdvancedServoStep { ServoIndex = 0, TargetPosition = 90, Duration=500 }
        };
    }
}
