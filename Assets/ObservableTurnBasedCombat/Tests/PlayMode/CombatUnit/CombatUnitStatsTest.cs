using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.TestTools;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatUnit
{
    using Application;


    public class CombatUnitStatsTest
    {
        [Test]
        public void Constructor_InitializeId_SameId()
        {
            // Arrange
            var id = new UnitStatsId(1, "Test");
            var stats = new CombatUnitStats<int>(id);


            // Act
            var result = id.Equals(stats.Id);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }


        [Test]
        public void Work_In_Progress()
        {
            Assert.That(false);
        }
    }
}
