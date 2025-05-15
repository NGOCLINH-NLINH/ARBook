using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrexRoar : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip[] roarClips;

     void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Roar()
    {
        int Index = Random.Range(0, roarClips.Length);

        AudioClip clip = roarClips[Index];
        audioSource.PlayOneShot(clip);
    }


}
