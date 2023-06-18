
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int generations;
    public bool newTest = false;
    public bool startPressed = false;
    public bool testRunning = false;
    public bool gamePaused = false;

    public TMP_Text deadBots_txt;
    public TMP_Text FurthPos_txt;
    public TMP_Text BestPosition_txt;
    public TMP_Text Generation_txt;

    public TMP_InputField TimeFrame;
    public TMP_InputField Population_input;
    public TMP_InputField Mutation_Chance_input;
    public TMP_InputField Mutation_Strength_input;
    public TMP_InputField Game_Speed_input;

    public int deadBots = 0;
    public int furthestPosition = 0;
    public int overallFurthestPos = 0;

    public float timeframe;
    public int populationSize;
    public GameObject prefab;

    public int[] layers = new int[3] { 5, 3, 2 };//initializing network to the right size

    [Range(0.0001f, 1f)] public float MutationChance = 0.35f;

    [Range(0f, 1f)] public float MutationStrength = 0.1f;

    [Range(0.01f, 10f)] public float Gamespeed = 2f;

    //public List<Bot> Bots;
    public List<NeuralNetwork> networks;
    private List<Bot> cars;

    private string GenPath = "Assets/StreamingAssets/GenerationSave.txt";
    private string SavePath = "Assets/StreamingAssets/Save.txt";


    private void Update()
    {
        if (startPressed == true)
        {
            if (populationSize % 2 != 0)
                populationSize = 100;
            
            testRunning = true;
            
            SaveGeneration();//if a new test is occuring wipe the file to store generation #, if not get last generation#
            InitNetworks();
            InvokeRepeating("CreateBots", 0.1f, timeframe);//repeating function
            
            startPressed = false;
            
        }
        if (!testRunning)
        {
            updateTestInfo();
        }

        update_ui_text();
    }
    /// <summary>
    ///  Write the last generation number to a file for next test
    /// </summary>

    private void OnApplicationQuit() 
    {
        File.Create(GenPath).Close();
        StreamWriter writer = new StreamWriter(GenPath);
        writer.WriteLine(generations.ToString());
        writer.Close();
    }

    public void InitNetworks()
    {
        networks = new List<NeuralNetwork>();
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Load(SavePath);//on start load the network save
            networks.Add(net);
        }
    }

    public void CreateBots()
    {
        Time.timeScale = Gamespeed;//sets gamespeed, which will increase to speed up training
        DestroyCars();//destroys bots from last generation
        cars = new List<Bot>();
        for (int i = 0; i < populationSize; i++)
        {
            //create bots ( if setting up from scratch find best starting position/rotation for the bots)
            Bot car = (Instantiate(prefab, new Vector3(27.18f, 129.71f, 23.67f), new Quaternion(0, 270.061f, 0, 0))).GetComponent<Bot>();
            car.network = networks[i];//deploys network to each learner
            cars.Add(car);
        }
        generations++;
        testRunning = true;
    }

    public void DestroyCars()
    {   // Check if Bot list is empty
        if ( cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i] != null)
                    GameObject.Destroy(cars[i].gameObject);// If there are Prefabs in the scene this will get rid of them
            }

            SortNetworks();// This sorts networks and mutates them
            furthestPosition = 0;
            deadBots = 0;
        }
    }

    public void SortNetworks()
    {
        for (int i = 0; i < populationSize; i++)
        {
            cars[i].UpdateFitness();// Gets bots to set their corrosponding networks fitness
        }
        networks.Sort();
        networks[populationSize - 1].Save(SavePath);// Saves networks weights and biases to file, to preserve network performance
        for (int i = 0; i < populationSize / 2; i++)
        {
            networks[i] = networks[i + populationSize / 2].copy(new NeuralNetwork(layers));
            networks[i].Mutate((int)(1 / MutationChance), MutationStrength);
        }

    }
    
    /// <summary>
    /// Handles saving the generation as well as clearing the saved NN if newTest
    /// If the GenSave file is not empty Read it to the generation variable
    /// </summary>
    public void SaveGeneration() 
    {
        if (newTest)
        {
            File.Create(GenPath).Close();
            File.WriteAllText(GenPath, "");
            File.Create(SavePath).Close();
            File.WriteAllText(SavePath, "");


            newTest = false;
            generations = 0;
            overallFurthestPos = 0;
            furthestPosition = 0;
            deadBots = 0;
        }
        else if (File.ReadAllText(GenPath) != "")
        {
            generations = int.Parse(File.ReadAllText(GenPath));
        }
    }

    /// <summary>
    /// Update info used in NN
    /// </summary>
    private void updateTestInfo() 
    {

        if (string.IsNullOrEmpty(Mutation_Chance_input.text))
        {
            MutationChance = 0.1f; // Default value when the input is empty
        }
        else
        {
            if (Mutation_Chance_input.text == ".")
            {
                return;
            }
            if (Mutation_Chance_input.text.StartsWith(".") && float.TryParse("0" + Mutation_Chance_input.text, out float parsedValue))
            {
                MutationChance = parsedValue;
            }
            else if (float.TryParse(Mutation_Chance_input.text, out parsedValue))
            {
                MutationChance = parsedValue;
            }
            else
            {
                // Handle the case where the input string is not a valid floating-point number
                Debug.LogError("Invalid mutation chance input");
                return;
            }
        }

        if (string.IsNullOrEmpty(Mutation_Strength_input.text))
        {
            MutationStrength = 0.1f; // Default value when the input is empty
        }
        else
        {
            if (Mutation_Strength_input.text == ".")
            {
                return;
            }
                if (Mutation_Strength_input.text.StartsWith(".") && float.TryParse("0" + Mutation_Strength_input.text, out float parsedValue))
                {
                    MutationStrength = parsedValue;
                }
                else if (float.TryParse(Mutation_Strength_input.text, out parsedValue))
                {
                    MutationStrength = parsedValue;
                }
                else
                {
                    // Handle the case where the input string is not a valid floating-point number
                    Debug.LogError("Invalid mutation strength input");
                    return;
                }
        }
        timeframe = TimeFrame.text == "" ? 1.0f : float.Parse(TimeFrame.text);
        Gamespeed = Game_Speed_input.text == "" ? 2.0f : float.Parse(Game_Speed_input.text);
        populationSize = Population_input.text == "" ? 100 : int.Parse(Population_input.text);
    }

    private void update_ui_text()
    {
        deadBots_txt.text = deadBots.ToString();
        FurthPos_txt.text = furthestPosition.ToString();
        BestPosition_txt.text = overallFurthestPos.ToString();
        Generation_txt.text = generations.ToString();
    }

    /// <summary>
    /// Stop the current generation of bots and saves the generation number
    /// </summary>
    public void StopNextGen() 
    {
        CancelInvoke("CreateBots");
        DestroyCars();
        File.WriteAllText(GenPath, generations.ToString());
        testRunning = false;
    }
}
