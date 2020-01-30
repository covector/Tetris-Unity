using UnityEngine;

public class SetBGMusic : MonoBehaviour
{
    public AudioSource bgm;
    void Start()
    {
        bgm.volume = PlayerPrefs.GetInt("Volume", 75) / 400f;
    }
}
