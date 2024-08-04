using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splash : MonoBehaviour {
    public float waitTime = 1.0f;
    public Loading _loading;
    // Use this for initialization
    void Start () {
        StartCoroutine(waitandLoad());
	}

    IEnumerator waitandLoad()
    {
        yield return new WaitForSeconds(waitTime);
        _loading.LoadScene(1);
    }
    
}
