using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace ObservableTurnBasedCombat.Application
{
    public class CombatCommandScheduler : ScriptableObject
    {
        [SerializeField] private bool _running = false;

        public ObservableCombatCommandRunner Runner { get; private set; }
        public ObservableCombatCommandQueue Queue { get; private set; }

        private CancellationTokenSource _ctn;


        public void Awake()
        {
            Runner = new ObservableCombatCommandRunner();
            Queue = new ObservableCombatCommandQueue();

            _ctn = new CancellationTokenSource();
        }


        public async UniTask RunAsync() 
        {
            _running = true;

            while (!Queue.isEmpty)
            {
                Runner.SetCommand(Queue.Dequeue());
                await Runner.RunAsync(_ctn.Token);
            }
        }
        public void Pause()
        {
            _running = false;
            _ctn.Cancel();
        }


    }
}
