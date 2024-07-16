using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatUnit
{
    using Application;


    public class BaseCombatUnitComponentTest
    {
        [Test]
        public void Constructor_SameIdsAndTypes_SameInstance()
        {
            // Arrange
            var id = new FakeUnitComponentId(1, "Test");
            var type = UnitComponentType.Attribute;

            var component1 = new FakeCombatUnitComponent(id, type);
            var component2 = new FakeCombatUnitComponent(id, type);


            // Act
            // Nothing to do


            // Assert
            Assert.That(component1.Id.Equals(component2.Id), Is.True);
            Assert.That(component1.Type.Equals(component2.Type), Is.True);
            Assert.That(component1.Equals(component2), Is.True);
        }
        [Test]
        public void Constructor_DifferentIds_DifferentInstance()
        {
            // Arrange
            var id1 = new FakeUnitComponentId(1, "Test1");
            var id2 = new FakeUnitComponentId(1, "Test2");
            var type = UnitComponentType.Attribute;

            var component1 = new FakeCombatUnitComponent(id1, type);
            var component2 = new FakeCombatUnitComponent(id2, type);


            // Act
            // Nothing to do


            // Assert
            Assert.That(component1.Id.Equals(component2.Id), Is.False);
            Assert.That(component1.Type.Equals(component2.Type), Is.True);
            Assert.That(component1.Equals(component2), Is.False);
        }
        [Test]
        public void Constructor_DifferentTypes_DifferentInstance()
        {
            // Arrange
            var id = new FakeUnitComponentId(1, "Test");
            var type1 = UnitComponentType.Attribute;
            var type2 = UnitComponentType.Stats;

            var component1 = new FakeCombatUnitComponent(id, type1);
            var component2 = new FakeCombatUnitComponent(id, type2);


            // Act
            // Nothing to do


            // Assert
            Assert.That(component1.Id.Equals(component2.Id), Is.True);
            Assert.That(component1.Type.Equals(component2.Type), Is.False);
            Assert.That(component1.Equals(component2), Is.False);
        }


        [Test]
        public void AddLink_AdditionLink_True()
        {
            // Arrange
            var id = new FakeUnitComponentId(1, "Test");
            var id1 = new FakeUnitComponentId(1, "Test1");

            var type = UnitComponentType.Attribute;
            var component = new FakeCombatUnitComponent
                (
                id,
                type,
                new List<FakeUnitComponentId> { /* Empty */ });


            // Act
            var result = component.AddLink(id1);


            // Assert
            Assert.That(result, Is.True);
            Assert.That(component.ContainsLink(id1), Is.True);
        }
        [Test]
        public void AddLink_LinkExists_False()
        {
            // Arrange
            var id = new FakeUnitComponentId(1, "Test");
            var id1 = new FakeUnitComponentId(1, "Test1");

            var type = UnitComponentType.Attribute;
            var component = new FakeCombatUnitComponent
                (
                id,
                type,
                new List<FakeUnitComponentId> { id1 });


            // Act
            var result = component.AddLink(id1);


            // Assert
            Assert.That(result, Is.False);
            Assert.That(component.ContainsLink(id1), Is.True);
        }


        [Test]
        public void RemoveLink_LinkExists_True()
        {
            // Arrange
            var id = new FakeUnitComponentId(1, "Test");
            var id1 = new FakeUnitComponentId(1, "Test1");

            var type = UnitComponentType.Attribute;
            var component = new FakeCombatUnitComponent
                (
                id,
                type,
                new List<FakeUnitComponentId> { id1 });


            // Act
            var contains = component.ContainsLink(id1);
            var result = component.RemoveLink(id1);


            // Assert
            Assert.That(contains, Is.True);
            Assert.That(result, Is.True);
            Assert.That(component.ContainsLink(id1), Is.False);
        }
        [Test]
        public void RemoveLink_LinkDoesNotExist_False()
        {
            // Arrange
            var id = new FakeUnitComponentId(1, "Test");
            var id1 = new FakeUnitComponentId(1, "Test1");

            var type = UnitComponentType.Attribute;
            var component = new FakeCombatUnitComponent
                (
                id,
                type,
                new List<FakeUnitComponentId> { /* Empty */ });


            // Act
            var contains = component.ContainsLink(id1);
            var result = component.RemoveLink(id1);


            // Assert
            Assert.That(contains, Is.False);
            Assert.That(result, Is.False);
            Assert.That(component.ContainsLink(id1), Is.False);
        }


        [Test]
        public void ContainsLink_ContainsId_False()
        {
            // Arrange
            var id = new FakeUnitComponentId(1, "Test");
            var id1 = new FakeUnitComponentId(1, "Test1");

            var type = UnitComponentType.Attribute;
            var component = new FakeCombatUnitComponent
                (
                id,
                type,
                new List<FakeUnitComponentId> { id1 });


            // Act
            // Nothing to do


            // Assert
            Assert.That(component.ContainsLink(id1), Is.True);
        }
        [Test]
        public void ContainsLink_DoesNotContainsId_False()
        {
            // Arrange
            var id = new FakeUnitComponentId(1, "Test");
            var id1 = new FakeUnitComponentId(1, "Test1");

            var type = UnitComponentType.Attribute;
            var component = new FakeCombatUnitComponent
                (
                id,
                type,
                new List<FakeUnitComponentId> { /* Empty */ });


            // Act
            // Nothing to do


            // Assert
            Assert.That(component.ContainsLink(id1), Is.False);
        }
    }
}
