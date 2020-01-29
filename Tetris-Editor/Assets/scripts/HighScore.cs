using UnityEngine.UI;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public Text hs;
    void Start()
    {
        hs.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

}
