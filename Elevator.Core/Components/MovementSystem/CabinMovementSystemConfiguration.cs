using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator.Core.Components.MovementSystem
{
    public class CabinMovementSystemConfiguration
    {
        public int MinFloor { get; private set; }
        public int MaxFloor { get; private set; }

        public float Speed { get; private set; }

        public GlogalConfiguration GlobalConfig { get; private set; }

        public CabinMovementSystemConfiguration(
            GlogalConfiguration globalConfig
            , int minFloor = 1
            , int maxFloor = 10
            , float speed = 0.1f)
        {
            GlobalConfig = globalConfig;
            MinFloor = minFloor;
            MaxFloor = maxFloor;
            Speed = speed;
        }
    }
}
