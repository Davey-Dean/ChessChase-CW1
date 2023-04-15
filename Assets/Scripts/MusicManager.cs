using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Music Manager class adapted from: https://stackoverflow.com/questions/27911324/play-continuous-music-when-swapping-between-multiple-scene-in-unity3d
*/

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    public AudioClip winMusic;

    public AudioClip calmMusic;

    private AudioSource audioSource;

    private static MusicManager _instance;

    public static MusicManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<MusicManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake() {
        if(_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this);

            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = 0.05f;
        } else {
            if(this != _instance)
                Destroy(this.gameObject);
        }
    }

    public void PlayMenuMusic() {
        if (audioSource.clip != menuMusic) {
            audioSource.Stop();
            audioSource.clip = menuMusic;
            audioSource.Play();
        }
    }

    public void PlayGameMusic() {
        if (audioSource.clip != gameMusic) {
            audioSource.Stop();
            audioSource.clip = gameMusic;
            audioSource.Play();
        }
    }

    public void PlayWinMusic() {
        if (audioSource.clip != winMusic) {
            audioSource.Stop();
            audioSource.clip = winMusic;
            audioSource.Play();
        }
    }

    public void PlayCalmMusic() {
        if (audioSource.clip != calmMusic) {
            audioSource.Stop();
            audioSource.clip = calmMusic;
            audioSource.Play();
        }
    }
}
