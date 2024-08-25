using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


namespace ObservableTurnBasedCombat.Editor
{
    internal class WindowStructure : ScriptableObject
    {
        [Serializable] private struct ItemGroup
        {
            [SerializeField] public Item item;
            [SerializeField] public List<Item> childItems;
        }
        [Serializable] private struct Item
        {
            [SerializeField] public string name;
            [SerializeField] public bool enable;
            [SerializeField] public VisualTreeAsset layout;
        }


        [SerializeField] private List<ItemGroup> _pageSelectorItems = default;
        [SerializeField] private List<Item> _preferenceSelectorItems = default;


        public IEnumerable<
            (
            string name, 
            bool enable, 
            VisualTreeAsset layout,
            IEnumerable<
                (
                string name, 
                bool enable, 
                VisualTreeAsset layout
                )> children
            )> 
            Structure
        {
            get
            {
                foreach (var items in _pageSelectorItems)
                {
                    var list = new List<(string, bool, VisualTreeAsset)>();
                    foreach (var child in items.childItems)
                    {
                        list.Add((child.name, child.enable, child.layout));
                    }

                    yield return (
                        items.item.name,
                        items.item.enable,
                        items.item.layout,
                        list
                    );
                } 
            }
        }
        public IEnumerable<
            (
            string name,
            bool enable,
            VisualTreeAsset layout
            )>
            PreferenceStructure
        {
            get => _preferenceSelectorItems.Select(item =>
            {
                return (item.name, item.enable, item.layout);
            });
        }


        public string GetFirstWindowNameOrEmpty()
        {
            if (_pageSelectorItems.Count == 0)
            {
                return "";
            }
            return _pageSelectorItems[0].item.name;
        }
        public VisualTreeAsset GetLayoutByName(string name)
        {
            var msg = "レイアウト";
            return GetItemByName(name, msg).layout;
        }
        private Item GetItemByName(string name, string logMsg = "アイテム")
        {
            foreach (var itemGroup in _pageSelectorItems)
            {
                if (name == itemGroup.item.name)
                {
                    if (null == itemGroup.item.layout) throw new NullReferenceException($"{name}に対応する{logMsg}が存在しません。");
                    return itemGroup.item;
                }

                foreach (var childItem in itemGroup.childItems)
                {
                    if (name == childItem.name)
                    {
                        if (null == childItem.layout) throw new NullReferenceException($"{name}に対応する{logMsg}が存在しません。");
                        return childItem;
                    }
                }
            }

            foreach(var item in _preferenceSelectorItems)
            {
                if (name == item.name)
                {
                    if(null  == item.layout) throw new NullReferenceException($"{name}に対応する{logMsg}が存在しません。");
                    return item;
                }
            }

            throw new NotImplementedException($"{name}は登録されていません。");
        }
    }
}
