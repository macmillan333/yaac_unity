using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadPanel : MonoBehaviour
{
    public Image panel;
    public Text text;

    private const float panelStayTime = 2f;
    private const float panelHideTime = 0.5f;

    public static event Delegates.Void SaveComplete;
    public static event Delegates.Void LoadComplete;

    void Start()
    {
        Hide();
    }
    
    void Update()
    {
        
    }

    public void StartSave()
    {
        StartCoroutine(SaveProcess());
    }

    // Will create new profile if none exists.
    // This should only be called when the application starts.
    public void StartLoad()
    {
        StartCoroutine(LoadProcess());
    }

    private void Show(string content)
    {
        panel.enabled = true;
        text.text = content;
        text.enabled = true;
    }

    private void Hide()
    {
        panel.enabled = false;
        text.text = "";
        text.enabled = false;
    }

    private IEnumerator SaveProcess()
    {
        Show("Saving profile. Do not turn off your PC.");
        Exception exception = null;
        Thread saveThread = new Thread(() => {
            try
            {
                ProfileManager.SaveToFile();
            }
            catch (Exception e)
            {
                exception = e;
            }
        });
        saveThread.Start();
        yield return new WaitForSeconds(panelStayTime);
        while (saveThread.IsAlive) yield return null;
        saveThread.Join();

        if (exception != null)
        {
            Hide();
            yield return new WaitForSeconds(panelHideTime);
            Show("An error occured when saving profile: \n" + exception.Message
                + "\n\nThe game cannot continue.");
        }
        else
        {
            Hide();
            yield return new WaitForSeconds(panelHideTime);
            Show("Save successful.");
            yield return new WaitForSeconds(panelStayTime);
            Hide();
            yield return new WaitForSeconds(panelHideTime);
            SaveComplete?.Invoke();
        }
    }

    private IEnumerator LoadProcess()
    {
        Show("Loading profile. Do not turn off your PC.");
        Exception exception = null;
        bool fileExists = true;
        Thread loadThread = new Thread(() => {
            try
            {
                ProfileManager.LoadFromFile();
            }
            catch (FileNotFoundException)
            {
                fileExists = false;
            }
            catch (DirectoryNotFoundException)
            {
                fileExists = false;
            }
            catch (Exception e)
            {
                exception = e;
            }
        });
        loadThread.Start();
        yield return new WaitForSeconds(panelStayTime);
        while (loadThread.IsAlive) yield return null;
        loadThread.Join();

        if (exception != null)
        {
            Hide();
            yield return new WaitForSeconds(panelHideTime);
            Show("An error occured when loading profile: \n" + exception.Message
                + "\n\nThe game cannot continue.");
        }
        else if (fileExists)
        {
            Hide();
            yield return new WaitForSeconds(panelHideTime);
            Show("Load successful.");
            yield return new WaitForSeconds(panelStayTime);
            Hide();
            yield return new WaitForSeconds(panelHideTime);
            LoadComplete?.Invoke();
        }
        else
        {
            Hide();
            yield return new WaitForSeconds(panelHideTime);

            Show("No profile found. Creating new profile.");
            Thread createThread = new Thread(() => {
                try
                {
                    ProfileManager.CreateAndSave();
                }
                catch (Exception e)
                {
                    exception = e;
                }
            });
            createThread.Start();
            yield return new WaitForSeconds(panelStayTime);
            while (createThread.IsAlive) yield return null;
            createThread.Join();

            if (exception != null)
            {
                Hide();
                yield return new WaitForSeconds(panelHideTime);
                Show("An error occured when creating profile: \n" + exception.Message
                    + "\n\nThe game cannot continue.");
            }
            else
            {
                Hide();
                yield return new WaitForSeconds(panelHideTime);
                LoadComplete?.Invoke();
            }
        }
    }
}
