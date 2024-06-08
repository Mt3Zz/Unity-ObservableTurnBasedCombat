using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObservableTurnBasedCombat.Application
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
        /// <summary>
        /// �ǉ��R�}���h�̎��ʎq�Ɗ��荞�݊֌W���ǂ����̃^�v�����擾���܂��B
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
