using System;

namespace VNCPhidgets21Explorer.Resources
{

    public class AdvancedServoSequence
    {
        public Host Host { get; set; }
            = new Host
            {
                Name = "localhost",
                IPAddress = "127.0.0.1",
                Port = 5001,
                AdvancedServos = new[]
                {
                    new AdvancedServo { Name = "AdvancedServo 1", SerialNumber = 99415, Open = true }
                }
            };

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
        /// Play AdvancedServoAction[] in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlayActionsInParallel { get; set; } = false;

        /// <summary>
        /// Name of PerformanceSequence to invoke at end of sequence loops (optional)
        /// none or null to stop
        /// </summary>
        public PerformanceSequence? NextSequence { get; set; }

        /// <summary>
        /// Array of actions in sequence
        /// </summary>
        public AdvancedServoServoAction[] AdvancedServoServoActions { get; set; }
    }
}
