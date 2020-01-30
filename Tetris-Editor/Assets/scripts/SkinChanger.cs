using UnityEngine;
using UnityEngine.UI;

public class SkinChanger : MonoBehaviour
{
    public Image[] boxes;
    void Start()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            if (i == PlayerPrefs.GetInt("SkinIndex", 0)) { boxes[i].color = new Color(0.2f, 0.75f, 0.25f); }
            else { boxes[i].color = Color.black; }
        }
    }
    public void ChangeSkin(int index)
    {
        PlayerPrefs.SetInt("SkinIndex", index);
        for (int i = 0; i < boxes.Length; i++)
        {
            if (i == index) { boxes[i].color = new Color(0.2f, 0.75f, 0.25f); }
            else { boxes[i].color = Color.black; }
        }
    }

}
