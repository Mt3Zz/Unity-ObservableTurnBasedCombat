using System;
using System.Collections.Generic;
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
            [SerializeField] public VisualTreeAsset uxml;
        }


        [SerializeField] private List<ItemGroup> _itemGroups = default;


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
                foreach (var items in _itemGroups)
                {
                    var list = new List<(string, bool, VisualTreeAsset)>();
                    foreach (var child in items.childItems)
                    {
                        list.Add((child.name, child.enable, child.uxml));
                    }

                    yield return (
                        items.item.name,
                        items.item.enable,
                        items.item.uxml,
                        list
                    );
                } 
            }
        }


        public string GetFirstWindowNameOrEmpty()
        {
            if (_itemGroups.Count == 0)
            {
                return "";
            }
            return _itemGroups[0].item.name;
        }
        public VisualTreeAsset GetUxmlByName(string name)
        {
            foreach(var itemGroup in _itemGroups)
            {
                if (name == itemGroup.item.name)
                {
                    if (null == itemGroup.item.uxml) throw new NullReferenceException($"{name}Ç…ëŒâûÇ∑ÇÈuxmlÇ™ë∂ç›ÇµÇ‹ÇπÇÒÅB");
                    return itemGroup.item.uxml;
                }

                foreach(var childItem in itemGroup.childItems)
                {
                    if (name == childItem.name)
                    {
                        if (null == childItem.uxml) throw new NullReferenceException($"{name}Ç…ëŒâûÇ∑ÇÈuxmlÇ™ë∂ç›ÇµÇ‹ÇπÇÒÅB");
                        return childItem.uxml;
                    }
                }
            }
            throw new NotImplementedException($"{name}ÇÕìoò^Ç≥ÇÍÇƒÇ¢Ç‹ÇπÇÒÅB");
        }
    }
}
