using System;
using System.Collections.Generic;
using ObservableCollections;

namespace ObservableTurnBasedCombat.Application
{
    /// <summary>
    /// オブザーバブルな戦闘コマンドキュー。
    /// </summary>
    public class ObservableCombatCommandQueue
    {
        private ObservableList<CommandMetadata> _metadata = new ObservableList<CommandMetadata>();
        private List<CombatCommandAsync> _commands = new List<CombatCommandAsync>();

        /// <summary>
        /// コレクションの変更イベントを取得します。
        /// </summary>
        public IObservableCollection<CommandMetadata> CollectionEvents => _metadata;

        /// <summary>
        /// キューが空かどうかを示す値を取得します。
        /// </summary>
        public bool isEmpty => _commands.Count == 0;

        /// <summary>
        /// 指定されたコマンドをキューにスケジュールします。
        /// </summary>
        /// <param name="command">スケジュールするコマンド。</param>
        public void Schedule(CombatCommandAsync command)
        {
            _commands.Add(command);
            _metadata.Add(command.Metadata);
        }

        /// <summary>
        /// キューからコマンドを取り出します。
        /// </summary>
        /// <returns>取り出されたコマンド。</returns>
        /// <exception cref="InvalidOperationException">キューが空の場合にスローされます。</exception>
        public CombatCommandAsync Dequeue()
        {
            if (_commands.Count == 0)
            {
                throw new InvalidOperationException("キューが空です。Dequeue 操作を実行する前に、キューにアイテムを追加してください。");
            }

            var result = _commands[0];
            _commands.RemoveAt(0);

            return result;
        }

        /// <summary>
        /// キューからコマンドを取り出し、取り出しに成功したかどうかを返します。
        /// </summary>
        /// <param name="result">取り出されたコマンド。取り出しが成功した場合は、このパラメータに取り出されたコマンドが格納されます。</param>
        /// <returns>取り出しに成功した場合は true。それ以外の場合は false。</returns>
        public bool TryDequeue(out CombatCommandAsync result)
        {
            if (_commands.Count == 0)
            {
                result = null;
                return false;
            }
            else
            {
                result = Dequeue();
                return true;
            }
        }
    }
}
