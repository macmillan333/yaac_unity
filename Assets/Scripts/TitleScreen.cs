using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public Animator animator;
    public AudioSource startSound;

    private enum Step
    {
        PressStart,
        Notifications,
        MainMenu,
        Settings
    }
    private Step step;

    void Start()
    {
        step = Step.PressStart;
    }
    
    void Update()
    {
        switch (step)
        {
            case Step.PressStart:
                {
                    UpdatePressStartStep();
                }
                break;
            case Step.Notifications:
                {
                    Debug.Log("Showing notifications.");
                }
                break;
            case Step.MainMenu:
                {

                }
                break;
            case Step.Settings:
                {

                }
                break;
        }
        
    }

    private void UpdatePressStartStep()
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TitleScreenIdle"))
        {
            step = Step.Notifications;
        }
    }
}
