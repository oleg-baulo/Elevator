namespace Elevator.Core.Components.DoorSystem
{
    public class CabinDoorSystemConfiguration
    {
        /// <summary>
        /// In 100 is fully open or close in one second.
        /// Min 1
        /// </summary>
        public int OpeninClosingSpeedInPercentsPerSecond { get; private set; }

        public GlogalConfiguration GlobalConfig { get; private set; }

        public CabinDoorSystemConfiguration(
            GlogalConfiguration globalConfig
            , int openinClosingSpeedInPercentsPerSecond = 100)
        {
            GlobalConfig = globalConfig;
            OpeninClosingSpeedInPercentsPerSecond = openinClosingSpeedInPercentsPerSecond;
        }
    }
}
