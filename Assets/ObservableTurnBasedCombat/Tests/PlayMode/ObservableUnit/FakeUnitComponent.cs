using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using Application;

    [Serializable]
    public class FakeUnitComponent : ObservableUnitComponent
    {
        [SerializeField] private int testValue = 0;
        public int TestValue 
        { 
            get => testValue; 
            set
            {
                testValue = value;
                NotifyComponentChanged();
            }
        }


        public FakeUnitComponent(int identify = 1)
            : base(
                  new UnitComponentId(1, $"Test{identify}")
                  )
        { }
    }
}
