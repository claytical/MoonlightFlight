using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour {
	public GameObject LevelCompletePanel;
	public GameObject LevelFailPanel;
	public GameObject WorldCompletePanel;
    public GameObject[] breakableObjects;
    public GameObject spawnLocations;
	public string nextWorldName;
    public Text fliesReleasedUI;
    public bool endless;
    public bool inkEnabled;
    public int fliesReleased;
    public AudioClip success;
    private string selectedScene;
	private AsyncOperation AO;
	private GameState gameState;
	private int nextLevel;
	private string currentWorld;
    private bool levelFinished;


	// Use this for initialization
	void Start () {
        Time.timeScale = 0f;
        fliesReleased = 0;
		gameState = (GameState)FindObjectOfType (typeof(GameState));	
		if (gameState != null) {
			currentWorld = gameState.SelectedWorld;
		}
        if (endless)
        {
            PlaceRandomBreakable();
        }
        }
	
	void FixedUpdate () {
        if (!levelFinished)
        {
            ScanForCompletion();
        }
	}


    public void AddFliesReleased(int flies, float inkAmount, int multiplier)
    {
        Debug.Log("There are " + flies + " flies");
        fliesReleased += flies * multiplier;

        if (inkEnabled)
        {
            fliesReleased += flies + (flies * (int) inkAmount);

        }

        fliesReleasedUI.text = fliesReleased.ToString();
    }
	public void ScanForCompletion() {
        if (endless)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Disappearing");
            if (gos.Length == 0)
            {
                PlaceRandomBreakable();
            }

        }
        else
        {
            Breakable[] gos = GetComponentsInChildren<Breakable>();
//            GameObject[] gos = GameObject.FindGameObjectsWithTag("Disappearing");
            if (gos.Length > 0)
            {
                Debug.Log("Still bumpable objects");

            }
            else
            {
                levelFinished = true;
                Debug.Log("Level Complete");
                Ball ball = GameObject.FindObjectOfType<Ball>();
                ball.gameObject.GetComponent<Collider2D>().enabled = false;
                ball.gameObject.GetComponent<Rigidbody2D>().simulated = false;
                ball.gameObject.GetComponentInParent<LevelSound>().SilentMode();
                ball.gameObject.GetComponent<AudioSource>().PlayOneShot(success);
                //chunk sets
                for(int i = 0; i < 100; i++)
                {
                    Instantiate(ball.fly, ball.gameObject.transform);
                }
//                EndOfLevel();
                    Invoke("EndOfLevel", 5);
            }
        }
	}

    public void EndOfLevel()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (gameState != null)
        {

            if (int.Parse(gameState.SelectedLevel) >= 10)
            {
                //unlock next world
                //WORLD COMPLETE UI
                gameState.SelectedWorld = nextWorldName;
                nextLevel = 1;
                PlayerPrefs.SetInt(gameState.SelectedWorld, 1);
                WorldCompletePanel.SetActive(true);
            }
            else
            {
                Debug.Log("Next Level Awaits...");
                //unlock next level
                nextLevel = int.Parse(gameState.SelectedLevel) + 1;
                PlayerPrefs.SetInt(gameState.SelectedWorld + "_" + nextLevel, 0);
                LevelCompletePanel.SetActive(true);
                if (LevelCompletePanel.GetComponent<LevelComplete>() != null)
                {
                    LevelCompletePanel.GetComponent<LevelComplete>().SetScore(fliesReleased);
                    Debug.Log("Setting score");
                }
            }
        }
        else
        {
            LevelCompletePanel.SetActive(true);
            if (LevelCompletePanel.GetComponent<LevelComplete>() != null)
            {
                LevelCompletePanel.GetComponent<LevelComplete>().SetScore(fliesReleased);
                Debug.Log("Setting score");
            }

        }

    }
    public void PlaceRandomBreakable()
    {

        Transform[] locations = spawnLocations.GetComponentsInChildren<Transform>();
        GameObject obj = Instantiate(breakableObjects[Random.Range(0, breakableObjects.Length)], locations[Random.Range(0, locations.Length)].position, Quaternion.identity, transform);
        
        //Place new object
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        Vector3 origin = Camera.main.ViewportToScreenPoint(new Vector3(-7, 0, 0));
        float startX = Camera.main.transform.position.x - (Screen.width / 2);
        float objX = obj.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        float objY = obj.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        //+ (_prefabSize.x / 2) + (_prefabSize.x * x);
        float startY = Camera.main.transform.position.y - (Screen.height / 2);
        //+ (_prefabSize.y / 2) + (_prefabSize.y * y);

        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(objX, Screen.width - objX), Random.Range(objY, Screen.height - objY), Camera.main.farClipPlane / 2));
  //      Vector3 position = new Vector3(origin.x, origin.y, 0);
        obj.transform.position = screenPosition;
//        GameObject obj = Instantiate(breakableObjects[Random.Range(0, breakableObjects.Length)], screenPosition, Quaternion.identity, transform);
        
    }
    public void NextLevel() {
		if (gameState != null) {
			gameState.SelectedLevel = nextLevel.ToString ();
			selectedScene = gameState.SelectedWorld + "_" + nextLevel;
			SelectScene ();
		}
	}

	public void RetryLevel() {
		if (gameState) {
			selectedScene = currentWorld + "_" + gameState.SelectedLevel;
			SelectScene ();
		} else {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	public void SetSceneToLoad(string s) {
		selectedScene = s;
	}

	public void SelectScene() {
		StartCoroutine("loadScene");
	}

	IEnumerator loadScene() {
		AO = SceneManager.LoadSceneAsync (selectedScene, LoadSceneMode.Single);
		AO.allowSceneActivation = false;
		while (AO.progress < 0.9f) {
			yield return null;
		}
		AO.allowSceneActivation = true;
	}

}
