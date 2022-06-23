using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite soundOn, soundOff;
    public float maxVolume = 1f;
    private bool muted = false;

    public void ClickMuteButton(){
        
        muted = !muted;

        float volume = muted ? 0 : maxVolume;
        SoundController.Instance.SetVolume(volume);

        if(muted) image.sprite = soundOff;
        else image.sprite = soundOn;
    }
}
