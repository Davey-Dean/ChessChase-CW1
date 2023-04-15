using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    //private PlayerMovement player;

    public SpriteRenderer SR;
    public Sprite doorOpenSprite;

    public int NextScene;

    public float DoorDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
        }
    }
}
