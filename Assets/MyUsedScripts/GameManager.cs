using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public levelManager[] GameLevels;
    [HideInInspector]
    public bool _isGameComp = false;
    public static GameManager instance;
    [HideInInspector]
    levelManager _currentLevel;
    public GameUiManager _uiManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }
    private void Start()
    {
        AdsManager.instance.CloseBannerAd();
        InitializeLevel();
        AudioManager.instance.StopBG("Menu");
        AudioManager.instance.PlayBG("GameMusic");
    }

  
    void InitializeLevel()
    {
      GameObject level = Instantiate(GameLevels[LevelSelManager.levelNumber - 1].gameObject, this.transform.position, this.transform.rotation);
        _currentLevel = level.GetComponent<levelManager>();
        //enemiestokillchk = level.GetComponent<levelManager>()._TotalMoves;
        _uiManager.LevelNum.text = "LEVEL" + " " + (LevelSelManager.levelNumber).ToString();

    }

    public void CheckandDestroyPreviousScrew()
    {
        
    }

    public void CheckWinCondition()
    {
            _isGameComp = true;

            CurrencyManager.instance.AddCoins(50);
            if (PlayerPrefs.GetInt("lvlunlock" + LevelSelManager.levelNumber) == 0 && LevelSelManager.levelNumber < 10)
            {
                PlayerPrefs.SetInt("lvlunlock" + LevelSelManager.levelNumber, 1);
                PlayerPrefs.SetInt("lvlAlreadyPlayed" + (LevelSelManager.levelNumber - 1), 1);
                PlayerPrefs.SetInt("currentLevelToPlay", PlayerPrefs.GetInt("currentLevelToPlay") + 1);
                Debug.Log("LevelIncreased");             
            }

        Invoke("levelwinchk", 2f);
    }

    public void levelwinchk()
    {
        _uiManager.LevelCompPanel.SetActive(true);
    }
    public void CheckMoveCounter()
    {
        //enemiestokillchk--;
        //if (enemiestokillchk <= 0)
        
         StartCoroutine(levelFailCo());
        
    }
    IEnumerator levelFailCo()
    {
        
        yield return new WaitForSeconds(3);
        _uiManager.LevelFailedPanel.SetActive(true);
        if (!_isGameComp) 
        {

        }
    }

}
