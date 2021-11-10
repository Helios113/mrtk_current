using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TestSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    MeshFilter myMesh;
    MeshRenderer myMat;

    public Material overrideMaterial;
    public List<Mesh> obj;
    public List<Material> mat;
    public float FrameTimeMilliseconds = 400;
    public bool play = true;
    public int boxColliderRefresh = 1;
    public bool isPrim = true;

    void Start()
    {
        myMesh = GetComponent<MeshFilter>();
        myMat = GetComponent<MeshRenderer>();
        foreach (Transform child in transform)
        {
            obj.Add(child.GetComponentInChildren<MeshFilter>().mesh);
            mat.Add(child.GetComponentInChildren<MeshRenderer>().material);
            child.gameObject.SetActive(false);
        }
        if(overrideMaterial)
            myMat.material = overrideMaterial;
        StartCoroutine(Play());
    }
    IEnumerator Play()
    {
        //gameObject.GetComponentInParent<BoxCollider>().size = myMesh.mesh.bounds.size * 1.1f;
        //gameObject.GetComponentInParent<BoxCollider>().center = myMesh.mesh.bounds.center;
        for (int i = 0; i < obj.Count;i++)
        {
            myMesh.mesh = obj[i];
            if(overrideMaterial == null)
                myMat.material = mat[i];
            if (i%boxColliderRefresh== 0 && isPrim)
            {
                gameObject.GetComponentInParent<BoxCollider>().size = myMesh.mesh.bounds.size * 1.1f;
                gameObject.GetComponentInParent<BoxCollider>().center = myMesh.mesh.bounds.center;
            }
            yield return new WaitForSeconds(FrameTimeMilliseconds/1000);
            if (!play)
                yield return new WaitUntil(new System.Func<bool>(() => play));
        }
        StartCoroutine(Play());
            
    }
    public void PlayButton()
    {
        play = true;
    }
    public void PauseButton()
    {
        play = false;
    }
}