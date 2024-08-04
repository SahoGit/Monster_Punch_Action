using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameObject loadingPanel;
    public Image sliderBar;
    public Text loadingText;
    public float waitTime = 0.2f;
    [SerializeField]
    private string[] _tips;
    [SerializeField]
    private Text _tipsText;

    public void LoadScene(int sceneNum)
    {
        loadingPanel.SetActive(true);

        StartCoroutine(LoadNewScene(sceneNum));
    }
    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene(int sceneNum)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNum);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            sliderBar.fillAmount = progress;

            if (loadingText)
                loadingText.text = "" + (int)(progress * 100) + "%";

            if (progress >= 0.9f /*&& AdsManager.instance.canStartGame*/)
            {
                // Load the scene
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

    }
}
