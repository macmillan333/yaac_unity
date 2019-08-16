using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public AudioSource containerDrop;
    public AudioSource containerOpen;
    public AudioSource getCard;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void PlayDropSound()
    {
        containerDrop.Play();
    }

    public void PlayOpenSound()
    {
        containerOpen.Play();
    }

    public void PlayGetCardSound()
    {
        getCard.Play();
    }
}
