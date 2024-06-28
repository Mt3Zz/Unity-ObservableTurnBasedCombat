using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObservableTurnBasedCombat.Editor
{
    internal class BuilderStruct : ScriptableObject
    {
        [SerializeField] private List<BuilderItem> _items = default;


        public string InitialBuilderName { get => GroupNames[0]; }
        public IList<string> GroupNames
        {
            get
            {
                var list = new List<string>();
                foreach ( var item in _items )
                {
                    if(item.isShown) list.Add(item.name);
                }
                return list;
            }
        }
        public IDictionary<string, IList<string>> ChildNamesByGroupName
        {
            get
            {
                var dict = new Dictionary<string, IList<string>>();
                foreach (var item in _items)
                {
                    if (item.isShown)
                    {
                        var list = new List<string>();
                        foreach(var childItem in item.childItems)
                        {
                            if(childItem.isShown) list.Add(childItem.name);
                        }

                        dict.Add(item.name, list);
                    }
                }
                return dict;
            }
        }


        public VisualTreeAsset GetUxmlByName(string name)
        {
            foreach(var item in _items)
            {
                if (name == item.name)
                {
                    if (null == item.uxml) throw new NullReferenceException($"{name}Ç…ëŒâûÇ∑ÇÈuxmlÇ™ë∂ç›ÇµÇ‹ÇπÇÒÅB");
                    return item.uxml;
                }

                foreach(var childItem in item.childItems)
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
    [Serializable]
    internal struct BuilderItem
    {
        [SerializeField] public bool isShown;
        [SerializeField] public string name;
        [SerializeField] public VisualTreeAsset uxml;
        [SerializeField] public List<BuilderChildItem> childItems;
    }
    [Serializable]
    internal struct BuilderChildItem
    {
        [SerializeField] public bool isShown;
        [SerializeField] public string name;
        [SerializeField] public VisualTreeAsset uxml;
    }
}
