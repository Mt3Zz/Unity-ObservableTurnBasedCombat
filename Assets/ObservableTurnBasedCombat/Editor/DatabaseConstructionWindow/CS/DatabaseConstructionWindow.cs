using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
        // メイン画面スタイル
        [SerializeField] private StyleSheet _style = default;


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
            root.styleSheets.Add(_style);


            // ページセレクターを作成
            CreatePageSelector(root);

            // プリファレンスリンクを作成
            CreatePreferenceLink(root);
        }
        private void OnFocus()
        {
            var root = rootVisualElement;
            CreatePageSelector(root);
        }


        private void CreatePageSelector(VisualElement root)
        {
            var sidebar = root.Q<VisualElement>("sidebar");
            if (IsNullElement(sidebar, "Sidebar")) return;


            CreateUnitStorageSelector(sidebar);
        }
        private void CreateUnitStorageSelector(VisualElement root)
        {
            var selector = root.Q<ListView>("unit-storage-selector");
            if (IsNullElement(selector, "Page Selector : unit-storage-selector")) return;


            var preferences = PackagePreferences.instance.UnitPreferences;
            var storages = preferences.UnitStorages;


            selector.itemsSource = storages;
            selector.makeItem = () =>
            {
                var label = new Label();
                label.AddToClassList("unit-storage-selector__items--margin");
                return label;
            };
            selector.bindItem = (element, index) =>
            {
                var label = element.Q<Label>();
                if (IsNullElement(label, "Unit Storage Selector : Item Label")) return;

                label.text = storages[index].name;
                label.RegisterCallback<ClickEvent>(evt =>
                {
                    var storage = storages[index];
                    RefreshPage(storage);
                });
            };
        }


        private void CreatePreferenceLink(VisualElement root)
        {
            var button = root.Q<Button>("preference-link__button");
            if (IsNullElement(button, "Preference Selector : Button")) return;


            button.clicked += () =>
            {
                SettingsService.OpenProjectSettings("Project/Observable Turn-Based Combat");
            };
        }


        private void RefreshPage(ObservableUnit.UnitConfigStorage storage)
        {
            var page = rootVisualElement.Q<ScrollView>("content");
            if (IsNullElement(page, "Content : page")) return;


            var inspector = new InspectorElement(storage);

            page.Clear();
            page.Add(inspector);
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
