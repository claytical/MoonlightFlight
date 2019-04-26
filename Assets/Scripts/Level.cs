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
	public Text nextWorldName;
    public Text fliesReleasedUI;
    public bool endless;
    public int fliesReleased;
    private string selectedScene;
	private AsyncOperation AO;
	private GameState gameState;
	private int nextLevel;
	private string currentWorld;


	// Use this for initialization
	void Start () {
        fliesReleased = 0;
		gameState = (GameState)FindObjectOfType (typeof(GameState));	
		if (gameState != null) {
			currentWorld = gameState.SelectedWorld;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddFliesReleased(int flies)
    {
        Debug.Log("There are " + flies + " flies");
        fliesReleased += flies;
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
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Disappearing");
            if (gos.Length > 0)
            {
                Debug.Log("Still bumpable objects");

            }
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                //set stars for current level
                if (gameState != null)
                {
                    PlayerPrefs.SetInt(gameState.SelectedWorld + "_" + gameState.SelectedLevel, player.GetComponent<PlayerControl>().balls + 1);
                    if (int.Parse(gameState.SelectedLevel) >= 10)
                    {
                        //unlock next world
                        //WORLD COMPLETE UI
                        gameState.SelectedWorld = nextWorldName.text;
                        nextLevel = 1;
                        PlayerPrefs.SetInt(gameState.SelectedWorld, 1);
                        WorldCompletePanel.SetActive(true);
                        ProcGenMusic.MusicGenerator.Instance.Stop();

                    }
                    else
                    {

                        //unlock next level
                        nextLevel = int.Parse(gameState.SelectedLevel) + 1;
                        PlayerPrefs.SetInt(gameState.SelectedWorld + "_" + nextLevel, 0);
                        LevelCompletePanel.SetActive(true);
                        if (LevelCompletePanel.GetComponent<LevelComplete>() != null)
                        {
                            LevelCompletePanel.GetComponent<LevelComplete>().SetStars(player.GetComponent<PlayerControl>().balls + 1);
                        }
                        BallHolder ballHolder = (BallHolder)FindObjectOfType(typeof(BallHolder));
                        Ball[] balls = ballHolder.GetComponentsInChildren<Ball>();
                        for (int i = 0; i < balls.Length; i++)
                        {
                            Destroy(balls[i].gameObject);
                        }



                    }
                }
                else
                {
                    //Debugging level

                    BallHolder ballHolder = (BallHolder)FindObjectOfType(typeof(BallHolder));
                    Ball[] balls = ballHolder.GetComponentsInChildren<Ball>();
                    for (int i = 0; i < balls.Length; i++)
                    {
                        Destroy(balls[i].gameObject);
                    }
                    LevelCompletePanel.SetActive(true);
                    LevelCompletePanel.GetComponent<LevelComplete>().SetStars(2);
                    ProcGenMusic.MusicGenerator.Instance.Stop();

                    /*
                    //Place new object
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
                    Vector3 origin = Camera.main.ViewportToScreenPoint(new Vector3(-7, 0, 0));
                    float startX = Camera.main.transform.position.x - (Screen.width / 2);
                    //+ (_prefabSize.x / 2) + (_prefabSize.x * x);
                    float startY = Camera.main.transform.position.y - (Screen.height / 2);
                    //+ (_prefabSize.y / 2) + (_prefabSize.y * y);

                    Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
                    Vector3 position = new Vector3(origin.x, origin.y, 0);
                    Instantiate(breakableObjects[Random.Range(0, breakableObjects.Length)], screenPosition, Quaternion.identity);
                    */

                }
            }
        }
	}
    public void PlaceRandomBreakable()
    {
        GameObject obj = Instantiate(breakableObjects[Random.Range(0, breakableObjects.Length)], transform.position, Quaternion.identity, transform);
        
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
