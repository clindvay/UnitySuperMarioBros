using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Controller : MonoBehaviour {

    public static Game_Controller Instance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Awake ()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
        
	}

    public void Restart()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentLevel);
    }


}
