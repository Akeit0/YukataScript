#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;
using YS.Collections;
using YS.Editor.CodeGeneration;
using YS.Modules;

namespace YS.Editor {
    
   
[Serializable]
    public struct NameAndCodeGenTypePair {
        public string Name;
        public CodeGenerationType CodeGenerationType;

        public NameAndCodeGenTypePair(string name, CodeGenerationType type) {
            Name = name;
            CodeGenerationType = type;
        }
    }
    public class WrapperGenerationWindow: EditorWindow {
        
        public static StringDictionary<Assembly> AssemblyDictionary;
        static readonly List<(string name,Assembly)>    _assemblies=new ();
        static readonly List<(string name ,CodeGenerationType codeGenerationType,Type type)>    _types=new();

        static HashSet<Type> _selectedTypesSet = new HashSet<Type>();

        static List<string>    _assemblyNames;
        static List<NameAndCodeGenTypePair>    _typeDatas;
        
        public static TreeDropDown<Assembly> ADropDown;
        public static TreeDropDown<Type> TypeDropDown;
        public static NameSplitTree<Type> typeTree;
        [SerializeField] int _selectedIndex = -1;
        VisualElement _rightPane;
        static ListView AssembliesView;
        static ListView TypesView;
        ListView InspectorTypesView;
        TwoPaneSplitView _splitView;

        public static List<string> List = new List<string>(){"Assemblies","ModuleTypes","ModuleTypes(Generic(WIP))","Settings(WIP)"};
        public static List<string> GenerationType = new() {"All","CodeOnly","InspectorOnly" };
        static string path = "Assets/YS.Generated";

        static WrapperGenerationWindow _window;
        [MenuItem("YS/Code Generation")]
        public static void OpenWindow()
        {
            if (_window != null) {
                _window.Close();
            }
            // This method is called when the user selects the menu item in the Editor
            _window = GetWindow<WrapperGenerationWindow>();
            _window.titleContent = new GUIContent("Code Generation");
            // Limit size of the window
            _window.minSize = new Vector2(450, 200);
            _window.maxSize = new Vector2(1920, 720);
        }
       
        static StringDictionary<Assembly> GetDictionary(Assembly[] assemblyArray) {
            var dict = new StringDictionary<Assembly>(assemblyArray.Length);
            foreach (var assembly in assemblyArray) {
                dict[assembly.GetName().Name]=assembly;
            }
            return dict;
        }
        static IEnumerable<(string,Type)> GetTypePairs(IEnumerable<Assembly> _assemblies) {
            foreach (var assembly in _assemblies) {
                if (assembly is null) continue;
                foreach (var type in assembly.GetTypes()) {
                    if(type.IsPublic) yield return (type.BuildFullName(),type);
                }
            }
        }
        static IEnumerable<(string,Type)> GetNonGenericTypePairs(List<(string name,Assembly)>    assemblyPairs) {
            foreach (var assemblyPair in _assemblies) {
                if (assemblyPair.Item2 is null) continue;
                if (assemblyPair.name == "mscorlib") {
                    foreach (var type in assemblyPair.Item2.GetTypes()) {
                        if((type.IsPublic || type.IsNestedPublic)&&!type.IsGenericTypeDefinition) {
                            yield return (type.BuildFullName(true), type);
                        }
                    }
                }
                else {
                    foreach (var type in assemblyPair.Item2.GetTypes()) {
                        if((type.IsPublic || type.IsNestedPublic)&&!type.IsGenericTypeDefinition) {
                            yield return (type.BuildFullName(true), type);
                        }
                    }
                }
            }
        }
        
