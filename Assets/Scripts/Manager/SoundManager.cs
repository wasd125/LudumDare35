using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundManager : MonoBehaviour {

    #region Member Variablen

    private enum EnumFadingState { Idle, FadingIn, FadingOut }

    private EnumFadingState FadingState;

    private float FadingVolume { get; set; }
    private float FadingAmount { get; set; }

    public AudioSource BackgroundMusicSource { get; private set; }

    private List<AudioSource> AmbienceSources;
    private List<AudioSource> SoundEffectSources;

    private float masterVolume, musikVolume, ambienceVolume, soundEffectVolume;
    public static SoundManager Instance;

    public float MasterVolume
    {
        get { return masterVolume; }
        set
        {
            masterVolume = value;

            SetMusikVolume(masterVolume * MusikVolume);
            SetAmbienceVolume(masterVolume * AmbienceVolume);
            SetSoundEffectsVolume(masterVolume * SoundEffectVolume);
        }
    }
    public float MusikVolume
    {
        get { return musikVolume; }
        set
        {
            musikVolume = value;

            SetMusikVolume(MasterVolume * musikVolume);
        }
    }
    public float AmbienceVolume
    {
        get { return ambienceVolume; }
        set
        {
            ambienceVolume = value;

            SetAmbienceVolume(MasterVolume * ambienceVolume);
        }
    }
    public float SoundEffectVolume
    {
        get { return soundEffectVolume; }
        set
        {
            soundEffectVolume = value;

            SetSoundEffectsVolume(MasterVolume * soundEffectVolume);
        }
    }
    #endregion

    void Awake() {
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

        Init();
    }
    void Update()
    {
        Fade();
    }

    /// <summary>
    /// Gibt eine AudioSource aus der Liste zurück welche gerade kein Clip abspielt. Sofern keine vorhanden ist wird eine neue Audiosource initialisiert und der Liste hinzugefügt
    /// </summary>
    /// <param name="list">Liste aus welcher die freie AudioSource gesucht werden soll</param>
    /// <returns></returns>
    private AudioSource GetFreeAudioSourceFromList(List<AudioSource> list)
    {
        // Hier prüfen wir ob eine freie AudioSource zur Verfügung steht
        // Falls nicht erstellen wir eine neue und fügen sie der Liste hinzu
        if (list.Where(p => p.isPlaying == false).ToList().Count < 1)
        {
            AudioSource source = InitAudioSource();
            list.Add(source);
            return source;
        }
        else
        {
            return list.Where(p => p.isPlaying == false).FirstOrDefault();
        }
    }
    /// <summary>
    /// Initialisiert eine AudioSource
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clip"></param>
    /// <param name="pitch"></param>
    /// <param name="playOnAwake"></param>
    /// <param name="loop"></param>
    /// <returns></returns>
    private AudioSource InitAudioSource(AudioSource source = null, AudioClip clip = null, float pitch = 1f, bool playOnAwake = false, bool loop = false)
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = playOnAwake;
        source.loop = loop;
        source.pitch = pitch;
        source.clip = clip;

        return source;
    }

    /// <summary>
    /// Wird einmal beim Start ausgeführt und initialisiert alle Variablen
    /// </summary>
    private void Init()
    {
        FadingState = EnumFadingState.Idle;
        BackgroundMusicSource = InitAudioSource(null, null, 1, false, true);

        // Hier fügen wir 3 Audiosourcen der SoundEffekt Liste hinzu
        SoundEffectSources = new List<AudioSource>();
        for (int i = 0; i < 3; i++)
        {
            AudioSource source = InitAudioSource();
            SoundEffectSources.Add(source);
        }

        // Hier fügen wir 3 Audiosourcen der AmbienceSourcenListe hinzu
        AmbienceSources = new List<AudioSource>();
        for (int i = 0; i < 3; i++)
        {
            AudioSource source = InitAudioSource(null, null, 1, false, true);
            AmbienceSources.Add(source);
        }

        SaveDataManager.Instance.LoadSoundData();
    }
    /// <summary>
    /// Läuft mit der Update-Funktion wird dazu verwendet die Hintergrundmusik ein oder auszublenden
    /// </summary>
    private void Fade()
    {
        if (FadingState != EnumFadingState.Idle)
        {
            BackgroundMusicSource.volume -= FadingAmount * Time.deltaTime;

            // Wenn wir die gewünschte Lautstärke erreicht haben setzen wir den Status auf idle 
            // und die Lautstärke auf FadingVolume für den fall das wir über das Ziel hinausgeschossen sind
            if ((FadingState == EnumFadingState.FadingOut && BackgroundMusicSource.volume <= FadingVolume) ||
                (FadingState == EnumFadingState.FadingIn && BackgroundMusicSource.volume >= FadingVolume))
            {
                FadingState = EnumFadingState.Idle;
                BackgroundMusicSource.volume = FadingVolume;
            }
        }
    }
    /// <summary>
    /// Stoppt alle abspielenden AudioSourcen in einer Liste
    /// </summary>
    /// <param name="list">Liste in der die AudioSourcen stoppen sollen</param>
    private void StopAllAudiosInList(List<AudioSource> list)
    {
        foreach (AudioSource item in list.Where(p => p.isPlaying == true).ToList())
        {
            item.Stop();
        }
    }
    /// <summary>
    /// Setzt die Lautstärke der AudioSource auf das angegebene Level
    /// </summary>
    /// <param name="sources"></param>
    /// <param name="volume"></param>
    private void SetVolume(List<AudioSource> sources, float volume = 1f)
    {
        foreach (AudioSource source in sources)
        {
            source.volume = volume;
        }
    }    

    #region Ambience
    /// <summary>
    /// Spielt einen Clip als Ambience ab
    /// </summary>
    /// <param name="clip">Abzuspielender Clip</param>
    /// <param name="volume">Lautstärke</param>
    public void PlayAmbience(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        AudioSource source = GetFreeAudioSourceFromList(AmbienceSources);
        source.loop = loop;
        source.clip = clip;
        source.Play();
       
    }
    /// <summary>
    /// Stoppt alle Ambience Sounds
    /// </summary>
    public void StopAllAmbience()
    {
        StopAllAudiosInList(AmbienceSources);
    }
    /// <summary>
    /// Setzt die Ambiencelautstärke auf das angegebene Level
    /// </summary>
    /// <param name="volume"></param>
    private void SetAmbienceVolume(float volume)
    {
        SetVolume(AmbienceSources,volume);
    }
    #endregion

    #region SoundEffect
    /// <summary>
    /// Spielt einen SoundEffekt ab
    /// </summary>
    /// <param name="clip">SoundEffekt</param>
    /// <param name="minPitch">Minwert für den Pitch</param>
    /// <param name="maxPitch">Maxwert für den Pitch</param>
    public void PlaySoundEffect(AudioClip clip, float minPitch, float maxPitch)
    {
        float pitch = Random.Range(minPitch, maxPitch);
        PlaySoundEffect(clip, pitch);
    }
    /// <summary>
    /// Spielt einen SoundEffekt ab
    /// </summary>
    /// <param name="clip">SoundEffekt</param>
    /// <param name="pitch"></param>
    public void PlaySoundEffect(AudioClip clip, float pitch = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetFreeAudioSourceFromList(SoundEffectSources);

        source.pitch = pitch;
        source.clip = clip;
        source.Play();
    }
    /// <summary>
    /// Stoppt alle Soundeffekte
    /// </summary>
    public void StopAllSoundEffects()
    {
        StopAllAudiosInList(SoundEffectSources);
    }
    /// <summary>
    /// Setzt die SoundEffectlautstärke auf das angegebene Level
    /// </summary>
    /// <param name="volume"></param>
    private void SetSoundEffectsVolume(float volume)
    {
        SetVolume(SoundEffectSources,volume);
    }
    #endregion

    #region Musik
    /// <summary>
    /// Spielt den gespeichtern Musik Clip ab
    /// </summary>
    public void PlayMusik()
    {
        if (BackgroundMusicSource.clip == null) return;

        BackgroundMusicSource.loop = true;
        BackgroundMusicSource.Play();
    }
    /// <summary>
    /// Spielt den Musikclip ab
    /// </summary>
    /// <param name="clip"></param>
    public void PlayMusik(AudioClip clip)
    {
        if (clip == null) return;

        BackgroundMusicSource.clip = clip;
        BackgroundMusicSource.loop = true;
        BackgroundMusicSource.Play();
    }
    /// <summary>
    /// Stop den MusikClip
    /// </summary>
    public void StopMusik()
    {
        BackgroundMusicSource.Pause();
    }
    /// <summary>
    /// Lässt die Musiklautstärke zu den Wert fadingVolume übergehen
    /// </summary>
    /// <param name="fadingVolume"></param>
    /// <param name="fadingTime"></param>
    public void FadeMusik(float fadingVolume,float fadingTime)
    {
        FadingVolume = MasterVolume * fadingVolume;
        FadingAmount = ((BackgroundMusicSource.volume - FadingVolume) * MasterVolume)  / fadingTime;

        if (FadingAmount > 0)
        {
            FadingState = EnumFadingState.FadingOut;
        }
        else if (FadingAmount < 0)
        {
            FadingState = EnumFadingState.FadingIn;
        }
    }
    /// <summary>
    /// Setzt die Musiklautstärke auf das angegebene Level
    /// </summary>
    /// <param name="volume"></param>
    private void SetMusikVolume(float volume)
    {
        List<AudioSource> musikSourceList = new List<AudioSource>();
        musikSourceList.Add(BackgroundMusicSource);
        SetVolume(musikSourceList,volume);
    }
    #endregion
}
