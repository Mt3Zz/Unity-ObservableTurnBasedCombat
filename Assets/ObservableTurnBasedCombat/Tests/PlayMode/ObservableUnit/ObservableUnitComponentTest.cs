using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using R3;


namespace ObservableTurnBasedCombat.Tests.PlayMode.ObservableUnit
{
    using Application;


    public class ObservableUnitComponentTest
    {
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
            component.TestValue =1;


            // Assert
            Assert.That(result, Is.True);
        }
    }
}
