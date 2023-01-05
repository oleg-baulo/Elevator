namespace Elevator.Core.Components.ExternalButtons
{

    /// <summary>
    /// Floor elevator button interface.
    /// </summary>
    public interface IElevatorExternalButtonUI
    {
        Task CallElevator();
        bool IsCalled { get; }
    }
}