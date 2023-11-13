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
        public PerformanceSequence? NextPerformance { get; set; }

        public PerformanceSequence[] PerformanceSequences { get; set; } = new[] // PerformanceSequence[0];
        {
            new PerformanceSequence
            { 
                Name = "psbc21_SequenceServo0",
                Description = "psbc21_SequenceServo0 1 Description",
                Loops = 1,
                SequenceType = "AS",
                NextPerformance = new PerformanceSequence { Name = "psbc21_SequenceServo0P Configure and Engage", SequenceType = "AS"}
            },
            new PerformanceSequence
            {
                Name = "psbc21_SequenceServo0P Configure and Engage",
                Description = "psbc21_SequenceServo0P Configure and Engage",
                Loops = 1,
                SequenceType = "AS",
                NextPerformance = null
            }
        };
    }
}
