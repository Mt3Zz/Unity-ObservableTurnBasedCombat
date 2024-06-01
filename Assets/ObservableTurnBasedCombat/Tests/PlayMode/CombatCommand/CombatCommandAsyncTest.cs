using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using BusinessLogic;

    public class CombatCommandAsyncTest
    {
        [UnityTest]
        public IEnumerator BeforeExecute_CallsOnce_Normal() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var combatCommandAsync = new CombatCommandAsync
                 (
                     new CommandId(1, "Test"),
                     new List<ICombatCommandEffectAsync>
                     {
                        new FakeCombatCommandEffectAsync()
                     }
                 );
                 var cancelToken = new CancellationTokenSource().Token;

                 // Act
                 var isCancelled = await combatCommandAsync.BeforeExecute(cancelToken).SuppressCancellationThrow();

                 // Assert
                 Assert.That(false == isCancelled);
             });

        [UnityTest]
        public IEnumerator Execute_CallsInOrder_Normal() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var combatCommandAsync = new CombatCommandAsync
                 (
                     new CommandId(1, "Test"),
                     new List<ICombatCommandEffectAsync>
                     {
                        new FakeCombatCommandEffectAsync()
                     }
                 );
                 var cancelToken = new CancellationTokenSource().Token;
                 var excepted = false;

                 // Act
                 await combatCommandAsync.BeforeExecute(cancelToken);
                 try
                 {
                     await combatCommandAsync.Execute(cancelToken);
                 }
                 catch (InvalidOperationException)
                 {
                     excepted = true;
                 }

                 // Assert
                 Assert.That(false == excepted);
             });

        [UnityTest]
        public IEnumerator Complete_CallsInOrder_Normal() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var combatCommandAsync = new CombatCommandAsync
                 (
                     new CommandId(1, "Test"),
                     new List<ICombatCommandEffectAsync>
                     {
                        new FakeCombatCommandEffectAsync()
                     }
                 );
                 var cancelToken = new CancellationTokenSource().Token;
                 var excepted = false;

                 // Act
                 await combatCommandAsync.BeforeExecute(cancelToken);
                 await combatCommandAsync.Execute(cancelToken);
                 try
                 {
                     await combatCommandAsync.Complete(cancelToken);
                 }
                 catch (InvalidOperationException)
                 {
                     excepted = true;
                 }

                 // Assert
                 Assert.That(false == excepted);
             });

        [UnityTest]
        public IEnumerator Execute_CallsOnce_ThrowsInvalidOperationException() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var combatCommandAsync = new CombatCommandAsync
                 (
                     new CommandId(1, "Test"),
                     new List<ICombatCommandEffectAsync>
                     {
                        new FakeCombatCommandEffectAsync()
                     }
                 );
                 var cancelToken = new CancellationTokenSource().Token;
                 var excepted = false;

                 // Act
                 try
                 {
                     await combatCommandAsync.Execute(cancelToken);
                 }
                 catch (InvalidOperationException)
                 {
                     excepted = true;
                 }

                 // Assert
                 Assert.That(true == excepted);
             });

        [UnityTest]
        public IEnumerator Complete_CallsOnce_ThrowsInvalidOperationException() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var combatCommandAsync = new CombatCommandAsync
                 (
                     new CommandId(1, "Test"),
                     new List<ICombatCommandEffectAsync>
                     {
                        new FakeCombatCommandEffectAsync()
                     }
                 );
                 var cancelToken = new CancellationTokenSource().Token;
                 var excepted = false;

                 // Act
                 try
                 {
                     await combatCommandAsync.Complete(cancelToken);
                 }
                 catch (InvalidOperationException)
                 {
                     excepted = true;
                 }

                 // Assert
                 Assert.That(true == excepted);
             });

        [UnityTest]
        public IEnumerator BeforeExecute_CalledTwice_ThrowsInvalidOperationException() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange
                var combatCommandAsync = new CombatCommandAsync
                (
                    new CommandId(1, "Test"),
                    new List<ICombatCommandEffectAsync>
                    {
                        new FakeCombatCommandEffectAsync()
                    }
                );
                var cancelToken = new CancellationTokenSource().Token;
                var excepted = false;

                // Act
                // 1回目の実行
                await combatCommandAsync.BeforeExecute(cancelToken);
                // 2回目の実行
                try
                {
                    await combatCommandAsync.BeforeExecute(cancelToken);
                }
                catch (InvalidOperationException)
                {
                    excepted = true;
                }

                // Assert
                Assert.That(true == excepted);
            });
    }
}