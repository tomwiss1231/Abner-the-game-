using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Behaviour;
using UnityEditor;
using UnityEngine;

public class GridWindow : EditorWindow
{
    Grid grid;
    public void Init()
    {
        grid = (Grid)FindObjectOfType(typeof(Grid));
 
    }

    void OnGUI()
    {
        grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200));
    }

}

