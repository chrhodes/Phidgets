using System;

namespace VNCPhidgets21Explorer.Resources
{
    public class PerformanceConfig
    {
        /// <summary>
        /// Name of Sequence
        /// </summary>
        public string Name { get; set; } = "PERFORMANCE NAME";

        /// <summary>
        /// Description of Sequence
        /// </summary>
        public string Description { get; set; } = "PERFORMANCE DESCRIPTION";

        ///// <summary>
        ///// Number of loops of Sequence
        ///// </summary>
        //public Int32 Loops { get; set; } = 1;

        ///// <summary>
        ///// Name of performanceSequence to invoke at end of performanceSequence (optional)
        ///// none or null to stop
        ///// </summary>
        //public PerformanceSequence? NextPerformance { get; set; }

        public Performance[] Performances { get; set; } = new[] // PerformanceSequence[0];
        {
            new Performance
            {
                Name = "psbc21_AS Performance 1",
                Description = "psbc21_AS Performance 1 Description",

                PerformanceSequences = new[] // PerformanceSequence[0];
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
                    }
                }
            },
            new Performance
            {
                Name = "psbc21_IK Performance 1",
                Description = "psbc21_IK Performance 1 Description",
                PlayInParallel = true,

                PerformanceSequences = new[] // PerformanceSequence[0];
                {
                    new PerformanceSequence
                    {
                        Name = "psbc21_SequenceIK 1",
                        Description = "psbc21_SequenceIK 1 Description",
                        Loops = 1,
                        SequenceType = "IK",
                        NextSequence = null
                    },
                    new PerformanceSequence
                    {
                        Name = "psbc22_SequenceIK 1",
                        Description = "psbc22_SequenceIK 1 Description",
                        Loops = 1,
                        SequenceType = "IK",
                        NextSequence = null
                    },
                    new PerformanceSequence
                    {
                        Name = "psbc23_SequenceIK 1",
                        Description = "psbc23_SequenceIK 1 Description",
                        Loops = 1,
                        SequenceType = "IK",
                        NextSequence = null
                    }
                }
            }
        };
    }
}
