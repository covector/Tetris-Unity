using UnityEngine;

public class Start_Game : MonoBehaviour
{
    public GameObject blackscrn;
    public GameObject option;
    public GameObject quit;
    public GameObject skinch;
    public void FadeToStart()
    {
        blackscrn.SetActive(true);
    }

    public void FadeToOption()
    {
        option.SetActive(true);
    }

    public void FadeToQuit()
    {
        quit.SetActive(true);
    }
    public void FadeToSkin()
    {
        skinch.SetActive(true);
    }
}
