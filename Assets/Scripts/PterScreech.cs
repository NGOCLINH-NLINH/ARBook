using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterScreech : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip[] screechClips;

     void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Screech()
    {
        int Index = Random.Range(0, screechClips.Length);

        AudioClip clip = screechClips[Index];
        audioSource.PlayOneShot(clip);
    }


}
