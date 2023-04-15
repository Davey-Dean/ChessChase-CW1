using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{   
    public AudioClip hitSound;
    public AudioClip collectSound;
    public AudioClip loseSound;
    public void PlayHitSound(Vector3 position) {
        AudioSource.PlayClipAtPoint(hitSound, position);
    }

    public void PlayCollectSound(Vector3 position) {
        AudioSource.PlayClipAtPoint(collectSound, position);
    }

    public void PlayLoseSound(Vector3 position) {
        AudioSource.PlayClipAtPoint(loseSound, position);
    }
}
