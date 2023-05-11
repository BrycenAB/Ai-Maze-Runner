using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Toggle_Handler : MonoBehaviour
{
    public Manager manager;
    public FPplayerControler player;
    public GameObject playerObject;
    
    //Make sure to attach these Buttons in the Inspector
    public Button m_StartButton, m_ResetButton, m_CloseHints,
        m_floor_tele, m_floor_tele2, m_floor_tele3, m_hide_settings;
    private bool ViewingUi = false;
    private bool ViewingSettings = true;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        m_StartButton.onClick.AddListener(TaskStartOnClick);
        m_ResetButton.onClick.AddListener(TaskResetOnClick);
        m_CloseHints.onClick.AddListener(TaskCloseHintsClicked);
        m_floor_tele.onClick.AddListener(TaskFloorTeleClicked);
        m_floor_tele2.onClick.AddListener(TaskFloorTeleClicked2);
        m_floor_tele3.onClick.AddListener(TaskFloorTeleClicked3);
        m_hide_settings.onClick.AddListener(TaskHideSettingsClicked);
        //m_PauseButton.onClick.AddListener(TaskPauseClicked);
        //m_YourThirdButton.onClick.AddListener(() => ButtonClicked(42));
        //m_YourThirdButton.onClick.AddListener(TaskOnClick);
    }
    private void Update()
    {
        unlockSettings();
    }

    void unlockSettings()
    {
        if (Input.GetKeyDown(KeyCode.I) && !ViewingUi)
        {
            player.LockCursor = false;
            player.UpdateCamera = false;
            ViewingUi = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && ViewingUi)
        {
            player.LockCursor = true;
            player.UpdateCamera = true;
            ViewingUi = false;
        }

    }

    void TaskHideSettingsClicked()
    {
        // hide settingsPannel object
        Canvas settingsPannel = GameObject.Find("Settings_Canvas").GetComponent<Canvas>();

        if (ViewingSettings)
        {
            ViewingSettings = false;
            settingsPannel.enabled = false;
            
        }
        else
        {
            ViewingSettings = true;
            settingsPannel.enabled = true;
        }
        
    }
    
   
    /*void TaskPauseClicked()
    {
        if (!manager.gamePaused)
            manager.Gamespeed = 0.0f;
        if (manager.gamePaused)
            manager.Gamespeed = manager.Game_Speed_input.text == "" ? 2.0f : float.Parse(manager.Game_Speed_input.text);
    }*/


    void TaskFloorTeleClicked()
    {
        Vector3 pos = new Vector3(18.5f, 154.17f, 19.0f);

        player.teleport(pos);
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

    }
    void TaskFloorTeleClicked2()
    {
        Vector3 pos = new Vector3(18.5f, 176.96f, 19.0f);

        player.teleport(pos);
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

    }
    void TaskFloorTeleClicked3()
    {
        Vector3 pos = new Vector3(18.5f, 218f, 19.0f);
        player.teleport(pos);
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

    }

    private void TaskCloseHintsClicked()
    {
        GameObject hintpannel = GameObject.Find("Hints");
        m_CloseHints.enabled = false;
        hintpannel.SetActive(false);
    }

    void TaskStartOnClick()
    {
        manager.startPressed = true;
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
    void TaskResetOnClick()
    {
        if (OnToggle.FindObjectOfType<Toggle>().isOn)
        {
            manager.OverallFurthestPos = 0;
            OnToggle.FindObjectOfType<Toggle>().isOn = false;
            manager.SaveGeneration();
        }
        /*manager.SaveGeneration();
        manager.CreateBots();*/
        try
        {
            if(manager.BotsInMaze == true)
            {
                manager.SaveGeneration();
                manager.CreateBots();
            }
            
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.Message);
        }

        


    }


}