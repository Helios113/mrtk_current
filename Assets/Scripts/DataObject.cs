using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObject
{
    public Dictionary<string, List<float>> scalarDict;
    public Dictionary<string, List<Vector3>> vectorDict;
    public Vector3[] vertexData;
    public int[] triangleData;
    public int vertexSize = 0;

    public DataObject(List<int> triangleData,
                      List<Vector3> vertexData, 
                      Dictionary<string, List<float>> scalarDict,
                      Dictionary<string, List<Vector3>> vectorDict)
    {
        this.triangleData = triangleData.ToArray();
        this.vertexData = vertexData.ToArray();
        this.vertexSize = vertexData.Count;
        this.scalarDict = scalarDict;
        this.vectorDict = vectorDict;
    }
}
