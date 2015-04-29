using Assets.Scripts.Behaviour;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(Grid))]
    class GridEditor : UnityEditor.Editor
    {
        Grid _grid;

        public void OnEnable()
        {
            _grid = (Grid)target;
            SceneView.onSceneGUIDelegate += GridUpdate;
        }

        public void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= GridUpdate;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" Grid Width ");
            _grid.width = EditorGUILayout.FloatField(_grid.width, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(" Grid Height");
            _grid.height = EditorGUILayout.FloatField(_grid.height, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            _grid.pre = (GameObject)EditorGUILayout.ObjectField(_grid.pre, typeof(UnityEngine.Object), true);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Open Grid Window", GUILayout.Width(255)))
            {
                GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow));
                window.Init();
            }

            SceneView.RepaintAll();
        }

        void GridUpdate(SceneView sceneview)
        {
            Event e = Event.current;
            Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
            Vector3 mousePos = r.origin;
            if(e.isKey && e.character == 'i')
            {
                GameObject obj = null;
                UnityEngine.Object prefab = EditorUtility.GetPrefabParent(_grid.pre);
                Debug.Log(_grid.pre.name);
                if (prefab)
                {
                    obj = (GameObject)EditorUtility.InstantiatePrefab(prefab);
                    Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / _grid.width) * _grid.width + _grid.width / 2.0f,
                        Mathf.Floor(mousePos.y / _grid.height) * _grid.height + _grid.height / 2.0f, 0.0f);
                    obj.transform.position = aligned;
                    Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                }
            }

            else if (e.isKey && e.character == 'a')
            {
                if (Selection.activeGameObject)
                {
                    GameObject obj = null;
                    UnityEngine.Object prefab = EditorUtility.GetPrefabParent(Selection.activeGameObject);
                    if (prefab)
                    {
                        obj = (GameObject)EditorUtility.InstantiatePrefab(prefab);
                        Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / _grid.width) * _grid.width + _grid.width / 2.0f,
                            Mathf.Floor(mousePos.y / _grid.height) * _grid.height + _grid.height / 2.0f, 0.0f);
                        obj.transform.position = aligned;
                        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                    }
                }
            }
            else if (e.isKey && e.character == 'd')
            {
                foreach (GameObject gameObj in Selection.gameObjects)
                    DestroyImmediate(gameObj);
            }

        }
    }
}
