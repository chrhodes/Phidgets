using System;

namespace VNCPhidgets21Explorer.Resources
{
    public class PerformanceSequence
    {
        /// <summary>
        /// Name of Sequence
        /// </summary>
        public string Name { get; set; } = "SEQUENCE NAME";

        /// <summary>
        /// Description of Sequence
        /// </summary>
        public string Description { get; set; } = "SEQUENCE DESCRIPTION";

        /// <summary>
        /// Number of loops of Sequence
        /// </summary>
        public Int32 Loops { get; set; } = 1;

        /// <summary>
        /// Type of Sequence {AS, IK, ST}
        /// Maybe make this enum
        /// </summary>
        public string SequenceType { get; set; }

        /// <summary>
        /// Name of performanceSequence to invoke at end of performanceSequence (optional)
        /// none or null to stop
        /// </summary>
        public PerformanceSequence? NextPerformance { get; set; }
    }
}
