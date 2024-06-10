using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObservableCollections;
using R3;
using System;
using System.Linq;

namespace ObservableTurnBasedCombat.Application
{
    public class ObservableCombatCommandQueue : IDisposable
    {
        public IObservableCollection<CommandMetadata> CollectionEvents 
        { 
            get => _metadata; 
        }
        private ObservableList<CommandMetadata> _metadata = new ObservableList<CommandMetadata>();
        private List<CombatCommandAsync> _commands = new List<CombatCommandAsync>();


        public void Dispose()
        {
            // ObservableListのストリームソースをDisposeする
            // まだ
        }


        public void Schedule(CombatCommandAsync command)
        {
            _commands.Add(command);
            _metadata.Add(command.Metadata);
        }
        public CombatCommandAsync Dequeue()
        {
            if (_commands.Count == 0)
            {
                throw new NullReferenceException($"コマンドが存在しません");
            }

            var result = _commands[0];
            _commands.RemoveAt(0);

            return result;
        }
    }
}
