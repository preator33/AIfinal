using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class Perceptron : MonoBehaviour
{
    public float[] featureVector;
    public float[] weights;
    public int featureVectorLength;
    public float bias; 




    public float sigmoid(float num)    {
        const float e = 2.71828182845904523536f;
        return 1.0f / (1.0f + Mathf.Pow(e, -num));
    }

   public Perceptron(int featureVectorSize)    { // basic constructor 
        weights = new float[featureVectorSize];
        featureVectorLength = featureVectorSize;
        featureVector = new float[featureVectorLength]; 
        for ( int i = 0; i < featureVectorSize; ++i)  {
            weights[i] = 0.0f;
        }
    }

    Perceptron(Perceptron parent)    { //copy constructor
        featureVectorLength = parent.featureVectorLength;

        for (int i =0; i < featureVectorLength; i++)        { 
            featureVector[i] = parent.featureVector[i];
            weights[i] = parent.weights[i];
        }
        bias = parent.bias; 
    }

    public void RandomizeValues()    {   
        for (int i = 0; i < featureVector.Length; i++)  {
            weights[i] = Random.Range(-2.0f, 2.0f); 
        }
        bias = Random.Range(-2.0f, 2.0f);
    }

    public Perceptron Crossover(Perceptron parent1, Perceptron parent2)    {
        Perceptron result;
        result = new Perceptron(parent1.featureVector.Length);
        for ( int i = 0; i < result.featureVector.Length; ++i)        {
            result.weights[i] = (parent1.weights[i] + parent2.weights[i]) / 2.0f;
        }

        result.bias = (parent1.bias + parent2.bias) / 2.0f;

        return result; 
    }

    public float Evaluate(float fvect)    {      

        float result = 0.0f;
        for ( int i = 0; i < featureVector.Length; ++i)
        {
            result += featureVector[i] * weights[i];
        }

        result += bias;

        return sigmoid(result);
        
    }

}




public class GameAI : MonoBehaviour
{
    Perceptron[] percivals; 
    float[] featureVector; 


    public GameAI()    {
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
    public void updateFeatureVector()    {
        // first check around it to see what is safe and what isn't 


        // load that into the feature vector 

        // for loop, pass them to perceptrons

    }

    public void makeDecision()    {
        //update the feature vector 

        // retrive result from perceptrons 

        // execute the one with the highest likelyhood 

    }



}


public class GameController : MonoBehaviour
{
    public int maxSize;
    public int currentSize;

    // Spawning range for the food
    public int xBound;
    public int yBound;

    public int score;
    public Text scoreText; 
    public GameObject foodPrefab;
    public GameObject currentFood;
    public GameObject snakePrefab;
    public Snake head;
    public Snake tail;
    public int direction;
    public Vector2 nextPosition;


    void OnEnable()
    {
        Snake.hit += Collision;
    }

    // Use this for initialization
    void Start ()
    {
        InvokeRepeating("TimerInvoke", 0, 0.1f); //Repeat every 0.5s
        FoodFunction();
    }

    void OnDisable()
    {
        Snake.hit -= Collision;
    }

    // Update is called once per frame
    void Update ()
    {
        
        ChangeDirection();
	}

    void TimerInvoke()
    {
        Movement();
        if (currentSize >= maxSize)
        {
            TailFUnc();
        }

        else
        {
            currentSize++;
        }
    }

    void Movement()
    {
        GameObject temp;
        nextPosition = head.transform.position;

        //Switch between 4 position - up/right/down/left
        switch(direction)
        {
            case 0: //UP
                nextPosition = new Vector2(nextPosition.x, nextPosition.y + 1);
                break;

            case 1: //RIGHT
                nextPosition = new Vector2(nextPosition.x + 1, nextPosition.y);
                break;

            case 2: //DOWN
                nextPosition = new Vector2(nextPosition.x, nextPosition.y - 1);
                break;

            case 3: //LEFT
                nextPosition = new Vector2(nextPosition.x - 1, nextPosition.y);
                break;
        }

        // Instantiate new snake prefab and set next variable of current head to this new instantiated object
        temp = (GameObject)Instantiate(snakePrefab, nextPosition, transform.rotation);
        head.SetNext(temp.GetComponent<Snake>());
        head = temp.GetComponent<Snake>();
    }

    void ChangeDirection()
    {
        // Make sure you can't go backwards
        if (direction != 2 && Input.GetKeyDown(KeyCode.W))      
        {
            direction = 0;
        }

        if (direction != 3 && Input.GetKeyDown(KeyCode.D))      
        {
            direction = 1;
        }

        if (direction != 0 && Input.GetKeyDown(KeyCode.S))      
        {
            direction = 2;
        }

        if (direction != 1 && Input.GetKeyDown(KeyCode.A))       
        {
            direction = 3;
        }
    }

    void TailFUnc()
    {
        Snake tempSnake = tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }

    void FoodFunction()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);

        currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
        StartCoroutine(CheckRender(currentFood));
    }
    
    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if (IN.GetComponent<Renderer>().isVisible == false)
        {
            if (IN.tag == "Food")
            {
                Destroy(IN);
                FoodFunction();
            }
        }
    }

    void Collision (string ObjectSent)
    {
        if (ObjectSent == "Food")
        {
            FoodFunction();
            score++;
            scoreText.text = score.ToString();
            maxSize += 5;

            int tempScore = PlayerPrefs.GetInt("HighScore");
            if (score > tempScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }

        if (ObjectSent == "Snake")
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        CancelInvoke("TimerInvoke");
        SceneManager.LoadScene(0);
    }

}
