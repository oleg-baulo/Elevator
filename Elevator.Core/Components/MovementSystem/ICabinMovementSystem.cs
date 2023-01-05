namespace Elevator.Core.Components.MovementSystem
{
    // movementSystem
    //   commands: moveTo, stop
    //   events: started, stopped
    //   position - float 1 -- 1.1 - 2 - 3 ...
    //   direction - up down stopped

    public interface ICabinMovementSystem
    {
        event EventHandler StartedEvent;
        event EventHandler StoppedEvent;

        Task MoveToFloor(int floor);
        Task StopMovement();

        int CurrentFloor { get; }
        TimeSpan GetTimeToTargetFloor();
    }
}
