using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashListener : MonoBehaviour
{
    public AudioSource splash;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            splash.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            splash.Play();
        }
    }
}