        static StringDictionary<Type> specialTypeDictionary = new StringDictionary<Type>() {
            {"int", typeof(int)},
            {"float", typeof(float)},
            {"double", typeof(double)},
            {"decimal", typeof(decimal)},
            {"short", typeof(short)},
            {"ushort", typeof(ushort)},
            {"char", typeof(char)},
            {"byte", typeof(byte)},
            {"sbyte", typeof(sbyte)},
            {"bool", typeof(bool)},
            {"long", typeof(ulong)},
            {"object", typeof(object)},
            {"string", typeof(string)},
        };
        static Type GetType(string typeName) {
            if(typeName.StartsWith('(')) {
                typeName = typeName.Split('.')[1];
            }
            if (specialTypeDictionary.TryGetValue(typeName, out var type)) {
                return type;
            }
            foreach (var pair in _assemblies) {
                if ((type = pair.Item2.GetType(typeName, false))!=null) {
                    return type;
                }
            }
            return null;
        }
        public void CreateGUI() {
            AssemblyDictionary??=GetDictionary(AppDomain.CurrentDomain.GetAssemblies());
           
            var  absPath= path.AssetsToAbsolutePath();
            if (_assemblyNames == null) {
                var jsonpath = absPath + "/Modules/assembly_data.json";
                if(File.Exists(jsonpath))
                    _assemblyNames = JsonUtility.FromJson<Variable<List<string>>>(File.ReadAllText(jsonpath)).value;
                else 
                    _assemblyNames = new List<string>(){"mscorlib",
                        "UnityEngine.CoreModule"};
                foreach (var assemblyName in _assemblyNames) {
                    _assemblies.Add(string.IsNullOrEmpty(assemblyName)
                        ? default
                        : (assemblyName,AssemblyDictionary[assemblyName]));
                }
            }
            if (_typeDatas == null) {
                var jsonpath = absPath + "/Modules/type_data.json";
                if(File.Exists(jsonpath))
                    _typeDatas = JsonUtility.FromJson<Variable<List<NameAndCodeGenTypePair>>>(File.ReadAllText(jsonpath)).value;
                else 
                    _typeDatas = new List<NameAndCodeGenTypePair>() {};
                
                foreach (var typeName in _typeDatas) {
                    if (string.IsNullOrEmpty(typeName.Name)) continue;
                    var type = GetType(typeName.Name);
                    if(!_selectedTypesSet.Contains(type)) {
                        _selectedTypesSet.Add(type);
                        _types.Add((typeName.Name, typeName.CodeGenerationType, GetType(typeName.Name)));
                    }
                }
                foreach (var type in TypeCache.GetTypesWithAttribute(typeof(Attributes.CodeGenAttribute))) {
                    if (type.IsGenericTypeDefinition||_selectedTypesSet.Contains(type)) {
                        continue;
                    }
                    var attribute = type.GetCustomAttribute<Attributes.CodeGenAttribute>();
                    _types.Add((type.Name, attribute.CodeGenerationType, type));
                    
                }
                foreach (var field in TypeCache.GetFieldsWithAttribute(typeof(Attributes.CodeGenAttribute))) {
                    if (field.IsStatic) {
                        var value = field.GetValue(null);
                        if (value is null) {
                            Debug.LogError(field.Name+"is null");
                            continue;
                        }
                        if (value is Type ) {
                            var type = (Type) value;
                            if (_selectedTypesSet.Contains(type)) continue;
                            var attribute = field.GetCustomAttribute<Attributes.CodeGenAttribute>();
                            _types.Add((type.Name, attribute.CodeGenerationType, type));
                            
                        }else if (value is List<Type> list) {
                            var attribute = field.GetCustomAttribute<Attributes.CodeGenAttribute>();
                            foreach (var type in list) {
                                if (_selectedTypesSet.Contains(type)) continue;
                                _types.Add((type.Name, attribute.CodeGenerationType, type));
                            }
                        }else if (value is List<(CodeGenerationType, Type)> detailedList) {
                            foreach (var element in detailedList) {
                                var type = element.Item2;
                                if (_selectedTypesSet.Contains(element.Item2)) continue;
                                _types.Add((type.Name, element.Item1, type));
                            }
                        }
                        else {
                            Debug.LogError(field.Name+"is not supported");
                        }
                    }
                }
            }
            
            _splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

            rootVisualElement.Add(_splitView);
            if (AssembliesView == null) {
                 AssembliesView =new ListView() {
                    reorderMode =ListViewReorderMode.Animated
                };
                 AssembliesView.makeItem = () => new ListElementButton(ShowAssemblyDropDown , (i) => {
                    _assemblies.RemoveAt(i);
                    AssembliesView.Rebuild();
                });
                 AssembliesView.bindItem = (item, index) => {
                    var element = (ListElementButton) item;
                    element.Button.text = _assemblies[index].name; 
                    element.Index=index; 
                };
                 AssembliesView.itemsSource = _assemblies;
            }

            if (TypesView == null) {
                var _typesView =new ListView() {
                    reorderMode =ListViewReorderMode.Animated
                };
                TypesView = _typesView;
                _typesView.makeItem = () => new ListElementButtonWithDropDown(this);
                _typesView.bindItem = (item, index) => {
                    var element = (ListElementButtonWithDropDown) item;
                    element.Button.text = _types[index].name; 
                    element.SelectionButton.text = _types[index].codeGenerationType.ToString(); 
                    element.Index=index; 
                };
                _typesView.itemsSource = _types;
            }
            
            var leftPane = new ListView();
            _splitView.Add(leftPane);
            _rightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            _splitView.Add(_rightPane);
            leftPane.makeItem = () => new Label();
            leftPane.bindItem = (item, index) => {
                var element = (Label) item;
                element.text = List[index]; 
            };
            leftPane.itemsSource = List;
            leftPane.onSelectionChange += OnLeftSelectionChange;
   
            leftPane.selectedIndex = _selectedIndex;

            leftPane.onSelectionChange += _ => { _selectedIndex = leftPane.selectedIndex; };
        }

