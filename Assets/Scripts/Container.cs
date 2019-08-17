using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public GameObject colorCard;
    public TextMesh colorCardTitle;
    public SpriteRenderer colorCardColor;
    public GameObject colorCardNew;
    public GameObject colorCardDuplicate;

    public GameObject enhancementCard;
    public TextMesh enhancementCardTitle;
    public TextMesh enhancementCardExplanation;
    public GameObject enhancementCardNew;
    public GameObject enhancementCardDuplicate;

    public AudioSource containerDrop;
    public AudioSource containerOpen;
    public AudioSource getCard;

    public void SetUpColor(int colorIndex, bool duplicate)
    {
        colorCard.SetActive(true);
        enhancementCard.SetActive(false);
        Color c = CustomizePanel.IndexToColor(colorIndex);
        colorCardTitle.text = CustomizePanel.ColorToHexText(c);
        colorCardColor.color = c;
        colorCardNew.SetActive(!duplicate);
        colorCardDuplicate.SetActive(duplicate);
    }

    public void SetUpEnhancement(string title, string explanation, bool duplicate)
    {
        colorCard.SetActive(false);
        enhancementCard.SetActive(true);
        enhancementCardTitle.text = title;
        enhancementCardExplanation.text = explanation;
        enhancementCardNew.SetActive(!duplicate);
        enhancementCardDuplicate.SetActive(duplicate);
    }

    public void Dismiss()
    {
        GetComponent<Animator>().SetTrigger("ContainerDisappear");
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
