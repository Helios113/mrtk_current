using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;
using System.Xml.Linq; 
using System;
[RequireComponent(typeof(ObjectRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class DataHandler : MonoBehaviour
{
    public TextAsset configFile;    
    void Start(){
        string[] frames = configFile.text.Split(',');
        DataObject[] dObj = new DataObject[frames.Length];
        for (int i = 0; i < frames.Length;i++)
        {
            dObj[i] = XMLUnstructuredGrid.Parse(XDocument.Parse(
                Resources.Load<TextAsset>(frames[i]).text
                ));
        }

        this.gameObject.GetComponent<ObjectRenderer>().SetFrames(dObj);

    }
}
