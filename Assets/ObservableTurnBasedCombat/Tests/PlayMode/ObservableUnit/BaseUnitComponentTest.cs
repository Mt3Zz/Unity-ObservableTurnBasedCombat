using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ObservableTurnBasedCombat.Tests.PlayMode.ObservableUnit
{
    using Application;


    public class BaseUnitComponentTest
    {
        private UnitComponentId id = default;
        private UnitComponentId id1 = default;
        private UnitComponentId id2 = default;


        [SetUp]
        public void SetUp()
        {
            id = new FakeUnitComponentId(1, "Test");
            id1 = new FakeUnitComponentId(1, "Test1");
            id2 = new FakeUnitComponentId(1, "Test2");
        }


        [Test]
        public void AddRequiredLink_AdditionLink_True()
        {
            // Arrange
            var component = new ObservableUnitComponent
                (
                id,
                new List<UnitComponentId> { /* Empty */ }
                );


            // Act
            var result = component.AddRequiredLink(id1);


            // Assert
            Assert.That(result, Is.True);
            Assert.That(component.ContainsRequiredLink(id1), Is.True);
        }
        [Test]
        public void AddRequiredLink_LinkExists_False()
        {
            // Arrange
            var component = new ObservableUnitComponent
                (
                id,
                new List<UnitComponentId> { id1 }
                );


            // Act
            var result = component.AddRequiredLink(id1);


            // Assert
            Assert.That(result, Is.False);
            Assert.That(component.ContainsRequiredLink(id1), Is.True);
        }


        [Test]
        public void RemoveRequiredLink_LinkExists_True()
        {
            // Arrange
            var component = new ObservableUnitComponent
                (
                id,
                new List<UnitComponentId> { id1 }
                );


            // Act
            var contains = component.ContainsRequiredLink(id1);
            var result = component.RemoveRequiredLink(id1);


            // Assert
            Assert.That(contains, Is.True);
            Assert.That(result, Is.True);
            Assert.That(component.ContainsRequiredLink(id1), Is.False);
        }
        [Test]
        public void RemoveRequiredLink_LinkDoesNotExist_False()
        {
            // Arrange
            var component = new ObservableUnitComponent
                (
                id,
                new List<UnitComponentId> { /* Empty */ }
                );


            // Act
            var contains = component.ContainsRequiredLink(id1);
            var result = component.RemoveRequiredLink(id1);


            // Assert
            Assert.That(contains, Is.False);
            Assert.That(result, Is.False);
            Assert.That(component.ContainsRequiredLink(id1), Is.False);
        }


        [Test]
        public void ContainsLink_ContainsId_False()
        {
            // Arrange
            var component = new ObservableUnitComponent
                (
                id,
                new List<UnitComponentId> { id1 }
                );


            // Act
            // Nothing to do


            // Assert
            Assert.That(component.ContainsRequiredLink(id1), Is.True);
        }
        [Test]
        public void ContainsLink_DoesNotContainsId_False()
        {
            // Arrange
            var component = new ObservableUnitComponent
                (
                id,
                new List<UnitComponentId> { /* Empty */ }
                );


            // Act
            // Nothing to do


            // Assert
            Assert.That(component.ContainsRequiredLink(id1), Is.False);
        }
    }
}
