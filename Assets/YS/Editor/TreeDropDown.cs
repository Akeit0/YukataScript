using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace YS.Editor {
    public class TreeDropDown<T> : AdvancedDropdown {
        public class DropdownItem : AdvancedDropdownItem {
           
            public (string,T)  Item { get; }

            public DropdownItem (NameSplitTree<T>.Node parentNode, (string,T) item) : base(item.Item1) {
              
                Item = item;
            }

        }
        public Action<string,T> onItemSelected = null;
        NameSplitTree<T> _tree;
        AdvancedDropdownItem _root;
        public Texture2D NodeIcon;

        public TreeDropDown(AdvancedDropdownState state, NameSplitTree<T> tree) : base(state) {
            _tree = tree;
            var newMinimumSize= minimumSize;
           newMinimumSize.y = 15 * EditorGUIUtility.singleLineHeight;
           minimumSize = newMinimumSize;
        }

        public void SetTree(NameSplitTree<T> tree) {
            _tree = tree;
        }
        public bool HasBuilt => _root != null;
        public void Clear() {
            _tree = null;
            _root = null;
        }
        protected override AdvancedDropdownItem BuildRoot() {
            if (_root != null) return _root;
            _root = new AdvancedDropdownItem("Search");
            foreach (var pair in _tree.Leaves) {
                _root.AddChild( new DropdownItem(null, (pair.Key,pair.Value)));
            }
            foreach (var pair in _tree.Children) {
                var node = pair.Value.GetDeep();
                var current = new AdvancedDropdownItem(node.Name);
                _root.AddChild(current);
                AddItems(current,node);
                if (NodeIcon != null) {
                    current.icon = NodeIcon;
                }
            }
            _tree = null;
            return _root;
        }

        void AddItems(AdvancedDropdownItem item,NameSplitTree<T>.Node node) {
            if(node.Leaves!=null) {
                foreach (var pair in node.Leaves) {
                    item.AddChild(new DropdownItem(node,pair.Value));
                }
            }
            var nameSpaces = node.Children;
            if (nameSpaces == null) return;
            foreach (var pair in nameSpaces) {
                var newNode = pair.Value.GetDeep();
                var newItem = new AdvancedDropdownItem(newNode.Name);
                item.AddChild(newItem);
                AddItems(newItem,newNode);
                if (NodeIcon != null) {
                    newItem.icon = NodeIcon;
                }
            }
            
        }


        protected override void ItemSelected(AdvancedDropdownItem item) {
            base.ItemSelected(item);
            if (item is DropdownItem dropdownItem) {
                onItemSelected?.Invoke(dropdownItem.Item.Item1,dropdownItem.Item.Item2);
            }
         
        }
    }
}
#endif