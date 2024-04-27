using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelText, _matchesPlayedText, _rightAnswersText;
    [SerializeField]
    private Slider _experienceSlider;
    private int _level,_rightAnswers,_matchesPlayed;
    private float _experience, _limit;
    [SerializeField]
    private float _stepMultiplier = 1.5f, _limitBase = 100, _rightAnswerExpAmount = 10;

    private static Statistics _instance;
    public static Statistics Instance => _instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null) Destroy(_instance);
        _instance = this;

        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level",1);
        }
        if (!PlayerPrefs.HasKey("experience"))
        {
            PlayerPrefs.SetFloat("experience", 0);
        }
        if (!PlayerPrefs.HasKey("matchesPlayed"))
        {
            PlayerPrefs.SetInt("matchesPlayed", 1);
        }
        if (!PlayerPrefs.HasKey("rightAnswers"))
        {
            PlayerPrefs.SetInt("rightAnswers", 0);
        }

        Initialize();
    }


    public void IncreaseLevel()
    {

    }

    private static int  Exp;


    public static void OnExpChanged(int value)
    {
        Exp += value;
        //print("Exp = " + Exp);
        PlayerPrefs.SetInt("Exp", Exp);
    }

    private void Initialize()
    {
        _level = PlayerPrefs.GetInt("level");
        _matchesPlayed = PlayerPrefs.GetInt("matchesPlayed");
        _rightAnswers = PlayerPrefs.GetInt("rightAnswers");
        _experience = PlayerPrefs.GetFloat("experience");

        _levelText.text = $"Lvl:{_level}";
        _matchesPlayedText.text = $"Matches Played: {_matchesPlayed}";
        _rightAnswersText.text = $"Right Answers:{_rightAnswers}";

        //print($" {_levelText.text} {_matchesPlayedText.text} {_rightAnswersText.text} ");
        _limit = _level * _limitBase * _stepMultiplier;
        _experienceSlider.value = _experience / _limit;

    }

    public void UpdateExp()
    {
        _experience += _rightAnswerExpAmount;
        if(_experience > _limit)
        {
            _level += 1;
            _levelText.text = $"Level: {_level}";
            _limit = _level * _limitBase * _stepMultiplier;
            _experience = 0;
        }
        _experienceSlider.value = _experience/_limit;
        PlayerPrefs.SetInt("level", _level);
        PlayerPrefs.SetFloat("experience", _experience);
    }

    public void UpdateMatchesPlayed()
    {
        _matchesPlayed++;
        print($"matches played {_matchesPlayed}");

        _matchesPlayedText.text = $"Matches Played: {_matchesPlayed}";
    }

    public void UpdateRightAnswers()
    {
        _rightAnswers++;
        //_rightAnswersText.text = $"Right Answers: {_rightAnswers}";
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("level", _level);
        PlayerPrefs.SetFloat("experience", _experience);
        PlayerPrefs.SetInt("matchesPlayed", _matchesPlayed);
        PlayerPrefs.SetInt("rightAnswers", _rightAnswers);
    }

    private void CountPercetage()
    {
        _rightAnswersText.text = $"Right Answers: {_rightAnswers}";
    }
}
