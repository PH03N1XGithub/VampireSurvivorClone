using TMPro;
using UnityEngine;

public class HighScore_UI : MonoBehaviour
{
    public string highScore;
    public TMP_Text highScoreText;
    private void OnEnable()
    {
        if (HighScore_Level.Instance == null)
        {
            highScoreText.text = "Killed enemys "+0;
        }
        else
        {
            highScore = HighScore_Level.Instance.playerScore.ToString();
            highScoreText.text = "killed enemys "+highScore;
        }
            
       
        
    }
}
