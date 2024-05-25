using NUnit.Framework;


namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using BusinessLogic;


    [TestFixture]
    public class AttributeIdTest
    {
        [Test]
        public void Equals_同じAttributeを持つ場合_Trueを返す()
        {
            // Arrange
            var attribute1 = new AttributeId(1, "Attribute");
            var attribute2 = new AttributeId(1, "Attribute");

            // Act
            var result = attribute1.Equals(attribute2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_異なるAttributeIDを持つ場合_Falseを返す()
        {
            // Arrange
            var attribute1 = new AttributeId(1, "Attribute");
            var attribute2 = new AttributeId(2, "Attribute");

            // Act
            var result = attribute1.Equals(attribute2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_異なるAttributeNameを持つ場合_Falseを返す()
        {
            // Arrange
            var attribute1 = new AttributeId(1, "Attribute1");
            var attribute2 = new AttributeId(1, "Attribute2");

            // Act
            var result = attribute1.Equals(attribute2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetHashCode_同じAttributeを持つ場合_同じハッシュコードを返す()
        {
            // Arrange
            var attribute1 = new AttributeId(1, "Attribute");
            var attribute2 = new AttributeId(1, "Attribute");

            // Act
            var hashCode1 = attribute1.GetHashCode();
            var hashCode2 = attribute2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCode1, hashCode2);
        }

        [Test]
        public void GetHashCode_異なるAttributeを持つ場合_異なるハッシュコードを返す()
        {
            // Arrange
            var attribute1 = new AttributeId(1, "Attribute1");
            var attribute2 = new AttributeId(2, "Attribute2");

            // Act
            var hashCode1 = attribute1.GetHashCode();
            var hashCode2 = attribute2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hashCode1, hashCode2);
        }
    }

}
