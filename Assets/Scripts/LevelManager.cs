using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float respawnDelay;
    public PlayerMovement gamePlayer;

    public SpriteRenderer cpSR;
    public SpriteRenderer bpSR;

    public Sprite blockEnabled;
    public Sprite blockDisabled;

    public BoxCollider2D cpSRCollider;
    public BoxCollider2D bpSRCollider;
    public BoxCollider2D blockTrigger;

    public PlayerHealth playerhealth;

    public Canvas gameOverScreen;

    public Canvas levelCompleteScreen;

    public List<Enemy> firstSectionEnemies;

    public List<Enemy> secondSectionEnemies;

    private MusicManager musicManager;

    // Start is called before the first frame update
    void Start()
    {
        //gamePlayer = FindObjectOfType<PlayerMovement>();
        if (cpSRCollider) {
            cpSRCollider.enabled = false;
        }
        if (bpSRCollider) {
            bpSRCollider.enabled = false;
        }
        gameOverScreen.gameObject.SetActive(false);
        musicManager = MusicManager.Instance;
        musicManager.PlayGameMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver() {
        gameOverScreen.gameObject.SetActive(true);
        foreach (var enemy in firstSectionEnemies)
        {
            enemy.Disable();
        }
        foreach (var enemy in secondSectionEnemies)
        {
            enemy.Disable();
        }
    }
    
    public void LevelComplete() {
        if (!gameOverScreen.isActiveAndEnabled) {
            levelCompleteScreen.gameObject.SetActive(true);
            musicManager.PlayWinMusic();
            playerhealth.LevelComplete();
            foreach (var enemy in firstSectionEnemies)
            {
                enemy.Disable();
            }
            foreach (var enemy in secondSectionEnemies)
            {
                enemy.Disable();
            }
        }
    }

    public void Respawn() 
    {   
        gameOverScreen.gameObject.SetActive(false);
        if (!cpSRCollider.enabled) {
            foreach (var enemy in firstSectionEnemies)
            {
                enemy.Disable();
            }
        }
        
        if (bpSRCollider.enabled) {
            foreach (var enemy in secondSectionEnemies)
            {
                enemy.Disable();
            }
        }

        gamePlayer.gameObject.SetActive(false);
        bpSRCollider.enabled = false;
        blockTrigger.enabled = true;
        bpSR.sprite = blockDisabled;
        gamePlayer.transform.position = gamePlayer.respawnPoint;
        playerhealth.currentHealth = 3;
        playerhealth.gameObject.GetComponent<PlayerMovement>().enabled = true;
        gamePlayer.immune = false;
        gamePlayer.StopAllCoroutines();

        if (firstSectionEnemies.Count > 0) {
            firstSectionEnemies[0].grid.ResetGrid();
        }
        
        if (secondSectionEnemies.Count > 0) {
            secondSectionEnemies[0].grid.ResetGrid();
        }

        if (!cpSRCollider.enabled) {
            foreach (var enemy in firstSectionEnemies)
            {
                enemy.Enable();
            }
        }

        gamePlayer.gameObject.SetActive(true);
    }

    public void Checkpoint()
    {
        cpSR.sprite = blockEnabled;
        cpSRCollider.enabled = true;
        musicManager.PlayCalmMusic();
        foreach (var enemy in firstSectionEnemies)
        {
            enemy.Disable();
        }
    }

    public void Blockpoint()
    {   
        bpSR.sprite = blockEnabled;
        bpSRCollider.enabled = true;
        musicManager.PlayGameMusic();
        foreach (var enemy in secondSectionEnemies) {
            enemy.Enable();
        }
    }

}
