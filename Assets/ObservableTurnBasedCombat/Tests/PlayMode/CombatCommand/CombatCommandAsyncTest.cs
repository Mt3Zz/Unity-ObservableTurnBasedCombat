using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatCommand
{
    using BusinessLogic;


    public class CombatCommandAsyncTests
    {
        [Test]
        public void SetAdditionalCommand_AddCommand_Success()
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
            var combatContinuousCommandAsync = new CombatCommandAsync(combatCommandAsync);

            // Act
            bool result = combatContinuousCommandAsync.SetAdditionalCommand(combatCommandAsync);

            // Assert
            Assert.That(true == result);
            Assert.That(true == combatContinuousCommandAsync.AdditionalCommand.Id.Equals(combatCommandAsync.Id));
        }


        [Test]
        public void SetAdditionalCommand_AddCommandToInstanceHavingAdditionalCommand_AddtionalCommandHasAdditionalCommand()
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
            var combatContinuousCommandAsync = new CombatCommandAsync
            (
                combatCommandAsync,
                combatCommandAsync
            );

            // Act
            bool result = combatContinuousCommandAsync.SetAdditionalCommand(combatCommandAsync);
            bool excepted = combatContinuousCommandAsync
                                .AdditionalCommand
                                .AdditionalCommand
                                .Id.Equals(combatCommandAsync.Id);

            // Assert
            Assert.That(true == result);
            Assert.That(true == excepted);
        }


        [UnityTest]
        public IEnumerator SetAdditionalCommand_AddCommandToCompletedInstance_Failure() =>
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
                 var combatContinuousCommandAsync = new CombatCommandAsync(combatCommandAsync);
                 var cancelToken = new CancellationTokenSource().Token;

                 // Act
                 await combatContinuousCommandAsync.BeforeExecute(cancelToken);
                 await combatContinuousCommandAsync.Execute(cancelToken);
                 await combatContinuousCommandAsync.Complete(cancelToken);
                 bool result = combatContinuousCommandAsync.SetAdditionalCommand(combatCommandAsync);

                 // Assert
                 Assert.That(false == result);
                 Assert.That(false == combatContinuousCommandAsync.hasAdditionalCommand());
             });


        [UnityTest]
        public IEnumerator SetAdditionalCommand_InterruptCommandToExecutedInstance_Failure() =>
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
                 var combatContinuousCommandAsync = new CombatCommandAsync(combatCommandAsync);
                 var cancelToken = new CancellationTokenSource().Token;

                 // Act
                 await combatContinuousCommandAsync.BeforeExecute(cancelToken);
                 await combatContinuousCommandAsync.Execute(cancelToken);
                 bool result = combatContinuousCommandAsync.SetAdditionalCommand(combatCommandAsync, true);

                 // Assert
                 Assert.That(false == result);
                 Assert.That(false == combatContinuousCommandAsync.hasAdditionalCommand());
             });


        [Test]
        public void RemoveAdditionalCommand_RemoveCommand_Success()
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
            var combatContinuousCommandAsync = new CombatCommandAsync
            (
                combatCommandAsync,
                combatCommandAsync
            );

            // Act
            bool result = combatContinuousCommandAsync.RemoveAdditionalCommand();

            // Assert
            Assert.That
            (
                true == result &&
                !combatContinuousCommandAsync.hasAdditionalCommand()
            );
        }

        [Test]
        public void HasAdditionalCommand_AdditionalCommandExists_True()
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
            var combatContinuousCommandAsync = new CombatCommandAsync
            (
                combatCommandAsync,
                combatCommandAsync
            );

            // Act
            // Notiong to do

            // Assert
            Assert.That(true == combatContinuousCommandAsync.hasAdditionalCommand());
        }

        [Test]
        public void HasAdditionalCommand_AdditionalCommandDoesNotExist_False()
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
            var combatContinuousCommandAsync = new CombatCommandAsync
            (
                combatCommandAsync
            );

            // Act
            // Notiong to do

            // Assert
            Assert.That(false == combatContinuousCommandAsync.hasAdditionalCommand());
        }
    }
}