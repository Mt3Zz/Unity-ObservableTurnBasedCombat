using System;
using System.Collections.Generic;
using System.Linq;

using R3;


namespace ObservableTurnBasedCombat.Application
{
    public class ObservableGraph<TNode> : IDisposable
        where TNode : IEquatable<TNode>
    {
        public (
            Observable<TNode> Add, 
            Observable<TNode> Remove
        ) Changes => (
            _addSubject, 
            _removeSubject
            );
        private Subject<TNode> _addSubject = new();
        private Subject<TNode> _removeSubject = new();


        protected Dictionary<TNode, List<TNode>> _linksByNode = new();


        public ObservableGraph() { }
        internal ObservableGraph(TNode node) : this(new List<TNode>() { node }) { }
        internal ObservableGraph(IEnumerable<TNode> nodes)
        {
            foreach (var node in nodes)
            {
                _linksByNode.Add(node, new List<TNode>());
            }
        }


        public void Dispose()
        {
            _addSubject.Dispose();
            _removeSubject.Dispose();
        }


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
                _linksByNode[node] = new();
                _addSubject.OnNext(node);
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
                _removeSubject?.OnNext(node);
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


        public void AddLink(TNode node1, TNode node2)
        {
            if (!_linksByNode.ContainsKey(node1))
            {
                throw new KeyNotFoundException("ノード1がグラフに存在しません。先にノードを追加してください。");
            }
            if (!_linksByNode.ContainsKey(node2))
            {
                throw new KeyNotFoundException("ノード2がグラフに存在しません。先にノードを追加してください。");
            }


            if (_linksByNode[node1].Contains(node2))
            {
                throw new ArgumentException("リンクは既に存在しています。");
            }
            if (_linksByNode[node2].Contains(node1))
            {
                throw new ArgumentException("リンクは既に存在しています。");
            }

            _linksByNode[node1].Add(node2);
            _linksByNode[node2].Add(node1);
        }
        public void RemoveLink(TNode node1, TNode node2)
        {
            if (!_linksByNode.ContainsKey(node1) || !_linksByNode.ContainsKey(node2))
            {
                throw new KeyNotFoundException("指定されたノードがグラフに存在しません。");
            }


            if
            (
                _linksByNode[node1].Contains(node2) ||
                _linksByNode[node2].Contains(node1)
            )
            {
                _linksByNode[node1].Remove(node2);
                _linksByNode[node2].Remove(node1);
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


            var result = true;
            result &= _linksByNode[node1].Contains(node2);
            result &= _linksByNode[node2].Contains(node1);

            return result;
        }
    }
}
