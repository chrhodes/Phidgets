namespace VNCPhidget21.Configuration
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
        public string? Description { get; set; } = "PERFORMANCE DESCRIPTION";

        /// <summary>
        /// Play PerformanceSequences in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlaySequencesInParallel { get; set; } = false;

        public PerformanceSequence[]? PerformanceSequences { get; set; }

        /// <summary>
        /// Name of Performance[] to call after executing PerformanceSequences
        /// before calling NextSequence
        /// </summary>
        public Performance[]? CallPerformances { get; set; }

        /// <summary>
        /// Duration in ms of sleep time after PerformanceSequences[] completed)
        /// </summary>
        public Int32? Duration { get; set; }

        /// <summary>
        /// Number of loops of Performance
        /// </summary>
        public Int32 Loops { get; set; } = 1;

        /// <summary>
        /// Performance to invoke at end of Loops of PerformanceSequences (optional)
        /// none or null to stop
        /// </summary>
        public Performance? NextPerformance { get; set; }
    }
}
