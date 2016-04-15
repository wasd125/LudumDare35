using UnityEngine;
using System.Collections;

public class SoundTester : MonoBehaviour {

    public AudioClip clip;

	// Use this for initialization
	void Start () {
        play();
        //Invoke("fade", 10f);
    }


    void play()
    {
        SoundManager.Instance.PlayMusik(clip);
    }

    void fade()
    {
        SoundManager.Instance.FadeMusik(0.5f, 1f);
    }

}
