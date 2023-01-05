using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Core.Components.DoorSystem
{
    // door system
    //  commands: open, close
    //  posiion float - 0 to 1 (0 opened - 1 closed)
    //  events: opened, closed

    public class CabinDoorSystem : ICabinDoorSystem
    {
        private readonly CabinDoorSystemConfiguration _configuration;
        private object _lock = new object();
        private CancellationTokenSource _openingOrClosingTaskCts;
        private Task _openingOrClosingTask;
        private EDoorsState _doorsState;

        public event EventHandler OpenedEvent;
        public event EventHandler ClosedEvent;

        public CabinDoorSystem(CabinDoorSystemConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EDoorsState DoorsState => _doorsState;

        public async Task OpenDoors()
        {
            //await Task.Delay(TimeSpan.FromSeconds(_configuration.OpeninClosingSpeedInPercentsPerSecond / 100), cts.Token);
            //await OpenedEvent(this, EventArgs.Empty);

            await RestartTaskWithEventOnCompletion(async () =>
            {
                lock (_lock)
                {
                    _doorsState = EDoorsState.Open;
                }

                if (OpenedEvent != null)
                {
                    OpenedEvent(this, EventArgs.Empty);
                }
            });
        }

        public async Task CloseDoors()
        {
            await RestartTaskWithEventOnCompletion(async () =>
            {
                lock (_lock)
                {
                    _doorsState = EDoorsState.Closed;
                }

                if (ClosedEvent != null)
                {
                    ClosedEvent(this, EventArgs.Empty);
                }
            });
        }

        private async Task RestartTaskWithEventOnCompletion(Func<Task> action)
        {
            lock (_lock)
            {
                if (_openingOrClosingTaskCts != null)
                {
                    _openingOrClosingTaskCts.Cancel();
                }

                _doorsState = EDoorsState.Moving;
                _openingOrClosingTaskCts = new CancellationTokenSource();
                _openingOrClosingTask = Task.Delay(TimeSpan.FromSeconds(_configuration.OpeninClosingSpeedInPercentsPerSecond / 100), _openingOrClosingTaskCts.Token)
                    .ContinueWith((t) =>
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            action();
                        }
                    });
            }
        }
    }
}
