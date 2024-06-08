using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObservableTurnBasedCombat
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


        internal void SetId(CommandId id)
        {
            Id = id;
        }
        internal void SetEffectIds(List<CommandEffectId> effectIds)
        {
            EffectIds = effectIds;
        }
    }
}
