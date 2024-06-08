using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObservableTurnBasedCombat
{
    public class CommandMetadata
    {
        /// <summary>
        /// �R�}���h�̎��ʎq���擾���܂��B
        /// </summary>
        public CommandId Id { get; protected set; }
        /// <summary>
        /// �R�}���h�Ɋ֘A�t����ꂽ���ʂ�Id�̃��X�g���擾���܂��B
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
