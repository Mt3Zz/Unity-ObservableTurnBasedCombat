using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.Tests.PlayMode
{
    using BusinessLogic;


    public class FakeObserver : CombatJobPresenterBase
    {
        public int BeforeExecuteCallCount { get; private set; }
        public int ExecuteCallCount { get; private set; }
        public int CompleteCallCount { get; private set; }


        public FakeObserver(CombatJobBase action) : base(action)
        {
            BeforeExecuteCallCount = 0;
            ExecuteCallCount = 0;
            CompleteCallCount = 0;
        }

        protected override void OnBeforeExecute()
        {
            BeforeExecuteCallCount++;
        }

        protected override void OnExecute()
        {
            ExecuteCallCount++;
        }

        protected override void OnComplete()
        {
            CompleteCallCount++;
        }
    }

}
