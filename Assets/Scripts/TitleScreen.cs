using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public Animator animator;
    public AudioSource startSound;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TitleScreenWaitForStart"))
        {
            // Waiting for start
            if (Input.GetKey(KeyCode.S) &&
                Input.GetKey(KeyCode.D) &&
                Input.GetKey(KeyCode.F) &&
                Input.GetKey(KeyCode.J) &&
                Input.GetKey(KeyCode.K) &&
                Input.GetKey(KeyCode.L))
            {
                animator.SetTrigger("PressedStart");
                startSound.Play();
            }
        }
    }
}
