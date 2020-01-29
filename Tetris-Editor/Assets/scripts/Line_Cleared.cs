using UnityEngine;
using UnityEngine.UI;

public class Line_Cleared : MonoBehaviour
{
    public Text linecleared;
    public int line = 0;

    void Start() { linecleared.text = "0"; }

    public void Addline(int lines)
    {
        line += lines;
        linecleared.text = line.ToString();
    }
}
