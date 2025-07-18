using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider bgmslider;
    float vol;
    void Start() {
        if (PlayerPrefs.HasKey("BGMvol")) {
            vol = PlayerPrefs.GetFloat("BGMvol");
        } else {
            PlayerPrefs.SetFloat("BGMvol", 1f);
            vol = 1f;
        }

        GetComponent<AudioSource>().volume = vol;
    }

    public void AdjustVolume() {
        PlayerPrefs.SetFloat("BGMvol", bgmslider.value); //This is required to save the preference in different scenes
        GetComponent<AudioSource>().volume = bgmslider.value;
    }
}