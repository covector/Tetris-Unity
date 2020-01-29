using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text score;
    public float totalscore = 0;

    void Start() { score.text = "0"; }

    public void AddScore(string scoretype, int rep = 1)
    {
        if (scoretype == "1") { totalscore += 200 * rep; }
        if (scoretype == "2") { totalscore += 500 * rep; }
        if (scoretype == "3") { totalscore += 1000 * rep; }
        if (scoretype == "4") { totalscore += 2000 * rep; }
        if (scoretype == "fast") { totalscore += 0.4f * rep; }
        if (scoretype == "insta") { totalscore += 20 * rep; }
        score.text = ((int)(totalscore)).ToString();
    }
}