using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public Animator animator;
    public AudioSource startSound;

    public GameObject updatePanel;
    public GameObject clubPanel;
    public GameObject announcementPanel;
    public Image announcementImage;

    private enum Step
    {
        PressStart,
        Announcements,
        MainMenu,
        Settings
    }
    private Step step;

    private enum AnnouncementSubstep
    {
        Undefined,
        Update,
        Club,
        Posters
    }
    private AnnouncementSubstep announcementSubstep;

    void Start()
    {
        step = Step.PressStart;
        announcementSubstep = AnnouncementSubstep.Undefined;
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
            case Step.Announcements:
                {
                    UpdateAnnouncementStep();
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
            step = Step.Announcements;
        }
    }

    private void UpdateAnnouncementStep()
    {
        switch (announcementSubstep)
        {
            case AnnouncementSubstep.Undefined:
                {
                    // Newly started
                    updatePanel.SetActive(true);
                    updatePanel.GetComponent<UpdatePanel>().StartUpdate();
                    UpdatePanel.updateComplete += OnUpdateComplete;
                    announcementSubstep = AnnouncementSubstep.Update;
                }
                break;
            case AnnouncementSubstep.Update:
                {
                    // Wait for updates to complete
                }
                break;
            case AnnouncementSubstep.Club:
                break;
            case AnnouncementSubstep.Posters:
                break;
        }
    }

    private void OnUpdateComplete()
    {
        UpdatePanel.updateComplete -= OnUpdateComplete;
        updatePanel.SetActive(false);

        // Show club stuff
    }
}
