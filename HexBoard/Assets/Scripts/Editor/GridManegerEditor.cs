using System;
using Assets.Scripts.Behaviour;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(GridManager))]
    class GridManegerEditor : UnityEditor.Editor
    {
        private GridManager grid;

        public void OnEnable()
        {
            grid = (GridManager)target;
            SceneView.onSceneGUIDelegate += UpdataGrid;

        }

        private void UpdataGrid(SceneView sceneview)
        {
            Event e = Event.current;
            if (e.isKey && e.character == 'b')
                try
                {
                    foreach (var hex in Selection.gameObjects)
                    {
                        //hex.GetComponent<TileBehaviour>().ChangePassebleStatus();
                        hex.transform.parent.GetComponent<TileBehaviour>().ChangePassebleStatus();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
        }

        public void OnDisable()
        {

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create Grid", GUILayout.Width(255)))
            {
                grid.init();
            }
        }
    }
}

