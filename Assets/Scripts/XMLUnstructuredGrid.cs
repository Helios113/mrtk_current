using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument
using System.Linq;
using System;

public class XMLUnstructuredGrid : MonoBehaviour
{
    public static DataObject Parse(XDocument doc)
    {
        //doc = XDocument.Load("Assets/abc.vtu");
        List<int> cellsDataConnectivity;
        List<int> cellsDataOffsets;
        List<int> cellsDataTypes;
        List<int> triangleData;

        List<float> vertexDataRaw;
        List<Vector3> vertexData;

        var pointsData = doc.Descendants("Points").Elements("DataArray").Single();

        var cellDataConn = doc.Descendants("Cells").Elements("DataArray").Where(x => (string) x.Attribute("Name") == "connectivity")
                            .Single();

        var cellDataOff = doc.Descendants("Cells").Elements("DataArray").Where(x => (string) x.Attribute("Name") == "offsets")
                            .Single();

        var cellDataTypes = doc.Descendants("Cells").Elements("DataArray").Where(x => (string) x.Attribute("Name") == "types")
                            .Single();

        var scalarData = from A in doc.Descendants("PointData").Elements("DataArray")
                    where A.Attribute("NumberOfComponents") == null
                    select A;
        var vectorData = from A in doc.Descendants("PointData").Elements("DataArray")
                    where A.Attribute("NumberOfComponents") != null
                    select A;

        cellsDataConnectivity = new string(cellDataConn.Value.ToCharArray())
                                .Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => int.Parse(p)).ToList();

        cellsDataOffsets = new string(cellDataOff.Value.ToCharArray())
                                .Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => int.Parse(p)).ToList();
        
        cellsDataTypes = new string(cellDataTypes.Value.ToCharArray())
                                .Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => int.Parse(p)).ToList();

        triangleData = CellToTris.GetTrianglesFromData(cellsDataConnectivity,cellsDataOffsets,cellsDataTypes);
        
        vertexDataRaw = new string(pointsData.Value.ToCharArray())
                                .Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => float.Parse(p)).ToList();
        vertexData = vertexDataConvert(vertexDataRaw);

        List<float> DataRaw;
        Dictionary<string, List<float>> scalarDict = new Dictionary<string, List<float>>();

        foreach(var i in scalarData)
        {
            DataRaw = new string(i.Value.ToCharArray())
                                .Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => float.Parse(p)).ToList();
            scalarDict.Add(((string)i.Attributes("Name").Single()), DataRaw);
        }
        List<float> DataRaw1;
        Dictionary<string, List<Vector3>> vectorDict = new Dictionary<string, List<Vector3>>();
        foreach(var i in vectorData)
        {
            DataRaw1 = new string(i.Value.ToCharArray())
                                .Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => float.Parse(p)).ToList();
            vectorDict.Add(((string)i.Attributes("Name").Single()), vertexDataConvert(DataRaw1));
        }
        return new DataObject(triangleData, vertexData, scalarDict, vectorDict);
    }

    static List<Vector3> vertexDataConvert(List<float> A)
    {
        List<Vector3> res = new List<Vector3>();
        for (int i = 0; i < A.Count/3;i++)
           res.Add(new Vector3(A[i*3],A[i*3+2], A[i*3+1]));
        return res;
    }
}   