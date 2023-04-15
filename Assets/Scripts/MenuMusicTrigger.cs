using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicTrigger : MonoBehaviour
{
    void Start() {
        MusicManager.Instance.PlayMenuMusic();
    }
}
