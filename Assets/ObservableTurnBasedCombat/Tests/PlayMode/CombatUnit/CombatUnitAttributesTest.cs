using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.TestTools;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatUnit
{
    using Application;
    using System.Xml.Linq;


    public class CombatUnitAttributesTest
    {
        [Test] public void Constructor_InitializeId_SameId()
        {
            // Arrange
            UnitAttributeId id = new UnitAttributeId(1, "Test");
            CombatUnitAttributes attributes = new CombatUnitAttributes(id);


            // Act
            var result = id.Equals(attributes.Id);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test] public void Constructor_InitializeIdAndEnum_SameId()
        {
            // Arrange
            UnitAttributeId id = new UnitAttributeId(1, "Test");
            CombatUnitAttributes attributes = new CombatUnitAttributes
            (
                id,
                new List<ICombatUnitAttribute> { }
            );


            // Act
            var result = id.Equals(attributes.Id);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }


        [Test] public void TryGetById_SameId_Success()
        {
            // Arrange
            UnitAttributeId id = new UnitAttributeId(1, "Test");
            CombatUnitAttributes attributes = new CombatUnitAttributes(id);


            // Act
            ICombatUnitAttribute result1;
            var result2 = attributes.TryGetById(id, out result1);


            // Assert
            Assert.That(result1, Is.EqualTo(attributes));
            Assert.That(result2, Is.EqualTo(true));
        }
        [Test] public void TryGetById_ChildId_Success()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id2);

            CombatUnitAttributes attributes = new CombatUnitAttributes
            (
                id1,
                new List<ICombatUnitAttribute> { attribute }
            );


            // Act
            ICombatUnitAttribute result1;
            var result2 = attributes.TryGetById(id2, out result1);


            // Assert
            Assert.That(result1, Is.EqualTo(attribute));
            Assert.That(result2, Is.EqualTo(true));
        }
        [Test] public void TryGetById_DifferentId_Failure()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttributes attributes = new CombatUnitAttributes(id1);


            // Act
            ICombatUnitAttribute result1;
            var result2 = attributes.TryGetById(id2, out result1);


            // Assert
            Assert.That(result1, Is.EqualTo(null));
            Assert.That(result2, Is.EqualTo(false));
        }


        [Test] public void ContainsById_ContainsId_True()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id2);

            CombatUnitAttributes attributes = new CombatUnitAttributes
            (
                id1,
                new List<ICombatUnitAttribute> { attribute }
            );


            // Act
            var result = attributes.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test] public void ContainsById_Empty_False()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id2);

            CombatUnitAttributes attributes = new CombatUnitAttributes
            (
                id1,
                new List<ICombatUnitAttribute> { /* Empty */ }
            );


            // Act
            var result = attributes.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test] public void ContainsById_DoesnotContainId_False()
        {
            // Arrange
            UnitAttributeId id = new UnitAttributeId(1, "Test1");
            UnitAttributeId id1 = new UnitAttributeId(1, "Test2");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test3");

            CombatUnitAttribute<int> attribute1 = new CombatUnitAttribute<int>(id1);
            CombatUnitAttribute<int> attribute2 = new CombatUnitAttribute<int>(id2);

            CombatUnitAttributes attributes = new CombatUnitAttributes
            (
                id,
                new List<ICombatUnitAttribute> { attribute1 }
            );


            // Act
            var result = attributes.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test] public void ContainsById_SelfId_False()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttributes attributes = new CombatUnitAttributes(id1);
            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id2);


            // Act
            attributes.Add(attribute);
            var result = attributes.Contains(id1);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }


        [Test] public void Add_AddAttribute_Success()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttributes attributes = new CombatUnitAttributes(id1);
            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id2);


            // Act
            attributes.Add(attribute);
            var result = attributes.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test] public void Add_AddSameAttributeTwice_ThrowArgumentException()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttributes attributes = new CombatUnitAttributes(id1);
            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id2);


            // Act
            attributes.Add(attribute);


            // Assert
            Assert.That
            (
                () => { attributes.Add(attribute); },
                Throws.TypeOf<ArgumentException>()
                .With.Message.EqualTo("同じIdを持つアトリビュートを追加することはできません。")
            );
        }


        [Test] public void Remove_RemoveAttribute_Success()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id2);

            CombatUnitAttributes attributes = new CombatUnitAttributes
            (
                id1,
                new List<ICombatUnitAttribute> { attribute }
            );


            // Act
            var result1 = attributes.RemoveById(attribute.Id);
            var result2 = attributes.Contains(attribute.Id);


            // Assert
            Assert.That(result1, Is.EqualTo(true));
            Assert.That(result2, Is.EqualTo(false));
        }
        [Test] public void Remove_RemoveNotHavingAttribute_False()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id2);

            CombatUnitAttributes attributes = new CombatUnitAttributes
            (
                id1,
                new List<ICombatUnitAttribute> { /* Empty */ }
            );


            // Act
            var result1 = attributes.RemoveById(attribute.Id);
            var result2 = attributes.Contains(attribute.Id);


            // Assert
            Assert.That(result1, Is.EqualTo(false));
            Assert.That(result1, Is.EqualTo(false));
        }


        [Test] public void Equals_SameAttributeIds_AreEqual()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test");

            CombatUnitAttributes attributes1 = new CombatUnitAttributes(id1);
            CombatUnitAttributes attributes2 = new CombatUnitAttributes(id2);

            var excepted = true;


            // Act
            bool result = attributes1.Equals(attributes2);


            // Assert
            Assert.That(result, Is.EqualTo(excepted));
        }
        [Test] public void Equals_DifferentAttributeIds_AreNotEqual()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttributes attributes1 = new CombatUnitAttributes(id1);
            CombatUnitAttributes attributes2 = new CombatUnitAttributes(id2);

            var excepted = false;


            // Act
            bool result = attributes1.Equals(attributes2);


            // Assert
            Assert.That(result, Is.EqualTo(excepted));
        }
    }
}
