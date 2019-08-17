using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnhancementProperty
{
    public Enhancement type;
    public string title;
    [TextArea]
    public string description;
}

public class SpaceStation : MonoBehaviour
{
    public Text gemCount;
    public GameObject menu;
    public Button buyOneButton;
    public Button buyFiveButton;
    public GameObject dropRatePanel;
    public GameObject inventoryPanel;

    public GameObject containerPrefab;
    public List<Transform> spawnPoints;

    public Curtain curtain;

    public List<EnhancementProperty> enhancementProperties;

    void Start()
    {
        // Dev path
        ProfileManager.inMemoryProfile.gems = 20;
    }
    
    void Update()
    {
        int gems = ProfileManager.inMemoryProfile.gems;
        gemCount.text = gems.ToString();
        buyOneButton.interactable = gems >= 2;
        buyFiveButton.interactable = gems >= 10;
    }

    public EnhancementProperty GetEnhancementProperty(Enhancement e)
    {
        foreach (EnhancementProperty p in enhancementProperties)
        {
            if (p.type == e) return p;
        }
        throw new System.ArgumentOutOfRangeException("Enhancement not found");
    }

    public void OnBuyOneClicked()
    {

    }

    public void OnBuyFiveClicked()
    {

    }

    public void OnDropRateClicked()
    {
        menu.SetActive(false);
        StartCoroutine(WaitThenOpen(dropRatePanel));
    }

    public void OnDropRateClosed()
    {
        dropRatePanel.SetActive(false);
        StartCoroutine(WaitThenOpen(menu));
    }

    public void OnInventoryClicked()
    {
        menu.SetActive(false);
        StartCoroutine(WaitThenOpen(inventoryPanel));
    }

    public void OnInventoryClosed()
    {
        inventoryPanel.SetActive(false);
        StartCoroutine(WaitThenOpen(menu));
    }

    public void OnReturnClicked()
    {
        menu.SetActive(false);
        curtain.DrawAndGotoScene(Scenes.mainMenu);
    }

    private IEnumerator WaitThenOpen(GameObject otherMenu)
    {
        yield return new WaitForSeconds(0.5f);
        otherMenu.SetActive(true);
    }
}
