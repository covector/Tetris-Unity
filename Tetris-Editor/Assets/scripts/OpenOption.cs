using UnityEngine;

public class OpenOption : MonoBehaviour
{
    public GameObject option;
    public GameObject optionfadein;
    public Master_Control esc;

    public void Ingameoption()
    {
        esc.escapable = true;
        option.SetActive(true);
        optionfadein.SetActive(true);
        gameObject.SetActive(false);
    }

}
