using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ObservableTurnBasedCombat.BusinessLogic
{
    /// <summary>
    /// �񓯊��Ŏ��s�\�Ȑ퓬�R�}���h��\���N���X�ł��B
    /// </summary>
    public class CombatCommandAsync
    {
        /// <summary>
        /// �R�}���h�̈�ӂ̎��ʎq���擾���܂��B
        /// </summary>
        public CommandId Id { get; }

        /// <summary>
        /// �R�}���h�Ɋ֘A�t����ꂽ���ʂ̎��ʎq�̃��X�g���擾���܂��B
        /// </summary>
        public List<CommandEffectId> EffectIds { get; }

        readonly Dictionary<CommandEffectId, ICombatCommandEffectAsync> _commandEffects;

        protected enum CommandState
        {
            NotStarted, // �܂����s����Ă��Ȃ����
            BeforeExecuteCalled, // BeforeExecute ���Ăяo���ꂽ���
            ExecuteCalled, // Execute ���Ăяo���ꂽ���
            Completed // Complete ���Ăяo���ꂽ���
        }
        private CommandState _state = CommandState.NotStarted;


        /// <summary>
        /// <see cref="CombatCommandAsync"/> �̐V�����C���X�^���X�����������܂��B
        /// </summary>
        /// <param name="id">�R�}���h�̈�ӂ̎��ʎq�B</param>
        /// <param name="commandEffects">�R�}���h�Ɋ֘A�t����ꂽ���ʂ̃��X�g�B</param>
        public CombatCommandAsync(CommandId id, List<ICombatCommandEffectAsync> commandEffects)
        {
            Id = id;
            EffectIds = commandEffects.Select(ce => ce.Id).ToList();
            _commandEffects = commandEffects.ToDictionary(ce => ce.Id, ce => ce);
        }

        /// <summary>
        /// �R�}���h�̎��s�O�Ɏ��s����񓯊��������J�n���܂��B
        /// </summary>
        /// <param name="token">�������L�����Z�����邽�߂̃g�[�N���B</param>
        /// <returns>�񓯊������\���^�X�N�B</returns>
        /// <exception cref="InvalidOperationException">BeforeExecute �� 2 ��ȏ�Ăяo�����Ƃ͂ł��܂���B</exception>
        public async UniTask BeforeExecute(CancellationToken token)
        {
            if (_state != CommandState.NotStarted)
                throw new InvalidOperationException("BeforeExecute�͊��ɌĂяo����Ă��܂�");

            await ProcessEffects(token, action => action.BeforeExecute(token));

            _state = CommandState.BeforeExecuteCalled;
        }

        /// <summary>
        /// �R�}���h�̎��s�������J�n���܂��B
        /// </summary>
        /// <param name="token">�������L�����Z�����邽�߂̃g�[�N���B</param>
        /// <returns>�񓯊������\���^�X�N�B</returns>
        /// <exception cref="InvalidOperationException">Execute ���Ăяo����Ă��Ȃ����A�܂��͏��ԂɌĂяo����Ă��Ȃ��ꍇ�ɃX���[����܂��B</exception>
        public async UniTask Execute(CancellationToken token)
        {
            if (_state != CommandState.BeforeExecuteCalled)
                throw new InvalidOperationException("BeforeExecute���Ăяo����Ă��Ȃ����A���łɎ��s�ς݂ł�");

            await ProcessEffects(token, action => action.Execute(token));

            _state = CommandState.ExecuteCalled;
        }

        /// <summary>
        /// �R�}���h�̎��s������Ɏ��s����񓯊��������J�n���܂��B
        /// </summary>
        /// <param name="token">�������L�����Z�����邽�߂̃g�[�N���B</param>
        /// <returns>�񓯊������\���^�X�N�B</returns>
        /// <exception cref="InvalidOperationException">Complete ���Ăяo����Ă��Ȃ����A�܂��͏��ԂɌĂяo����Ă��Ȃ��ꍇ�ɃX���[����܂��B</exception>
        public async UniTask Complete(CancellationToken token)
        {
            if (_state != CommandState.ExecuteCalled)
                throw new InvalidOperationException("Execute���Ăяo����Ă��Ȃ����A���łɎ��s�ς݂ł�");

            await ProcessEffects(token, action => action.Complete(token));

            _state = CommandState.Completed;
        }

        private async UniTask ProcessEffects(CancellationToken token, Func<ICombatCommandEffectAsync, UniTask> action)
        {
            var taskList = new List<UniTask>();
            foreach (var commandEffect in _commandEffects.Values)
            {
                taskList.Add(action(commandEffect));
            }
            await UniTask.WhenAll(taskList);
        }
    }
}