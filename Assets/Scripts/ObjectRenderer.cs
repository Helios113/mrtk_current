using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ObjectRenderer : MonoBehaviour
{
    Mesh mesh;
    [Header("Graphics")]
    public Gradient grad;
    public float majorAxisScale = 0.3f;
       
    [Header("Play Settings")]
    public bool play = true;
    public bool isPrimaryObject = false;

    public GameObject[] siblingObjects;
    public GameObject node;
    public int frameTimeMilliSeconds = 400;

    public DataObject[] frames;
    public string[] vectors;
    public string[] scalars = {};

    [Header("Wrapping and Coloring")]
    public float wrapFactor = 1; 
    public float maxColorBar = 1;
    public float minColorBar = 0;
    float deltaColorBar = 0;
    public int currentScalar = 0;
    public int currentVector = 0;
    private bool oneShot = true;
    private Vector3 colliderSize;
    public int cI = 0;
    int frameCount;
    bool isLoaded = false;

    public void SetFrames(DataObject[] frames)
    {
        this.frames = frames;
        frameCount = frames.Length;
        deltaColorBar = maxColorBar - minColorBar;
        List<string> A = new List<string>();
        A.Add("None");
        A.AddRange(frames[0].vectorDict.Keys);
        vectors = A.ToArray();
        A = new List<string>();
        A.Add("None");
        A.AddRange(frames[0].scalarDict.Keys);
        scalars = A.ToArray();
        CreateMesh();
        isLoaded = true;
        Render();
    }
    public void CreateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    public void Render(bool lessMem = false)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateCoroutine());
    }

    IEnumerator AnimateCoroutine()
    {
        do
        {
            //mesh.Clear();
            mesh.vertices = Vertices(cI, vectors[currentVector]);
            
            mesh.colors = ColorMap(cI);
            mesh.RecalculateNormals();
            if (oneShot && cI == 0 && isPrimaryObject)
            {
                mesh.triangles = frames[cI].triangleData;
                
                Vector3 v3 = mesh.bounds.size;
                float size = Mathf.Max(Mathf.Max(v3.x, v3.y), v3.z);
                colliderSize = majorAxisScale * Vector3.one / size;
                node.transform.localScale = colliderSize;
                oneShot = false;
            }
            cI++;
            cI %= frameCount;
            if (isPrimaryObject)
            {
                gameObject.GetComponentInParent<BoxCollider>().size = mesh.bounds.size * 1.1f;
                gameObject.GetComponentInParent<BoxCollider>().center = mesh.bounds.center;
            }

            yield return new WaitForSeconds(frameTimeMilliSeconds / 1000.0f);
            if (!play)
                yield return new WaitUntil(new System.Func<bool>(() => play));
        }while(true);

    }
    Vector3[] Vertices(int frame, string vector)
    {
        if (vector == "None")
        {
            return frames[frame].vertexData;
        }
        Vector3[] res = new Vector3[frames[frame].vertexData.Length];
        for (int i = 0; i < frames[frame].vertexData.Length; i++)
        {
            res[i] = frames[frame].vertexData[i] + wrapFactor * frames[frame].vectorDict[vector][i];
        }
        return res;
    }

    Color[] ColorMap(int frame)
    {
        Color[] color = new Color[frames[frame].vertexSize];
        if (currentScalar == 0)
        {
            for (int j = 0; j < frames[frame].vertexSize; j++)
            {
                color[j] = grad.Evaluate(0);
            }
            return color;
        }
        int cnt = frames[frame].scalarDict[scalars[currentScalar]].Count;
        for (int j = 0; j < cnt; j++)
        {
                color[j] = grad.Evaluate((frames[frame].scalarDict[scalars[currentScalar]][j] - minColorBar) / deltaColorBar);
        }
        return color;

    }
    public void TogglePlay()
    {
        play = !play;
    }
    /*Communication
    public void SetFrameTime(int time)
    {
        frameTimeMili = time;
    }
    public void TogglePlay()
    {
        play = !play;
    }
    public void ToggleMode()
    {
        single = !single;
    }
    public void SetVector(int a)
    {
        currentVector = a;
    }*/
}
