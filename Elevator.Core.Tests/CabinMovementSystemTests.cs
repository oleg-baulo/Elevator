using Elevator.Core.Components.MovementSystem;

namespace Elevator.Core.Tests
{
    public class CabinMovementSystemTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task MoveToFloorFloorIsChanged()
        {
            CabinMovementSystem cms = new CabinMovementSystem(
                new CabinMovementSystemConfiguration(
                    new GlogalConfiguration()
                    , minFloor: 1
                    , maxFloor: 2
                    , speed: 100
                    ));
            
            await cms.MoveToFloor(2);
            await Task.Delay(TimeSpan.FromSeconds(1));

            Assert.That(2, Is.EqualTo(cms.CurrentFloor));
        }

        [Test]
        public async Task MoveToFloorCheckStartedStoppedEvents()
        {
            CabinMovementSystem cms = new CabinMovementSystem(
                new CabinMovementSystemConfiguration(
                    new GlogalConfiguration()
                    , minFloor: 1
                    , maxFloor: 2
                    , speed: 100
                    ));
            cms.StartedEvent += Cms_StartedEvent;
            cms.StoppedEvent += Cms_StoppedEvent;
            
            bool startedCalled = false;
            bool stoppedCalled = false;
            void Cms_StartedEvent(object? sender, EventArgs e)
            {
                startedCalled = true;
            }

            void Cms_StoppedEvent(object? sender, EventArgs e)
            {
                stoppedCalled = true;
            }

            await cms.MoveToFloor(2);
            await Task.Delay(TimeSpan.FromSeconds(1));
            
            Assert.IsTrue(startedCalled);
            Assert.IsTrue(stoppedCalled);
        }

        [Test]
        public async Task StopMovementCheckFloorIsNotChangedAfterThat()
        {
            var gc = new GlogalConfiguration();
            CabinMovementSystem cms = new CabinMovementSystem(
                new CabinMovementSystemConfiguration(
                    gc
                    , minFloor: 1
                    , maxFloor: 100
                    , speed: 1.2f
                    ));

            
            int lastRecorderFloor = cms.CurrentFloor;
            await cms.MoveToFloor(100);
            await Task.Delay(gc.GlogalUpdatePeriod * 2);

            // check that value is changing while we are moving up
            Assert.That(lastRecorderFloor, Is.LessThan(cms.CurrentFloor));

            TimeSpan estimatedTime = cms.GetTimeToTargetFloor();

            // check that the value is not changed after we stopped movement.
            await cms.StopMovement();
            await Task.Delay(gc.GlogalUpdatePeriod * 2);
            lastRecorderFloor = cms.CurrentFloor;
            await Task.Delay(gc.GlogalUpdatePeriod * 2);
            Assert.That(lastRecorderFloor, Is.EqualTo(cms.CurrentFloor));
        }
    }
}