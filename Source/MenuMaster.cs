using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuMaster : MonoBehaviour
{
    public GameObject main;
    public GameObject options;
    public GameObject info;

    public TMP_InputField lengthText;
    public int length;
    AudioSource sound;

    bool customOnly = false;
    public Toggle custom;
    public TextMeshProUGUI switchText;

    public GameObject infoText;
    public GameObject shortcutText;

    void Start()
    {
        lengthText.text = FindObjectOfType<DataTransfer>().length.ToString();
        sound = GetComponent<AudioSource>();
        main.SetActive(true);
        options.SetActive(false);
        info.SetActive(false);

        #if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
        #endif

        if(FindObjectOfType<DataTransfer>().customOnly)
        {
            custom.isOn = true;
        }
    }

    public void Play()
    {
        PlaySound();
        SceneManager.LoadScene("Play");
    }

    public void TrackEditor()
    {
        PlaySound();
        SceneManager.LoadScene("LevelCreate");
    }

    public void Options()
    {
        PlaySound();
        main.SetActive(false);
        options.SetActive(true);
    }

    public void Info()
    {
        PlaySound();
        options.SetActive(false);
        info.SetActive(true);

        shortcutText.SetActive(true);
        infoText.SetActive(false);
    }

    public void Back()
    {
        PlaySound();
        main.SetActive(true);
        options.SetActive(false);
    }

    public void InfoBack()
    {
        PlaySound();
        options.SetActive(true);
        info.SetActive(false);
    }


    public void ToggleInfo()
    {
        PlaySound();
        if(shortcutText.activeSelf)
        {
            shortcutText.SetActive(false);
            infoText.SetActive(true);
            switchText.text = "Shortcuts";
        } else {
            shortcutText.SetActive(true);
            infoText.SetActive(false);
            switchText.text = "Steps";
        }

    }

    public void ToggleOnlyCustom()
    {
        customOnly = !customOnly;
        FindObjectOfType<DataTransfer>().customOnly = this.customOnly;

        PlaySound();
    }

    void Update()
    {
        int num;
        int.TryParse(lengthText.text, out num);

        length = num;
    }

    public void UpdateLength()
    {
        //removes invalid text from input field

        int num = 0;
        int.TryParse(lengthText.text, out num);

        if(num == 0)
        {
            lengthText.text = "Invalid!";
            Invoke("ResetLength", 0.5f);
        } else {
            length = Mathf.Clamp(length, 1, FindObjectOfType<DataTransfer>().maxLength);
            lengthText.text = length.ToString();
            FindObjectOfType<DataTransfer>().length = length;
        }
    }

    void ResetLength()
    {
        lengthText.text = "";
    }

    void PlaySound(){
        sound.Play();
    }

}
