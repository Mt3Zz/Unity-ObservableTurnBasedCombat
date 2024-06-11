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
        public ObservableCombatCommandRunner Runner { get; private set; } = new ObservableCombatCommandRunner();
        public ObservableCombatCommandQueue Queue { get; private set; } = new ObservableCombatCommandQueue();

        private CancellationTokenSource _ctn = new CancellationTokenSource();


        public async UniTask RunAsync()
        {
            while (!Queue.isEmpty)
            {
                Runner.SetCommand(Queue.Dequeue());
                await Runner.RunAsync(_ctn.Token);
            }
        }
        public void Pause()
        {
            _ctn.Cancel();
            _ctn.Dispose(); // Dispose the CancellationTokenSource to release resources
            _ctn = new CancellationTokenSource(); // Create a new CancellationTokenSource for potential future use
        }
    }
}
