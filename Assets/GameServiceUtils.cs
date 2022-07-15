using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GleyGameServices;
using UnityEngine.UI;
public class GameServiceUtils : MonoBehaviour
{
    public Image GameServiceButton;
    public Sprite GoogleAchievementButtonIcon;
    public Sprite GoogleLoginButtonIcon;
    // Start is called before the first frame update
    void Start()
    {

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {

            if (GameServices.Instance.IsLoggedIn())
            {
                UseAchievements();
            }
            else

            {
                RequiresLogin();
            }
        }
        else
        {
            GameServiceButton.gameObject.SetActive(false);
        }

    }

    public void UseAchievements()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GameServiceButton.sprite = GoogleAchievementButtonIcon;
        }
    }

    public void RequiresLogin()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GameServiceButton.sprite = GoogleLoginButtonIcon;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
