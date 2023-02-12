#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;
using YS.Collections;
using YS.Editor.CodeGeneration;
using YS.Modules;
using YS.VM;

namespace YS.Editor {


   
    public class LibraryWindow: EditorWindow  {
        [MenuItem("YS/Library")]
        public static void OpenWindow()
        {
            _window = GetWindow<LibraryWindow>();
            _window.titleContent = new GUIContent("Library");
            // Limit size of the window
            _window.minSize = new Vector2(450, 200);
            _window.maxSize = new Vector2(1920, 720);
        }
        static LibraryWindow _window;


        public void CreateGUI() {
            GlobalModule.Activate();
            var list = new ListView {
                makeItem = () => new ListedButton(),
                bindItem = (item, index) => {
                    var button = (ListedButton) item;
                    button.Button.text = DelegateLibrary.Delegates[index].Data.MethodName;
                    button.Action = DelegateLibrary.Delegates[index].Action;
                },
                itemsSource = new object[DelegateLibrary.ActionCount]
            };
            rootVisualElement.Add(list);
        }
        
    }
    public class ListedButton : VisualElement {
        public Button Button;
        public int Index;
        public Action Action;
        public ListedButton( ) {
            Button = new Button(() => this.Action?.Invoke());
            Add(Button);
            
        }
    }
}
#endif