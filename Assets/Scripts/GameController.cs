using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public GameAI Ai;


    void OnEnable()
    {
        Snake.hit += Collision;
    }

    // Use this for initialization
    void Start ()
    {
        InvokeRepeating("TimerInvoke", 0, 0.1f); //Repeat every 0.5s
        Ai = new GameAI();
        Ai.Initialize();
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

       
        Vector3 foodLocation = foodPrefab.transform.position;
        Ai.UpdateFeatureVector(nextPosition, foodLocation);

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
