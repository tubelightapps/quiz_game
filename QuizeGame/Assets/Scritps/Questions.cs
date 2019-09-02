using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Questions : MonoBehaviour
{
    #region Variables

    public Image image_Question;
    public GameObject goNextButton;
    public List<Sprite> spriteList;
    public List<Text> toggleTextList;


    private int currentQuestionIndex = -1;
    private List<string> optionAnswers;

    private Toggle toggleSelected;
    private string optionSelected;
    private int correctAnswersCount = 0;

    #endregion


    #region Builtin Methods

    void Start()
    {
        NextQuestion();
    }

    #endregion


    #region Custom Methods

    //get and set next question
    public void NextQuestion()
    {
        //check correct answer
        if (currentQuestionIndex > -1)
        {
            if (optionSelected.Equals(spriteList[currentQuestionIndex].name))
            {
                ++correctAnswersCount;
                Debug.Log("Correct answer -> " + optionSelected);
            }
        }

        //set toggle off
        foreach (Toggle t in FindObjectsOfType<Toggle>())
        {
            t.isOn = false;
        }

        goNextButton.SetActive(false);

        ++currentQuestionIndex;
        if (currentQuestionIndex >= spriteList.Count)
        {
            currentQuestionIndex = 0;
            //game end logic
            Debug.Log("Game end " + correctAnswersCount + " out of " + spriteList.Count);
            SceneManager.LoadScene("End");
        }

        image_Question.sprite = spriteList[currentQuestionIndex];
        image_Question.SetNativeSize();

        optionAnswers = new List<string>();

        //add correct answer
        optionAnswers.Add(spriteList[currentQuestionIndex].name);

        //Random options
        optionAnswers.Add(spriteList[GetRandomIndex()].name);
        optionAnswers.Add(spriteList[GetRandomIndex()].name);
        optionAnswers.Add(spriteList[GetRandomIndex()].name);

        //shuffle options
        ShuffleAnswers();

        //set answers text
        for (int i = 0; i < toggleTextList.Count; ++i)
        {
            toggleTextList[i].text = optionAnswers[i];
        }
    }

    //get random index which is not equal to currentIndex
    private int GetRandomIndex()
    {
        int rand = Random.Range(0, spriteList.Count);
        if (rand == currentQuestionIndex)
            return GetRandomIndex();
        return rand;
    }

    private void ShuffleAnswers()
    {
        string answer = optionAnswers[0];
        int rand = Random.Range(0, optionAnswers.Count);
        string randString = optionAnswers[rand];
        optionAnswers[rand] = answer;
        optionAnswers[0] = randString;
    }

    public void OnToggleClick(Text t)
    {
        goNextButton.SetActive(true);
        optionSelected = t.text;
    }

    #endregion
}