        void OnLeftSelectionChange(IEnumerable<object> selectedItems) {
            TypesView.selectedIndex = -1;
            // Clear all previous content from the pane
            _rightPane.Clear();

            // Get the selected sprite
            var selectedText = selectedItems.First() as string;
            if (selectedText == null)
                return;
            if (selectedText == "ModuleTypes") {
                _rightPane.Add(TypesView);
                _rightPane.Add(new Button(() => {
                    ShowTypeDropDown(_types.Count);
                    _types.Add(("select",default,default));
                    TypesView.Rebuild();
                }){text = "+"});
                _rightPane.Add(new Button(() => {
                    WrapperGenerator.GenerateScript(path,_types);
                    _typeDatas.Clear();
                    _typeDatas.AddRange(_types.Select(tuple=>new NameAndCodeGenTypePair(tuple.name,tuple.codeGenerationType)));
                    _assemblyNames.Clear();
                    _assemblyNames.AddRange(_assemblies.Select(tuple=>tuple.name));
                    var  absPath= path.AssetsToAbsolutePath();
                    File.WriteAllText(absPath+"/Modules/type_data.json", JsonUtility.ToJson(new Variable<List<NameAndCodeGenTypePair>>(_typeDatas),true));
                    File.WriteAllText(absPath+"/Modules/assembly_data.json", JsonUtility.ToJson(new Variable<List<string>>(_assemblyNames),true));
                    AssetDatabase.Refresh();
                    TypesView.Rebuild();
                }){text = "Generate"});
                return;
            }
            if (selectedText == "Assemblies") {
                _rightPane.Add(AssembliesView);
                _rightPane.Add(new Button(() => {
                    ShowAssemblyDropDown(_assemblies.Count);
                    _assemblies.Add(("select",null));
                    AssembliesView.Rebuild();
                }){text = "+"});
                return;
            }
            var selectedLabel = new Label(selectedText);
            _rightPane.Add(selectedLabel);
        }
        
        void ShowAssemblyDropDown(int index) {
            ADropDown ??= new TreeDropDown<Assembly>(new AdvancedDropdownState(),
                new NameSplitTree<Assembly>(AssemblyDictionary)) {
                NodeIcon = Resources.Load("namespaceIcon") as Texture2D
            };
            ADropDown.onItemSelected = null;
            ADropDown.onItemSelected = (assemblyName,assembly) => {
                _assemblies[index] = (assemblyName, assembly);
                TypeDropDown?.Clear();
                AssembliesView.Rebuild();
            };
            var element = AssembliesView.ElementAt(index);
            if (element != null) ADropDown.Show(element.worldBound);
            else ADropDown.Show(AssembliesView.worldBound);
        }
        void ShowTypeDropDown(int index) {
            if (TypeDropDown == null) {
                typeTree = new NameSplitTree<Type>(GetNonGenericTypePairs(_assemblies));
                TypeDropDown = new TreeDropDown<Type>(new AdvancedDropdownState(),
                    typeTree) {
                    NodeIcon = (Resources.Load("namespaceIcon") as Texture2D),
                };
            }
            else if(!TypeDropDown.HasBuilt) {
                if (typeTree == null) typeTree = new NameSplitTree<Type>(GetNonGenericTypePairs(_assemblies));
                else {
                    typeTree.Clear();
                    typeTree.Add(GetNonGenericTypePairs(_assemblies));
                }
                TypeDropDown.SetTree( typeTree);
            }
            TypeDropDown.onItemSelected = null;
            TypeDropDown.onItemSelected = (typeName,type) => {
                _types[index] = (typeName,0,type) ;
                TypeDropDown.Clear();
                TypesView.Rebuild();
            };
            TypeDropDown.Show(TypesView.worldBound);
        }void ShowTypeDropDown(ListElementButtonWithDropDown dropDown) {
            if (TypeDropDown == null) {
                typeTree = new NameSplitTree<Type>(GetNonGenericTypePairs(_assemblies));
                TypeDropDown = new TreeDropDown<Type>(new AdvancedDropdownState(),
                    typeTree) {
                    NodeIcon = (Resources.Load("namespaceIcon") as Texture2D),
                };
            }
            else if(!TypeDropDown.HasBuilt) {
                if (typeTree == null) typeTree = new NameSplitTree<Type>(GetNonGenericTypePairs(_assemblies));
                else {
                    typeTree.Clear();
                    typeTree.Add(GetNonGenericTypePairs(_assemblies));
                }
                TypeDropDown.SetTree( typeTree);
            }
            TypeDropDown.onItemSelected = null;
            TypeDropDown.onItemSelected = (typeName,type) => {
                _types[dropDown.Index] = (typeName,0,type) ;
                TypeDropDown.Clear();
                TypesView.Rebuild();
            };
            TypeDropDown.Show(dropDown.worldBound);
        }
        public class ListElementButtonWithDropDown : VisualElement {
            public Button Button;
            public int Index;
            public Button SelectionButton;

