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
        /// Description of sequence
        /// </summary>
        public string Description { get; set; } = "SEQUENCE DESCRIPTION";

        /// <summary>
        /// Number of loops of sequence
        /// </summary>
        public Int32 Loops { get; set; } = 1;

        /// <summary>
        /// Play Actions[] in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlayActionsInParallel { get; set; } = false;

        /// <summary>
        /// Name of PerformanceSequence to invoke at end of sequence loops (optional)
        /// none or null to stop
        /// </summary>
        public PerformanceSequence? NextSequence { get; set; }
    }
}
