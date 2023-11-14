using System;

namespace VNCPhidgets21Explorer.Resources
{

    public class InterfaceKitSequence
    {
        public Host Host { get; set; }
            = new Host
            {
                Name = "localhost",
                IPAddress = "127.0.0.1",
                Port = 5001,
                InterfaceKits = new[]
                {
                    new InterfaceKit { Name = "InterfaceKit 1", SerialNumber = 124744, Embedded = false, Open = true }
                },
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
        /// Play InterfaqceKitAction[] in Parallel or Sequential (false)
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
        public InterfaceKitAction[] InterfaceKitActions { get; set; }
    }
}
