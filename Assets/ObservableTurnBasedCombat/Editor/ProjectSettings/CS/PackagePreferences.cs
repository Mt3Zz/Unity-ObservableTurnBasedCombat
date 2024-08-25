using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ObservableTurnBasedCombat.Editor
{
    [FilePath("ProjectSettings/OTBCWindowPreferences.asset",
        FilePathAttribute.Location.ProjectFolder)]
    internal class PackagePreferences : ScriptableSingleton<PackagePreferences>
    {
        public FolderPreferences FolderPreferences => _folderPreferences;
        [SerializeField]
        private FolderPreferences _folderPreferences = new();


        public UnitPreferences UnitPreferences => _unitPreferences;
        [SerializeField]
        private UnitPreferences _unitPreferences = new();


        internal void Save()
        {
            Save(true);
        }
    }
    [Serializable]
    internal class FolderPreferences
    {
        internal string RootFolder => _rootFolder;
        internal string InitialRootFolder => _initialRootFolder;
        internal string UnitFolder => _rootFolder + "/Units";
        internal string UnitStorageFolder => _rootFolder + "/UnitStorages";
        internal string TypeFolder => _rootFolder + "/Types";

        [SerializeField]
        private string _rootFolder = _initialRootFolder;
        [SerializeField]
        private const string _initialRootFolder = "Assets/ObservableTurnBasedCombat";



        /// <summary>
        /// AssetDatabase.CreateFolderで再帰的にディレクトリを作成する。
        /// そのまま呼び出すと、途中にディレクトリが存在しない場合エラーになる。
        /// </summary>
        /// <param name="dirPath">"Assets/"からはじまるディレクトリパス</param>
        internal void CreateDirectoryRecursively(string dirPath)
        {
            // Assetsから始まらない場合は処理をスキップ
            if (!dirPath.StartsWith("Assets/"))
            {
                var msg = ""
                    + "ディレクトリパスが\"Assets/\"からはじまりません。\n"
                    + "ディレクトリ作成処理をスキップしました。\n"
                    + $"dirPath : {dirPath}";
                Debug.Log(msg);
                return;
            }

            // 区切り文字"/"で分解
            var dirs = dirPath.Split("/");
            var path = dirs[0];

            // Assetsの部分をスキップ
            foreach (var dir in dirs.Skip(1))
            {
                // ディレクトリが存在するか確認
                if (!AssetDatabase.IsValidFolder(path + "/" + dir))
                {
                    AssetDatabase.CreateFolder(path, dir);
                }
                path += "/" + dir;
            }
        }
    }
    [Serializable]
    internal class UnitPreferences
    {
        internal List<ObservableUnit.UnitConfigStorage> UnitStorages => _unitStorages;
        [SerializeField]
        private List<ObservableUnit.UnitConfigStorage> _unitStorages = new();


        internal IEnumerable<string> StorageNames
            => _unitStorages
            .Select(storage => storage.name);


        internal ObservableUnit.TypeStorage UnitType
        {
            get => _unitType;
            set => _unitType = value;
        }
        [SerializeField]
        private ObservableUnit.TypeStorage _unitType;

    }
}
