using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettingsUI : MonoBehaviour
{
    public Button turnWindowedOnButton;
    public Button turnWindowedOffButton;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;



    //Function called when this object is enabled
    private void OnEnable()
    {
        //Showing the correct button for turning on/off windowed mode
        if(Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            this.turnWindowedOffButton.gameObject.SetActive(true);
            this.turnWindowedOnButton.gameObject.SetActive(false);
        }
        else
        {
            this.turnWindowedOffButton.gameObject.SetActive(false);
            this.turnWindowedOnButton.gameObject.SetActive(true);
        }
    }


    //Function called externally from the TurnWindowedOff/OnButtons in the Video Settings when its state changes
    public void UpdateWindowedMode(bool isOn_)
    {
        if (isOn_)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        Debug.Log("Current screen mode: " + Screen.fullScreenMode);
    }


    //Function called externally from the Resolution drop box in the Video Settings
    public void ChangeResolution()
    {
        switch(this.resolutionDropdown.value)
        {
            case 0://640x480
                Screen.SetResolution(640, 480, Screen.fullScreenMode);
                break;
            case 1://800x600
                Screen.SetResolution(800, 600, Screen.fullScreenMode);
                break;
            case 2://960x720
                Screen.SetResolution(960, 720, Screen.fullScreenMode);
                break;
            case 3://1024x768
                Screen.SetResolution(1024, 768, Screen.fullScreenMode);
                break;
            case 4://1280x960
                Screen.SetResolution(1280, 960, Screen.fullScreenMode);
                break;
            case 5://1400x1050
                Screen.SetResolution(1400, 1050, Screen.fullScreenMode);
                break;
            case 6://1440x1080
                Screen.SetResolution(1440, 1080, Screen.fullScreenMode);
                break;
            case 7://1600x1200
                Screen.SetResolution(1600, 1200, Screen.fullScreenMode);
                break;
            case 8://1856x1392
                Screen.SetResolution(1856, 1392, Screen.fullScreenMode);
                break;
            case 9://1920x1440
                Screen.SetResolution(1920, 1440, Screen.fullScreenMode);
                break;
            case 10://2048x1536
                Screen.SetResolution(2048, 1536, Screen.fullScreenMode);
                break;
            default://1400x1050
                Screen.SetResolution(1400, 1050, Screen.fullScreenMode);
                break;
        }
        Debug.Log("Index: " + this.resolutionDropdown.value + ", Resolution: " + 
            Screen.currentResolution.width + "x" + Screen.currentResolution.height);
    }
}
