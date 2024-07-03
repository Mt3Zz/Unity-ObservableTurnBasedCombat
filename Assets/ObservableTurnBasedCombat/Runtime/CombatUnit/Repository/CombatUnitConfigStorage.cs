using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.DataAccess
{
    public class CombatUnitConfigStorage : ScriptableObject
    {
        [SerializeField] private CombatUnitConfig _commonComponent = new();
        [SerializeField] private List<CombatUnitConfig> _uniqueComponents = new();


        public void Reset()
        {
            _uniqueComponents = new List<CombatUnitConfig>
            {
                new() { type = 1, name = "test attribute" },
                new() { type = 2, name = "test stats" },
                new() { type = 3, name = "test timer" },
            };
        }
    }

    [Serializable]
    public struct CombatUnitConfig
    {
        public int type;
        public string name;
    }
}
