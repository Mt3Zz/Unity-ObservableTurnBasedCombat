using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObservableTurnBasedCombat.Editor
{
    [CustomEditor(typeof(PackagePreferences))]
    public class PackagePreferencesEditor : UnityEditor.Editor
    {
        [SerializeField]
        private VisualTreeAsset _layout = default;


        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            _layout.CloneTree(root);


            CreateRootFolderPreferences(root);
            CreateUnitPreferences(root);


            return root;
        }


        private void CreateRootFolderPreferences(VisualElement root)
        {
            var instance = PackagePreferences.instance;
            var preferences = instance.FolderPreferences;


            var rootFolderSetter = root.Q<Button>("root-folder-preferences__folder-setter");
            if(IsNullElement(rootFolderSetter, "Root Folder Setter"))
            {
                return;
            }
            var rootFolderInitializer = root.Q<Button>("root-folder-preferences__folder-initializer");
            if (IsNullElement(rootFolderInitializer, "Root Folder Initializer"))
            {
                return;
            }
            var rootFolderField = root.Q<TextField>("root-folder-preferences__current-folder");
            if (IsNullElement(rootFolderField, "Root Folder Field")) return;


            rootFolderSetter.clicked += () =>
            {
                preferences.CreateDirectoryRecursively(preferences.RootFolder);

                // 保存先のフォルダパスを取得
                var fullPath = EditorUtility.SaveFolderPanel(
                    "Root Folder", // 開かれるウィンドウのタイトル
                    "Assets", // 開いたとき表示されるフォルダ
                    "ObservableTurnBasedCombat" // 入力されている保存先
                    );

                // 選択されたならパスが入っている。キャンセルされたなら入っていない。
                if (!string.IsNullOrEmpty(fullPath))
                {
                    // フルパスを相対パスに変換
                    var matchedFullPath = System.Text.RegularExpressions.Regex.Match(fullPath, "Assets/.*");
                    var folderPath = matchedFullPath.Value;


                    // 保存処理
                    //Debug.Log(folderPath);
                    rootFolderField.value = folderPath;
                }
            };


            rootFolderInitializer.clicked += () =>
            {
                preferences.CreateDirectoryRecursively(preferences.InitialRootFolder);

                rootFolderField.value = preferences.InitialRootFolder;
            };
        }


        private void CreateUnitPreferences(VisualElement root)
        {
            CreateStorageFactoryButton(root);
            CreateTypePreferenceFactoryButton(root);
        }
        private void CreateStorageFactoryButton(VisualElement root)
        {
            var instance = PackagePreferences.instance;
            var folderPreferences = instance.FolderPreferences;
            var unitPreferences = instance.UnitPreferences;


            var storageFactory = root.Q<Button>("unit-preferences__storage-factory");
            if (IsNullElement(storageFactory, "Storage Factory")) return;


            storageFactory.clicked += () =>
            {
                folderPreferences.CreateDirectoryRecursively(folderPreferences.UnitStorageFolder);
                //Debug.Log(instance.UnitStorageFolder);


                // 保存先のファイルパスを取得
                var path = EditorUtility.SaveFilePanelInProject(
                    "Save Unit Storage", // 開かれるウィンドウのタイトル
                    "UnitStorage", // 入力されている名前
                    "asset", // 入力されている拡張子
                    "", // なんだかわからない
                    folderPreferences.UnitStorageFolder // 開いたとき表示されるフォルダ
                    );

                // 選択されたならパスが入っている。キャンセルされたなら入っていない。
                if (!string.IsNullOrEmpty(path))
                {
                    // 保存処理
                    var storage = CreateInstance<ObservableUnit.UnitConfigStorage>();
                    AssetDatabase.CreateAsset(storage, path);

                    unitPreferences.UnitStorages.Add(storage);
                }
            };
        }
        private void CreateTypePreferenceFactoryButton(VisualElement root)
        {
            var instance = PackagePreferences.instance;
            var folderPreferences = instance.FolderPreferences;
            var unitPreferences = instance.UnitPreferences;


            var typeFactory = root.Q<Button>("unit-preferences__type-preference-factory");
            if (IsNullElement(typeFactory, "Type Factory")) return;


            typeFactory.clicked += () =>
            {
                folderPreferences.CreateDirectoryRecursively(folderPreferences.TypeFolder);


                // 保存先のファイルパスを取得
                var path = EditorUtility.SaveFilePanelInProject(
                    "Save Type Preference", // 開かれるウィンドウのタイトル
                    "UnitTypes", // 入力されている名前
                    "asset", // 入力されている拡張子
                    "", // なんだかわからない
                    folderPreferences.TypeFolder // 開いたとき表示されるフォルダ
                    );

                // 選択されたならパスが入っている。キャンセルされたなら入っていない。
                if (!string.IsNullOrEmpty(path))
                {
                    // 保存処理
                    var storage = CreateInstance<ObservableUnit.TypeStorage>();
                    AssetDatabase.CreateAsset(storage, path);

                    unitPreferences.UnitType = storage;
                }
            };
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
