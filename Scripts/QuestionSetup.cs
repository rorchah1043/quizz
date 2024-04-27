using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestionSetup : MonoBehaviour
{
    [SerializeField] private List<QuestionData> _questions;
    private static QuestionData _currentQuestion;
    public static QuestionData CurrentQuestion => _currentQuestion;

    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private TextMeshProUGUI _categoryText;
    [SerializeField] private AnswerButton[] _answerButtons;

    [SerializeField] private int _correctAnswerChoice;

    [Header("Gameplay Panels ")]
    [SerializeField] private GameObject _panelSelectable;
    [SerializeField] private GameObject _panelInput;

    private bool _selectableActive = false, _inputActive = false;

    [Header("Input Elements")]
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextMeshProUGUI _inputQuestionText;
    private string _inputRightAnswer;

    [Space(5)]
    [SerializeField] private TextMeshProUGUI _questionCountText;

    public static int MaxNumber = 0;
    private static QuestionSetup _instance;
    public static QuestionSetup Instance => _instance;

    private static int _currentNumber = 0;
    public static int CurrentNumber => _currentNumber;

    void Awake()
    {
        if (_instance) Destroy(this);
        _instance = this;
    }


    private void TruncateByHalf()
    {
        if (GameManager.Instance.CategoryNumber == "2")
        {
            int remainder = _questions.Count % 2;
            _questions = _questions.Skip(_questions.Count / 2 + remainder).ToList();
            SetCountValues();
        }
    }


    private void GetQuestionAssets()
    {
        print("GetQuestionAssets");
        if (GameManager.Instance.CategoryNumber == "0")
        {
            //print($"category number = {GameManager.Instance.CategoryNumber}");
            _questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("Questions0"));
            
        }
        else if(GameManager.Instance.CategoryNumber == "1")
        {
            //print($"category number = {GameManager.Instance.CategoryNumber}");
            _questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("Questions1"));
        }
        else if (GameManager.Instance.CategoryNumber == "2")
        {
            //print($"category number = 2");
            _questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("Questions" + "0"));
            _questions.AddRange(new List<QuestionData>(Resources.LoadAll<QuestionData>("Questions" + "1")));
        }


    }

    private void SelectNewQuestion()
    {
        _currentQuestion = _questions[_currentNumber];
        _currentQuestion.IsDone = false;
        if (_currentQuestion.Category == Category.Selectable)
        {
            _inputActive = false;
            _selectableActive = true;
        }
        else if (_currentQuestion.Category == Category.Input)
        {
            _inputActive = true;
            _selectableActive = false;
        }
    }

    private void SetQuestionValues()
    {
        if (_selectableActive)
        {
            _questionText.text = _currentQuestion.Question;
        }
        else if(_inputActive) 
        {
            _inputQuestionText.text = _currentQuestion.Question;
        }
    }

    private void SetAnswerValues()
    {
        List<string> answers = RandomizeAnswers(new List<string>(_currentQuestion.Answers));

        if (_selectableActive)
        {
            for (int i = 0; i < _answerButtons.Length; i++)
            {
                bool isCorrect = false;

                if (i == _correctAnswerChoice)
                {
                    isCorrect = true;
                }

                _answerButtons[i].SetIsCorrect(isCorrect);
                _answerButtons[i].SetAnswerText(answers[i]);
            }
        }
        else if (_inputActive)
        {
            _inputRightAnswer = answers[0];
        }

    }

    private void SetCountValues()
    {
        _questionCountText.text = $"{_currentNumber+1}/{_questions.Count}";
    }

    private List<string> RandomizeAnswers(List<string> originalList)
    {
        bool correctAnswerChosen = false;

        List<string> newList = new();

        for (int i = 0; i < _answerButtons.Length; i++)
        {
            int random = Random.Range(0, originalList.Count);

            if(random == 0 && !correctAnswerChosen)
            {
                _correctAnswerChoice = i;
                correctAnswerChosen = true;
            }

            newList.Add(originalList[random]);
            originalList.RemoveAt(random);
        }

        return newList;
    }

    public static void NextQuestion()
    {
        _instance.SelectNewQuestion();
        _instance.SetQuestionValues();
        _instance.SetAnswerValues();
        _instance.SetCountValues();
        _instance.ActivateGamePanel();
    }

    private static readonly System.Random rng = new();  

    private static void Shuffle<T>( IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public void ChangeCurrentNumber(int value)
    {
        if (value > 0 && _currentNumber < MaxNumber)
        {
            _currentNumber += value;
            NextQuestion();
            print(_currentNumber);
        }
        else if (value < 0 && _currentNumber > 0)
        {
            _currentNumber += value;
            NextQuestion();
            print(_currentNumber);
        }
    }

    public void MarkDone()
    {
        if (!_currentQuestion.IsDone)
        {
            _currentQuestion.IsDone = true;
            MaxNumber++;
            if (_currentNumber == MaxNumber-1 && MaxNumber == _questions.Count) 
            {
                print("MARK DONE");
                GameManager.Instance.GameFinished = true;
                 _currentNumber = MaxNumber =  0;
                    return;
            }
            _currentNumber = MaxNumber;
            Statistics.Instance.UpdateExp();
        }
    }

    public void LoadQuestionsOnClick()
    {
        StartCoroutine(StepLoader());
    }

    private IEnumerator StepLoader()
    {
        GetQuestionAssets();
        _instance.SetCountValues();
        //yield return new WaitUntil(() => !_questions.IsEmpty());
        Shuffle(_questions);
        SelectNewQuestion();
        SetQuestionValues();
        SetAnswerValues();
        TruncateByHalf();
        yield return null;
    }

    public void ActivateGamePanel()
    {
        print($"{_selectableActive}{ _inputActive}");
        _panelSelectable.SetActive(_selectableActive); _panelInput.SetActive(_inputActive);

    }

    public void CheckRight()
    {
        if (_inputRightAnswer == _inputField.text)
        {
            print("Right Input");
            Statistics.Instance.UpdateExp();
            Statistics.Instance.UpdateRightAnswers();
        }
    }
}
