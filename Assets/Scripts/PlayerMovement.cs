using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    public float turnSpeed;

    private Rigidbody2D rb;
    public Animator animator;
    public PlayerHealth playerHealth;

    public Color flashColour;
    public Color regColour;
    public float flashDuration;
    public float numFlashes;
    public bool immune = false;
    public SpriteRenderer playerSprite;

    public GameObject healParticle;
    public GameObject cpText;

    private float horizontalInput;
    private float verticalInput;

    public Vector3 respawnPoint;
    public LevelManager gameLevelManager;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        gameLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", horizontalInput);
        animator.SetFloat("Vertical", verticalInput);
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
    }

    void FixedUpdate() {
        float hComponent = 0;
        float vComponent = 0;

        float stopThreshold = deceleration / 100f;

        if (horizontalInput != 0) {
            //turn around with different speed
            if (horizontalInput > 0 && rb.velocity.x < 0) {
                hComponent = turnSpeed;
            } else if (horizontalInput < 0 && rb.velocity.x > 0) {
                hComponent = turnSpeed * -1;
            } else {
                hComponent = acceleration * horizontalInput;
            }
        } else if (Mathf.Abs(rb.velocity.x) >= stopThreshold) {
            //only decelerate if above a certain velocity
            if (rb.velocity.x > 0) {
                hComponent = deceleration * -1;
            } else if (rb.velocity.x < 0) {
                hComponent = deceleration;
            }
        } else {
            //set velocity component to 0 if under a small enough speed
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (verticalInput != 0) {
            //turn around with different speed
            if (verticalInput > 0 && rb.velocity.y < 0) {
                vComponent = turnSpeed;
            } else if (verticalInput < 0 && rb.velocity.y > 0) {
                vComponent = turnSpeed * -1;
            } else {
                vComponent = acceleration * verticalInput;
            }
        } else if (Mathf.Abs(rb.velocity.y) >= stopThreshold) {
            //only decelerate if above a certain velocity
            if (rb.velocity.y > 0) {
                vComponent = deceleration * -1;
            } else if (rb.velocity.y < 0) {
                vComponent = deceleration;
            }
        } else {
            //set velocity component to 0 if under a small enough speed
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        rb.velocity += Time.fixedDeltaTime * new Vector2(hComponent, vComponent);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);        
    }

    public void Stop() {
        rb.velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Checkpoint")
        {
            respawnPoint = other.transform.position;
            Instantiate(cpText, this.transform.position, this.transform.rotation);
            gameLevelManager.Checkpoint();
            other.enabled = false;
        }

        if (other.tag == "Blockpoint") 
        {
            gameLevelManager.Blockpoint();
            other.enabled = false; // current bug, this needs to be reset to true on respawn
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && immune == false)
        {
            if (playerHealth.currentHealth == 1)
            {
                playerHealth.TakeDamage(1);
            }
            else
            {
                playerHealth.TakeDamage(1);
                StartCoroutine(Invulnerability());
            }      
        } else if (other.gameObject.CompareTag("King")) {
            this.enabled = false;
            immune = true;
            gameLevelManager.LevelComplete();
        }
        if (other.gameObject.tag == "Health")
        {
            playerHealth.Heal(3);
            Instantiate(healParticle, this.transform.position, this.transform.rotation);
            other.gameObject.SetActive(false);
            var audio = gameObject.GetComponent<PlayerAudio>();
            if (audio) {
                audio.PlayCollectSound(transform.position);
            }
        }
    }
    private IEnumerator Invulnerability()
    {
        int count = 0;
        immune = true;
        while (count < numFlashes)
        {
            playerSprite.color = flashColour;
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = regColour;
            yield return new WaitForSeconds(flashDuration);
            count++;
        }
        immune = false;
    }
}
