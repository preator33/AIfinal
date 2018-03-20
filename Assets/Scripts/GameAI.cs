using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using GControl = GameController;


public class GameAI : MonoBehaviour
{
    Perceptron[] percivals;
    float[] featureVector;
    public Vector2 nextPosition;
    public GameObject aiCheck;

    void Awake()
    {
        // 5 because 4 of them check the surounding tiles to see if it is safe and the final one is the direction too food
        percivals = new Perceptron[4];
        featureVector = new float[5];
        /*
        percivals[0] = new Perceptron(5); // turn down 
        percivals[1] = new Perceptron(5); // turn up
        percivals[2] = new Perceptron(5); // turn left 
        percivals[3] = new Perceptron(5); // turn right 
        */
    }
    public void UpdateFeatureVector(Vector2 currentPosition)
    {

        // first check around it to see what is safe and what isn't 
        Vector2 upPosition = new Vector2(nextPosition.x, nextPosition.y + 1);
        Vector2 rightPosition = new Vector2(nextPosition.x + 1, nextPosition.y); 
        Vector2 downPosition = new Vector2(nextPosition.x, nextPosition.y - 1); 
        Vector2 leftPosition = new Vector2(nextPosition.x - 1, nextPosition.y);

        Physics.CheckBox(currentPosition, Vector3 (0.475f), transform.rotation, 0, 0);

        // load that into the feature vector 

        // for loop, pass them to perceptrons


        // nextPosition   = FindObjectOfType(typeof(GameController));
        //GameController.head
        // .transform.position;

        //Switch between 4 position - up/right/down/left










        // Instantiate new snake prefab and set next variable of current head to this new instantiated object
        temp = (GameObject)Instantiate(snakePrefab, nextPosition, transform.rotation);
        

    }

    public int MakeDecision()
    {
        //update the feature vector 

        // retrive result from perceptrons 

        // execute the one with the highest likelyhood 

    }



}

