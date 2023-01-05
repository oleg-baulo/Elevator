using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator.Core.Components.MovementSystem
{
    // movementSystem
    //   commands: moveTo, stop
    //   events: started, stopped
    //   position - float 1 -- 1.1 - 2 - 3 ...
    //   direction - up down stopped

    public class CabinMovementSystem : ICabinMovementSystem
    {
        private volatile float _position;
        private int _targetFloor;
        private bool _enabled;
        private object _lock = new object();

        private Timer _timer;
        private CabinMovementSystemConfiguration _configuration;

        public CabinMovementSystem(CabinMovementSystemConfiguration config)
        {
            _configuration = config;
            _position = config.MinFloor;
            _targetFloor = config.MinFloor;
            _enabled = true;

            _timer = new Timer((state) =>
            {
                lock (_lock)
                {
                    if (_enabled && _targetFloor != _position)
                    {
                        int direction = _targetFloor > _position ? 1 : -1;

                        float distanceToTarget = _targetFloor > _position ? _targetFloor - _position : _position - _targetFloor;

                        _position = _position + direction * Math.Min(_configuration.Speed, distanceToTarget);

                        if (_position == _targetFloor)
                        {
                            Task.Run(() =>
                            {
                                StoppedEvent(null, EventArgs.Empty);
                            });
                        }
                    }
                }
            }, null, _configuration.GlobalConfig.GlogalUpdatePeriod, _configuration.GlobalConfig.GlogalUpdatePeriod);
        }

        public int CurrentFloor
        {
            get
            {
                lock (_lock)
                {
                    return (int)_position;
                }
            }
        }

        public event EventHandler StartedEvent;
        public event EventHandler StoppedEvent;

        public TimeSpan GetTimeToTargetFloor()
        {
            lock (_lock)
            {
                TimeSpan result = TimeSpan.Zero;
                float distanceToTarget = _targetFloor > _position ? _targetFloor - _position : _position - _targetFloor;
                if (distanceToTarget > 0.0f)
                {
                    double seconds = distanceToTarget / _configuration.Speed * _configuration.GlobalConfig.GlogalUpdatePeriod.TotalSeconds;
                    result = TimeSpan.FromSeconds(seconds);
                }

                return result;
            }
        }

        public async Task MoveToFloor(int floor)
        {
            if (floor < _configuration.MinFloor || floor > _configuration.MaxFloor)
            {
                throw new ArgumentOutOfRangeException(nameof(floor));
            }

            lock (_lock)
            {
                _targetFloor = floor;
                _enabled = true;
            }

            if (StartedEvent != null)
            {
                StartedEvent(this, EventArgs.Empty);
            }
        }

        public async Task StopMovement()
        {
            lock (_lock)
            {
                _enabled = false;
            }

            if (StoppedEvent != null)
            {
                StoppedEvent(this, EventArgs.Empty);
            }
        }
    }
}
