using UnityEngine;
using TMPro;


public class Lederboard : MonoBehaviour
{
    public string nameScene;
    public GameObject panelDeath;
    public TMP_Text endText;
    private bool endGame;
    void Start()
    {
        if(nameScene == "Menu") SetHighScoreOnLederboard();
        if (!PlayerPrefs.HasKey("HighScore")) PlayerPrefs.SetInt("HighScore", 0);
        
    }

    public void SetHighScoreOnLederboard()
    {
        
        int best = PlayerPrefs.GetInt("HighScore");
#if UNITY_WEBGL && !UNITY_EDITOR
    	WebGLPluginJS.SetLeder(best);
#endif
    }

    public void HighScore()
    {
        endGame = endText.text == "Рабочий день закончен!" ? true : false;
        if ((PlayerPrefs.GetInt("ScoreMoney") > PlayerPrefs.GetInt("HighScore")) && panelDeath.activeSelf && endGame)
        {
            PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("ScoreMoney"));
            Debug.Log(PlayerPrefs.GetInt("HighScore"));
            SetHighScoreOnLederboard();
        }
    }

    private void Update()
    {
        HighScore();
    }



}
