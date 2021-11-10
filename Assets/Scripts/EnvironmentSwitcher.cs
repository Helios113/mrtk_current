using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject room;
    public bool active = false;
    public void SwitchEnvironment()
    {
        room.SetActive(active);
        active = !active;
    }
}
