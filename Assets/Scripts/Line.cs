using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Line : MonoBehaviour {
	private LineRenderer line;
	public List<Vector3> pointsList;
	public GameObject point;

	//The First and Second Color for the line gradient
	public Color color1 = new Color(0.97f, 0.23f, 0.98f, 1.00f);
	public Color color2 = new Color(0.21f, 0.86f, 0.86f, 1.00f);
	public float fadeInTime = 3f;
	public float fadeOutTime = 3f;
    public Material material;

        
	struct myLine {
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	}

	void Awake () {
		// Create line renderer component and set its property
		line = gameObject.AddComponent<LineRenderer> ();
        // Give the line a default shader
        //		line.material = new Material(Shader.Find ("Assets/Sprites/Pieces/Outline.mat"));
        line.material = material;

        line.SetVertexCount (0);
		line.SetWidth (0.1f, 0.15f);
		line.SetColors (Color.white, Color.white);
		line.useWorldSpace = true;    
		pointsList = new List<Vector3> ();
		line.SetVertexCount (0);
		pointsList.RemoveRange (0, pointsList.Count);
		//line.SetColors (Color.white, Color.white);
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

    public bool Timer(float timeLimit)
    {
            for (int i = 0; i < pointsList.Count; i++)
            {
                Vector3 point = pointsList[i];
                point.z += 1f;
                pointsList[i] = point;
                if(pointsList[i].z > timeLimit) {
                    pointsList.RemoveAt(i);
                    if (line)
                    {
                        line.SetPositions(pointsList.ToArray());
                    }
                //remove point edges
                  if(pointsList.Count > 1)
                {
                    EdgeCollider2D[] points = gameObject.GetComponentsInChildren<EdgeCollider2D>();
                    Destroy(points[0].gameObject);

                }
                    else
                {
                    Destroy(this.gameObject);
                    return true;
                }
            }
        }
        return false;
    }

    public void addPoint(Vector3 mousePos) {
		pointsList.Add (mousePos);
		line.SetVertexCount (pointsList.Count);
		line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
		if(pointsList.Count > 1) {
            Vector3 position = transform.position;
            position.z = 0;
			GameObject p = (GameObject) Instantiate(point, transform.position, transform.rotation);
			p.transform.parent = transform;
			List<Vector2> verticies = new List<Vector2>();
			verticies.Add(pointsList[pointsList.Count - 2]);
			verticies.Add(mousePos);
			p.GetComponent<EdgeCollider2D> ().edgeRadius = .05f;
			p.GetComponent<EdgeCollider2D>().points = verticies.ToArray();
		}
	}
	// Use this for initialization
	void Start () {
		StartCoroutine(ColorChangeIn());
	}

	IEnumerator ColorChangeIn() {
		// Fade the Colors into each other
		for (float t = 0.01f; t < fadeInTime; t += 0.1f)
		{
			line.startColor = Color.Lerp(color1, color2, t/fadeInTime); 
			line.endColor = Color.Lerp(color2, color1, t/fadeInTime);
			yield return null;
		}
		yield return new WaitForSeconds(.5f);
		StartCoroutine(ColorChangeOut());
	}


	IEnumerator ColorChangeOut() {
		// Go back to the first color assigned
		for (float t = 0.01f; t < fadeOutTime; t += 0.1f)
		{
			line.startColor = Color.Lerp(color2, color1, t / fadeInTime);
            line.endColor = Color.Lerp(color1, color2, t / fadeInTime);
            yield return null;
		}
		yield return new WaitForSeconds(.5f);
		//Repeat
        StartCoroutine(ColorChangeIn());
	}



}
