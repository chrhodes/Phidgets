using System;

namespace VNCPhidgets21Explorer.Resources
{
    public class AdvancedServoPerformance
    {
        /// <summary>
        /// Name of Performance
        /// </summary>
        public string Name { get; set; } = "PERFORMANCE NAME";

        /// <summary>
        /// Description of Performance
        /// </summary>
        public string Description { get; set; } = "PERFORMANCE DESCRIPTION";

        /// <summary>
        /// Number of loops of Performance
        /// </summary>
        public Int32 Loops { get; set; } = 1;

        /// <summary>
        /// Play AdvancedServoSequence[] in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlayInParallel { get; set; } = false;

        /// <summary>
        /// Name of performance to invoke at end of performance (optional)
        /// none, null, or empty string to stop
        /// </summary>
        public string? ContinueWith { get; set; } = "CONTINUE WITH PERFORMANCE NAME";

        public AdvancedServoSequence[] AdvancedServoSequences { get; set; } = new[] // AdvancedServoSequence[0];
        {
            new AdvancedServoSequence
            { 
                Host = new Host(), 
                Name="SequenceName", 
                Description="", 
                AdvancedServoServoActions = new AdvancedServoServoAction[0]
            },
            new AdvancedServoSequence 
            { 
                Host = new Host(), 
                Name="SequenceName", 
                Description="", 
                AdvancedServoServoActions = new AdvancedServoServoAction[0]
            }
        };
    }
}
