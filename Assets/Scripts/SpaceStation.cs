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
    public string explanation;
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
    private List<Container> containers;
    public List<Transform> spawnPoints;
    public AudioSource pickUpSound;
    public GameObject okButton;
    private int gemsToCollectOnOk;

    public SaveLoadPanel saveLoadPanel;
    public Curtain curtain;

    public List<EnhancementProperty> enhancementProperties;

    void Start()
    {
        containers = new List<Container>();
        // Dev path
        // ProfileManager.inMemoryProfile.gems = 1000;
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
        BuyContainers(1);
    }

    public void OnBuyFiveClicked()
    {
        BuyContainers(5);
    }

    private void BuyContainers(int amount)
    {
        menu.SetActive(false);
        okButton.SetActive(false);
        ProfileManager.inMemoryProfile.gems -= amount * 2;
        StartCoroutine(SpawnContainers(amount));
    }

    private void GenerateContainer(Container c, bool enhancementOnly)
    {
        bool isColor = Random.value <= 0.9f;
        if (enhancementOnly) isColor = false;
        bool duplicate = false;

        if (isColor)
        {
            int colorIndex = Random.Range(0, 216);
            duplicate = ProfileManager.inMemoryProfile.HasColor(colorIndex);
            c.SetUpColor(colorIndex, duplicate);
            if (!duplicate) ProfileManager.inMemoryProfile.UnlockColor(colorIndex);
        }
        else
        {
            Enhancement e = (Enhancement)Random.Range(0, (int)Enhancement.Count);
            EnhancementProperty p = GetEnhancementProperty(e);
            duplicate = ProfileManager.inMemoryProfile.HasEnhancement(e);
            c.SetUpEnhancement(p.title, p.explanation, duplicate);
            ProfileManager.inMemoryProfile.SetEnhancement(e, true);
        }

        if (duplicate) gemsToCollectOnOk++;
    }

    private IEnumerator SpawnContainers(int amount)
    {
        containers.Clear();
        gemsToCollectOnOk = 0;

        const float waitPerContainer = 0.3f;
        if (amount == 1)
        {
            yield return new WaitForSeconds(waitPerContainer);
            GameObject o = Instantiate(containerPrefab);
            o.transform.position = spawnPoints[2].position;
            Container c = o.GetComponentInChildren<Container>();
            GenerateContainer(c, enhancementOnly: false);
            containers.Add(c);
        }
        else
        {
            int containerWithEnhancement = Random.Range(0, 5);
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(waitPerContainer);
                GameObject o = Instantiate(containerPrefab);
                o.transform.position = spawnPoints[i].position;
                Container c = o.GetComponentInChildren<Container>();
                GenerateContainer(c, enhancementOnly: i == containerWithEnhancement);
                containers.Add(c);
            }
        }

        yield return new WaitForSeconds(4f);
        okButton.SetActive(true);
    }

    public void OnOkClicked()
    {
        okButton.SetActive(false);
        StartCoroutine(CollectGemsAndReturnToMenu());
    }

    private IEnumerator CollectGemsAndReturnToMenu()
    {
        for (int i = 0; i < gemsToCollectOnOk; i++)
        {
            yield return new WaitForSeconds(0.5f);
            pickUpSound.Play();
            ProfileManager.inMemoryProfile.gems++;
        }

        yield return new WaitForSeconds(1f);
        foreach (Container c in containers)
        {
            c.Dismiss();
        }
        containers.Clear();

        yield return new WaitForSeconds(0.6f);
        foreach (Container c in containers)
        {
            Destroy(c.gameObject);
        }
        menu.SetActive(true);
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
        SaveLoadPanel.SaveComplete += OnSaveComplete;
        saveLoadPanel.StartSave();
    }

    private void OnSaveComplete()
    {
        SaveLoadPanel.SaveComplete -= OnSaveComplete;
        curtain.DrawAndGotoScene(Scenes.mainMenu);
    }

    private IEnumerator WaitThenOpen(GameObject otherMenu)
    {
        yield return new WaitForSeconds(0.5f);
        otherMenu.SetActive(true);
    }
}
