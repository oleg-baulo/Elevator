using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Elevator.Core.Components.DoorSystem;
using Elevator.Core.Components.MovementSystem;

namespace Elevator.Core.Components.ControlSystem
{

    public class ElevatorControlSystem : IElevatorControlSystem
    {
        private readonly ICabinMovementSystem _movementSystem;
        private readonly ICabinDoorSystem _cabinDoorSystem;
        private Queue<int> _floorQueue = new Queue<int>();
        private object _lock = new object();
        private EElevatorState _state;

        public ElevatorControlSystem(
            ICabinMovementSystem movementSystem
            , ICabinDoorSystem cabinDoorSystem)
        {
            _movementSystem = movementSystem;
            _cabinDoorSystem = cabinDoorSystem;
            _state = EElevatorState.Idle;
        }

        public event EventHandler ArrivedToFloor;
        public event EventHandler EmergencyStopActivated;

        public async Task EmergencyStop()
        {
            lock (_lock)
            {
                _floorQueue.Clear();
                _state = EElevatorState.EmergencyStop;
            }
            await _movementSystem.StopMovement();
        }

        public async Task EmergencyStopRelease()
        {
            lock (_lock)
            {
                _state = EElevatorState.Idle;
            }
        }

        public async Task RequestFloor(int floor)
        {
            lock(_lock )
            {
                if(_state != EElevatorState.EmergencyStop)
                {
                    if(!_floorQueue.Any(o => o == floor))
                    {
                        _floorQueue.Enqueue(floor);
                    }
                }
            }
        }
    }
}
