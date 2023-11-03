using System;

namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServoPerformance
    {
        /// <summary>
        /// Name of performance
        /// </summary>
        public string Name { get; set; } = "PERFORMANCE NAME";

        /// <summary>
        /// Description of performance
        /// </summary>
        public string Description { get; set; } = "PERFORMANCE DESCRIPTION";

        /// <summary>
        /// Number of loops of Performance
        /// </summary>
        public Int32 Loops { get; set; } = 1;

        /// <summary>
        /// Name of performance to invoke at end of performance (optional)
        /// none, null, or empty string to stop
        /// </summary>
        public string? ContinueWith { get; set; } = "CONTINUE WITH PERFORMANCE NAME";

        /// <summary>
        /// Array of steps in performance
        /// </summary>
        public AdvancedServoStep[] AdvancedServoSteps { get; set; } = new AdvancedServoStep[0];
        //{
        //    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 90, Duration=1000 },
        //    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 95, Duration=1000 },
        //    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 100, Duration=2000 },
        //    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 95, Duration=500 },
        //    new AdvancedServoStep { ServoIndex = 0, Engaged = true, TargetPosition = 90, Duration=500 }
        //};
    }
}
