using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Questions : MonoBehaviour
{
    #region Variables

    public Image imageQuestion;
    public Text textQuestion;
    public ToggleGroup toggleGroup;
    public GameObject goNextButton;
    public Image imageHighlight;
    public List<Sprite> spriteList;
    public List<Text> toggleTextList;


    private int _currentQuestionIndex = -1;
    private int _correctAnswersCount = 0;

    private List<string> _optionAnswers;
    private Text _textOptionSelected;
    private Animator _animator;

    private string _correctAnswer;
    private string _textForQuestion = "What is this picture of?";

    #endregion


    #region Builtin Methods

    private void Awake()
    {
        if (imageQuestion)
            _animator = imageQuestion.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(ShowNextQuestion());
    }

    #endregion


    #region Custom Methods

    public void OnNextClick()
    {
        //hide next button
        goNextButton.SetActive(false);

        //show answer
        textQuestion.text = _correctAnswer;

        //set highlight position to selected toggle
        Vector2 rect = imageHighlight.GetComponent<RectTransform>().anchoredPosition;
        Vector2 targetRect = _textOptionSelected.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        rect.x = targetRect.x;
        rect.y = targetRect.y;
        imageHighlight.GetComponent<RectTransform>().anchoredPosition = rect;

        //check correct answer
        bool isCorrect = CheckCorrectAnswer();

        if (isCorrect)
        {
            imageHighlight.color = new Color32(41, 118, 6, 86);
        }
        else
        {
            imageHighlight.color = new Color32(118, 11, 7, 86);
        }

        imageHighlight.gameObject.SetActive(true);

        StartCoroutine(HideQuestion());
    }

    private IEnumerator HideQuestion()
    {
        yield return new WaitForSeconds(2f);

        imageQuestion.gameObject.SetActive(false);
        textQuestion.gameObject.SetActive(false);

        toggleGroup.SetAllTogglesOff(true);
        toggleGroup.gameObject.SetActive(false);

        imageHighlight.gameObject.SetActive(false);

        //hide next button
        goNextButton.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ShowNextQuestion());
    }


    private IEnumerator ShowNextQuestion()
    {
        NextQuestion();
        imageQuestion.gameObject.SetActive(true);
        _animator.SetTrigger("In");


        yield return new WaitForSeconds(0.5f);
        textQuestion.text = _textForQuestion;
        textQuestion.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        toggleGroup.gameObject.SetActive(true);
    }

    //get and set next question
    private void NextQuestion()
    {
        //increase index to show next question
        ++_currentQuestionIndex;

        //if last question is over then show game over
        if (_currentQuestionIndex >= spriteList.Count)
        {
            GameOver();
        }

        //set answer
        _correctAnswer = spriteList[_currentQuestionIndex].name;

        //set question image
        imageQuestion.sprite = spriteList[_currentQuestionIndex];
        imageQuestion.SetNativeSize();

        //set options
        SetOptionAnswers();
    }

    //set options on toggle text
    private void SetOptionAnswers()
    {
        _optionAnswers = new List<string>();

        //add correct answer
        _optionAnswers.Add(_correctAnswer);

        //Random options
        _optionAnswers.Add(spriteList[GetRandomIndex()].name);
        _optionAnswers.Add(spriteList[GetRandomIndex()].name);
        _optionAnswers.Add(spriteList[GetRandomIndex()].name);

        //shuffle options
        ShuffleAnswers();

        //set answers text
        for (int i = 0; i < toggleTextList.Count; ++i)
        {
            toggleTextList[i].text = _optionAnswers[i];
        }
    }

    //get random index which is not equal to currentIndex
    private int GetRandomIndex()
    {
        int rand = Random.Range(0, spriteList.Count);

        //check if answer is already added to the option list
        for (int i = 0; i < _optionAnswers.Count; ++i)
        {
            if (spriteList[rand].name.Equals(_optionAnswers[i]))
                return GetRandomIndex();
        }

        return rand;
    }

    //randomly assign correct answer to any of options
    private void ShuffleAnswers()
    {
        string answer = _optionAnswers[0];
        int rand = Random.Range(0, _optionAnswers.Count);
        string randString = _optionAnswers[rand];
        _optionAnswers[rand] = answer;
        _optionAnswers[0] = randString;
    }

    private bool CheckCorrectAnswer()
    {
        if (_currentQuestionIndex > -1)
        {
            if (_textOptionSelected.text.Equals(_correctAnswer))
            {
                ++_correctAnswersCount;
                return true;
            }
        }

        return false;
    }

    public void OnToggleClick(Text t)
    {
        goNextButton.SetActive(true);
        _textOptionSelected = t;
    }

    private void GameOver()
    {
        PlayerPrefs.SetInt("CorrectAnswers", _correctAnswersCount);
        SceneManager.LoadScene("End");
    }

    #endregion
}