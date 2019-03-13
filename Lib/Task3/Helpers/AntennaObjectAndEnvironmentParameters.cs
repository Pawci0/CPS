namespace Lib.Task3.Helpers
{
    public class AntennaObjectAndEnvironmentParameters
    {
        public AntennaObjectAndEnvironmentParameters(double simulatorTimeUnit, double realSpeedOfTheObject, double speedOfSignalPropagationInEnvironment)
        {
            SimulatorTimeUnit = simulatorTimeUnit;
            RealSpeedOfTheObject = realSpeedOfTheObject;
            SpeedOfSignalPropagationInEnvironment = speedOfSignalPropagationInEnvironment;
        }

        public double SimulatorTimeUnit { get; }

        public double RealSpeedOfTheObject { get; }

        public double SpeedOfSignalPropagationInEnvironment { get; }
    }
}
