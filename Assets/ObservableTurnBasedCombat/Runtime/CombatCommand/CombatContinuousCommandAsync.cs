﻿using System;

namespace ObservableTurnBasedCombat.BusinessLogic
{
    /// <summary>
    /// 連続コマンドを表すクラスです。
    /// </summary>
    public class CombatContinuousCommandAsync : CombatCommandAsync
    {
        /// <summary>
        /// 追加コマンドを取得します。
        /// </summary>
        public CombatContinuousCommandAsync AdditionalCommand { get; private set; }
        /// <summary>
        /// このコマンドと追加コマンドが割り込み関係であるかを取得します。
        /// </summary>
        /// <remarks>
        /// Trueのとき、追加コマンドのメソッドはCommand.BeforeExecuteとCommand.Executeの間に実行されます
        /// Falseのとき、追加コマンドのメソッドはCommand.ExecuteとCommand.Completeの間に実行されます
        /// </remarks>
        public bool Interruption { get; private set; } = false;


        /// <summary>
        /// <see cref="CombatContinuousCommandAsync"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="command">このノードに関連付けられた戦闘コマンド。</param>
        public CombatContinuousCommandAsync(CombatCommandAsync command) : base(command) { }
        /// <summary>
        /// このノードに子ノードを持たせて<see cref="CombatContinuousCommandAsync"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="command">このノードに関連付けられた戦闘コマンド。</param>
        /// <param name="additionalCommand">子ノードに関連付けられる戦闘コマンド。</param>
        /// <param name="interruption">このノードとその子ノードとの関係。</param>
        public CombatContinuousCommandAsync
        (
            CombatCommandAsync command,
            CombatCommandAsync additionalCommand,
            bool interruption = false
        )
            : base(command)
        {
            AdditionalCommand = new CombatContinuousCommandAsync(additionalCommand);
            Interruption = interruption;
        }


        /// <summary>
        /// このコマンドに追加コマンドを設定します。
        /// </summary>
        /// <param name="command">設定する追加コマンド。</param>
        /// <param name="interruption">追加コマンドが割り込みするか。</param>
        /// <returns>追加コマンドが正常に設定された場合はtrue、それ以外の場合はfalse。</returns>
        public bool SetAdditionalCommand(CombatCommandAsync command, bool interruption = false)
        {
            if (hasAdditionalCommand())
            {
                return AdditionalCommand.SetAdditionalCommand(command, interruption);
            }


            switch (_state)
            {
                case CommandState.NotStarted:
                case CommandState.BeforeExecuteCalled:
                    break;
                case CommandState.ExecuteCalled:
                    if (interruption) return false;
                    break;
                case CommandState.Completed:
                    return false;
                default:
                    throw new NotImplementedException();
            }

            AdditionalCommand = new CombatContinuousCommandAsync(command);
            Interruption = interruption;
            return true;
        }
        public bool RemoveAdditionalCommand()
        {
            if (!hasAdditionalCommand())
            {
                return false;
            }

            switch (_state)
            {
                case CommandState.NotStarted:
                case CommandState.BeforeExecuteCalled:
                    break;
                case CommandState.ExecuteCalled:
                    if (Interruption) return false;
                    break;
                case CommandState.Completed:
                    return false;
                default:
                    throw new NotImplementedException();
            }

            AdditionalCommand = null;
            return true;
        }

        public bool hasAdditionalCommand()
        {
            return null != AdditionalCommand;
        }
    }
}
