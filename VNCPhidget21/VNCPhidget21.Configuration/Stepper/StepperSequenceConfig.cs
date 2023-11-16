namespace VNCPhidget21.Configuration
{
    public class StepperSequenceConfig
    {
        public StepperSequence[] StepperSequences { get; set; } = new[]
        {
            new StepperSequence
            {
                Host = new Host
                {
                    Name = "localhost",
                    IPAddress = "127.0.0.1",
                    Port = 5001,
                    Steppers = new[]
                    {
                        new Stepper { Name = "Stepper 1", SerialNumber = 46049, Open = true }
                    }
                },
                Name="SequenceStepper 1",

                Actions = new[]
                {
                    new StepperAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true }
                }
            } 
        };
    }
}
