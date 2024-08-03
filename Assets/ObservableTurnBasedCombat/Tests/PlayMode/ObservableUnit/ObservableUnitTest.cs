using NUnit.Framework;
using R3;


namespace ObservableTurnBasedCombat.Tests.PlayMode.ObservableUnitTest
{
    using ObservableUnit;


    public class ObservableUnitTest
    {
        private UnitId id;
        ObservableUnitComponent component1 = default;
        ObservableUnitComponent component2 = default;

        [SetUp]
        public void SetUp()
        {
            id = new UnitId(1, "Test");
            component1 = new FakeUnitComponent(1);
            component2 = new FakeUnitComponent(2);
        }


        [Test]
        public void Graph_AddComponentHavingRequiredLink_FetchAndAddLinkedComponent()
        {
            // Arrenge
            var repository = new FakeUnitRepository(component2);
            var unit = new ObservableUnit(id, repository);


            // Act
            component1.AddRequiredLink(component2.Id);
            unit.Graph.AddNode(component1);


            // Assert
            Assert.That(unit.Graph.ContainsNode(component1), Is.True);
            Assert.That(unit.Graph.ContainsNode(component2), Is.True);
            Assert.That(unit.Graph.ContainsLink(component1, component2), Is.True);
        }
        [Test]
        public void Graph_AddComponentHavingRequiredLink_SubscribeChange()
        {
            // Arrenge
            var repository = new FakeUnitRepository(component2);
            var unit = new ObservableUnit(id, repository);

            var result = "";
            var expected = ""
                + "{\"Id\":"
                + $"{unit.Id.Serialize()},"
                + "\"Components\":["
                + $"{component1.SerializeToJson()},"
                + $"{component2.SerializeToJson()}"
                + "]}";


            // Act
            unit.ObservableChanges.Snapshot
                .Subscribe(snapshot =>
                {
                    result = snapshot;
                });

            component1.AddRequiredLink(component2.Id);
            unit.Graph.AddNode(component1);
            unit.Publish();


            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        public void ObservableChanges_GetSnapshot_GetAllComponents()
        {
            // Arrange
            var unit = new ObservableUnit(id);

            var result = "";
            var expected = ""
                + "{\"Id\":"
                + $"{unit.Id.Serialize()},"
                + "\"Components\":["
                + $"{component1.SerializeToJson()},"
                + $"{component2.SerializeToJson()}"
                + "]}";


            // Act
            unit.ObservableChanges.Snapshot
            .Subscribe(snapshot =>
            {
                result = snapshot;
            });

            unit.Graph.AddNode(component1);
            unit.Graph.AddNode(component2);
            unit.Publish();


            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
        [Test]
        public void ObservableChanges_ChangeComponentValue_SubscribeChange()
        {
            // Arrange
            var unit = new ObservableUnit(id);
            var component = new FakeUnitComponent();

            var result = "";


            // Act
            unit.ObservableChanges.Snapshot
                .Subscribe(snapshot =>
                {
                    result = snapshot;
                });
            unit.Graph.AddNode(component);
            component.TestValue = 1;
            var expected = ""
                + "{\"Id\":"
                + $"{unit.Id.Serialize()},"
                + "\"Components\":["
                + $"{component.SerializeToJson()}"
                + "]}";

            unit.Publish();


            // Assert
            Assert.That(unit.Graph.ContainsNode(component), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
        [Test]
        public void ObservableChanges_GetDiff_GetAdditionComponent()
        {
            // Arrange
            var unit = new ObservableUnit(id);

            var result = "";
            var expected = ""
                + "{\"Id\":"
                + $"{unit.Id.Serialize()},"
                + "\"Components\":["
                + $"{component2.SerializeToJson()}"
                + "]}";


            // Act
            unit.ObservableChanges.Diff
            .Subscribe(snapshot =>
            {
                result = snapshot;
            });

            unit.Graph.AddNode(component1);
            unit.Publish();
            unit.Graph.AddNode(component2);
            unit.Publish();


            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
