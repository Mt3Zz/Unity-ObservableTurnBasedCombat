using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using R3;


namespace ObservableTurnBasedCombat.Tests.PlayMode.ObservableUnitTest
{
    using ObservableUnit;


    public class ObservableUnitComponentTest
    {
        private UnitComponentId id1 = default;
        private UnitComponentId id2 = default;


        [SetUp]
        public void SetUp()
        {
            id1 = new UnitComponentId(1, "Test1");
            id2 = new UnitComponentId(1, "Test2");
        }


        [Test]
        public void OnComponentChanged_ChangeComponentValue_SubscribeChange()
        {
            // Arrange
            var component = new FakeUnitComponent();
            var result = false;


            // Act
            component.OnComponentChanged.Subscribe(c =>
            {
                result = c.Equals(component);
                //Debug.Log(c.SerializeToJson(true));
            });
            component.TestValue = 1;


            // Assert
            Assert.That(result, Is.True);
        }



        [Test]
        public void Equals_SameIds_SameInstance()
        {
            // Arrange
            var component1 = new ObservableUnitComponent(id1);
            var component2 = new ObservableUnitComponent(id1);


            // Act
            // Nothing to do


            // Assert
            Assert.That(component1.Id.Equals(component2.Id), Is.True);
            Assert.That(component1.Equals(component2), Is.True);
        }
        [Test]
        public void Equals_DifferentIds_DifferentInstance()
        {
            // Arrange
            var component1 = new ObservableUnitComponent(id1);
            var component2 = new ObservableUnitComponent(id2);


            // Act
            // Nothing to do


            // Assert
            Assert.That(component1.Id.Equals(component2.Id), Is.False);
            Assert.That(component1.Equals(component2), Is.False);
        }
    }
}
