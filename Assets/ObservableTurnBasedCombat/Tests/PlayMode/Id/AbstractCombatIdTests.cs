using NUnit.Framework;
using System;


namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    public class AbstractCombatIdTests
    {
        [Test]
        public void Equals_SameArguments_SameInstance()
        {
            // Arrange
            var id1 = new FakeCombatId(1, "Test");
            var id2 = new FakeCombatId(1, "Test");

            // Act
            var same = id1.Equals(id2);

            // Assert
            Assert.That(same);
        }

        [Test]
        public void Equals_DifferentArguments_DifferentInstance()
        {
            // Arrange
            var id1 = new FakeCombatId(1, "Test1");
            var id2 = new FakeCombatId(1, "Test2");

            // Act
            var same = id1.Equals(id2);

            // Assert
            Assert.That(!same);
        }

        [Test]
        public void GetHashCode_MatchManualCalculation_True()
        {
            // Arrange
            var id = new FakeCombatId       (1, "Test");
            var expected = HashCode.Combine (1, "Test");

            // Act
            var result = id.GetHashCode();

            // Assert
            Assert.That(expected == result);
        }

        [Test]
        public void GetHashCode_UnmatchManualCalculation_False()
        {
            // Arrange
            var id = new FakeCombatId           (1, "Test1");
            var unexpected = HashCode.Combine   (1, "Test2");

            // Act
            var result = id.GetHashCode();

            // Assert
            Assert.That(unexpected != result);
        }

        [Test]
        public void Serialize_SerializeInstance_SameJson()
        {
            // Arrange
            var id = new FakeCombatId(1, "Test");
            var expected = "{\"id\":1,\"name\":\"Test\"}";


            // Act
            var serialized = id.Serialize();


            // Assert
            Assert.That(expected == serialized);
        }

        [Test]
        public void Deserialize_DeserializeJson_SameInstance()
        {
            // Arrange
            var json = "{\"id\":1,\"name\":\"Test\"}";
            var expected = new FakeCombatId(1, "Test");

            // Act
            var deserialized = AbstractCombatId.Deserialize<FakeCombatId>(json);


            // Assert
            Assert.That(expected.Equals(deserialized));
        }

        [Test]
        public void Deserialize_ArgmentTypeIsAbstractCombatId_ThrowArgumentException()
        {
            // Arrange
            var json = "{\"id\":1,\"name\":\"Test\"}";

            // Act
            // Nothing to do

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                AbstractCombatId.Deserialize<AbstractCombatId>(json);
            });
        }
    }
}
