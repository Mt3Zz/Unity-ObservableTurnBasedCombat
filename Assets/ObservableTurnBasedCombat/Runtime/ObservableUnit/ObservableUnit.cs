using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using R3;


namespace ObservableTurnBasedCombat.ObservableUnit
{
    [Serializable]
    public class ObservableUnit : IDisposable
    {
        public UnitId Id { get => _id; }
        [SerializeField] private UnitId _id = default;

        public IEnumerable<ObservableUnitComponent> Components { get => _componentById.Values; }
        protected Dictionary<UnitComponentId, ObservableUnitComponent> _componentById = new();

        public ObservableGraph<ObservableUnitComponent> Graph { get; } = new();


        [SerializeReference] private IUnitRepository repository;


        /// <summary>
        /// R3.Observableを使用してJSON文字列を通知します。
        /// </summary>
        public (
            Observable<string> Snapshot,
            Observable<string> Diff
        ) ObservableChanges => (
            _snapshotSubject,
            _diffSubject
        );
        private readonly Subject<string> _snapshotSubject = new();
        private readonly Subject<string> _diffSubject = new();

        private CompositeDisposable _disposables = new();


        // 変更されたUnitComponentのリスト
        private List<ObservableUnitComponent> _changedComponents = new();
        //*/


        // コンストラクタ
        public ObservableUnit(UnitId id)//, ICombatUnitRepository repository)
        {
            _id = id;

            Graph.Changes.Add
                .Subscribe(OnComponentAdded)
                .AddTo(_disposables);
            Graph.Changes.Remove
                .Subscribe(OnComponentRemoved)
                .AddTo(_disposables);
        }
        internal ObservableUnit(UnitId id, IUnitRepository repository)
            : this(id)
        { this.repository = repository; }


        public void Dispose()
        {
            _disposables.Dispose();

            _snapshotSubject.Dispose();
            _diffSubject.Dispose();
        }


        /// <summary>
        /// スナップショットと差分をJSON文字列にシリアライズして通知するメソッド
        /// </summary>
        /// <param name="prettyPrint"></param>
        public void Publish(bool prettyPrint = false)
        {
            // 変更されたコンポーネントがない場合、なにも処理せず終了
            if (_changedComponents.Count == 0) return;


            _snapshotSubject.OnNext(CreateJson(_componentById.Values, prettyPrint));
            _diffSubject.OnNext(CreateJson(_changedComponents, prettyPrint));

            // 変更のキャッシュをクリア
            _changedComponents.Clear();

        }


        // コンポーネントが追加されたときの処理
        private void OnComponentAdded(ObservableUnitComponent component)
        {
            _componentById.Add(component.Id, component);
            _changedComponents.Add(component);


            // サブスクライブ
            component.OnComponentChanged
                .Subscribe(OnComponentChanged)
                .AddTo(_disposables);


            // リポジトリからフェッチ
            foreach(var requiredLink in component.RequiredLinks)
            {
                if (!_componentById.Keys.Contains(requiredLink))
                {
                    var additionalComponent = repository.FetchComponentById(requiredLink);

                    Graph.AddNode(additionalComponent);
                    Graph.AddLink(component, additionalComponent);
                }
            }
        }
        // コンポーネントが削除されたときの処理
        private void OnComponentRemoved(ObservableUnitComponent component)
        {
            _componentById.Remove(component.Id);
            //_changedComponents.Add(component);
            //component.OnComponentChanged.Subscribe(OnComponentChanged);
        }
        // コンポーネントが変更されたときの処理
        private void OnComponentChanged(ObservableUnitComponent component)
        {
            if (!_changedComponents.Contains(component))
            {
                _changedComponents.Add(component);

                // 変更があったらすぐにPublishする場合
                //Publish();
            }
        }


        private string CreateJson(IEnumerable<ObservableUnitComponent> components, bool prettyPrint= false)
        {
            // 手作業でjson形式に整形する。
            // （いまいちポイント）

            // UnitComponentをJsonUtilityでシリアライズすると、
            // サブクラスのフィールドが捨てられてしまう。
            
            // 外部のjsonライブラリを採用すれば手作業でやる必要がなくなるかも？


            var sb = new StringBuilder();
            var first = true;


            Action<string> set = (str) => { sb.Append(str); };
            Action setReturn = () =>
            {
                if (prettyPrint) set("\n");
            };
            Action setIndent = () =>
            {
                if (prettyPrint) set("    ");
            };


            // Jsonの作成（手作業で！！）
            set("{"); setReturn();

            setIndent(); set("\"Id\":");
            set(Id.Serialize());

            foreach (var component in components)
            {
                set(","); setReturn();

                if (first)
                {
                    setIndent();  set("\"Components\":["); setReturn();
                    first = false;
                }

                setIndent(); setIndent(); set(component.SerializeToJson());
            }

            setReturn(); 
            setIndent(); set("]"); setReturn(); 
            set("}");


            //UnityEngine.Debug.Log(sb.ToString());
            return sb.ToString();
        }
    }
}
