
using UnityEngine;
using UnityEngine.UI;

public class BtnUiUpdater : MonoBehaviour
{
  
    public Button _thisBtn;
    [SerializeField]
    int LevelNum = 0;
    public GameObject Highlight;
    public bool locked = true;
    [SerializeField]
    LevelSelManager _levelSelManager;
    public Image ColorChangeImg;

    public void UpdateUI()
    {
        _levelSelManager.Select(LevelNum);
    }
   
}
