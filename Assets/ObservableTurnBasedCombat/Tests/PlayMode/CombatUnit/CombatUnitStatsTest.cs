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
            UnitStatId id = new UnitStatId(1, "Test");
            CombatUnitStats stats = new CombatUnitStats(id);


            // Act
            var result = id.Equals(stats.Id);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void Constructor_InitializeIdAndEnum_SameId()
        {
            // Arrange
            UnitStatId id = new UnitStatId(1, "Test");
            CombatUnitStats stats = new CombatUnitStats
            (
                id,
                new List<ICombatUnitStat> { }
            );


            // Act
            var result = id.Equals(stats.Id);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }


        [Test]
        public void TryGetById_SameId_Success()
        {
            // Arrange
            UnitStatId id = new UnitStatId(1, "Test");
            CombatUnitStats stats = new CombatUnitStats(id);


            // Act
            ICombatUnitStat result1;
            var result2 = stats.TryGetById(id, out result1);


            // Assert
            Assert.That(result1, Is.EqualTo(stats));
            Assert.That(result2, Is.EqualTo(true));
        }
        [Test]
        public void TryGetById_ChildId_Success()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStat<int> stat = new CombatUnitStat<int>(id2);

            CombatUnitStats stats = new CombatUnitStats
            (
                id1,
                new List<ICombatUnitStat> { stat }
            );


            // Act
            ICombatUnitStat result1;
            var result2 = stats.TryGetById(id2, out result1);


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

            CombatUnitStats stats = new CombatUnitStats(id1);


            // Act
            ICombatUnitStat result1;
            var result2 = stats.TryGetById(id2, out result1);


            // Assert
            Assert.That(result1, Is.EqualTo(null));
            Assert.That(result2, Is.EqualTo(false));
        }


        [Test]
        public void ContainsById_ContainsId_True()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStat<int> stat = new CombatUnitStat<int>(id2);

            CombatUnitStats stats = new CombatUnitStats
            (
                id1,
                new List<ICombatUnitStat> { stat }
            );


            // Act
            var result = stats.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void ContainsById_Empty_False()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStat<int> stat = new CombatUnitStat<int>(id2);

            CombatUnitStats stats = new CombatUnitStats
            (
                id1,
                new List<ICombatUnitStat> { /* Empty */ }
            );


            // Act
            var result = stats.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void ContainsById_DoesnotContainId_False()
        {
            // Arrange
            UnitStatId id = new UnitStatId(1, "Test");
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStat<int> stat = new CombatUnitStat<int>(id1);

            CombatUnitStats stats = new CombatUnitStats
            (
                id,
                new List<ICombatUnitStat> { stat }
            );


            // Act
            var result = stats.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void ContainsById_SelfId_False()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStats stats = new CombatUnitStats(id1);
            CombatUnitStat<int> stat = new CombatUnitStat<int>(id2);


            // Act
            stats.Add(stat);
            var result = stats.Contains(id1);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }


        [Test]
        public void Add_AddStat_Success()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStats stats = new CombatUnitStats(id1);
            CombatUnitStat<int> stat = new CombatUnitStat<int>(id2);


            // Act
            stats.Add(stat);
            var result = stats.Contains(id2);


            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void Add_AddSameStatTwice_ThrowArgumentException()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStats stats = new CombatUnitStats(id1);
            CombatUnitStat<int> stat = new CombatUnitStat<int>(id2);


            // Act
            stats.Add(stat);


            // Assert
            Assert.That
            (
                () => { stats.Add(stat); },
                Throws.TypeOf<ArgumentException>()
                .With.Message.EqualTo("同じIdを持つステータスを追加することはできません。")
            );
        }


        [Test]
        public void Remove_RemoveStat_Success()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStat<int> stat = new CombatUnitStat<int>(id2);

            CombatUnitStats stats = new CombatUnitStats
            (
                id1,
                new List<ICombatUnitStat> { stat }
            );


            // Act
            var result1 = stats.RemoveById(stat.Id);
            var result2 = stats.Contains(stat.Id);


            // Assert
            Assert.That(result1, Is.EqualTo(true));
            Assert.That(result2, Is.EqualTo(false));
        }
        [Test]
        public void Remove_RemoveNotHavingStat_False()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStat<int> stat = new CombatUnitStat<int>(id2);

            CombatUnitStats stats = new CombatUnitStats
            (
                id1,
                new List<ICombatUnitStat> { /* Empty */ }
            );


            // Act
            var result1 = stats.RemoveById(stat.Id);
            var result2 = stats.Contains(stat.Id);


            // Assert
            Assert.That(result1, Is.EqualTo(false));
            Assert.That(result1, Is.EqualTo(false));
        }


        [Test]
        public void Equals_SameStatIds_AreEqual()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test");
            UnitStatId id2 = new UnitStatId(1, "Test");

            CombatUnitStats stats1 = new CombatUnitStats(id1);
            CombatUnitStats stats2 = new CombatUnitStats(id2);

            var excepted = true;


            // Act
            bool result = stats1.Equals(stats2);


            // Assert
            Assert.That(result, Is.EqualTo(excepted));
        }
        [Test]
        public void Equals_DifferentStatIds_AreNotEqual()
        {
            // Arrange
            UnitStatId id1 = new UnitStatId(1, "Test1");
            UnitStatId id2 = new UnitStatId(1, "Test2");

            CombatUnitStats stats1 = new CombatUnitStats(id1);
            CombatUnitStats stats2 = new CombatUnitStats(id2);


            // Act
            bool result = stats1.Equals(stats2);


            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
