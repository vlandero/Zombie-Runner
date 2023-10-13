using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource mainAudio;
    public AudioSource engagedAudio;

    private float mainAudioOriginalVolume;
    private float engagedAudioOriginalVolume;

    private bool isEngaged = false;

    private void Start()
    { 
        mainAudioOriginalVolume = mainAudio.volume;
        engagedAudioOriginalVolume = engagedAudio.volume;
        engagedAudio.volume = 0f;
    }

    private void Update()
    {
        bool newEngagedState = CheckIfPlayerIsEngaged();

        if (newEngagedState != isEngaged)
        {
            isEngaged = newEngagedState;

            if (isEngaged)
            {
                mainAudio.volume = 0;
                engagedAudio.volume = engagedAudioOriginalVolume;
            }
            else
            {
                mainAudio.volume = mainAudioOriginalVolume;
                engagedAudio.volume = 0f;
            }
        }
    }

    private bool CheckIfPlayerIsEngaged()
    {
        return PlayerManager.instance.engagedZombies > 0;
    }
}
