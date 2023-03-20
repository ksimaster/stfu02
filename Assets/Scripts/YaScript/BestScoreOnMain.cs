using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BestScoreOnMain : MonoBehaviour
{
    public TMP_Text bestScore;
    private void Start()
    {
        //if (!PlayerPrefs.HasKey("HighScore")) PlayerPrefs.SetInt("HighScore", 0);
        bestScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
    private void Update()
    {
        bestScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

}
