using UnityEngine;
using System.Collections;

public static class Configuration {
	
	#region Game Center Configuration
	public static string LeaderBoardID = "";
	public static string AchievementID = "";
	
	#endregion
	
	#region In App Purchases Configuration
	//Available in pro version only
	//Download it at https://www.assetstore.unity3d.com/#/content/12342
	public static string[] ProductIdentifiers = 
	{
		//Example
		//"com.yourdomain.productID", 
		//"com.yourdomain.productID"
	};
	
	#endregion
	
	public static string Version = "1.0.0";
	public static string documentationURL = "http://www.imagitechdj.com/ios-sdk-unity/";
	
}
