using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ObjectRenderer))]
public class EditorSettings : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectRenderer myScript = (ObjectRenderer)target;
        if (GUILayout.Button("Play"))
        {
            myScript.Render();
        }
    }

}
