namespace VNCPhidget21.Configuration
{
    public class PhidgetSequenceBase
    {
        /// <summary>
        /// Host on which to run sequence (optional)
        /// </summary>
        public Host? Host { get; set; }

        /// <summary>
        /// Name of sequence
        /// </summary>
        public string Name { get; set; } = "SEQUENCE NAME";

        /// <summary>
        /// Description of sequence (optional)
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Description of sequence (optional)
        /// </summary>
        public string? UsageNotes { get; set; }

        /// <summary>
        /// Number of loops of sequence Actions to execute
        /// </summary>
        public Int32 Loops { get; set; } = 1;

        /// <summary>
        /// Play Actions[] in Parallel or Sequentially (false)
        /// </summary>
        public Boolean ExecuteActionsInParallel { get; set; } = false;

        /// <summary>
        /// Name of PerformanceSequence[] to call after executing Actions
        /// before calling NextSequence
        /// </summary>
        public PerformanceSequence[]? CallSequences { get; set; }

        /// <summary>
        /// Duration of Sequence in ms (sleep time after Actions and CallSequences completed)
        /// </summary>
        public Int32? Duration { get; set; }

        /// <summary>
        /// Name of PerformanceSequence to invoke at end of sequence loops (optional)
        /// none or null to stop
        /// </summary>
        public PerformanceSequence? NextSequence { get; set; }
    }
}
