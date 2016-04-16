using UnityEngine;
using System.Collections;

public class PulseManagerHardCoded : MonoBehaviour {

    public static PulseManagerHardCoded Instance;
    public AudioClip clip;
    public SpriteRenderer rend;

    const float MAX_PULSE_TIMER = 0.481f;
    float currentPulseTimer = 0f;

    Color[] colors = new Color[] { Color.black, Color.blue, Color.green, Color.white,Color.red,Color.yellow,Color.gray };

    void Awake()
    {
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
    }

    void Start()
    {
        SoundManager.Instance.PlayMusik(clip);
    }

	// Update is called once per frame
	void Update () {

        currentPulseTimer -= Time.deltaTime;

        if (currentPulseTimer <= 0)
        {
            rend.color = colors[Random.Range(0, colors.Length - 1)];
            currentPulseTimer = MAX_PULSE_TIMER;
            GetComponent<Animator>().SetTrigger("Pulse");
        }
	}
}
