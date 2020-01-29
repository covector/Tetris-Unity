using UnityEngine;

public class SelfDisable : MonoBehaviour
{
    public void disableself()
    {
        gameObject.SetActive(false);
    }
}
