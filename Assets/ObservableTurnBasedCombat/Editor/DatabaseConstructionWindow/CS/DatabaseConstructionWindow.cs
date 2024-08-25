using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace ObservableTurnBasedCombat.Editor
{
    public sealed class DatabaseConstructionWindow : EditorWindow
    {
        // メニューの名前
        private const string MENU_TITLE = "OTBC Database Construction";
        // ウィンドウの名前
        private const string WINDOW_TITLE = "Database Construction";


        // メイン画面レイアウト
        [SerializeField] private VisualTreeAsset _layout = default;
        // ウィンドウの構造データ
        [SerializeField] private WindowStructure _structure = default;


        [MenuItem("Window/" + MENU_TITLE)]
        private static void ShowEditor()
        {
            // Unityのデフォルトの挙動ではウィンドウが再利用されるようになっており、
            // メニュー項目を再選択すると既存のウィンドウが表示される。

            // 関数 EditorWindow.GetWindow を使用することで
            // 再選択時に既存のウィンドウを使用するデフォルトの挙動になる。
            DatabaseConstructionWindow window = GetWindow<DatabaseConstructionWindow>();
            
            // ウィンドウのタイトルを変更する
            window.titleContent = new GUIContent(WINDOW_TITLE);
        }
        private void CreateGUI()
        {
            var root = rootVisualElement;
            _layout.CloneTree(root);
            //root.styleSheets.Add(_rootStyleSheet);


            // ページセレクターを作成
            //Debug.Log("Create Page Selector : Started");
            CreatePageSelector(root);
            //Debug.Log("Create Page Selector : Completed");


            // プリファレンスセレクターを作成
            CreatePreferenceSelector(root);


            // ページを作成
            CreatePage(root);
        }
        private void Reset()
        {
            //Debug.Log("Reset Page Selector : Started");
            ResetPageSelector();
            //Debug.Log("Reset Page Selector : Completed");
        }




        private readonly List<TreeViewItemData<string>> _pageSelectorItems = new();
        private void CreatePageSelector(VisualElement root)
        {
            var treeView = root.Q<TreeView>("page-selector");
            // nullチェック
            if (IsNullElement(treeView, "Side Bar : page selector"))
            {
                return;
            }


            // アイテム作成設定
            treeView.SetRootItems(_pageSelectorItems);
            treeView.makeItem = () =>
            {
                var element = new Label();
                element.name = "page-selector__item";

                return element;
            };
            treeView.bindItem = (item, index) =>
            {
                var label = item.Q<Label>();
                if (IsNullElement(label, "Sidebar : Label")) return;
                label.text = treeView.GetItemDataForIndex<string>(index);

                // アイテムがクリックされたときの処理を設定
                label.RegisterCallback<ClickEvent>(OnPageSelectorItemClicked);
            };


            treeView.ExpandAll();
        }
        private void ResetPageSelector()
        {
            var id = 0;
            // ScriptableObjectから構造を取得する
            foreach (var (groupName, enable, uxml, children) in _structure.Structure)
            {
                if (!enable) continue;

                // TreeViewItemDataはreadonly structなので
                // 先にリストをつくって初期化時に入れる必要がある
                var items = new List<TreeViewItemData<string>>();

                // 子要素を作成
                foreach (var child in children)
                {
                    var item = new TreeViewItemData<string>(id++, child.name);
                    items.Add(item);
                }

                // 親要素を作成
                var rootItem = new TreeViewItemData<string>(id++, groupName, items);

                // 親要素をデータソースに追加
                _pageSelectorItems.Add(rootItem);
            }


            /* Log Message for Debug
            var msg = "";
            foreach(var item in _sidebarItems)
            {
                msg += $"item.data : {item.data}\n";
                foreach(var child in item.children)
                {
                    msg += $"    child.data : {child.data}\n";
                }
            }
            Debug.Log(msg);
            //*/
        }
        private void OnPageSelectorItemClicked(ClickEvent clickEvent)
        {
            // クリックされた要素のテキストを取得
            var element = clickEvent.target as VisualElement;

            var label = element.Q<Label>();
            if (IsNullElement(label, "クリックされたVisualElement : Label"))
            {
                return;
            }


            //Debug.Log($"Clicked Element : {label.text}");
            RefreshPage(rootVisualElement, label.text);
        }


        private void CreatePreferenceSelector(VisualElement root)
        {
            var listView = root.Q<ListView>("preference-selector");
            if (IsNullElement(listView, "Preference Selector : ListView")) return;

            
            var list = new List<string>();
            foreach(var (name, enable, _)  in _structure.PreferenceStructure)
            {
                if (enable) list.Add(name);
            }
            //Debug.Log($"Preference Selector Items\n{string.Join("\n", list)}");


            listView.itemsSource = list;
            listView.makeItem = () => new Label();
            listView.bindItem = (item, index) =>
            {
                var label = item.Q<Label>();
                if (IsNullElement(label, "Sidebar : Label")) return;

                var text = (string)listView.itemsSource[index];
                label.text = text;

                // アイテムがクリックされたときの処理を設定
                label.RegisterCallback<ClickEvent>(OnReferenceSelectorItemClicked);
            };
        }
        private void OnReferenceSelectorItemClicked(ClickEvent clickEvent)
        {
            // クリックされた要素のテキストを取得
            var element = clickEvent.target as VisualElement;

            var label = element.Q<Label>();
            if (IsNullElement(label, "クリックされたVisualElement : Label"))
            {
                return;
            }


            // ここでは「Player Settings」を開きます。
            // 他の設定ウィンドウを開きたい場合は、適切なメニューアイテムに変更してください。
            switch (label.text)
            {
                case "Preferences":
                    SettingsService.OpenProjectSettings("Project/Observable Turn-Based Combat");
                    break;
                default:
                    Debug.Log($"対応する設定ウィンドウが存在しません。\n{label.text}\n");
                    break;
            }
        }


        private void CreatePage(VisualElement root)
        {
            var firstWindowName = _structure.GetFirstWindowNameOrEmpty();
            RefreshPage(root, firstWindowName);
        }
        private void RefreshPage(VisualElement root, string windowName)
        {
            var page = rootVisualElement.Q<ScrollView>(name: "content");
            if (IsNullElement(page, "Database Construction Window : Page")) return;


            // 表示中のページをクリア
            page.Clear();

            // データ構造からレイアウトを取得して追加
            var layout = _structure.GetLayoutByName(windowName);
            var container = layout.CloneTree();
            //var storage = _structure.GetStorageByName(windowName);
            //var container = new InspectorElement(storage);
            page.Add(container);
        }


        private bool IsNullElement(VisualElement element, string logTitle = "")
        {
            if (element == null)
            {
                var msg = ""
                    + logTitle
                    + "\n"
                    + " 対応するVisualElementが見つかりません。\n"
                    + " 表示をスキップしました。\n";
                Debug.Log(msg);


                return true;
            }
            return false;
        }
    }
}
