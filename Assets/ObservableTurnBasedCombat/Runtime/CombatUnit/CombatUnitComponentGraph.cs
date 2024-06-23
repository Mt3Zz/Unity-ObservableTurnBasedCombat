using ObservableCollections;
using ObservableTurnBasedCombat.Application;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitComponentGraph
    {
        protected Dictionary<ICombatUnitComponent, List<CombatUnitComponentLink>> _linksByNode;


        public IEnumerable<ICombatUnitComponent> GetNeighborNodes(ICombatUnitComponent node)
        {
            if (_linksByNode.ContainsKey(node))
            {
                return _linksByNode[node].Cast<ICombatUnitComponent>();
            }
            else
            {
                throw new KeyNotFoundException("指定されたノードはグラフに存在しません。");
            }
        }


        public void AddNode(ICombatUnitComponent node)
        {
            if (!_linksByNode.ContainsKey(node))
            {
                _linksByNode[node] = new List<CombatUnitComponentLink>();
            }
            else
            {
                throw new ArgumentException("ノードは既に存在しています。");
            }
        }
        public void RemoveNode(ICombatUnitComponent node)
        {
            if (_linksByNode.ContainsKey(node))
            {
                _linksByNode.Remove(node);
            }
            else
            {
                throw new ArgumentException("ノードがグラフに存在しません。");
            }
        }
        public bool ContainsNode(ICombatUnitComponent node) 
        {
            return _linksByNode.ContainsKey(node);
        }


        public void AddLink(ICombatUnitComponent node1, ICombatUnitComponent node2, int weight = 1)
        {
            if (!_linksByNode.ContainsKey(node1))
            {
                throw new ArgumentException("ノード1がグラフに存在しません。先にノードを追加してください。");
            }
            if (!_linksByNode.ContainsKey(node2))
            {
                throw new ArgumentException("ノード2がグラフに存在しません。先にノードを追加してください。");
            }

            _linksByNode[node1].Add(new CombatUnitComponentLink(node2, weight));
            _linksByNode[node2].Add(new CombatUnitComponentLink(node1, weight));
        }
        public void RemoveLink(ICombatUnitComponent node1, ICombatUnitComponent node2)
        {
            if (!_linksByNode.ContainsKey(node1) || !_linksByNode.ContainsKey(node2))
            {
                throw new ArgumentException("指定されたノードがグラフに存在しません。");
            }

            foreach (var link in _linksByNode[node1])
            {
                if (link.Adjacency.Equals(node2))
                {
                    _linksByNode[(node1)].Remove(link);
                    continue;
                }
            }
            foreach (var link in _linksByNode[node2])
            {
                if (link.Adjacency.Equals(node1))
                {
                    _linksByNode[(node2)].Remove(link);
                    return;
                }
            }
        }
        public bool ContainsLink(ICombatUnitComponent node1, ICombatUnitComponent node2)
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
