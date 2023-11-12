using System;

namespace VNCPhidgets21Explorer.Resources
{
    public class PerformanceConfig
    {
        /// <summary>
        /// Name of Sequence
        /// </summary>
        public string Name { get; set; } = "PERFORMANCE NAME";

        /// <summary>
        /// Description of Sequence
        /// </summary>
        public string Description { get; set; } = "PERFORMANCE DESCRIPTION";

        /// <summary>
        /// Number of loops of Sequence
        /// </summary>
        public Int32 Loops { get; set; } = 1;

        /// <summary>
        /// Name of performanceSequence to invoke at end of performanceSequence (optional)
        /// none or null to stop
        /// </summary>
        public PerformanceSequence? ContinueWith { get; set; }

        public PerformanceSequence[] PerformanceSequences { get; set; } = new[] // PerformanceSequence[0];
{
            new PerformanceSequence
            { 
                Name = "AdvancedServoSequence 1",
                Description = "AdvancedServoSequence 1 Description",
                Loops = 1,
                SequenceType = "AS",
                ContinueWith = new PerformanceSequence { Name = "AdvancedServoSequence 2", SequenceType = "AS"}
            },
            new PerformanceSequence
            {
                Name = "AdvancedServoSequence 2",
                Description = "AdvancedServoSequence 2 Description",
                Loops = 1,
                SequenceType = "AS",
                ContinueWith = null
            }
        };
    }
}
