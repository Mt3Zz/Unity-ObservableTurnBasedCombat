using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat
{
    public class UnitMetadata
    {
        public UnitId Id { get; protected set; }


        internal UnitMetadata(UnitId id)
        {
            Id = id;
        }
    }
}
