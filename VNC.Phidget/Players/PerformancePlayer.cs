using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using VNCPhidget21.Configuration;

namespace VNC.Phidget.Players
{
    public class PerformancePlayer
    {
        #region Constructors, Initialization, and Load

        public PerformancePlayer()
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            LoadPerformances();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public bool LogPerformance { get; set; }

        public PerformanceSequencePlayer PerformanceSequencePlayer { get; set; }

        public static Dictionary<string, Performance> AvailablePerformances { get; set; } = 
            new Dictionary<string, Performance>();

        //public IEnumerable<Performance> Performances { get; set; }

        #endregion

        #region Event Handlers (None)


        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        public void LoadPerformances()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailablePerformances.Clear();

            foreach (string configFile in GetListOfPerformanceConfigFiles())
            {
                string jsonString = File.ReadAllText(configFile);

                PerformanceConfig? performanceConfig
                    = JsonSerializer.Deserialize<PerformanceConfig>
                    (jsonString, GetJsonSerializerOptions());

                foreach (var performance in performanceConfig.Performances.ToDictionary(k => k.Name, v => v))
                {
                    AvailablePerformances.Add(performance.Key, performance.Value);
                }
            }

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunPerformanceLoops(Performance performance)
        {
            long startTicks = 0;

            if (LogPerformance)
            {
                startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

                Log.Trace($"Running performance:{performance.Name} description:{performance.Description}" +
                    $" beforePerformanceLoopPerformances:{performance.BeforePerformanceLoopPerformances?.Count()}" +
                    $" performanceSequences:{performance.PerformanceSequences?.Count()} playSequencesInParallel:{performance.PlaySequencesInParallel}" +
                    $" afterPerformanceLoopPerformances:{performance.AfterPerformanceLoopPerformances?.Count()}" +
                    $" loops:{performance.PerformanceLoops} duration:{performance.Duration}" +
                    $" nextPerformance:{performance.NextPerformance}", Common.LOG_CATEGORY);
            }

            for (int performanceLoop = 0; performanceLoop < performance.PerformanceLoops; performanceLoop++)
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
                            for (int sequenceLoop = 0; sequenceLoop < sequence.SequenceLoops; sequenceLoop++)
                            {
                                await PerformanceSequencePlayer.ExecutePerformanceSequence(sequence);
                            }
                        }
                    }
                }

                // NOTE(crhodes)
                // Then execute CallPerformances if any

                if (performance.AfterPerformanceLoopPerformances is not null)
                {
                    foreach (Performance callPerformance in performance.AfterPerformanceLoopPerformances)
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
                    Thread.Sleep((int)performance.Duration);
                }
            }

            if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }


        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods

        private IEnumerable<string> GetListOfPerformanceConfigFiles()
        {
            // TODO(crhodes)
            // Read a directory and return files, perhaps with RegEx name match

            List<string> files = new List<string>
            {
                @"Performances\PerformanceConfig_1.json",
                @"Performances\PerformanceConfig_2.json",
                @"Performances\PerformanceConfig_3.json",

                @"Performances\PerformanceConfig_Skulls.json",
            };

            return files;
        }

        private JsonSerializerOptions GetJsonSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            return jsonOptions;
        }

        #endregion
    }
}
