using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _gameFinished;
    public bool GameFinished { get { return _gameFinished; } set { _gameFinished = value; for (int i = 0; i < _uiObjects.Length; i++) { _uiObjects[i].SetActive(!_uiObjects[i].activeSelf); } Statistics.Instance.UpdateMatchesPlayed(); } }

    [SerializeField]
    private GameObject[] _uiObjects;

    private string _categoryNumber = "-1" ;
    public string CategoryNumber { get { return _categoryNumber; } set { _categoryNumber = value; } }

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    void Awake()
    {
        if (_instance != null) Destroy(this);
        _instance = this;
        Application.targetFrameRate = 60;
        _gameFinished = false;
    }


    public void SetQuizCategory(string value)
    {
        _categoryNumber = value;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
