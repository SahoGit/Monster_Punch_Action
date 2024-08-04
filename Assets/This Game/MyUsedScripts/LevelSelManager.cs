using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelManager : MonoBehaviour
{

    public BtnUiUpdater[] levelButtons;
    public static int levelNumber;
    public Color OrangeColor;
    [SerializeField]
    Loading _loading;

    void OnEnable()
    {

        foreach (var item in levelButtons)
        {
            item.Highlight.SetActive(false);
        }

        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.SetInt("lvlunlock" + 0, 1);
            PlayerPrefs.SetInt("currentLevelToPlay", 1);
        }
        int levelReached = PlayerPrefs.GetInt("currentLevelToPlay", 1);
        //UpdateLevelBtns();
        Select(levelReached);
    }

    public void Start()
    {
        //AdsManager.instance.RequestBanner();
        AudioManager.instance.PlayBG("Menu");
        AudioManager.instance.StopBG("GameMusic");
    }

    public void ADD1000Coins()
    {
        CurrencyManager.instance.AddCoins(1000);
    }

    public void RewardedCOins()
    {
        AdsManager.instance.ShowRewardAdWithDelegate(ADD1000Coins);
    }

    void UpdateLevelBtns()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("lvlunlock" + i) == 0)
            {
                levelButtons[i]._thisBtn.interactable = false;
                levelButtons[i].locked = true;
                levelButtons[i].ColorChangeImg.color = Color.gray;


            }
            else if (PlayerPrefs.GetInt("lvlunlock" + i) == 1 && PlayerPrefs.GetInt("lvlAlreadyPlayed" + i) == 0)
            {
                levelButtons[i].locked = false;
                levelButtons[i]._thisBtn.interactable = true;
                levelButtons[i].ColorChangeImg.color = OrangeColor;

            }
            else if (PlayerPrefs.GetInt("lvlunlock" + i) == 1 && PlayerPrefs.GetInt("lvlAlreadyPlayed" + i) == 1)
            {
                levelButtons[i].locked = false;
                levelButtons[i]._thisBtn.interactable = true;
                levelButtons[i].ColorChangeImg.color = Color.green;
            }

        }
    }
    public void Select(int _levelNumber)
    {
        //  Debug.LogError("dsD|SAda");
        levelNumber = _levelNumber;
       // UpdateBtnsUI(levelNumber);
    }
    void UpdateBtnsUI(int _selectedLvlNum)
    {

        foreach (var item in levelButtons)
        {
            item.Highlight.SetActive(false);
        }
        levelButtons[_selectedLvlNum - 1].Highlight.SetActive(true);
    }

    public void StartSelectedLevel()
    {
        _loading.LoadScene(2);

    }

  
}