            public ListElementButtonWithDropDown(WrapperGenerationWindow target) {
                style.maxHeight = 22;
                style.marginBottom=1;
                style.marginTop=1;
                style.flexDirection = FlexDirection.Row;
                Button = new Button(() => target.ShowTypeDropDown(this)) {
                    style = {
                        flexGrow = 1,  
                        unityTextAlign = TextAnchor.LowerLeft
                    }
                };
                Add(Button);

                SelectionButton = new Button(() => {
                    switch (_types[Index].codeGenerationType) {
                        case CodeGenerationType.All: {
                            var typeData = _types[Index];
                            typeData.codeGenerationType = CodeGenerationType.WrapperOnly;
                            _types[Index] = typeData;
                            SelectionButton.text = "GenerateWrapper";
                            break;
                        }
                        case CodeGenerationType.InspectorOnly: {
                            var typeData = _types[Index];
                            typeData.codeGenerationType = CodeGenerationType.All;
                            _types[Index] = typeData;
                            SelectionButton.text = "All";
                            break;
                        } case CodeGenerationType.WrapperOnly: {
                            var typeData = _types[Index];
                            typeData.codeGenerationType = CodeGenerationType.InspectorOnly;
                            _types[Index] = typeData;
                            SelectionButton.text = "Inspector";
                            break;
                        }
                    }
                }) {
                    tooltip = " Toggle Generation type with click ",
                    text = "All",
                    style = {  maxWidth = 120,
                        minWidth = 120 }
                };
                Add(SelectionButton);

                var RemoveButton = new Button(() => {
                    _types.RemoveAt(Index);
                    TypesView.Rebuild();
                }) {
                    tooltip = " Remove this element ",
                    text = "-",
                    style = {
                        flexGrow = 1,  
                    }
                };
                Add(RemoveButton);
                RemoveButton.style.flexGrow = 1;
                RemoveButton.style.maxWidth = 20;
                RemoveButton.style.minWidth = 20;
                RemoveButton.style.maxHeight = 20;
                RemoveButton.style.minHeight = 20;
            }
        }
    }
    public class ListElementButton : VisualElement {
        public Button Button;
        public int Index;
        
        public ListElementButton(Action<int> selectAction  ,Action<int> removeAction) {
            style.maxHeight = 22;
            style.marginBottom=1;
            style.marginTop=1;
            style.flexDirection = FlexDirection.Row;
             Button = new Button(() => selectAction.Invoke(Index)) {
                 style = {
                    flexGrow = 1,  
                    unityTextAlign = TextAnchor.LowerLeft
                }
            };
            Add(Button);
            var RemoveButton = new Button(() => removeAction.Invoke(Index)) {
                text = "-",
                style = {
                    flexGrow = 1,  
                }
            };
            Add(RemoveButton);
            RemoveButton.style.flexGrow = 1;
            RemoveButton.style.maxWidth = 20;
            RemoveButton.style.minWidth = 20;
            RemoveButton.style.maxHeight = 20;
            RemoveButton.style.minHeight = 20;
        }
    }
   
    
}
#endif