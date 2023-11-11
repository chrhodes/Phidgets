using System;

namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServoPerformance
    {
        public AdvancedServoSequence[] AdvancedServoSequence { get; set; } = new[] // AdvancedServoSequence[0];
        {
            new AdvancedServoSequence { Host = new Host(), Name="SequenceName", Description="", AdvancedServoServoActions = new AdvancedServoServoAction[0] },
            new AdvancedServoSequence { Host = new Host(), Name="SequenceName", Description="", AdvancedServoServoActions = new AdvancedServoServoAction[0] }
        };
    }
}
