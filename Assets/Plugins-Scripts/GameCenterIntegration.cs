using UnityEngine;
using System.Collections;

public class GameCenterIntegration : MonoBehaviour {
	
	/*
	private GameCenter gameCenter;
	public GUISkin spaceAdventureSkinPlain;
	public bool isGamePlayGameCenter = false;
	//Game Center Button
	public Rect gameCenter_btn_pos;
	public Texture2D gameCenter_Texture;
	
	void Awake(){
		if(Globals.IsIPhone5Display()){
			AdjustButtonsForIPhone5();
		}else if(Globals.IsIPadBeforeRetinaDisplay()){
			AdjustButtonsForIPadNonRetina();
		}else if(Globals.IsIPadRetinaDisplay()){
			AdjustButtonsForIPadRetina();
		}else if(Globals.IsIPhone4SOr4Display()){
			AdjustButtonsForIPhone4or4s();
		}else if(Globals.IsIPhoneBeforeRetinaDisplay()){
			AdjustButtonsForIPhoneNonRetina();
		}
	}
	
	// Use this for initialization
	void Start () {
		gameCenter = GameCenter.Instance;
		gameCenter.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		if(Globals.gameStatus == Globals.GameStatus.GameOver){
			if(PlayerPrefs.HasKey("Higher_Score")) {
				//Report High Score
				if(!Globals.scoreSent) {
					Debug.Log("Achievement sent...");
					Globals.scoreSent = true;
					gameCenter.ReportScore(long.Parse(PlayerPrefs.GetFloat("Higher_Score").ToString()));
					//Determine if Achievement was met
					DetermineAcquiredAchievement();
				}
			}
		}
	}
	/// <summary>
	/// DetermineAcquiredAchievement.
	/// </summary>
	void DetermineAcquiredAchievement() {
		
		Debug.Log("DetermineAcquiredAchievement -> " + PlayerPrefs.GetFloat("Higher_Score").ToString());
		
		//Check if Achievment was reached and key has not been set to avoid calling ReportAchievementProgress multiple times
		if(PlayerPrefs.GetFloat("Higher_Score") >= Globals.Achievements[0].Value && !PlayerPrefs.HasKey(Globals.Achievements[0].Key)) {
			PlayerPrefs.SetInt(Globals.Achievements[0].Key,1);
			gameCenter.ReportAchievementProgress(Globals.Achievements[0].Key,100);
		}
		if(PlayerPrefs.GetFloat("Higher_Score") >= Globals.Achievements[1].Value && !PlayerPrefs.HasKey(Globals.Achievements[1].Key)) {
			PlayerPrefs.SetInt(Globals.Achievements[1].Key,1);
			gameCenter.ReportAchievementProgress(Globals.Achievements[1].Key,100);
		}
		if(PlayerPrefs.GetFloat("Higher_Score") >= Globals.Achievements[2].Value && !PlayerPrefs.HasKey(Globals.Achievements[2].Key)) {
			PlayerPrefs.SetInt(Globals.Achievements[2].Key,1);
			gameCenter.ReportAchievementProgress(Globals.Achievements[2].Key,100);
		}
		if(PlayerPrefs.GetFloat("Higher_Score") >= Globals.Achievements[3].Value && !PlayerPrefs.HasKey(Globals.Achievements[3].Key)) {
			PlayerPrefs.SetInt(Globals.Achievements[3].Key,1);
			gameCenter.ReportAchievementProgress(Globals.Achievements[3].Key,100);
		}
	}
	void ShowLeaderBoard(){
		if(gameCenter.IsUserAuthenticated()) {	
			gameCenter.ShowLeaderboardUI();
		}
	}
	void OnGUI() {
		if(!Globals.loaded)
			return;
		//SHOW LEADER BOARD
		if(GUI.Button(gameCenter_btn_pos,gameCenter_Texture,spaceAdventureSkinPlain.button))
		{
			Debug.Log("Showing Leader Board");
			ShowLeaderBoard();
		}
	}
		#region Multiple Resolution Procedures
	
	void AdjustButtonsForIPhone5(){
		if(isGamePlayGameCenter) {
			gameCenter_btn_pos = new Rect(1069, 570,60,60);
		}
			
	}//Preset for iPhone 5
	
	void AdjustButtonsForIPhoneNonRetina()
	{
		if(isGamePlayGameCenter){
			gameCenter_btn_pos = new Rect(433, 238,40,40);
		}
		else
			gameCenter_btn_pos = new Rect(373, 173,60,60);
	}
	
	void AdjustButtonsForIPhone4or4s()
	{
		if(isGamePlayGameCenter){
			gameCenter_btn_pos = new Rect(885, 504,60,60);
		}
		else {
			gameCenter_btn_pos.x = 741.0f;
			gameCenter_btn_pos.y = 301.0f;
		}
	}
	//iPAD 1 / 2 - 1024 Width
	void AdjustButtonsForIPadNonRetina()
	{
		if(isGamePlayGameCenter){
			gameCenter_btn_pos = new Rect(962, 687,60,60);
		}
		else {
			gameCenter_btn_pos.x = 790.0f;
			gameCenter_btn_pos.y = 408.0f;
		}
	}
	void AdjustButtonsForIPadRetina()
	{
		if(isGamePlayGameCenter){
			gameCenter_btn_pos = new Rect(962 * 2, 687 * 2,60 * 2,60 * 2);
		}
		else {
			gameCenter_btn_pos = new Rect(790.0f * 2, 408.0f * 2,
			gameCenter_btn_pos.width * 2,gameCenter_btn_pos.height * 2);
		}
	}
	#endregion
	
	*/
}
