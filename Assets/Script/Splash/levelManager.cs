using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    public int enemiestokill;
    public int enemykilled;
    // Start is called before the first frame update
    public void EnemyKilled()
    {
        enemiestokill--;
        Invoke("LevelLaod", 1.5f);
        // Check if all enemies are killed
       
    }

    public void LevelLaod()
    {
        if (enemiestokill <= 0)
        {
            AudioManager.instance.PlaySFX("Win");
            if (LevelSelManager.levelNumber < 10)
            {
                LevelSelManager.levelNumber++;
            }
            else
            {
                LevelSelManager.levelNumber = 1;
            }

            SceneManager.LoadScene(2);
            Time.timeScale = 1;
        }
    }

}
