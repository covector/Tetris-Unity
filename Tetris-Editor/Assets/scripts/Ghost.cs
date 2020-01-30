using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Transform state;
    public float x;
    public float y;
    public string type;
    public int rot;

    private void Start()
    {
        UpdateState(y, x, rot);
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

    public void TellCoord(float Y, float X, int Rot = 0)
    {
        x = X;
        y = Y;
        rot = Rot;
    }
}
