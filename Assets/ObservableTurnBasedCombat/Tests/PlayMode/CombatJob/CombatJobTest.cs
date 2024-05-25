using NUnit.Framework;
using R3;


namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using BusinessLogic;


    [TestFixture]
    public class CombatJobTest
    {
        // BeforeExecuteメソッドが1回呼び出されることを確認するテスト
        [Test]
        public void BeforeExecute_CallsMethodOnce_ListenOnce()
        {
            // Arrange
            var count = 0;
            var job = new FakeCombatJob();
            job.ObservableMethods.BeforeExecute.Subscribe(_ =>
            {
                count++;
            });

            // Act
            job.BeforeExecute();

            // Assert
            Assert.AreEqual(1, count);
        }

        // BeforeExecuteメソッドが2回呼び出されることを確認するテスト
        [Test]
        public void BeforeExecute_CallsMethodTwice_ListenTwice()
        {
            // Arrange
            var count = 0;
            var job = new FakeCombatJob();
            job.ObservableMethods.BeforeExecute.Subscribe(_ =>
            {
                count++;
            });

            // Act
            job.BeforeExecute();
            job.BeforeExecute();

            // Assert
            Assert.AreEqual(2, count);
        }

        // Executeメソッドが1回呼び出されることを確認するテスト
        [Test]
        public void Execute_CallsMethodOnce_ListenOnce()
        {
            // Arrange
            var count = 0;
            var job = new FakeCombatJob();
            job.ObservableMethods.Execute.Subscribe(_ =>
            {
                count++;
            });

            // Act
            job.Execute();

            // Assert
            Assert.AreEqual(1, count);
        }

        // Executeメソッドが2回呼び出されることを確認するテスト
        [Test]
        public void Execute_CallsMethodTwice_ListenTwice()
        {
            // Arrange
            var count = 0;
            var job = new FakeCombatJob();
            job.ObservableMethods.Execute.Subscribe(_ =>
            {
                count++;
            });

            // Act
            job.Execute();
            job.Execute();

            // Assert
            Assert.AreEqual(2, count);
        }

        // Completeメソッドが1回呼び出されることを確認するテスト
        [Test]
        public void Complete_CallsMethodOnce_ListenOnce()
        {
            // Arrange
            var count = 0;
            var job = new FakeCombatJob();
            job.ObservableMethods.Complete.Subscribe(_ =>
            {
                count++;
            });

            // Act
            job.Complete();

            // Assert
            Assert.AreEqual(1, count);
        }

        // Completeメソッドが2回呼び出されることを確認するテスト
        [Test]
        public void Complete_CallsMethodTwice_ListenTwice()
        {
            // Arrange
            var count = 0;
            var job = new FakeCombatJob();
            job.ObservableMethods.Complete.Subscribe(_ =>
            {
                count++;
            });

            // Act
            job.Complete();
            job.Complete();

            // Assert
            Assert.AreEqual(2, count);
        }
    }
}