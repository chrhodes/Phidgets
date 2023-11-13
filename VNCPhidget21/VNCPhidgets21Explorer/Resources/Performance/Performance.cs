using System;

namespace VNCPhidgets21Explorer.Resources
{
    public class Performance
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
        /// Play PerformanceSequence[] in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlayInParallel { get; set; } = false;

        /// <summary>
        /// Name of performanceSequence to invoke at end of performanceSequence (optional)
        /// none or null to stop
        /// </summary>
        public Performance? NextPerformance { get; set; }

        public PerformanceSequence[] PerformanceSequences { get; set; } = new[] // PerformanceSequence[0];
        {
            new PerformanceSequence
            {
                Name = "psbc21_SequenceServo0",
                Description = "psbc21_SequenceServo0 1 Description",
                Loops = 1,
                SequenceType = "AS",
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo0P Configure and Engage", SequenceType = "AS"}
            },
            new PerformanceSequence
            {
                Name = "psbc21_SequenceServo0P Configure and Engage",
                Description = "psbc21_SequenceServo0P Configure and Engage",
                Loops = 1,
                SequenceType = "AS",
                NextSequence = null
            },
            new PerformanceSequence
            {
                Name = "psbc21_SequenceIK 1",
                Description = "psbc21_SequenceIK 1 Description",
                Loops = 1,
                SequenceType = "IK",
                NextSequence = null
            }
        };
    }
}
