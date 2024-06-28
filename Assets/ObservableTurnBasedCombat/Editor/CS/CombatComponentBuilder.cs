using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObservableTurnBasedCombat.Editor
{
    public sealed class CombatComponentBuilder : EditorWindow
    {
        // メニューに表示される名前
        private const string MENU_NAME = "Observable Turn-Based Combat";
        // ウィンドウタイトル
        private const string WINDOW_TITLE = "Combat";


        // メイン画面レイアウト
        [SerializeField] private VisualTreeAsset _rootLayout = default;
        // サイドバーレイアウト
        [SerializeField] private VisualTreeAsset _sidebarLayout = default;
        // ウィンドウの構造ファイル
        [SerializeField] private BuilderStruct _builderStruct = default;


        private readonly List<TreeViewItemData<Item>> _sidebarItems = new();


        [MenuItem("Window/" + MENU_NAME)]
        private static void ShowEditor()
        {
            // Unityのデフォルトの挙動ではウィンドウが再利用されるようになっており、
            // メニュー項目を再選択すると既存のウィンドウが表示される。

            // 関数 EditorWindow.GetWindow を使用することで
            // 再選択時に既存のウィンドウを使用するデフォルトの挙動になる。
            CombatComponentBuilder window = GetWindow<CombatComponentBuilder>();

            // ウィンドウのタイトルを変更する
            window.titleContent = new GUIContent(WINDOW_TITLE);
        }


        private void CreateGUI()
        {
            _rootLayout.CloneTree(rootVisualElement);
            //rootVisualElement.styleSheets.Add(_rootStyleSheet);


            // === サイドバーの設定 ===================================================================

            var sidebar = rootVisualElement.Q<TreeView>(name: "side-bar");

            // データソースを設定
            sidebar.SetRootItems(_sidebarItems);
            // アイテムのレイアウトを作成する処理
            sidebar.makeItem = _sidebarLayout.CloneTree;
            // アイテムの内容を設定する処理
            sidebar.bindItem = (item, index) =>
            {
                var label = item.Q<Label>();

                // アイテムのラベルを設定
                label.text = sidebar.GetItemDataForIndex<Item>(index).label;
                // アイテムがクリックされたときの処理を設定
                label.RegisterCallback<ClickEvent>(OnSidebarContentClicked);
            };


            // === 編集画面の設定 =========================================================

            var editor = rootVisualElement.Q<ScrollView>(name: "content");

            // データソースを設定
            var initBuilderName = _builderStruct.InitialBuilderName;
            editor.Add(_builderStruct.GetUxmlByName(initBuilderName).Instantiate());

        }
        private void Reset()
        {
            CreateSidebar();
        }


        private void OnSidebarContentClicked(ClickEvent clickEvent)
        {
            // クリックされた要素のテキストを取得
            var element = clickEvent.target as VisualElement;
            var text = element.Q<Label>().text;

            // すでにあるウィンドウをいったんクリア
            var editor = rootVisualElement.Q<ScrollView>(name: "content");
            editor.Clear();


            // テキストに対応するUxmlを追加
            editor.Add(_builderStruct.GetUxmlByName(text).Instantiate());
        }
        private void CreateSidebar()
        {
            var id = 0;
            foreach (var groupName in _builderStruct.GroupNames)
            {
                var items = new List<TreeViewItemData<Item>>();
                foreach (var itemName in _builderStruct.ChildNamesByGroupName[groupName])
                {
                    // 子要素を追加
                    items.Add(new TreeViewItemData<Item>(id++, new Item { label = itemName }));
                }

                // 子要素を持つ親要素を追加
                _sidebarItems.Add(new TreeViewItemData<Item>(id++, new Item { label = groupName }, items));
            }
        }
        [Serializable] public struct Item
        {
            public string label;
        }
    }
}
