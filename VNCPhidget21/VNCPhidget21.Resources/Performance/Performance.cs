namespace VNCPhidgets21Explorer.Configuration
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
        /// Play PerformanceSequences in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlayInParallel { get; set; } = false;

        public PerformanceSequence[] PerformanceSequences { get; set; }

        /// <summary>
        /// Performance to invoke at end of Loops of PerformanceSequences (optional)
        /// none or null to stop
        /// </summary>
        public Performance? NextPerformance { get; set; }
    }
}
