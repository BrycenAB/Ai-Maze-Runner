using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnToggle : MonoBehaviour
{
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
            manager.NewTest = true;
        }
        if (toggle.GetComponent<Toggle>().isOn == false)
        {
            manager.NewTest = false;
        }

    }
}
