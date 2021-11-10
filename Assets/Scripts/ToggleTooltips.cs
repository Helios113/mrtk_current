using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTooltips : MonoBehaviour
{
    public bool toggle = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Toggle()
    {
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("tooltip");
 
        foreach(GameObject go in gameObjectArray)
        {
            print(go.name);
            go.SetActive (toggle);
        }
        toggle = !toggle;

    }
}
