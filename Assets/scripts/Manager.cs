
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int Generations;
    public bool NewTest = false;
    public bool startPressed = false;
    public bool BotsInMaze = false;
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

    public int DeadBots = 0;
    public int FurthestPosition = 0;
    public int OverallFurthestPos = 0;

    public float timeframe;
    public int populationSize;//creates population size
    public GameObject prefab;//holds bot prefab

    public int[] layers = new int[3] { 5, 3, 2 };//initializing network to the right size

    [Range(0.0001f, 1f)] public float MutationChance = 0.01f;

    [Range(0f, 1f)] public float MutationStrength = 0.5f;

    [Range(0.01f, 10f)] public float Gamespeed = 2f;

    //public List<Bot> Bots;
    public List<NeuralNetwork> networks;
    private List<Bot> cars;


    private void Update()
    {



        if (startPressed == true)
        {
            if (populationSize % 2 != 0)
                populationSize = 100;//if population size is not even, sets it to 100

            InitNetworks();
            InvokeRepeating("CreateBots", 0.1f, timeframe);//repeating function
            SaveGeneration();//if a new test is occuring create a new file to store generation#, if not get last generation#
            startPressed = false;
        }


        if (cars != null)
        {
            BotsInMaze = true;
        }
        else
        {
            BotsInMaze = false;
        }

        if (!BotsInMaze)
        {
            updateTestInfo(); //update parameters for test if no bots are in maze

        }

        update_ui_text();
    }
    private void OnApplicationQuit() //write the last generation number to a file for next test
    {
        string path = "Assets/StreamingAssets/GenerationSave.txt";
        File.Create(path).Close();
        StreamWriter writer = new StreamWriter(path);
        writer.WriteLine(Generations);
        writer.Close();

    }

    public void InitNetworks()
    {
        networks = new List<NeuralNetwork>();
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Load("Assets/StreamingAssets/Save.txt");//on start load the network save
            networks.Add(net);
        }
    }

    public void CreateBots()
    {
        Time.timeScale = Gamespeed;//sets gamespeed, which will increase to speed up training
        if (cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                GameObject.Destroy(cars[i].gameObject);//if there are Prefabs in the scene this will get rid of them
            }
            DeadBots = 0;
            FurthestPosition = 0;

            SortNetworks();//this sorts networks and mutates them
        }

        cars = new List<Bot>();
        for (int i = 0; i < populationSize; i++)
        {
            //create bots ( if setting up from scratch find best starting position/rotation for the bots)
            Bot car = (Instantiate(prefab, new Vector3(27.18f, 129.71f, 23.67f), new Quaternion(0, 270.061f, 0, 0))).GetComponent<Bot>();
            car.network = networks[i];//deploys network to each learner
            cars.Add(car);
        }
        Generations++;

    }

    public void DestroyCars()
    {
        if (cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                GameObject.Destroy(cars[i].gameObject);//if there are Prefabs in the scene this will get rid of them
            }

            SortNetworks();//this sorts networks and mutates them
        }
    }

    public void SortNetworks()
    {
        string path = "Assets/StreamingAssets/GenerationSave.txt";
        for (int i = 0; i < populationSize; i++)
        {
            cars[i].UpdateFitness();//gets bots to set their corrosponding networks fitness
        }
        networks.Sort();
        networks[populationSize - 1].Save(path);//saves networks weights and biases to file, to preserve network performance
        for (int i = 0; i < populationSize / 2; i++)
        {
            networks[i] = networks[i + populationSize / 2].copy(new NeuralNetwork(layers));
            networks[i].Mutate((int)(1 / MutationChance), MutationStrength);
        }

    }

    public void SaveGeneration()
    {
        string path = "Assets/StreamingAssets/GenerationSave.txt";
        string NN_path = "Assets/StreamingAssets/Save.txt";
        if (NewTest)
        {
            File.Create(path).Close();
            File.Create(NN_path).Close();
            StreamWriter writer = new StreamWriter(NN_path, false);
            writer.WriteLine("");

            NewTest = false;
            Generations = 0;
            OverallFurthestPos = 0;

        }
        else
        {

            string content = File.ReadAllText(path);
            if (content == "")
            {
                Generations = 0;
            }
            else
            {
                Generations = int.Parse(content);
            }

        }
    }

    private void updateTestInfo()
    {
        MutationChance = Mutation_Chance_input.text == "" ? 0.01f : float.Parse(Mutation_Chance_input.text);
        MutationStrength = Mutation_Strength_input.text == "" ? 0.5f : float.Parse(Mutation_Strength_input.text);
        timeframe = TimeFrame.text == "" ? 1.0f : float.Parse(TimeFrame.text);
        Gamespeed = Game_Speed_input.text == "" ? 2.0f : float.Parse(Game_Speed_input.text);
        populationSize = Population_input.text == "" ? 100 : int.Parse(Population_input.text);

    }

    private void update_ui_text()
    {
        deadBots_txt.text = DeadBots.ToString();
        FurthPos_txt.text = FurthestPosition.ToString();
        BestPosition_txt.text = OverallFurthestPos.ToString();
        Generation_txt.text = Generations.ToString();
    }
    
}
