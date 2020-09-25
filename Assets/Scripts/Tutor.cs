using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutor : MonoBehaviour {

	public string[] instructions;
	public GameObject[] panels; 
	
	public GameObject previous; 
	public GameObject next; 
	public GameObject back;
	public Text instruction;
	private int instructionIndex = 0;


	void Start () {
		instruction.text = instructions[instructionIndex];
		panels[instructionIndex].SetActive(true);
		next.SetActive(true);
		previous.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {


	}

	public void Next()
	{
		hidePanels();
		instructionIndex++;
		if(instructionIndex == instructions.Length - 2)
        {
			//done
			next.SetActive(false);
			back.SetActive(true);
			instruction.text = instructions[instructionIndex];
			panels[instructionIndex].SetActive(true);
		}
		else
        {
			previous.SetActive(true);
			Debug.Log("Next Panel");
			instruction.text = instructions[instructionIndex];
			panels[instructionIndex].SetActive(true);
		}
	}

	private void hidePanels()
    {
		for (int i = 0; i < panels.Length; i++)
		{
			panels[i].SetActive(false);
		}
	}

	public void Previous()
	{
		hidePanels();
		back.SetActive(false);
		instructionIndex--;
		if (instructionIndex == 0)
		{
			instruction.text = instructions[instructionIndex];
			panels[instructionIndex].SetActive(true);
			previous.SetActive(false);
			//done
		}
		else
		{
			instruction.text = instructions[instructionIndex];
			panels[instructionIndex].SetActive(true);

			next.SetActive(true);

		}
	}


}
