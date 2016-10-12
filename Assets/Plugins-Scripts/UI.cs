using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
	
	#region UI Graphics
	public Texture2D background;
	public Texture2D window;
	public Texture2D bottomBar;
	public Texture2D logo;
	public Texture2D infoBtnBackground;
	public Texture2D proBackground;
	
	//icons
	public Texture2D gameCenterIcon;
	public Texture2D inAppPurchasesIcon;
	public Texture2D uiKitIcon;
	public Texture2D uiKitMediaPlayerIcon;
	#endregion
	
	#region Game Center UI
	public Rect gameCenterIconPos;
	public Rect loginGameCenterUIPos;
	public Rect showAchievementsUIPos;
	public Rect showLeaderboardUIPos;
	public Rect reportScoreUIPos;
	public Rect reportAchievementProgressUIPos;
	public Rect gameCenterStatusUIPos;
	#endregion
	
	#region In App Purchases UI
	public Rect inAppPurchasesIconPos;
	public Rect loadStorePos;
	public Rect getProductInfoPos;
	public Rect restoreInAppsPos;
	public Rect purchaseProductPos;
	public Rect inAppPurchasesStatusUIPos;
	#endregion
	
	#region In UIKit UI
	public Rect uiKitIconPos;
	public Rect showBasicAlertPos;
	public Rect showDoubleButtonsAlertPos;
	public Rect openWebsitePos;
	//Media Player
	public Rect uiKitMediaPlayerIconPos;
	public Rect playVideo1Pos;
	public Rect playVideo2Pos;
	#endregion
	
	public Rect logoPos;
	public Rect infoPos;
	public Rect proPos;
	
	#region Skin
	public GUISkin skin;
	#endregion
	
	#region Plugins variables
	private GameCenter gameCenter = null;
	#endregion
	
	#region Initialization
	void Start(){
		//Game Center Ininitalization
		gameCenter = new GameCenter();
	}
	
	void OnGUI(){
	
		setupUI();
		
		handleGameCenterOptions();
		
		handleInAppPurchasesOptions();
		
		handleUIKitOptions();
		
		if(GUI.Button(infoPos,infoBtnBackground,skin.label)){
			Application.OpenURL(Configuration.documentationURL);
		}
		//Draw Pro Version Background
		GUI.DrawTexture(proPos,proBackground,ScaleMode.StretchToFill,true,0);
		
	}
	#endregion
	
	#region Setup UI
	/// <summary>
	/// Setups the Demo User Interface
	/// </summary>
	void setupUI(){
		//Draw iOS 7 Background
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background,ScaleMode.StretchToFill,true,0);
		
		//Draw Window
		var x = (Screen.width * 0.1f) / 2;
		var y = (Screen.height * 0.1f) / 2;
		var width = Screen.width - (Screen.width * 0.1f);
		var height = Screen.height - (Screen.height * 0.1f);
		GUI.DrawTexture(new Rect(x,y,width,height),window,ScaleMode.StretchToFill,true,0);
		
		//Draw Bottom Bar
		var heightBottomBar = 20.0f;
		var bottomBarOffset = 20.0f;
		GUI.DrawTexture(new Rect(x,height + bottomBarOffset,width,heightBottomBar),bottomBar,ScaleMode.StretchToFill,true,0);
		
		GUI.DrawTexture(logoPos,logo,ScaleMode.StretchToFill,true,0);
		
		var versionToWindowOffset = 30.0f;
		GUI.Label(new Rect(width - versionToWindowOffset,height - versionToWindowOffset,200.0f,50.0f),Configuration.Version,skin.label);
	}
	#endregion
	
	#region Game Center Actions
	/// <summary>
	/// Handles the game center options.
	/// </summary>
	void handleGameCenterOptions(){
		GUI.DrawTexture(gameCenterIconPos,gameCenterIcon,ScaleMode.StretchToFill,true,0);
		
		if(string.IsNullOrEmpty(Configuration.LeaderBoardID) || string.IsNullOrEmpty(Configuration.AchievementID))
			GUI.Label(gameCenterStatusUIPos,"Missing configuration",skin.label);
		else
			GUI.Label(gameCenterStatusUIPos,"Configured",skin.label);
		
		if(GUI.Button(loginGameCenterUIPos,"Game Center Login",skin.button)){
#if UNITY_EDITOR
		Debug.Log("You must be running from Simulator or Device");
#else
		gameCenter.Initialize();	
#endif
	
		}
		if(GUI.Button(showAchievementsUIPos,"Show Achievements",skin.button)){
#if UNITY_IPHONE
			Debug.Log("You must be running from Simulator or Device");
#else
			if(gameCenter.IsUserAuthenticated())
				gameCenter.ShowAchievementUI();
			else
				GUI.Label(gameCenterStatusUIPos,"You must authenticate!",skin.label);
#endif
		}
		if(GUI.Button(showLeaderboardUIPos,"Show Leaderboards",skin.button)){
#if UNITY_EDITOR
			Debug.Log("You must be running from Simulator or Device");
#else
			if(gameCenter.IsUserAuthenticated())
				gameCenter.ShowLeaderboardUI();
			else
				GUI.Label(gameCenterStatusUIPos,"You must authenticate!",skin.label);
#endif
		}
		if(GUI.Button(reportScoreUIPos,"Report Score",skin.button)){
#if UNITY_EDITOR
			Debug.Log("You must be running from Simulator or Device");
#else
			if(gameCenter.IsUserAuthenticated())
				gameCenter.ReportScore(2500);
			else
				GUI.Label(gameCenterStatusUIPos,"You must authenticate!",skin.label);
#endif
		}
		if(GUI.Button(reportAchievementProgressUIPos,"Report Achievement",skin.button)){
#if UNITY_EDITOR
				Debug.Log("You must be running from Simulator or Device");
#else
			if(gameCenter.IsUserAuthenticated())
				gameCenter.ReportAchievementProgress(Configuration.AchievementID,100.0f);
			else
				GUI.Label(gameCenterStatusUIPos,"You must authenticate!",skin.label);
#endif
		}
		
	}
	#endregion
	
	#region In App Actions
	void handleInAppPurchasesOptions(){
		GUI.DrawTexture(inAppPurchasesIconPos,inAppPurchasesIcon,ScaleMode.StretchToFill,true,0);
		
		if(Configuration.ProductIdentifiers == null || Configuration.ProductIdentifiers.Length == 0)
			GUI.Label(inAppPurchasesStatusUIPos,"Missing configuration",skin.label);
		else
			GUI.Label(inAppPurchasesStatusUIPos,"Configured",skin.label);
		
		if(GUI.Button(loadStorePos,"Load Store",skin.button)){
			Debug.Log("Pro Version Only see code comments");
			//Pro Only 
			//StoreManager.instance.LoadStore();
		}
		if(GUI.Button(purchaseProductPos,"Purchase Product",skin.button)){
			Debug.Log("Pro Version Only see code comments");
			//Pro Only 
			//if(StoreBinding.CanMakeStorePurchases())
			//	StoreBinding.PurchaseProduct("com.adegamestudio.eagleSpaceship");
		}
		if(GUI.Button(getProductInfoPos,"Get Product Info",skin.button)){
			Debug.Log("Pro Version Only see code comments");
			//Pro Only 	
			//if(StoreBinding.CanMakeStorePurchases())
			//StoreBinding.GetProductInfo("com.adegamestudio.eagleSpaceship");
		}
		if(GUI.Button(restoreInAppsPos,"Restore Purchases",skin.button)){
			Debug.Log("Pro Version Only see code comments");
			//Pro Only 
			//if(StoreBinding.CanMakeStorePurchases())
			//	StoreBinding.RestoreProducts();
		}
	}
	#endregion
	
	#region UIKit Actions
	void handleUIKitOptions(){
		GUI.DrawTexture(uiKitIconPos,uiKitIcon,ScaleMode.StretchToFill,true,0);
		
		if(GUI.Button(showBasicAlertPos,"Show Alert",skin.button)){
			Debug.Log("Pro Version Only see code comments");
			//Pro Only 
			//UIKit.ShowBasicAlert("Basic Alert","This is an example of a basic alert");
		}
		if(GUI.Button(showDoubleButtonsAlertPos,"Alert Buttons",skin.button)){
			Debug.Log("Pro Version Only see code comments");
			//Pro Only 
			//UIKit.ShowAlertWithOtherButton("Other Button Alert","This is an example of an alert with multiple buttons","Retry");
		}
			if(GUI.Button(openWebsitePos,"Open Website",skin.button)){
			Debug.Log("Pro Version Only see code comments");
			//Pro Only 
			//UIKit.OpenWebURL("http://www.imagitechdj.com");
		}
		GUI.DrawTexture(uiKitMediaPlayerIconPos,uiKitMediaPlayerIcon,ScaleMode.StretchToFill,true,0);
		
		if(GUI.Button(playVideo1Pos,"Play Video 1",skin.button)){
			Debug.Log("Pro Version Only see code comments");
			//Pro Only 
			//UIKit.PlayVideo("Video1.mp4");
		}
		if(GUI.Button(playVideo2Pos,"Play Video 2",skin.button)){
			Debug.Log("Pro Version Only see code comments");
			//Pro Only 
			//UIKit.PlayVideo("Video2.mp4");
		}
	}
	#endregion
}