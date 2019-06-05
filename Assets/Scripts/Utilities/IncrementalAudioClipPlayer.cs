using UnityEngine;

public class IncrementalAudioClipPlayer : MonoBehaviour 
{
    public AudioSource audioSource;
    public AudioClip[] clipsToPlay;

    public bool HasFinished {get; private set;}

    public bool isPlaying;

    private int currentClipIndex = 0;

    void Update() 
    {
        var shouldReturn = (!isPlaying || clipsToPlay.Length == 0 || !audioSource.isPlaying);
        if (shouldReturn) return;

        if (currentClipIndex == clipsToPlay.Length)
        {
            ResetPlayback();
            return;
        }

        currentClipIndex++;
        audioSource.PlayOneShot(clipsToPlay[currentClipIndex]);
    }

    public void PlayFromBeginning()
    {
        ResetPlayback();
        isPlaying = true;
    }

    public void ResetPlayback()
    {
        currentClipIndex = 0;
        HasFinished = false;
    }
}