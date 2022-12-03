using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperHardEnemy : MonoBehaviour
{
    public float speed;
    public bool OtherSide;
    public float changeTime = 3.0f;
    public static int health = 2;
    public int seeNumber;
    public int count = 1;
    public AudioSource Noise;
    public AudioClip Repair;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;

    Animator animator;
    public ParticleSystem smokeEffect;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        Noise = GetComponent<AudioSource>();
    }

    void Update()
    {
        seeNumber = health;

        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;       
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
            count--;
            if(count == 0 && OtherSide == false)
            {
                OtherSide = true;
                count = 2;
            }
            if(count == 0 && OtherSide == true)
            {
                OtherSide = false;
                count = 2;
            }
        }
    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;
        if(OtherSide == false)
        {
        position.y = position.y + Time.deltaTime * speed * direction;
        position.x = position.x + Time.deltaTime * speed * direction;            
        animator.SetFloat("Move X", direction);
        animator.SetFloat("Move Y", direction);
        }
        else
        {
        position.y = position.y + Time.deltaTime * speed * direction;
        position.x = position.x + Time.deltaTime * speed * -direction;            
        animator.SetFloat("Move X", direction);
        animator.SetFloat("Move Y", direction);
        }

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-3);
        }
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        if(health > 0)
        {
            health--;
        }
        else
        {
            broken = false;
            rigidbody2D.simulated = false;
            //optional if you added the fixed animation
            animator.SetTrigger("Fixed");
            smokeEffect.Stop();
            PlaySound(Repair);

            RubyController.RFixed += 1;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        Noise.PlayOneShot(clip);
    }
}
