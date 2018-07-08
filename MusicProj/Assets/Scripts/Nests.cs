using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nests : MonoBehaviour
{

    public AudioSource aud;

    public AudioClip nest1,
        nest2;

    // Use this for initialization
    void Start()
    {
        aud = GameObject.FindGameObjectWithTag("FX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(1, 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (aud.isPlaying)
            {

            }
            else
            {
                aud.clip = nest1;
                aud.Play();

            }
        }
    }

}
