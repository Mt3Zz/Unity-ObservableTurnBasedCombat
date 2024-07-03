using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ObservableTurnBasedCombat.Tests.PlayMode.CombatUnit
{
    using Application;

    public class ComponentGraphTests
    {
        /*
        [Test]
        public void Initialize_Graph_InitializedCorrectly()
        {
            // Arrange
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            // No action needed as initialization is implicit


            // Assert
            Assert.That(graph, Is.Not.Null);
        }*/


        [Test]
        public void AddNode_NodeAdded_Correctly()
        {
            // Arrange
            var node = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            graph.AddNode(node);


            // Assert
            Assert.That(graph.ContainsNode(node), Is.True);
        }
        [Test]
        public void AddNode_AlreadyExists_ThrowsArgumentException()
        {
            // Arrange
            var node = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


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
            var node = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();
            graph.AddNode(node);


            // Act
            graph.RemoveNode(node);


            // Assert
            Assert.That(graph.ContainsNode(node), Is.False);
        }
        [Test]
        public void RemoveNode_NotExistsNodeRemoved_ThrowsKeyNotFoundException()
        {
            // Arrange
            var node = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


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
            var node = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            graph.AddNode(node);


            // Assert
            Assert.That(graph.ContainsNode(node), Is.True);
        }
        [Test]
        public void ContainsNode_NodeDoesNotExists_False()
        {
            // Arrange
            var node = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            // Nothing to do


            // Assert
            Assert.That(graph.ContainsNode(node), Is.False);
        }


        [Test]
        public void AddLink_LinkAdded_Correctly()
        {
            // Arrange
            var node1 = new FakeComponentNode();
            var node2 = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddLink(node1, node2);


            // Assert
            Assert.That(graph.ContainsLink(node1, node2), Is.True);
        }
        [Test]
        public void AddLink_AlreadyExists_ThrowsArgumentException()
        {
            // Arrange
            var node1 = new FakeComponentNode();
            var node2 = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);
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
            var node1 = new FakeComponentNode();
            var node2 = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddLink(node1, node2);
            graph.RemoveLink(node1, node2);


            // Assert
            Assert.That(graph.ContainsLink(node1, node2), Is.False);
        }
        [Test]
        public void RemoveLink_LinkDoesNotExists_ThrowsException()
        {
            // Arrange
            var node1 = new FakeComponentNode();
            var node2 = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();
            graph.AddNode(node1);
            graph.AddNode(node2);


            // Act & Assert
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
            var node1 = new FakeComponentNode();
            var node2 = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            graph.AddNode(node1);


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
            var node1 = new FakeComponentNode();
            var node2 = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddLink(node1, node2);


            // Assert
            Assert.That(graph.ContainsLink(node1, node2), Is.True);
        }
        [Test]
        public void ContainsLink_LinkDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var node1 = new FakeComponentNode();
            var node2 = new FakeComponentNode();
            var graph = new ComponentGraph<FakeComponentNode>();


            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);


            // Assert
            Assert.That(graph.ContainsLink(node1, node2), Is.False);
        }
    }
}