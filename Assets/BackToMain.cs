using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour {

    void Start()
    {
        SoundManager.Instance.StopMusik();
        SoundManager.Instance.PlayMusik();
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
