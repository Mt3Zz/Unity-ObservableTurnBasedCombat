using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using System.Threading;
using R3;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatCommand
{
    using Application;
    using UnityEngine.Profiling.Memory.Experimental;

    public class ObservableCombatCommandRunnerTest
    {
        [UnityTest]
        public IEnumerator RunAsync_CommandWithoutAdditionalCommand_SubscribeInOrder() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var runner = new ObservableCombatCommandRunner();
                 var id = new CommandId(1, "Test");
                 var combatCommandAsync = new FakeCombatCommandAsync(id);

                 var result = "";
                 var expected =
                     $"BeforeExecute : {id.GetHashCode()}\n" +
                     $"Execute : {id.GetHashCode()}\n" +
                     $"Complete : {id.GetHashCode()}\n";

                 var cancelToken = new CancellationTokenSource().Token;


                 // Act
                 runner.Initialize(combatCommandAsync);
                 runner.ObservableEvents.BeforeExecute.Subscribe(metadata =>
                 {
                     result += $"BeforeExecute : {metadata.Id.GetHashCode()}\n";
                 });
                 runner.ObservableEvents.Execute.Subscribe(metadata =>
                 {
                     result += $"Execute : {metadata.Id.GetHashCode()}\n";
                 });
                 runner.ObservableEvents.Complete.Subscribe(metadata =>
                 {
                     result += $"Complete : {metadata.Id.GetHashCode()}\n";
                 });

                 await runner.RunAsync(cancelToken);


                 // Assert
                 Assert.That(expected == result);
             });


        [UnityTest]
        public IEnumerator RunAsync_CommandWithAdditionalCommand_SubscribeInOrder() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var runner = new ObservableCombatCommandRunner();
                 var id1 = new CommandId(1, "Test1");
                 var id2 = new CommandId(1, "Test2");
                 var combatCommandAsync = new FakeCombatCommandAsync(id1);
                 var additionalCommandAsync = new FakeCombatCommandAsync(id2);
                 combatCommandAsync.SetAdditionalCommand(additionalCommandAsync);

                 var result = "";
                 var expected =
                     $"BeforeExecute : {id1.GetHashCode()}\n" +
                     $"Execute : {id1.GetHashCode()}\n" +
                     $"BeforeExecute : {id2.GetHashCode()}\n" +
                     $"Execute : {id2.GetHashCode()}\n" +
                     $"Complete : {id2.GetHashCode()}\n" +
                     $"Complete : {id1.GetHashCode()}\n";

                 var cancelToken = new CancellationTokenSource().Token;


                 // Act
                 runner.Initialize(combatCommandAsync);
                 runner.ObservableEvents.BeforeExecute.Subscribe(metadata =>
                 {
                     result += $"BeforeExecute : {metadata.Id.GetHashCode()}\n";
                 });
                 runner.ObservableEvents.Execute.Subscribe(metadata =>
                 {
                     result += $"Execute : {metadata.Id.GetHashCode()}\n";
                 });
                 runner.ObservableEvents.Complete.Subscribe(metadata =>
                 {
                     result += $"Complete : {metadata.Id.GetHashCode()}\n";
                 });

                 await runner.RunAsync(cancelToken);


                 // Assert
                 Assert.That(expected == result);
             });


        [UnityTest]
        public IEnumerator RunAsync_CommandWithInterruptCommand_SubscribeInOrder() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var runner = new ObservableCombatCommandRunner();
                 var id1 = new CommandId(1, "Test1");
                 var id2 = new CommandId(1, "Test2");
                 var combatCommandAsync = new FakeCombatCommandAsync(id1);
                 var interruptCommandAsync = new FakeCombatCommandAsync(id2);

                 var result = "";
                 var expected =
                     $"BeforeExecute : {id1.GetHashCode()}\n" +
                     $"BeforeExecute : {id2.GetHashCode()}\n" +
                     $"Execute : {id2.GetHashCode()}\n" +
                     $"Complete : {id2.GetHashCode()}\n" +
                     $"Execute : {id1.GetHashCode()}\n" +
                     $"Complete : {id1.GetHashCode()}\n";

                 var cancelToken = new CancellationTokenSource().Token;


                 // Act
                 combatCommandAsync.SetAdditionalCommand(interruptCommandAsync, true);

                 runner.Initialize(combatCommandAsync);
                 runner.ObservableEvents.BeforeExecute.Subscribe(metadata =>
                 {
                     result += $"BeforeExecute : {metadata.Id.GetHashCode()}\n";
                 });
                 runner.ObservableEvents.Execute.Subscribe(metadata =>
                 {
                     result += $"Execute : {metadata.Id.GetHashCode()}\n";
                 });
                 runner.ObservableEvents.Complete.Subscribe(metadata =>
                 {
                     result += $"Complete : {metadata.Id.GetHashCode()}\n";
                 });

                 await runner.RunAsync(cancelToken);


                 // Assert
                 Assert.That(expected == result);
             });

        /*
        [Test]
        public void Running_SetToTrue_Test()
        {
            // Arrange
            var runner = new ObservableCombatCommandRunner();


            // Act
            runner.Running = true;


            //Assert
            Assert.That(true == runner.Running);
        }*/
    }
}
