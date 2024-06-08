using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObservableTurnBasedCombat.Application
{
    public class CommandMetadata
    {
        /// <summary>
        /// コマンドの識別子を取得します。
        /// </summary>
        public CommandId Id { get; protected set; }
        /// <summary>
        /// コマンドに関連付けられた効果のIdのリストを取得します。
        /// </summary>
        public List<CommandEffectId> EffectIds { get; protected set; }
        /// <summary>
        /// 追加コマンドの識別子と割り込み関係かどうかのタプルを取得します。
        /// </summary>
        public (CommandId Id, bool Interruption) AdditionalCommand { get; protected set; }
    }


    internal class EditableCommandMetadata : CommandMetadata
    {
        public void SetId(CommandId id)
        {
            Id = id;
        }
        public void SetEffectIds(List<CommandEffectId> effectIds)
        {
            EffectIds = effectIds;
        }
        public void SetAdditionalCommand(CommandId id, bool interruption)
        {
            AdditionalCommand = (id, interruption);
        }
    }
}
