using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    //player stats
    public float speed = 3.0f;
    public int maxHealth = 5;
    public GameObject Player;

    //win text
    public GameObject winText;
    public GameObject loseText;
    public TextMeshProUGUI FixText;

    //projectile
    public GameObject projectilePrefab;
    public TextMeshProUGUI ammoText;
    public int ammo { get { return currentAmmo; } }
    public static int currentAmmo;


    //sound clips
    public AudioClip throwSound;
    public AudioClip hitSound;

    public static int RFixed = 0;
    public int count;

    public int health { get { return currentHealth; } }
    public static int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    AudioSource audioSource;
    public ParticleSystem particle;
    public ParticleSystem dmgEffect;
    public ParticleSystem slowEffect;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        RFixed = 0;
        currentAmmo = 20;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        winText.SetActive(false);
        loseText.SetActive(false);

        fixRobotManager();
        LoseGameManager();
        AmmoCounter();

        if (currentHealth > 0)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(horizontal, vertical);

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }

            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);

            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                    isInvincible = false;
            }

            if (Input.GetKeyDown(KeyCode.C) && ammo > 0)
            {
                currentAmmo--;
                Launch();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
                if (hit.collider != null)
                {
                    if (RFixed >= 5 && SceneManager.GetActiveScene().name != "Level2")
                    {
                        SceneManager.LoadScene("Level2");
                    }
                    //NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    else//(character != null)
                    {
                        NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                        character.DisplayDialog();
                    }
                }
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
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            if (currentHealth > 0)
            {
                PlaySound(hitSound);
                dmgEffect.Play();
            }
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth <= 0)
        {
            loseText.SetActive(true);
        }
    }

    void Launch()
    {

        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayEffect(ParticleSystem effect)
    {
        particle.Play(effect);
    }

    public void SlowEffect(ParticleSystem Seffect)
    {
        if(speed == 1.5f)
        {
            slowEffect.Play(Seffect);
        }
        else
        {
            slowEffect.Stop(Seffect);
        }
    }

    public void fixRobotManager()
    {
        FixText.text = "Robots Fixed " + RFixed + "/5";

        if (RFixed >= 5 && SceneManager.GetActiveScene().name != "Main")
        {
            winText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
                RFixed = 0;
            }
        }
    }

    public void LoseGameManager()
    {
        if (currentHealth <= 0)
        {
            loseText.SetActive(true);
            //Player.SetActive(false);
            speed = 0f;

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
                RFixed = 0;
            }
        }
    }

    public void AmmoCounter()
    {
        ammoText.text = "Cogs: " + currentAmmo;
    }

    public void AmmoUp()
    {
        currentAmmo = currentAmmo + 5;
    }
}