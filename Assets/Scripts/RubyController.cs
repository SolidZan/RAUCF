using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public int projectileForce = 300;

    public float speed = 3.0f;
    public float timeInvincible = 2.0f;

    public int health { get { return currentHealth;}}
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 lookDirection = new Vector2(1,0);

    Rigidbody2D rigidbody2d;
    float horizontal; 
    float vertical;

    public GameObject projectilePrefab; 
    public ParticleSystem healEffect;
    public ParticleSystem damageEffect;

    AudioSource audioSource;
    public AudioClip playerHurtClip;
    public AudioClip throwCogClip;

    public int score;
    public TextMeshProUGUI scoreText;
    bool gameOver;

    public GameObject youWin;
    public GameObject youLose;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if(Input.GetButtonDown("Fire1") && !gameOver)
        {
            Launch();
        }

        if(Input.GetButtonDown("Fire2"))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (gameOver == true)
        {
            //spriteRenderer.enabled = false;
            speed = 0;
            isInvincible = true;

            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
            }
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            if (!isInvincible)
            {
                ParticleSystem damageParticle = Instantiate(damageEffect, GetComponent<Rigidbody2D>().position + Vector2.up * 0.5f, Quaternion.identity);
                PlaySound(playerHurtClip);
            }

            isInvincible = true;
            invincibleTimer = timeInvincible;

        }
        if (amount > 0)
        {
            ParticleSystem healParticle = Instantiate(healEffect, GetComponent<Rigidbody2D>().position + Vector2.up * 0.5f, Quaternion.identity);
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth == 0)
        {
            youLose.SetActive(true);
            gameOver = true;
        }
    }

    public void ChangeScore(int scoreAmount)
    {
        score = score + scoreAmount;
        scoreText.text = score.ToString();
        print(score);
        if (score == 4)
        {        
            youWin.SetActive(true);
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, GetComponent<Rigidbody2D>().position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, projectileForce);
        animator.SetTrigger("Launch");
        PlaySound(throwCogClip);
    }

}
