using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    AudioSource _audioSource;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("volume")) _musicSlider.value = PlayerPrefs.GetFloat("volume");
    }
    public void UpdateVolume()
    {
        _audioSource.volume = _musicSlider.value;
        PlayerPrefs.SetFloat("volume", _musicSlider.value);
    }
}
