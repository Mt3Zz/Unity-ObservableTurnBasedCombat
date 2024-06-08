using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatCommand
{
    using Application;

    public class BaseCombatCommandAsyncTest
    {
        [Test]
        public void Constructer_SameId_SameId()
        {
            // Arrange
            var command1 = new BaseCombatCommandAsync
            (
                new CommandId(1, "Test"),
                new List<ICombatCommandEffectAsync> { }
            );
            var command2 = new BaseCombatCommandAsync
            (
                new CommandId(1, "Test"),
                new List<ICombatCommandEffectAsync> { }
            );

            var excepted = true;


            // Act
            var result = command1.Id.Equals(command2.Id);


            // Assert
            Assert.That(excepted == result);
        }


        [Test]
        public void Constructer_SameEffectList_MatadataHasSameEffectId()
        {
            // Arrange
            var command1 = new BaseCombatCommandAsync
            (
                new CommandId(1, "Test"),
                new List<ICombatCommandEffectAsync> { }
            );
            var command2 = new BaseCombatCommandAsync
            (
                new CommandId(1, "Test"),
                new List<ICombatCommandEffectAsync> { }
            );

            var excepted = true;


            // Act
            var result = command1.Metadata.EffectIds.SequenceEqual(command2.Metadata.EffectIds);


            // Assert
            Assert.That(excepted == result);
        }


        [Test]
        public void Constructer_DifferentId_DifferentId()
        {
            // Arrange
            var command1 = new BaseCombatCommandAsync
            (
                new CommandId(1, "Test1"),
                new List<ICombatCommandEffectAsync> { }
            );
            var command2 = new BaseCombatCommandAsync
            (
                new CommandId(1, "Test2"),
                new List<ICombatCommandEffectAsync> { }
            );

            var excepted = false;


            // Act
            var result = command1.Id.Equals(command2.Id);


            // Assert
            Assert.That(excepted == result);
        }


        [Test]
        public void Constructer_DifferentEffectList_MatadataHasDifferentEffectId()
        {
            // Arrange
            var command1 = new BaseCombatCommandAsync
            (
                new CommandId(1, "Test"),
                new List<ICombatCommandEffectAsync> { }
            );
            var command2 = new BaseCombatCommandAsync
            (
                new CommandId(1, "Test"),
                new List<ICombatCommandEffectAsync>
                {
                    new FakeCombatCommandEffectAsync()
                }
            );

            var excepted = false;


            // Act
            var result = command1.Metadata.EffectIds.SequenceEqual(command2.Metadata.EffectIds);


            // Assert
            Assert.That(excepted == result);
        }


        [Test]
        public void Constructer_EffectList_MatadataHasEffectId()
        {
            // Arrange
            var fakeCommandEffect = new FakeCombatCommandEffectAsync();
            var command = new BaseCombatCommandAsync
            (
                new CommandId(1, "Test"),
                new List<ICombatCommandEffectAsync>
                {
                    fakeCommandEffect
                }
            );

            var excepted = true;


            // Act
            var result = fakeCommandEffect.Id.Equals(command.Metadata.EffectIds[0]);


            // Assert
            Assert.That(excepted == result);
        }


        [UnityTest]
        public IEnumerator BeforeExecute_CallsOnce_Normal() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var combatCommandAsync = new BaseCombatCommandAsync
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
                 var combatCommandAsync = new BaseCombatCommandAsync
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
                 var combatCommandAsync = new BaseCombatCommandAsync
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
                 var combatCommandAsync = new BaseCombatCommandAsync
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
                 var combatCommandAsync = new BaseCombatCommandAsync
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
        public IEnumerator BeforeExecute_CallsTwice_ThrowsInvalidOperationException() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange
                var combatCommandAsync = new BaseCombatCommandAsync
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
                // 1��ڂ̎��s
                await combatCommandAsync.BeforeExecute(cancelToken);
                // 2��ڂ̎��s
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