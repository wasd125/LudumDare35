using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

    public static LevelController Instance;
    public AudioClip clip;
    public List<PulsingObject> PulseObjects { get; private set; }

    void Awake()
    {
        PulseObjects = new List<PulsingObject>();

        // First we check if there are any other instances conflicting
        if (Instance == null)
        {
            // If that is the case, we destroy other instances
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        SoundManager.Instance.PlayMusik(clip);
    }

    public void ResgisterPulsingObject(PulsingObject obj)
    {
        PulseObjects.Add(obj);
    }

    public void LevelCompleted()
    {
        UnregisterAllPulsingObjects();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void PlayerDied()
    {
        UnregisterAllPulsingObjects();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SoundManager.Instance.PlayMusik(clip);
    }

    public void UnregisterAllPulsingObjects()
    {
        for (int i = PulseObjects.Count -1 ; i >= 0; i--)
        {
            UnregisterPulsingObject(PulseObjects[i]);
        }
    }

    public void UnregisterPulsingObject(PulsingObject obj)
    {
        obj.Unregister();
        PulseObjects.Remove(obj);
    }

}
