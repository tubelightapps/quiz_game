using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    #region Variables
    public Text details;
    #endregion


    #region Builtin Methods
    void Start()
    {
        SetText();
    }
    #endregion


    #region Custom Methods
    private void SetText()
    {
        int totalAnswers = PlayerPrefs.GetInt("CorrectAnswers", 0);
        details.text = totalAnswers + " correct answers out of 15";
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application. Quit();
#endif
    }
    #endregion
}