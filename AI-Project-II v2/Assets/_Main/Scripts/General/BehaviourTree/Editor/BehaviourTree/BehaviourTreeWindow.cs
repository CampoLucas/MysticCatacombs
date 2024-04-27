using BehaviourTreeAsset.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviourTreeAsset.EditorUI
{
    public class BehaviourTreeWindow : EditorWindow
    {
        public static readonly string UxmlPath = "BehaviourTreeUITest";
        public static readonly string StyleSheetPath = "BehaviourTreeStyle";

        private VisualElement _root;
        private BehaviourTreeView _treeView;
        private TabView _tabView;

        private InspectorElement _inspectorElement;

        [MenuItem("Window/BehaviourTree/Editor")]
        public static void OpenWindow()
        {
            var wnd = GetWindow<BehaviourTreeWindow>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        }

        /// <summary>
        /// Opens the behaviour tree window when double clicking the behaviour tree scriptable object.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                OpenWindow();
                return true;
            }

            return false;
        }

        public void CreateGUI()
        {
            _root = rootVisualElement;
            
            SetUxml();
            SetStyle();
            SetTreeElements();
        }

        private void OnSelectionChange()
        {
            Debug.Log("OnSelectionChange");
            var tree = Selection.activeObject as BehaviourTree;
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                Debug.Log("Populate view");
                _treeView.PopulateView(tree);
                tree.OnPopulateTree();
                //GetInspectorView(out var inspector);
                _inspectorElement.ClearSelection();
            }
        }

        private void OnNodeSelectionChanged(NodeView node)
        {
            //GetInspectorView(out var inspector);
            _inspectorElement.UpdateSelection(node);
        }

        private void SetUxml()
        {
            var visualTree = Resources.Load(UxmlPath) as VisualTreeAsset;
            if (visualTree != null) visualTree.CloneTree(_root);
            else Debug.LogError("[BehaviourTreeWindow] Error while trying to instantiate the window: VisualTree is null.");
        }

        private void SetStyle()
        {
            var styleSheet = Resources.Load(StyleSheetPath) as StyleSheet;
            if (styleSheet != null) _root.styleSheets.Add(styleSheet);
            else Debug.LogError("[BehaviourTreeWindow] Error while trying to set the style: StyleSheet is null.");
        }
        
        private void SetTreeElements()
        {
            _treeView = _root.Q<BehaviourTreeView>();
            _tabView = _root.Q<TabView>();

            _inspectorElement = new InspectorElement();
            _tabView.AddTab("Inspector", _inspectorElement);
            _tabView.AddTab("Parameters", new Blackboard());
            _tabView.AddTab("Description", new VisualElement());

            _treeView.OnNodeSelected = OnNodeSelectionChanged;
            OnSelectionChange();
        }
    }
}