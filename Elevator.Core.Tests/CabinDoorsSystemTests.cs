using Elevator.Core.Components.DoorSystem;

namespace Elevator.Core.Tests
{
    public class CabinDoorsSystemTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestDoorStates()
        {
            CabinDoorSystem cds = new CabinDoorSystem(
                new CabinDoorSystemConfiguration(
                    new GlogalConfiguration()
                    , openinClosingSpeedInPercentsPerSecond: 100
                    ));

            Assert.That(EDoorsState.Closed, Is.EqualTo(cds.DoorsState));
            await cds.OpenDoors();
            await Task.Delay(TimeSpan.FromSeconds(0.1f));
            Assert.That(EDoorsState.Moving, Is.EqualTo(cds.DoorsState));

            await Task.Delay(TimeSpan.FromSeconds(1.1f));
            Assert.That(EDoorsState.Open, Is.EqualTo(cds.DoorsState));

            await cds.CloseDoors();
            await Task.Delay(TimeSpan.FromSeconds(0.1f));
            Assert.That(EDoorsState.Moving, Is.EqualTo(cds.DoorsState));

            await Task.Delay(TimeSpan.FromSeconds(1.1f));
            Assert.That(EDoorsState.Closed, Is.EqualTo(cds.DoorsState));
        }

        [Test]
        public async Task OpenDoorsCheckEvents()
        {
            CabinDoorSystem cds = new CabinDoorSystem(
                new CabinDoorSystemConfiguration(
                    new GlogalConfiguration()
                    , openinClosingSpeedInPercentsPerSecond: 100
                    ));

            cds.OpenedEvent += openedEvent;
            cds.ClosedEvent += closedEvent;

            bool openedCalled = false;
            bool closedCalled = false;
            void openedEvent(object? sender, EventArgs e)
            {
                openedCalled = true;
            }

            void closedEvent(object? sender, EventArgs e)
            {
                closedCalled = true;
            }

            await cds.OpenDoors();
            await Task.Delay(TimeSpan.FromSeconds(1.1f));

            await cds.CloseDoors();
            await Task.Delay(TimeSpan.FromSeconds(1.1f));

            Assert.IsTrue(openedCalled);
            Assert.IsTrue(closedCalled);
        }
    }
}