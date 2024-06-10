using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using System.Threading;
using R3;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatCommand
{
    using Application;
    using System;
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
                 runner.SetCommand(combatCommandAsync);
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
                 runner.SetCommand(combatCommandAsync);
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

                 runner.SetCommand(combatCommandAsync);
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
        public IEnumerator RunAsync_BeforeExecutedCommand_SubscribeExcludingBeforeExecute() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var runner = new ObservableCombatCommandRunner();
                 var id = new CommandId(1, "Test");
                 var combatCommandAsync = new FakeCombatCommandAsync(id);

                 runner.SetCommand(combatCommandAsync);

                 var result = "";
                 var expected =
                     $"Execute : {id.GetHashCode()}\n" +
                     $"Complete : {id.GetHashCode()}\n";

                 var cancelToken = new CancellationTokenSource().Token;


                 // Act
                 // 事前にBeforeExecuteを実行
                 await runner.Command.BeforeExecute(cancelToken);

                 // 購読
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

                 // 実行
                 await runner.RunAsync(cancelToken);


                 // Assert
                 Assert.That(expected == result);
             });


        [UnityTest]
        public IEnumerator RunAsync_RunAndCancelAndRun_SubscribeInOrder() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var runner = new ObservableCombatCommandRunner();
                 var id = new CommandId(1, "Test");
                 var combatCommandAsync = new FakeCombatCommandAsync(id);

                 runner.SetCommand(combatCommandAsync);

                 var result = "";
                 var expected =
                     $"BeforeExecute : {id.GetHashCode()}\n" +
                     $"Execute : {id.GetHashCode()}\n" +
                     $"Complete : {id.GetHashCode()}\n";

                 var cancelToken = new CancellationTokenSource();


                 // Act
                 // 購読
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

                 // 実行
                 await runner.RunAsync(cancelToken.Token);
                 cancelToken.Cancel();
                 await runner.RunAsync(cancelToken.Token);


                 // Assert
                 Assert.That(expected == result);
             });


        [Test]
        public void SetCommand_NotStartedCommand_Success()
        {
            // Arrange
            var runner = new ObservableCombatCommandRunner();
            var id = new CommandId(1, "Test");
            var combatCommandAsync = new FakeCombatCommandAsync(id);


            // Act
            runner.SetCommand(combatCommandAsync);


            // Assert
            Assert.That(true);
        }


        [UnityTest]
        public IEnumerator SetCommand_BeforeExecutedCommand_ThrowArgumentException() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var runner = new ObservableCombatCommandRunner();
                 var id = new CommandId(1, "Test");
                 var command = new FakeCombatCommandAsync(id);

                 var cancelToken = new CancellationTokenSource().Token;


                 // Act
                 await command.BeforeExecute(cancelToken);


                 // Assert
                 Assert.That
                 (
                     () => { runner.SetCommand(command); },
                     Throws.TypeOf<ArgumentException>().With.Message.EqualTo("ステートがNotStarted以外のコマンドをセットすることはできません。")
                 );
             });


        [UnityTest]
        public IEnumerator SetCommand_ExecuteCommandMethodAndSetCommand_ThrowInvalidOperationException() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var runner = new ObservableCombatCommandRunner();
                 var id = new CommandId(1, "Test");

                 var cancelToken = new CancellationTokenSource().Token;


                 // Act
                 runner.SetCommand(new FakeCombatCommandAsync(id));
                 await runner.Command.BeforeExecute(cancelToken);


                 // Assert
                 Assert.That
                 (
                     () => { runner.SetCommand(new FakeCombatCommandAsync(id)); },
                     Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("実行中のコマンドを上書きすることはできません。")
                 );
             });


        [UnityTest]
        public IEnumerator SetCommand_RunAsyncAndSetCommand_Saccess() =>
             UniTask.ToCoroutine(async () =>
             {
                 // Arrange
                 var runner = new ObservableCombatCommandRunner();
                 var id = new CommandId(1, "Test");

                 var cancelToken = new CancellationTokenSource().Token;


                 // Act
                 runner.SetCommand(new FakeCombatCommandAsync(id));
                 await runner.RunAsync(cancelToken);
                 runner.SetCommand(new FakeCombatCommandAsync(id));


                 // Assert
                 Assert.That(true);
             });
    }
}
