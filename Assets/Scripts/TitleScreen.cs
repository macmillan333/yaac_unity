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
    public List<Sprite> posters;
    private int posterIndex;

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
        if (announcementSubstep == AnnouncementSubstep.Undefined)
        {
            // Newly started
            StartCoroutine(WaitAndShowUpdatePanel());
            announcementSubstep = AnnouncementSubstep.Update;
        }
    }

    private IEnumerator WaitAndShowUpdatePanel()
    {
        yield return new WaitForSeconds(0.5f);
        updatePanel.SetActive(true);
        updatePanel.GetComponent<UpdatePanel>().StartUpdate();
        UpdatePanel.updateComplete += OnUpdateComplete;
    }

    private void OnUpdateComplete()
    {
        UpdatePanel.updateComplete -= OnUpdateComplete;
        updatePanel.SetActive(false);

        StartCoroutine(WaitAndShowClubPanel());
        announcementSubstep = AnnouncementSubstep.Club;
    }

    private IEnumerator WaitAndShowClubPanel()
    {
        yield return new WaitForSeconds(0.5f);
        clubPanel.SetActive(true);
    }

    public void OnNoThanksClicked()
    {
        clubPanel.SetActive(false);

        StartCoroutine(WaitAndShowNextAnnouncement());
        announcementSubstep = AnnouncementSubstep.Posters;
        posterIndex = 0;
    }

    private IEnumerator WaitAndShowNextAnnouncement()
    {
        yield return new WaitForSeconds(0.5f);
        announcementImage.sprite = posters[posterIndex];
        announcementPanel.SetActive(true);
    }

    public void OnClosePosterClicked()
    {
        announcementPanel.SetActive(false);
        posterIndex++;

        if (posterIndex >= posters.Count)
        {
            // Move on
        }
        else
        {
            // Next poster
            StartCoroutine(WaitAndShowNextAnnouncement());
        }
    }
}
