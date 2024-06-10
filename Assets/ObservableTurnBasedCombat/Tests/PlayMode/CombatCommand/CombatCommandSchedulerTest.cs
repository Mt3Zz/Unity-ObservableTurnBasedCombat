using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using R3;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatCommand
{
    using Application;
    using Cysharp.Threading.Tasks;
    using System.Reflection;
    using System.Threading;
    using UnityEngine;

    public class CombatCommandSchedulerTest
    {
        [UnityTest]
        public IEnumerator RunAsync_ScheduleCommandOnce_SubscribeRun() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange
                var scheduler = ScriptableObject.CreateInstance<CombatCommandScheduler>();

                var id = new CommandId(1, "Test");
                var command = new FakeCombatCommandAsync(id);

                var result = "";
                var expected = "" +
                $"{id.GetHashCode()}\n";


                // Act
                scheduler.Runner.ObservableEvents.BeforeExecute.Subscribe(metadata =>
                {
                    result +=
                    $"{metadata.Id.GetHashCode()}\n";
                });

                scheduler.Queue.Schedule(command);
                await scheduler.RunAsync();


                // Assert
                Assert.That(expected == result);
            });

        [UnityTest]
        public IEnumerator RunAsync_ScheduleCommandTwice_SubscribeRun() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange
                var scheduler = ScriptableObject.CreateInstance<CombatCommandScheduler>();

                var id = new CommandId(1, "Test");

                var result = "";
                var expected = "" +
                $"{id.GetHashCode()}\n" +
                $"{id.GetHashCode()}\n";


                // Act
                scheduler.Runner.ObservableEvents.BeforeExecute.Subscribe(metadata =>
                {
                    result +=
                    $"{metadata.Id.GetHashCode()}\n";
                });

                scheduler.Queue.Schedule(new FakeCombatCommandAsync(id));
                scheduler.Queue.Schedule(new FakeCombatCommandAsync(id));
                await scheduler.RunAsync();


                // Assert
                //UnityEngine.Debug.Log(result);
                //UnityEngine.Debug.Log(expected);
                Assert.That(expected == result);
            });
    }
}
