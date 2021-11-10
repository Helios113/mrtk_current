using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DataConverter))]
public class EditorConvert : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DataConverter myScript = (DataConverter)target;
        if (GUILayout.Button("Convert"))
        {
            myScript.make();
        }
    }

}
