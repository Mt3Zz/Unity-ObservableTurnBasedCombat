using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using R3;
using ObservableCollections;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatCommand
{
    using Application;
    using UnityEngine.Profiling.Memory.Experimental;

    public class ObservableCombatCommandQueueTest
    {
        [Test]
        public void Schedule_ScheduleCommand_SubscribeAdd()
        {
            // Arrange
            var queue = new ObservableCombatCommandQueue();

            var id = new CommandId(1, "Test");
            var command = new FakeCombatCommandAsync(id);
            var result = "";
            var expected =
                $"Add Metadata[0] = {command.Id.GetHashCode()}" + "\n";
            //Debug.Log(expected);


            // Act
            queue.CollectionEvents.ObserveAdd().Subscribe(metadata =>
            {
                result +=
                $"Add Metadata[{metadata.Index}] = {metadata.Value.Id.GetHashCode()}\n";
            });

            queue.Schedule(command);


            // Assert
            //Debug.Log(result);
            Assert.That(expected == result);
        }
    }
}
