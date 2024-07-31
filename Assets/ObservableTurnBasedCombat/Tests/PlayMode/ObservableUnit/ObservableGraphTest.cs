using NUnit.Framework;
using System;
using System.Collections.Generic;

using R3;


namespace ObservableTurnBasedCombat.Tests.PlayMode.ObservableUnit
{
    using Application;

    public class ObservableGraphTest
    {
        ObservableUnitComponent node = default;
        ObservableUnitComponent node1 = default;
        ObservableUnitComponent node2 = default;

        [SetUp]
        public void Setup()
        {
            node = new FakeUnitComponent();
            node1 = new FakeUnitComponent(1);
            node2 = new FakeUnitComponent(2);
        }


        [Test]
        public void Changes_AddNode_SubscribeAdd()
        {
            // Arrange
            var graph = new ObservableGraph<ObservableUnitComponent>();
            ObservableUnitComponent result = default;


            // Act
            graph.Changes.Add.Subscribe(node =>
            {
                result = node;
            });
            graph.AddNode(node);


            // Assert
            Assert.That(result, Is.EqualTo(node));
        }
        [Test]
        public void Changes_RemoveNode_SubscribeRemove()
        {
            // Arrange
            var graph = new ObservableGraph<ObservableUnitComponent>(node);
            ObservableUnitComponent result = default;


            // Act
            graph.Changes.Remove.Subscribe(node =>
            {
                result = node;
            });
            graph.RemoveNode(node);


            // Assert
            Assert.That(result, Is.EqualTo(node));
        }


        [Test]
        public void AddNode_NodeAdded_Correctly()
        {
            // Arrange
            var graph = new ObservableGraph<ObservableUnitComponent>();


            // Act
            graph.AddNode(node);


            // Assert
            Assert.That(graph.ContainsNode(node), Is.True);
        }
        [Test]
        public void AddNode_AlreadyExists_ThrowsArgumentException()
        {
            // Arrange
            var graph = new ObservableGraph<ObservableUnitComponent>();


            // Act
            graph.AddNode(node);


            // Assert
            Assert.That(
                () => { graph.AddNode(node); },
                Throws.ArgumentException
                .With.Message.EqualTo("ノードは既に存在しています。"));
        }


        [Test]
        public void RemoveNode_NodeRemoved_Normal()
        {
            // Arrange
            var graph = new ObservableGraph<ObservableUnitComponent>(node);


            // Act
            graph.RemoveNode(node);


            // Assert
            Assert.That(graph.ContainsNode(node), Is.False);
        }
        [Test]
        public void RemoveNode_NotExistsNodeRemoved_ThrowsKeyNotFoundException()
        {
            // Arrange
            var graph = new ObservableGraph<ObservableUnitComponent>();


            // Act
            // Nothing to do


            // Assert
            Assert.That(
                () => { graph.RemoveNode(node); },
                Throws.TypeOf<KeyNotFoundException>()
                .With.Message.EqualTo("ノードがグラフに存在しません。"));
        }


        [Test]
        public void ContainsNode_NodeExists_True()
        {
            // Arrange
            var graph = new ObservableGraph<ObservableUnitComponent>(node);


            // Act
            // Nothing to do



            // Assert
            Assert.That(graph.ContainsNode(node), Is.True);
        }
        [Test]
        public void ContainsNode_NodeDoesNotExists_False()
        {
            // Arrange
            var graph = new ObservableGraph<ObservableUnitComponent>();


            // Act
            // Nothing to do


            // Assert
            Assert.That(graph.ContainsNode(node), Is.False);
        }


        [Test]
        public void AddLink_LinkAdded_Correctly()
        {
            // Arrange
            var nodes = new List<ObservableUnitComponent>() { node1, node2 };
            var graph = new ObservableGraph<ObservableUnitComponent>(nodes);


            // Act
            graph.AddLink(node1, node2);


            // Assert
            Assert.That(graph.ContainsLink(node1, node2), Is.True);
        }
        [Test]
        public void AddLink_AllreadyExists_ThrowsArgumentException()
        {
            // Arrange
            var nodes = new List<ObservableUnitComponent>() { node1, node2 };
            var graph = new ObservableGraph<ObservableUnitComponent>(nodes);


            // Act
            graph.AddLink(node1, node2);


            // Assert
            Assert.That(
                () => { graph.AddLink(node1, node2); },
                Throws.TypeOf<ArgumentException>()
                .With.Message.EqualTo("リンクは既に存在しています。"));
        }


        [Test]
        public void RemoveLink_LinkRemoved_Correctly()
        {
            // Arrange
            var nodes = new List<ObservableUnitComponent>() { node1, node2 };
            var graph = new ObservableGraph<ObservableUnitComponent>(nodes);


            // Act
            graph.AddLink(node1, node2);
            graph.RemoveLink(node1, node2);


            // Assert
            Assert.That(graph.ContainsLink(node1, node2), Is.False);
        }
        [Test]
        public void RemoveLink_LinkDoesNotExists_ThrowsException()
        {
            // Arrange
            var nodes = new List<ObservableUnitComponent>() { node1, node2 };
            var graph = new ObservableGraph<ObservableUnitComponent>(nodes);


            // Act
            // Nothing to do


            // Assert
            Assert.That(
                () => { graph.RemoveLink(node1, node2); },
                Throws.TypeOf<ArgumentException>()
                .With.Message.EqualTo("指定されたリンクがグラフに存在しません。")
                );
        }
        [Test]
        public void RemoveLink_NodeDoesNotExists_ThrowsKeyNotFoundException()
        {
            // Arrange
            var graph = new ObservableGraph<ObservableUnitComponent>(node1);


            // Act
            // Nothing to do


            // Assert
            Assert.That(
                () => { graph.RemoveLink(node1, node2); },
                Throws.TypeOf<KeyNotFoundException>()
                .With.Message.EqualTo("指定されたノードがグラフに存在しません。")
                );
        }


        [Test]
        public void ContainsLink_LinkExists_ReturnsTrue()
        {
            // Arrange
            var nodes = new List<ObservableUnitComponent>() { node1, node2 };
            var graph = new ObservableGraph<ObservableUnitComponent>(nodes);


            // Act
            graph.AddLink(node1, node2);


            // Assert
            Assert.That(graph.ContainsLink(node1, node2), Is.True);
        }
        [Test]
        public void ContainsLink_LinkDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var nodes = new List<ObservableUnitComponent>() { node1, node2 };
            var graph = new ObservableGraph<ObservableUnitComponent>(nodes);


            // Act
            // Nothing to do


            // Assert
            Assert.That(graph.ContainsLink(node1, node2), Is.False);
        }
    }
}