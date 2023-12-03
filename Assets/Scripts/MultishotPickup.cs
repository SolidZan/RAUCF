using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultishotPickup : MonoBehaviour
{

    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other) //Multishot code done by Alex Martinez
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.Upgrade();
            Destroy(gameObject);

            controller.PlaySound(collectedClip); //Multishot pickup code done by Alex Martinez
        }
    }
}
