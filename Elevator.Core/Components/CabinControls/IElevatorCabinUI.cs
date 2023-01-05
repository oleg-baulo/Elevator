namespace Elevator.Core.Components.CabinControls
{
    /// <summary>
    /// Elevator cabin UI.
    /// </summary>
    public interface IElevatorCabinUI
    {
        void EmergencyStop();
        void EmergencyStopRelease();
        void PressFloor(int floor);

        TimeSpan? GetTimeToNextFloor { get; }
    }
}