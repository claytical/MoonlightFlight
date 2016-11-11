using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Line : MonoBehaviour {
	private LineRenderer line;
	public List<Vector3> pointsList;
	public GameObject point;

	struct myLine {
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	};

	void Awake () {
		// Create line renderer component and set its property
		line = gameObject.AddComponent<LineRenderer> ();
		line.material = new Material (Shader.Find ("Particles/Additive"));
		line.SetVertexCount (0);
		line.SetWidth (0.1f, 0.1f);
		line.SetColors (Color.green, Color.green);
		line.useWorldSpace = true;    
		pointsList = new List<Vector3> ();
		line.SetVertexCount (0);
		pointsList.RemoveRange (0, pointsList.Count);
		line.SetColors (Color.green, Color.green);
	}

	public bool Shorten() {
		if(pointsList.Count > 1) {
			pointsList.RemoveAt(0);
			if(line) {
				line.SetPositions(pointsList.ToArray());
			}
			//remove point edges
			EdgeCollider2D[] points =  gameObject.GetComponentsInChildren<EdgeCollider2D>();
			Destroy(points[0].gameObject);
//			points[0].enabled = false;


		}
		else {
			Destroy(this.gameObject);
			return true;
		}
		return false;
	}

	public void addPoint(Vector3 mousePos) {
		pointsList.Add (mousePos);
		line.SetVertexCount (pointsList.Count);
		line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
		if(pointsList.Count > 1) {
			GameObject p = (GameObject) Instantiate(point, transform.position, transform.rotation);
			p.transform.parent = transform;
			List<Vector2> verticies = new List<Vector2>();
			verticies.Add(pointsList[pointsList.Count - 2]);
			verticies.Add(mousePos);
			p.GetComponent<EdgeCollider2D>().points = verticies.ToArray();
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
