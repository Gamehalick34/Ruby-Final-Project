using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
   ParticleSystem SEffect;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        controller.speed = 1.5f;
        controller.SlowEffect(SEffect);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        controller.speed = 3f;
        controller.SlowEffect(SEffect);
    }
}
