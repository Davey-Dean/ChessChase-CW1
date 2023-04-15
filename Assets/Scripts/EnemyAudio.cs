using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip reserveSound;
    public AudioClip attackSound;

    public void PlayReservePathSound(Vector3 position) {
        AudioSource.PlayClipAtPoint(reserveSound, position);
    }

    public void PlayAttackSound(Vector3 position) {
        AudioSource.PlayClipAtPoint(attackSound, position);
    }
}
