using UnityEngine;

public class Ingamebackoption : MonoBehaviour
{
    public GameObject extra;
    public GameObject fade;
    public Master_Control esc;
    public void Gameback2option()
    {
        fade.SetActive(true);
        extra.SetActive(false);
        esc.escapception = false;
        esc.escapable = true;
        gameObject.SetActive(false);
        
    }
}
