using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using GControl = GameController;


public class GameAI : MonoBehaviour
{
    
    private Perceptron percivalUp;
    private Perceptron percivalDown;
    private Perceptron percivalLeft;
    private Perceptron percivalRight;
    
   // private Perceptron[] percivals;

    private float[] featureVector;
    //public Vector2 currentPosition;
    public GameObject aiCheck;
    bool iniitialized = false; //for some reason initialized is never called so i will run initialize if it hasn't been initialized 



    public void Initialize()
    {
        iniitialized = true; 
        // 5 because 4 of them check the surounding tiles to see if it is safe and the final one is the direction too food
      
        featureVector = new float[5];
       // percivals = new Perceptron[4];


       percivalUp.Initialize(5);
       percivalDown.Initialize(5);
       percivalLeft.Initialize(5);
       percivalRight.Initialize(5);

    }
    public void UpdateFeatureVector(Vector2 currentPosition, Vector3 foodLocation)
    {
        if (iniitialized == false)
            Initialize();

        // first check around it to see what is safe and what isn't 
        Vector2 upPosition = new Vector2(currentPosition.x, currentPosition.y + 1);
        Vector2 rightPosition = new Vector2(currentPosition.x + 1, currentPosition.y); 
        Vector2 downPosition = new Vector2(currentPosition.x, currentPosition.y - 1); 
        Vector2 leftPosition = new Vector2(currentPosition.x - 1, currentPosition.y);

        //use the physics2d raycast 
        RaycastHit2D upcheck = Physics2D.Raycast(currentPosition, upPosition);
        RaycastHit2D downcheck = Physics2D.Raycast(currentPosition, downPosition);
        RaycastHit2D leftcheck = Physics2D.Raycast(currentPosition, leftPosition);
        RaycastHit2D rightcheck = Physics2D.Raycast(currentPosition, rightPosition);

       bool upsafe      = (upcheck.collider == null);
       bool downsafe    = (downcheck.collider == null);
       bool leftsafe    = (leftcheck.collider == null);
       bool rightsafe   = (rightcheck.collider == null);

        if(!upsafe)
             upsafe   = !(upcheck.collider.gameObject.CompareTag("snake"));
        if(!downsafe)
             downsafe = !(downcheck.collider.gameObject.CompareTag("snake"));
        if(!leftsafe)
            leftsafe = !(leftcheck.collider.gameObject.CompareTag("snake"));
        if(!rightsafe)
            rightsafe= !(rightcheck.collider.gameObject.CompareTag("snake"));

        featureVector[0] = upsafe ? 1 : 0;
        featureVector[1] = downsafe ? 1 : 0;
        featureVector[2] = leftsafe ? 1 : 0;
        featureVector[3] = rightsafe ? 1 : 0;

        Vector2 foodPosition2d = (Vector2)foodLocation;
       featureVector[4] =  Vector2.Angle(currentPosition, foodPosition2d);

        // load that into the feature vector 
        // for loop, pass them to perceptrons
        for(int i = 0; i< featureVector.Length;i ++)
        {
            percivalUp.featureVector[i] = featureVector[i];
            percivalDown.featureVector[i] = featureVector[i];
            percivalLeft.featureVector[i] = featureVector[i];
            percivalRight.featureVector[i] = featureVector[i];
        }
        
       
        
        // for (int i =0; i < percivals.Length;i++ )        {
        //     for (int j = 0; j < percivals[i].featureVector.Length; i++) {
        //         percivals[i].featureVector[j] = featureVector[j];
        //     }
        // }
        /// all percivals should have their feature vectors loaded 

    }

    public int MakeDecision()
    {
        //update the feature vector 
        

        // retrive result from perceptrons 

        // execute the one with the highest likelyhood 
        return 4;
    }



}

