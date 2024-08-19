using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    //[SerializeField] private TextAsset audioSheet;
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private AudioSource Oneshot;
    [SerializeField] private AudioSource Loop;
    [SerializeField] private AudioSource Ambi;
    [SerializeField] private AudioSource Mus;
    [SerializeField] private AudioSource UIs;

    [SerializeField] private AudioClip[] OneshotSFX;
    [SerializeField] private AudioClip[] LoopSFX;
    [SerializeField] private AudioClip[] Ambiance;
    [SerializeField] private AudioClip[] Music;
    [SerializeField] private AudioClip[] Ui;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Audio Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    //public void LoadAudio( string SceneName ){}

    private AudioClip FindClip(AudioClip[] source, string clipName)
    {
        foreach (AudioClip clip in source)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
        }
        Debug.LogError("No clip with this source and name found");
        return null;
    }


    public void PlayAudio(string AudioType, string AudioName, ulong delay = 0)
    {
        AudioClip clip;
        //Oneshots
        if (AudioType == "Oneshot")
        {
            clip = FindClip(OneshotSFX, AudioName);
            Oneshot.PlayOneShot(clip);
        }
        if (AudioType == "UI")
        {
            clip = FindClip(Ui, AudioName);
            UIs.PlayOneShot(clip);
        }

        //Plays
        if (AudioType == "Loop")
        {
            clip = FindClip(LoopSFX, AudioName);
            Loop.clip = clip;
            Loop.Play(delay);
        }
        if (AudioType == "Ambiance")
        {
            clip = FindClip(Ambiance, AudioName);
            Ambi.clip = clip;
            Ambi.Play(delay);
        }
        if (AudioType == "Music")
        {
            clip = FindClip(Music, AudioName);
            Mus.clip = clip;
            Mus.Play(delay);
        }

    }

    public void StopAudio(string AudioType)
    {
        if (AudioType == "Loop")
        {
            Loop.Stop();
        }
        if (AudioType == "Ambiance")
        {
            Ambi.Stop();
        }
        if (AudioType == "Music")
        {
            Mus.Stop();
        }
    }

    public void PauseUnpauseAudio(string AudioType, int mode = 1)
    {
        AudioSource audioSource = null;
        if (AudioType == "Loop") { audioSource = Loop; }
        else if (AudioType == "Ambiance") { audioSource = Ambi; }
        else if (AudioType == "Music") { audioSource = Mus; }

        if (audioSource == null) Debug.LogError("Not a Pausable audiotype");
        else
        {
            if (mode == 1) audioSource.Pause();
            if (mode == -1) audioSource.UnPause();
        }
    }

    public void SetAudioChange(string ChangeName, float volume)
    {
        audioMixer.SetFloat(ChangeName, volume);
    }
}
