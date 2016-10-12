using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public LevelRow[] rows;
	public GameObject[] staticBumpers;
	public GameObject[] movingBumpers;
	public GameObject[] deathTraps;
	public int maxMovingBumpers;
	public int maxStaticBumpers;
	public int maxTraps;
	private int currentBumpers;
	public GameObject levelCounter;
	public int currentLevel;
	// Use this for initialization
	void Start () {
		Generate();
		currentLevel = 1;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void removeBumper() {
		currentBumpers--;
		if(currentBumpers <= 0) {
			//Level Up
			ClearGrid();
			currentLevel++;
			//procedural logic for level creation
			if(currentLevel%3 == 2) {
				//starting at level 2, then 5, then 8, etc.
				maxTraps++;
			}
			if(currentLevel%3 == 0) {
				//starting at level 3, then 6, then 9, etc.
				maxMovingBumpers++;
			}
			if(currentLevel%3 == 1) {
				//starting at level 1, then 4, then 7, etc.
				maxStaticBumpers++;
			}
			levelCounter.SetActive(true);
		}	

	}

	public void ClearGrid() {
		for(int i = 0; i < rows.Length; i++) {
			for(int j = 0; j < rows[i].columns.Length; j++) {
				foreach (Transform child in rows[i].columns[j].GetComponentInChildren<Transform>()) {
					Destroy(child.gameObject);
				}
			}
		}
	}

	public void Retry() {
		ClearGrid();
		currentLevel = 1;
		maxStaticBumpers = 1;
		maxMovingBumpers = 0;
		maxTraps = 0;
		Generate();
	}

	public void Generate() {
		int movingBumperCount = 0;
		int staticBumperCount = 0;
		int trapCount = 0;
		currentBumpers = 0;
		int verticalMover = 0;
		int horizontalMover = 0;

		for(int i = 0; i < rows.Length; i++) {
			int thirds = Random.Range(0,100);
			if (thirds < 33 && maxMovingBumpers > 0 && movingBumperCount < maxMovingBumpers) {
				//clear a row for movement. using bumper 0
				movingBumperCount++;
				if(movingBumperCount <= maxMovingBumpers) {
					int randomColumn = Random.Range(0,rows[i].columns.Length);
					int mbType = Random.Range(0, movingBumpers.Length);
					currentBumpers++;
					placeObstacle(movingBumpers[0], rows[i].columns[randomColumn].transform.position, rows[i].columns[randomColumn], true);
				}
			}
			else {
				int selectedStartingColumn = Random.Range(0, rows[i].columns.Length);
				for(int j = selectedStartingColumn; j < rows[i].columns.Length; j++) {
					if(thirds > 66) {
						staticBumperCount++;
						if(staticBumperCount <= maxStaticBumpers) {
							int bType = Random.Range(0, staticBumpers.Length);
							currentBumpers++;
							placeObstacle(staticBumpers[0], rows[i].columns[j].transform.position, rows[i].columns[j], false);
						}					
					}
					else {
						trapCount++;
						if(trapCount <= maxTraps) {
							int tType = Random.Range(0, deathTraps.Length);
							placeObstacle(deathTraps[0], rows[i].columns[j].transform.position, rows[i].columns[j], false);

						}					
					}
				}
			}
		}
		//make sure at least one bumper is generated.
		if(staticBumperCount == 0 && movingBumperCount == 0) {
			Generate();
		}
	}

	private void placeObstacle(GameObject go, Vector3 place, GameObject parent, bool moving) {
		GameObject o = (GameObject) Instantiate(go, place, transform.rotation);
		o.transform.parent = parent.transform;
		o.GetComponent<Obstacle>().moves = moving;
	}
}
