using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionScreen : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;

    public List<ResItem> resItems = new List<ResItem>();
    private int selectRes;
    public TMP_Text resLabel;
    // Start is called before the first frame update
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;
        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResLeft()
    {
        selectRes--;
        if (selectRes < 0) selectRes = 0;
        ResLabel();
    }
    
    public void ResRight()
    {
        selectRes++;
        if (selectRes > resItems.Count - 1) selectRes = resItems.Count - 1;
        ResLabel();
    }

    public void ResLabel() {
        resLabel.text = resItems[selectRes].horizontal.ToString() + " x " + resItems[selectRes].vertical.ToString();
    }

    public void ApplyScreen()
    {
        //Screen.fullScreen = fullscreenTog.isOn;
        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        Screen.SetResolution(resItems[selectRes].horizontal, resItems[selectRes].vertical, fullscreenTog.isOn);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}

