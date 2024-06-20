using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.TestTools;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatUnit
{
    using Application;

    public class CombatUnitAttributeTest
    {
        [Test] public void Constructor_InitializeId_SameId()
        {
            // Arrange
            UnitAttributeId id = new UnitAttributeId(1, "Test");
            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id);

            var excepted = true;


            // Act
            var result = id.Equals(attribute.Id);


            // Assert
            Assert.That(result, Is.EqualTo(excepted));
        }


        [Test] public void TryGetById_SameId_Success()
        {
            // Arrange
            UnitAttributeId id = new UnitAttributeId(1, "Test");
            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id);


            // Act
            ICombatUnitAttribute result1;
            var result2 = attribute.TryGetById(id, out result1);


            // Assert
            Assert.That(result1, Is.EqualTo(attribute));
            Assert.That(result2, Is.EqualTo(true));
        }
        [Test] public void TryGetById_DifferentId_Failure()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id1);


            // Act
            ICombatUnitAttribute result1;
            var result2 = attribute.TryGetById(id2, out result1);


            // Assert
            Assert.That(result1, Is.EqualTo(null));
            Assert.That(result2, Is.EqualTo(false));
        }


        [Test] public void ContainsById_SameId_True()
        {
            // Arrange
            UnitAttributeId id = new UnitAttributeId(1, "Test");
            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id);


            // Act
            var result = attribute.Contains(id);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test] public void ContainsById_DifferentId_False()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id1);


            // Act
            var result = attribute.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }


        [Test]
        public void Add_Execute_ThrowInvalidOperationException()
        {
            // Arrange
            UnitAttributeId id = new UnitAttributeId(1, "Test");
            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id);


            // Act
            // Nothing to do


            // Assert
            Assert.That
            (
                () => { attribute.Add(attribute); },
                Throws.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("単体のアトリビュートにアトリビュートを追加することはできません。")
            );
        }
        [Test]
        public void Remove_Execute_ThrowInvalidOperationException()
        {
            // Arrange
            UnitAttributeId id = new UnitAttributeId(1, "Test");
            CombatUnitAttribute<int> attribute = new CombatUnitAttribute<int>(id);


            // Act
            // Nothing to do


            // Assert
            Assert.That
            (
                () => { attribute.RemoveById(id); },
                Throws.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("単体のアトリビュートからアトリビュートを削除することはできません。")
            );
        }


        [Test] public void Equals_SameAttributeIds_AreEqual()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test");

            CombatUnitAttribute<int> attribute1 = new CombatUnitAttribute<int>(id1);
            CombatUnitAttribute<int> attribute2 = new CombatUnitAttribute<int>(id2);

            var excepted = true;


            // Act
            bool result = attribute1.Equals(attribute2);


            // Assert
            Assert.That(result, Is.EqualTo(excepted));
        }
        [Test] public void Equals_DifferentAttributeIds_AreNotEqual()
        {
            // Arrange
            UnitAttributeId id1 = new UnitAttributeId(1, "Test1");
            UnitAttributeId id2 = new UnitAttributeId(1, "Test2");

            CombatUnitAttribute<int> attribute1 = new CombatUnitAttribute<int>(id1);
            CombatUnitAttribute<int> attribute2 = new CombatUnitAttribute<int>(id2);

            var excepted = false;


            // Act
            bool result = attribute1.Equals(attribute2);


            // Assert
            Assert.That(result, Is.EqualTo(excepted));
        }
    }
}
