using System;
using System.Collections.Generic;
using System.Linq;

namespace ObservableTurnBasedCombat.Application
{
    public class ComponentGraph<TNode>
        where TNode : IComponentNode
    {
        protected Dictionary<TNode, List<ComponentLink>> _linksByNode = new();


        public IEnumerable<TNode> GetNeighborNodes(TNode node)
        {
            if (_linksByNode.ContainsKey(node))
            {
                return _linksByNode[node].Cast<TNode>();
            }
            else
            {
                throw new KeyNotFoundException("指定されたノードはグラフに存在しません。");
            }
        }


        public void AddNode(TNode node)
        {
            if (!_linksByNode.ContainsKey(node))
            {
                _linksByNode[node] = new List<ComponentLink>();
            }
            else
            {
                throw new ArgumentException("ノードは既に存在しています。");
            }
        }
        public void RemoveNode(TNode node)
        {
            if (_linksByNode.ContainsKey(node))
            {
                _linksByNode.Remove(node);
            }
            else
            {
                throw new KeyNotFoundException("ノードがグラフに存在しません。");
            }
        }
        public bool ContainsNode(TNode node) 
        {
            return _linksByNode.ContainsKey(node);
        }


        public void AddLink(TNode node1, TNode node2, int weight = 1)
        {
            if (!_linksByNode.ContainsKey(node1))
            {
                throw new KeyNotFoundException("ノード1がグラフに存在しません。先にノードを追加してください。");
            }
            if (!_linksByNode.ContainsKey(node2))
            {
                throw new KeyNotFoundException("ノード2がグラフに存在しません。先にノードを追加してください。");
            }

            var link1 = new ComponentLink(node2, weight);
            var link2 = new ComponentLink(node1, weight);

            if (_linksByNode[node1].Contains(link1))
            {
                throw new ArgumentException("リンクは既に存在しています。");
            }
            if (_linksByNode[node2].Contains(link2))
            {
                throw new ArgumentException("リンクは既に存在しています。");
            }

            _linksByNode[node1].Add(link1);
            _linksByNode[node2].Add(link2);
        }
        public void RemoveLink(TNode node1, TNode node2)
        {
            if (!_linksByNode.ContainsKey(node1) || !_linksByNode.ContainsKey(node2))
            {
                throw new KeyNotFoundException("指定されたノードがグラフに存在しません。");
            }

            var link1 = new ComponentLink(node2);
            var link2 = new ComponentLink(node1);

            if 
            (
                _linksByNode[node1].Contains(link1) ||
                _linksByNode[node2].Contains(link2)
            )
            {
                _linksByNode[node1].Remove(link1);
                _linksByNode[node2].Remove(link2);
            }
            else
            {
                throw new ArgumentException("指定されたリンクがグラフに存在しません。");
            }
        }
        public bool ContainsLink(TNode node1, TNode node2)
        {
            if (!_linksByNode.ContainsKey(node1) || !_linksByNode.ContainsKey(node2))
            {
                return false;
            }

            foreach (var link in _linksByNode[node1])
            {
                if(link.Adjacency.Equals(node2)) return true;
            }
            foreach (var link in _linksByNode[node2])
            {
                if (link.Adjacency.Equals(node1)) return true;
            }

            return false;
        }
    }
}
