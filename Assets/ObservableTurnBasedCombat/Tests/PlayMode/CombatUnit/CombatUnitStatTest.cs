using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.TestTools;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatUnit
{
    using Application;

    public class CombatUnitStatTest
    {
        [Test]
        public void Constructor_InitializeId_SameId()
        {
            // Arrange
            UnitStatId id = new UnitStatId(1, "Test");
            CombatUnitStat<int> stat = new CombatUnitStat<int>(id);


            // Act
            var result = id.Equals(stat.Id);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }


        [Test]
        public void TryGetById_SameId_Success()
        {
            // Arrange
            UnitStatId id = new UnitStatId(1, "Test");
            CombatUnitStat<int> stat = new CombatUnitStat<int>(id);


            // Act
            ICombatUnitStat result1;
            var result2 = stat.TryGetById(id, out result1);


            // Assert
            Assert.That(result1, Is.EqualTo(stat));
            Assert.That(result2, Is.EqualTo(true));
        }
        [Test]
        public void TryGetById_DifferentId_Failure()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStat<int> stat = new CombatUnitStat<int>(id1);


            // Act
            ICombatUnitStat result1;
            var result2 = stat.TryGetById(id2, out result1);


            // Assert
            Assert.That(result1, Is.EqualTo(null));
            Assert.That(result2, Is.EqualTo(false));
        }


        [Test]
        public void ContainsById_SameId_True()
        {
            // Arrange
            UnitStatId id = new UnitStatId(1, "Test");
            CombatUnitStat<int> stat = new CombatUnitStat<int>(id);


            // Act
            var result = stat.Contains(id);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void ContainsById_DifferentId_False()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStat<int> stat = new CombatUnitStat<int>(id1);


            // Act
            var result = stat.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }


        [Test]
        public void Add_Execute_ThrowInvalidOperationException()
        {
            // Arrange
            UnitStatId id = new UnitStatId(1, "Test");
            CombatUnitStat<int> stat = new CombatUnitStat<int>(id);


            // Act
            // Nothing to do


            // Assert
            Assert.That
            (
                () => { stat.Add(stat); },
                Throws.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("単体のステータスにステータスを追加することはできません。")
            );
        }
        [Test]
        public void Remove_Execute_ThrowInvalidOperationException()
        {
            // Arrange
            UnitStatId id = new UnitStatId(1, "Test");
            CombatUnitStat<int> stat = new CombatUnitStat<int>(id);


            // Act
            // Nothing to do


            // Assert
            Assert.That
            (
                () => { stat.RemoveById(id); },
                Throws.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("単体のステータスからステータスを削除することはできません。")
            );
        }


        [Test]
        public void Equals_SamestatIds_AreEqual()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test");
            UnitStatId id2 = new UnitStatId(1, "Test");

            CombatUnitStat<int> stat1 = new CombatUnitStat<int>(id1);
            CombatUnitStat<int> stat2 = new CombatUnitStat<int>(id2);


            // Act
            bool result = stat1.Equals(stat2);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void Equals_DifferentstatIds_AreNotEqual()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStat<int> stat1 = new CombatUnitStat<int>(id1);
            CombatUnitStat<int> stat2 = new CombatUnitStat<int>(id2);


            // Act
            bool result = stat1.Equals(stat2);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
