using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public float speed;//Speed Multiplier
    public float rotation = 8;//Rotation multiplier
    public LayerMask raycastMask;//Mask for the sensors

   // public float CurTurnAngle; // to show the decisions being made (if using un comment code in FixedUpdate)
    private float[] input = new float[5];//input to the neural network

    public NeuralNetwork network;
    private Renderer mr;
    public Manager manager;
    private int listcount = 0;
    private bool dead = false;
    private List<string> list1 = new List<string>();

    public int position;//Checkpoint number on the course
    public bool collided;//To tell if the car has crashed


    private void Start()
    {
        mr = GetComponent<Renderer>();
        manager = GameObject.FindObjectOfType<Manager>();
    }

    private void Update()
    {
        if (manager.furthestPosition < position)
        {
            manager.furthestPosition = position;
        }
        if(position > manager.overallFurthestPos)
        {
            manager.overallFurthestPos = position;
        }
    }

    void FixedUpdate()
    {
        if (!collided)//if the car has not collided with the wall, it uses the neural network to get an output
        {
            for (int i = 0; i < 5; i++)//draws five debug rays as inputs
            {
                Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right;//calculating angle of raycast
                RaycastHit hit;
                Ray Ray = new Ray(transform.position, newVector);
                Debug.DrawRay(transform.position, newVector,Color.red);

                if (Physics.Raycast(Ray, out hit, 10f, raycastMask))
                {
                    input[i] = (10 - hit.distance) / 10;//return distance, 1 being close
                }
                else
                {
                    input[i] = 0;//if nothing is detected, will return 0 to network
                }
            }

            float[] output = network.FeedForward(input);//Call to network to feedforward
        
            transform.Rotate(0, output[0] * rotation, 0, Space.World);//controls the cars movement
            //Debug.Log(output[0] * rotation);
            //CurTurnAngle = output[0] * rotation;
            transform.position += this.transform.right * output[1] * speed;//controls the cars turning
           

            

        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))//check if the car passes a gate
        {
            GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
            //Debug.Log("collision name:"+(string)collision.gameObject.name);
            //Debug.Log("outside for loop"+list1.Count);
            for (int i = 0; i <= listcount; i++)
            {
                if (list1.Count == 0)
                {
                    list1.Add(collision.gameObject.name.ToString());
                    position++;//incriment position if checkpoint is collided with, and checkpoint has not been collided with before
                }
                if (!list1.Contains(collision.gameObject.name.ToString()))//check if checkpoint has been activated before
                {
                    list1.Add(collision.gameObject.name.ToString());
                    position++;//incriment position if checkpoint is collided with, and checkpoint has not been collided with before
                    break;
                    
                }
            }
            listcount = list1.Count;

        }
        else if(collision.collider.gameObject.layer != LayerMask.NameToLayer("Learner") && !dead)
        {
            collided = true;//stop operation if car has collided
            //Debug.Log("collision name:" + (string)collision.gameObject.name);
            mr.material.color = new Color(2f, 0f, 1f, 1f);
            dead = true;
            if (dead)
            {
                manager.deadBots++;
            }
        }
    }

    /// <summary>
    /// Updates fitness of network for sorting
    /// </summary>
    public void UpdateFitness()
    {
        network.fitness = position;
    }
}
