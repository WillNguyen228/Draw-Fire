using UnityEngine;
using UnityEngine.UI;

public class ManageSoundEffects : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider sfslider;
    void Start() {
        float vol;
        if (PlayerPrefs.HasKey("soundEffectToggle")) {
            vol = PlayerPrefs.GetFloat("soundEffectToggle");
        } else {
            vol = 1f;
            PlayerPrefs.SetFloat("soundEffectToggle", 1f);
        }

        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).GetComponent<AudioSource>().volume = vol;
        }
    }

    public void AdjustVolume() {
        PlayerPrefs.SetFloat("soundEffectToggle", sfslider.value);
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).GetComponent<AudioSource>().volume = sfslider.value;
        }
    }

}