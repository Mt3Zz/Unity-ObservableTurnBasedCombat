using NUnit.Framework;
using Cysharp.Threading.Tasks;
using UnityEngine.TestTools;
using System.Collections;
using System.Threading;

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    public class ICombatCommandEffectAsyncTest
    {
        [Test]
        public void Id_SameId_SameInstance()
        {
            // Arrange
            var effect = new FakeCombatCommandEffectAsync();
            var exceptedId = new CommandEffectId(1, "Test");

            // Act
            // Nothing to do

            // Assert
            Assert.That(effect.Id.Equals(exceptedId));
        }

        [UnityTest]
        public IEnumerator BeforeExecute_CallsMethod_Normal() => 
            UniTask.ToCoroutine(async () =>
            {
                // Arrange
                var effect = new FakeCombatCommandEffectAsync();
                var cancelToken = new CancellationTokenSource().Token;

                // Act
                var isCancelled = await effect.BeforeExecute(cancelToken).SuppressCancellationThrow();

                // Assert
                Assert.That(false == isCancelled);
            });

        [UnityTest]
        public IEnumerator Execute_CallsMethod_Normal() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange
                var effect = new FakeCombatCommandEffectAsync();
                var cancelToken = new CancellationTokenSource().Token;

                // Act
                var isCancelled = await effect.Execute(cancelToken).SuppressCancellationThrow();

                // Assert
                Assert.That(false == isCancelled);
            });

        [UnityTest]
        public IEnumerator Complete_CallsMethod_Normal() =>
            UniTask.ToCoroutine(async () =>
            {
                // Arrange
                var effect = new FakeCombatCommandEffectAsync();
                var cancelToken = new CancellationTokenSource().Token;

                // Act
                var isCancelled = await effect.Complete(cancelToken).SuppressCancellationThrow();

                // Assert
                Assert.That(false == isCancelled);
            });
    }
}
