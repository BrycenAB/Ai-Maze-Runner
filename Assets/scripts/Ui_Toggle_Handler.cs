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
    
    /// <summary>
    /// Ui Buttons and controls
    /// </summary>
    public Button m_StartButton, m_ResetButton, m_CloseHints,
        m_floor_tele, m_floor_tele2, m_floor_tele3, m_hide_settings, m_stop_next_gen;
    private bool ViewingUi = false;
    private bool ViewingSettings = true;

    void Start()
    {
        //Calls method when you click coresponding button in ui
        m_StartButton.onClick.AddListener(TaskStartOnClick);
        m_ResetButton.onClick.AddListener(TaskResetOnClick);
        m_CloseHints.onClick.AddListener(TaskCloseHintsClicked);
        m_floor_tele.onClick.AddListener(TaskFloorTeleClicked);
        m_floor_tele2.onClick.AddListener(TaskFloorTeleClicked2);
        m_floor_tele3.onClick.AddListener(TaskFloorTeleClicked3);
        m_hide_settings.onClick.AddListener(TaskHideSettingsClicked);
        m_stop_next_gen.onClick.AddListener(TaskStopNextGenClicked);
    }
    private void Update()
    {
        unlockSettings();
    }

    /// <summary>
    /// Unlock cursor to use mouse
    /// </summary>
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
    

    void TaskStopNextGenClicked()
    {
        manager.StopNextGen();
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }


    /// <summary>
    ///  Teleportation to different floors
    /// </summary>
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


    /// <summary>
    /// Close Hint Window
    /// </summary>
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

    /// <summary>
    /// Checks if New Test toggle is selected, if not saves generation #
    /// </summary>
    void TaskResetOnClick()
    {
        manager.testRunning = false;
        if (OnToggle.FindObjectOfType<Toggle>().isOn)
        {
            manager.overallFurthestPos = 0;
            OnToggle.FindObjectOfType<Toggle>().isOn = false;
            manager.SaveGeneration();
            string path = "Assets/StreamingAssets/Save.txt";
            System.IO.File.WriteAllText(path, string.Empty);
        }
        try
        {
            if(manager.testRunning == true)
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