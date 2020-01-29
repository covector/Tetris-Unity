using UnityEngine.UI;
using UnityEngine;

public class ExtraOptions : MonoBehaviour
{

    public Slider inertia;
    public Slider horispeed;
    public Slider vertispeed;
    public Slider vertispeedmulti;
    public Slider freeze;
    public Slider absfreeze;
    public Slider delay;
    public Slider accel;

    public Text iner;
    public Text hori;
    public Text verts;
    public Text vertm;
    public Text free;
    public Text abs;
    public Text de;
    public Text acc;

    private void Start()
    {
        inertia.value = PlayerPrefs.GetFloat("Inertia", 0.1f);
        horispeed.value = PlayerPrefs.GetFloat("HoriSpeed", 16f);
        vertispeed.value = PlayerPrefs.GetFloat("VertiSpeed", 2f);
        vertispeedmulti.value = PlayerPrefs.GetFloat("VertiMulti", 8f);
        freeze.value = PlayerPrefs.GetFloat("Freeze", 0.5f);
        absfreeze.value = PlayerPrefs.GetFloat("ABSFreeze", 2f);
        delay.value = PlayerPrefs.GetFloat("Delay", 0.25f);
        accel.value = PlayerPrefs.GetFloat("Accel", 0.01f);

        iner.text = PlayerPrefs.GetFloat("Inertia", 0.1f).ToString();
        hori.text = PlayerPrefs.GetFloat("HoriSpeed", 16f).ToString();
        verts.text = PlayerPrefs.GetFloat("VertiSpeed", 2f).ToString();
        vertm.text = PlayerPrefs.GetFloat("VertiMulti", 8f).ToString();
        free.text = PlayerPrefs.GetFloat("Freeze", 0.5f).ToString();
        abs.text = PlayerPrefs.GetFloat("ABSFreeze", 2f).ToString();
        de.text = PlayerPrefs.GetFloat("Delay", 0.25f).ToString();
        acc.text = PlayerPrefs.GetFloat("Accel", 0.01f).ToString();
    }

    public void ChangeInertia()
    {
        PlayerPrefs.SetFloat("Inertia", inertia.value);
        string textstring = inertia.value.ToString();
        if (textstring.Length > 4)
        {
            iner.text = textstring.Substring(0, 4);
        }
        else 
        {
            iner.text = textstring;
        }
    }
    public void ChangeHoriSpeed()
    {
        PlayerPrefs.SetFloat("HoriSpeed", horispeed.value);
        string textstring = horispeed.value.ToString();
        if (textstring.Length > 4)
        {
            hori.text = textstring.Substring(0, 4);
        }
        else
        {
            hori.text = textstring;
        }
    }
    public void ChangeVertiSpeed()
    {
        PlayerPrefs.SetFloat("VertiSpeed", vertispeed.value);
        string textstring = vertispeed.value.ToString();
        if (textstring.Length > 4)
        {
            verts.text = textstring.Substring(0, 4);
        }
        else
        {
            verts.text = textstring;
        }
    }
    public void ChangeVertiMult()
    {
        PlayerPrefs.SetFloat("VertiMulti", vertispeedmulti.value);
        string textstring = vertispeedmulti.value.ToString();
        if (textstring.Length > 4)
        {
            vertm.text = textstring.Substring(0, 4);
        }
        else
        {
            vertm.text = textstring;
        }
    }
    public void ChangeFreeze()
    {
        PlayerPrefs.SetFloat("Freeze", freeze.value);
        string textstring = freeze.value.ToString();
        if (textstring.Length > 4)
        {
            free.text = textstring.Substring(0, 4);
        }
        else
        {
            free.text = textstring;
        }
    }
    public void ChangeABSFreeze()
    {
        PlayerPrefs.SetFloat("ABSFreeze", absfreeze.value);
        string textstring = absfreeze.value.ToString();
        if (textstring.Length > 4)
        {
            abs.text = textstring.Substring(0, 4);
        }
        else
        {
            abs.text = textstring;
        }
    }
    public void ChangeDelay()
    {
        PlayerPrefs.SetFloat("Delay", delay.value);
        string textstring = delay.value.ToString();
        if (textstring.Length > 4)
        {
            de.text = textstring.Substring(0, 4);
        }
        else
        {
            de.text = textstring;
        }
    }
    public void ChangeAccel()
    {
        PlayerPrefs.SetFloat("Accel", accel.value);
        string textstring = accel.value.ToString();
        if (textstring.Length > 5)
        {
            acc.text = textstring.Substring(0, 5);
        }
        else
        {
            acc.text = textstring;
        }
    }

    public void ResetSettings()
    {
        PlayerPrefs.SetFloat("Inertia", 0.1f);
        PlayerPrefs.SetFloat("HoriSpeed", 16f);
        PlayerPrefs.SetFloat("VertiSpeed", 2f);
        PlayerPrefs.SetFloat("VertiMulti", 8f);
        PlayerPrefs.SetFloat("Freeze", 0.5f);
        PlayerPrefs.SetFloat("ABSFreeze", 2f);
        PlayerPrefs.SetFloat("Delay", 0.25f);
        PlayerPrefs.SetFloat("Accel", 0.02f);
        Start();
    }
}
