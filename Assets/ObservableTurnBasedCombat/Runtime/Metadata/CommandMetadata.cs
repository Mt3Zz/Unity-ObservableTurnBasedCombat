using System;
using System.Collections.Generic;


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
        /// <summary>
        /// �R�}���h�̎��s��Ԃ��擾���܂��B
        /// </summary>
        public CommandProgressState ProgressState { get; protected set; } = CommandProgressState.NotStarted;


        internal void SetId(CommandId id)
        {
            Id = id;
        }
        internal void SetEffectIds(List<CommandEffectId> effectIds)
        {
            EffectIds = effectIds;
        }
        internal void SetProgressState(CommandProgressState progressState)
        {
            ProgressState = progressState;
        }
    }

    public struct CommandProgressState : IEquatable<CommandProgressState>
    {
        public static readonly CommandProgressState NotStarted = new CommandProgressState(0);
        public static readonly CommandProgressState BeforeExecuted = new CommandProgressState(1);
        public static readonly CommandProgressState Executed = new CommandProgressState(2);
        public static readonly CommandProgressState Completed = new CommandProgressState(3);

        public int State { get; }
        private CommandProgressState(int state)
        {
            State = state;
        }

        public bool Equals(CommandProgressState other)
        {
            return State == other.State;
        }
    }
}
