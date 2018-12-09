using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Controller : MonoBehaviour {

    public static Game_Controller Instance;

    public int coinCounter = 0;
    public int extraLives;
    public Text coinDisplay;
    public int score;
    public Text scoreDisplay;

	
	// Update is called once per frame
	void Awake ()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
        
	}

    void Start()
    {
        
    }


    public void Restart()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentLevel);
    }

    public void AddScore(int scoreAdd)
    {
        score += scoreAdd;
        scoreDisplay.text = score.ToString();
    }

    public void SetScore (int scoreSet)
    {
        score = scoreSet;
        scoreDisplay.text = score.ToString();
    }

    public void AddCoin()
    {
        coinCounter++;
        if (coinCounter == 100)
        {
            coinCounter = 0;
            extraLives++;
        }
        coinDisplay.text = coinCounter.ToString();


    }

}
