using System;
using System.Collections.Generic;
using ObservableCollections;

namespace ObservableTurnBasedCombat.Application
{
    /// <summary>
    /// �I�u�U�[�o�u���Ȑ퓬�R�}���h�L���[�B
    /// </summary>
    public class ObservableCombatCommandQueue
    {
        private ObservableList<CommandMetadata> _metadata = new ObservableList<CommandMetadata>();
        private List<CombatCommandAsync> _commands = new List<CombatCommandAsync>();

        /// <summary>
        /// �R���N�V�����̕ύX�C�x���g���擾���܂��B
        /// </summary>
        public IObservableCollection<CommandMetadata> CollectionEvents => _metadata;

        /// <summary>
        /// �L���[���󂩂ǂ����������l���擾���܂��B
        /// </summary>
        public bool isEmpty => _commands.Count == 0;

        /// <summary>
        /// �w�肳�ꂽ�R�}���h���L���[�ɃX�P�W���[�����܂��B
        /// </summary>
        /// <param name="command">�X�P�W���[������R�}���h�B</param>
        public void Schedule(CombatCommandAsync command)
        {
            _commands.Add(command);
            _metadata.Add(command.Metadata);
        }

        /// <summary>
        /// �L���[����R�}���h�����o���܂��B
        /// </summary>
        /// <returns>���o���ꂽ�R�}���h�B</returns>
        /// <exception cref="InvalidOperationException">�L���[����̏ꍇ�ɃX���[����܂��B</exception>
        public CombatCommandAsync Dequeue()
        {
            if (_commands.Count == 0)
            {
                throw new InvalidOperationException("�L���[����ł��BDequeue ��������s����O�ɁA�L���[�ɃA�C�e����ǉ����Ă��������B");
            }

            var result = _commands[0];
            _commands.RemoveAt(0);

            return result;
        }

        /// <summary>
        /// �L���[����R�}���h�����o���A���o���ɐ����������ǂ�����Ԃ��܂��B
        /// </summary>
        /// <param name="result">���o���ꂽ�R�}���h�B���o�������������ꍇ�́A���̃p�����[�^�Ɏ��o���ꂽ�R�}���h���i�[����܂��B</param>
        /// <returns>���o���ɐ��������ꍇ�� true�B����ȊO�̏ꍇ�� false�B</returns>
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
