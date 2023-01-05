namespace Elevator.Core.Components.ControlSystem
{
    public interface IElevatorControlSystem
    {
        event EventHandler ArrivedToFloor;
        event EventHandler EmergencyStopActivated;

        Task EmergencyStop();
        Task EmergencyStopRelease();
        Task RequestFloor(int floor);
    }
}
