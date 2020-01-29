using UnityEngine;
using UnityEngine.UI;

public class GameReport : MonoBehaviour
{
    public Text report;
    public Line_Cleared lineclr;
    public Score scorescrpt;
    public void ChangeNumber()
    {
        int curscore = (int)(scorescrpt.totalscore);
        int lines = lineclr.line;
        report.text = string.Format("LINE CLEARED: {0}			 SCORE: {1}", lines, curscore);
    }
}
