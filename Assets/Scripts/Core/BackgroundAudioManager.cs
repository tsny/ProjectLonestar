using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BackgroundAudioManager : MonoBehaviour
{
    public BackgroundAudioManager instance;
    private AudioSource audioSource;

    // So this class needs to listen to changes in the player's environment
    // i.e, playing spooky music in nebulas and serene music on planets
    private void Awake()
    {
        name = "BACKGROUND AUDIO";
    }

    public void ChangeBackgroundAudio()
    {

    }

    public void ChangeBackgroundMusic()
    {

    }
}
