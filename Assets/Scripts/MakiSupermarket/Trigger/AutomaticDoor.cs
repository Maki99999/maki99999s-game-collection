using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    public AudioSource audioSource;
    public Animator anim;
    
    public void ForceOpen(bool open)
    {
        audioSource.Play();
        anim.SetBool("Open", open);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            anim.SetBool("Open", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            anim.SetBool("Open", false);
        }
    }
}
