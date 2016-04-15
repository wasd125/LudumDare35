using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIAudioController : MonoBehaviour {

    public Slider MasterSlider;
    public Slider MusikSlider;
    public Slider AmbienceSlider;
    public Slider SoundEffectSlider;

    // Use this for initialization
    void Start () {
        MasterSlider.value = SoundManager.Instance.MasterVolume;
        MasterSlider.onValueChanged.AddListener(delegate { MasterVolumeChanged(); });
        MusikSlider.value = SoundManager.Instance.MusikVolume;
        MusikSlider.onValueChanged.AddListener(delegate { MusikVolumeChanged(); });
        AmbienceSlider.value = SoundManager.Instance.AmbienceVolume;
        AmbienceSlider.onValueChanged.AddListener(delegate { AmbienceVolumeChanged(); });
        SoundEffectSlider.value = SoundManager.Instance.SoundEffectVolume;
        SoundEffectSlider.onValueChanged.AddListener(delegate { SoundEffectVolumeChanged(); });
    }

    public void BackButtonPressed()
    {
        SoundManager.Instance.PlaySoundEffect(MenuItemController.Instance.SelectClip);
        MenuItemController.Instance.OptionsCanvas.gameObject.SetActive(false);
        MenuItemController.Instance.MainCanvas.gameObject.SetActive(true);
    }

    void MasterVolumeChanged()
    {
        SoundManager.Instance.MasterVolume = MasterSlider.value;
        SaveDataManager.Instance.SaveSoundData();
    }
    void MusikVolumeChanged()
    {
        SoundManager.Instance.MusikVolume = MusikSlider.value;
        SaveDataManager.Instance.SaveSoundData();
    }
    void AmbienceVolumeChanged()
    {
        SoundManager.Instance.AmbienceVolume = AmbienceSlider.value;
        SaveDataManager.Instance.SaveSoundData();
    }
    void SoundEffectVolumeChanged()
    {
        SoundManager.Instance.SoundEffectVolume = SoundEffectSlider.value;
        SaveDataManager.Instance.SaveSoundData();
    }

}
