using System;

namespace VNCPhidgets21Explorer.Resources
{

    public class StepperSequence
    {
        public Host? Host { get; set; }

        /// <summary>
        /// Name of performance
        /// </summary>
        public string Name { get; set; } = "SEQUENCE NAME";

        /// <summary>
        /// Description of performance
        /// </summary>
        public string Description { get; set; } = "SEQUENCE DESCRIPTION";

        /// <summary>
        /// Number of loops of Performance
        /// </summary>
        public Int32 Loops { get; set; } = 1;

        /// <summary>
        /// Play ServoAction[] in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlayInParallel { get; set; } = false;

        /// <summary>
        /// Name of StepperSequence to invoke at end of sequence (optional)
        /// none, null, or empty string to stop
        /// </summary>
        public string? ContinueWith { get; set; } = "";

        /// <summary>
        /// Array of actions in sequence
        /// </summary>
        public StepperAction[]? StepperActions { get; set; }
    }
}
