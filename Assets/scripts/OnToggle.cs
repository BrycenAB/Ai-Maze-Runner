using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnToggle : MonoBehaviour
{
    /// <summary>
    /// Script to control "WipeTest" Toggle and update manager.newTest
    /// </summary>

    public bool Toggled = false;
    public GameObject toggle;
    public Manager manager;

    void Start()
    {
        toggle = GameObject.Find("ResetGeneticsToggle");
    }

    
    void Update()
    {
        if (toggle.GetComponent<Toggle>().isOn == true)
        {
            manager.newTest = true;
        }
        if (toggle.GetComponent<Toggle>().isOn == false)
        {
            manager.newTest = false;
        }

    }
}
