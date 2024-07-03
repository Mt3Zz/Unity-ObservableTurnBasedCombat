using ObservableTurnBasedCombat.Application;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class ComponentLink : IEquatable<ComponentLink>
    {
        public IComponentNode Adjacency { get; protected set; }
        public int Weight { get; protected set; }


        public ComponentLink(IComponentNode adjacency, int weight = 1)
        {
            Adjacency = adjacency;
            Weight = weight;
        }


        public void SetWeight(int weight) {  Weight = weight; }


        // IEquatableÇÃèàóù
        public bool Equals(ComponentLink other)
        {
            if (other == null || GetType() != other.GetType()) {  return false; }
            return Adjacency.Equals(other.Adjacency);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as ComponentLink);
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Adjacency);
        }
    }
}
