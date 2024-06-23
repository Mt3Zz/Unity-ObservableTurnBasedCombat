using ObservableTurnBasedCombat.Application;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatUnitComponentLink
    {
        public ICombatUnitComponent Adjacency { get; private set; }
        public int Weight { get; private set; }


        // コンストラクタ
        public CombatUnitComponentLink(ICombatUnitComponent adjacency, int weight = 1)
        {
            Adjacency = adjacency;
            Weight = weight;
        }

    }
}
