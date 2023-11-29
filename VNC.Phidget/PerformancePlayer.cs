using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;

using VNCPhidget21.Configuration;

namespace VNC.Phidget
{
    public class PerformancePlayer
    {
        public PerformancePlayer()
        {

        }

        public bool LogPerformance { get; set; }

        public PerformanceSequencePlayer PerformanceSequencePlayer { get; set; }

        public Dictionary<string, Performance> AvailablePerformances { get; set; }

        public async Task RunPerformanceLoops(Performance performance)
        {
            Int64 startTicks = 0;

            if (LogPerformance)
            {
                startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

                Log.Trace($"Running performance:{performance.Name} description:{performance.Description}" +
                    $" performanceSequences:{performance.PerformanceSequences?.Count()} playSequencesInParallel:{performance.PlaySequencesInParallel}" +
                    $" callPerformances:{performance.CallPerformances?.Count()}" +
                    $" loops:{performance.Loops} duration:{performance.Duration}" +
                    $" nextPerformance:{performance.NextPerformance}", Common.LOG_CATEGORY);
            }
            //PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();



            for (int performanceLoop = 0; performanceLoop < performance.Loops; performanceLoop++)
            {
                // NOTE(crhodes)
                // First execute PerformanceSequences if any

                if (performance.PerformanceSequences is not null)
                {
                    if (performance.PlaySequencesInParallel)
                    {
                        if (LogPerformance) Log.Trace($"Parallel Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        Parallel.ForEach(performance.PerformanceSequences, async sequence =>
                        {
                            await PerformanceSequencePlayer.ExecutePerformanceSequence(sequence);
                        });
                    }
                    else
                    {
                        if (LogPerformance) Log.Trace($"Sequential Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        foreach (PerformanceSequence sequence in performance.PerformanceSequences)
                        {
                            for (int sequenceLoop = 0; sequenceLoop < sequence.Loops; sequenceLoop++)
                            {
                                await PerformanceSequencePlayer.ExecutePerformanceSequence(sequence);
                            }
                        }
                    }
                }

                // NOTE(crhodes)
                // Then execute CallPerformances if any

                if (performance.CallPerformances is not null)
                {
                    foreach (Performance callPerformance in performance.CallPerformances)
                    {
                        Performance nextPerformance = null;

                        if (AvailablePerformances.ContainsKey(callPerformance.Name ?? ""))
                        {
                            nextPerformance = AvailablePerformances[callPerformance.Name];

                            await RunPerformanceLoops(nextPerformance);

                            // TODO(crhodes)
                            // Should we process Next Performance if exists.  Recursive implications need to be considered.
                            // May have to detect loops.

                            nextPerformance = nextPerformance?.NextPerformance;
                        }
                        else
                        {
                            Log.Error($"Cannot find performance:>{nextPerformance?.Name}<", Common.LOG_CATEGORY);
                            nextPerformance = null;
                        }
                    }
                }

                // NOTE(crhodes)
                // Then sleep if necessary before next loop

                if (performance.Duration is not null)
                {
                    if (LogPerformance)
                    {
                        Log.Trace($"Zzzzz End of Performance Sleeping:>{performance.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((Int32)performance.Duration);
                }
            }

            if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }
    }
}
