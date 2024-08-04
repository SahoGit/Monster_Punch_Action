using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameUiManager : MonoBehaviour
{
    public GameObject LevelCompPanel,LevelFailedPanel;
    public Text LevelNum;
    public Loading loadingPanel;
    // Start is called before the first frame update
    public void HomeBtn()
    {
        AudioManager.instance.PlaySFX("Btnclick");
        loadingPanel.LoadScene(1);
        AdsManager.instance.ShowInterstitialAd();
    }
    public void Restart()
    {

        loadingPanel.LoadScene(2);
    }
    public void Next()
    {
        AudioManager.instance.PlaySFX("Btnclick2");
      
    }
}
