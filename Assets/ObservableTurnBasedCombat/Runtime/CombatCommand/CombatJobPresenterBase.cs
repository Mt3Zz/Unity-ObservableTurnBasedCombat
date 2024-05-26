using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObservableTurnBasedCombat.BusinessLogic
{
    public abstract class CombatJobPresenterBase
    {
        private readonly BaseCombatCommandAsync _job;

        public CombatJobPresenterBase(BaseCombatCommandAsync job)
        {
            _job = job;
            /*
            _job.ObservableMethods.BeforeExecute
                .Subscribe(_ => {
                //OnBeforeExecute();
            });
            _job.ObservableMethods.Excute.Subscribe(_ => OnExecute());
            _job.ObservableMethods.Complete.Subscribe(_ => OnComplete());
        */
        }

        protected abstract void OnBeforeExecute();
        protected abstract void OnExecute();
        protected abstract void OnComplete();
    }
}
