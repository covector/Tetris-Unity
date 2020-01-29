using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Transform state;

    private void Start()
    {
        Spin_Control_Opiece fuckYou = FindObjectOfType<Spin_Control_Opiece>();
        if (fuckYou != null)
        {
            UpdateState(FindObjectOfType<Master_Control>().InstaDrop(fuckYou.transform.position.y, fuckYou.transform.position.x, "O", 0, true), fuckYou.transform.position.x, 0);
        }

    }
    public void UpdateState(float y, float x, int rotation)
    {
        state.position = new Vector3(x, y, 0);
        state.eulerAngles = new Vector3(0, 0, rotation);
    }

    public void DestroyGhost()
    {
        Destroy(gameObject);
    }
}
