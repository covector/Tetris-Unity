using UnityEngine;

public class Back2game : MonoBehaviour
{
    public GameObject countingdown;
    public GameObject option;

    public void Continuegame()
    {
        option.SetActive(false);
        countingdown.SetActive(true);
        gameObject.SetActive(false);
    }
}
