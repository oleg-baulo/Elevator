namespace Elevator.Core.Components.DoorSystem
{
    public interface ICabinDoorSystem
    {
        event EventHandler OpenedEvent;
        event EventHandler ClosedEvent;

        EDoorsState DoorsState { get; }

        Task OpenDoors();
        Task CloseDoors();
    }
}
