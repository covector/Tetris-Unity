using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using System.Collections;

public class Hotkey : MonoBehaviour
{
    public Text ltext;
    public GameObject leftbutton;

    public Text rtext;
    public GameObject rightbutton;

    public Text dtext;
    public GameObject downbutton;

    public Text ctext;
    public GameObject clockwiserot;

    public Text atext;
    public GameObject anticlockwiserot;

    public Text htext;
    public GameObject holdbutton;

    public Text itext;
    public GameObject instabutton;

    public Slider vol;
    public AudioSource bgmusic;

    public Toggle ghost;
    private void Start()
    {
        ltext.text = PlayerPrefs.GetString("Left", "LeftArrow");
        rtext.text = PlayerPrefs.GetString("Right", "RightArrow");
        dtext.text = PlayerPrefs.GetString("Down", "DownArrow");
        ctext.text = PlayerPrefs.GetString("Clockwise", "UpArrow");
        atext.text = PlayerPrefs.GetString("Anticlockwise", "Z");
        htext.text = PlayerPrefs.GetString("Hold", "C");
        itext.text = PlayerPrefs.GetString("Insta", "Space");
        vol.value = PlayerPrefs.GetInt("Volume", 75);
        if (PlayerPrefs.GetInt("Ghost", 1) == 1) { ghost.isOn = true; }
        else { ghost.isOn = false; }
    }

    private void Update()
    {
        string entered = Input.inputString.ToUpper();
        string pressed = entered;
        if (entered != "") { pressed = entered[0].ToString(); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { pressed = "LeftArrow"; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { pressed = "RightArrow"; }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { pressed = "DownArrow"; }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { pressed = "UpArrow"; }
        if (Input.GetKeyDown(KeyCode.Space)) { pressed = "Space"; }
        if (pressed != "")
        {
            if (EventSystem.current.currentSelectedGameObject == leftbutton)
            {
                ltext.text = pressed;
                PlayerPrefs.SetString("Left", pressed);
            }
            if (EventSystem.current.currentSelectedGameObject == rightbutton)
            {
                rtext.text = pressed;
                PlayerPrefs.SetString("Right", pressed);
            }
            if (EventSystem.current.currentSelectedGameObject == downbutton)
            {
                dtext.text = pressed;
                PlayerPrefs.SetString("Down", pressed);
            }
            if (EventSystem.current.currentSelectedGameObject == clockwiserot)
            {
                ctext.text = pressed;
                PlayerPrefs.SetString("Clockwise", pressed);
            }
            if (EventSystem.current.currentSelectedGameObject == anticlockwiserot)
            {
                atext.text = pressed;
                PlayerPrefs.SetString("Anticlockwise", pressed);
            }
            if (EventSystem.current.currentSelectedGameObject == holdbutton)
            {
                htext.text = pressed;
                PlayerPrefs.SetString("Hold", pressed);
            }
            if (EventSystem.current.currentSelectedGameObject == instabutton)
            {
                itext.text = pressed;
                PlayerPrefs.SetString("Insta", pressed);
            }
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    public void ChangeVolume()
    {
        PlayerPrefs.SetInt("Volume", (int)vol.value);
        bgmusic.volume = vol.value / 200;
    }

    public void ToggleGhost()
    {
        if (ghost.isOn) { PlayerPrefs.SetInt("Ghost", 1); }
        else { PlayerPrefs.SetInt("Ghost", 0); }
    }

    public void FadeVolume()
    {
        StartCoroutine(DecayVolume());
    }

    IEnumerator DecayVolume()
    {
        float volume = PlayerPrefs.GetInt("Volume", 75);
        for (int x = 30; x >= 0; x--)
        {
            bgmusic.volume = (x * volume) / 6000;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ResetHotKeys()
    {
        PlayerPrefs.SetString("Left", "LeftArrow");
        PlayerPrefs.SetString("Right", "RightArrow");
        PlayerPrefs.SetString("Down", "DownArrow");
        PlayerPrefs.SetString("Clockwise", "UpArrow");
        PlayerPrefs.SetString("Anticlockwise", "Z");
        PlayerPrefs.SetString("Hold", "C");
        PlayerPrefs.SetString("Insta", "Space");
        PlayerPrefs.SetInt("Volume", 75);
        Start();
    }
}
