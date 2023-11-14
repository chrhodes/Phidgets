﻿namespace VNCPhidgets21Explorer.Resources
{
    public class PerformanceConfig
    {
        /// <summary>
        /// Name of file
        /// </summary>
        public string Name { get; set; } = "PerformanceConfig NAME";

        /// <summary>
        /// Description of this file
        /// </summary>
        public string Description { get; set; } = "PerformanceConfig DESCRIPTION";

        public Performance[] Performances { get; set; } = new[] // PerformanceSequence[0];
        {
            new Performance
            {
                Name = "psbc21_AS Performance 1",
                Description = "psbc21_AS Performance 1 Description",

                PerformanceSequences = new[] // PerformanceSequence[0];
                {
                    new PerformanceSequence { Name = "psbc21_SequenceServo0", SequenceType = "AS", Loops = 1 },
                    new PerformanceSequence { Name = "psbc21_SequenceServo0P Configure and Engage", SequenceType = "AS", Loops = 1 }
                }
            },
            new Performance
            {
                Name = "psbc21_IK Performance 1",
                Description = "psbc21_IK Performance 1 Description",
                PlayInParallel = true,

                PerformanceSequences = new[]
                {
                    new PerformanceSequence { Name = "psbc21_SequenceIK 1", SequenceType = "IK", Loops = 1 },
                    new PerformanceSequence { Name = "psbc22_SequenceIK 1", SequenceType = "IK", Loops = 1 },
                    new PerformanceSequence { Name = "psbc23_SequenceIK 1", SequenceType = "IK", Loops = 1 }
                }
            }
        };
    }
}
