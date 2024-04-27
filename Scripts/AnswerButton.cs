using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnswerButton : MonoBehaviour
{
    private bool _isCorrect = false;
    [SerializeField] private TextMeshProUGUI _answerText;

    public void SetAnswerText(string newText)
    {
        _answerText.text = newText;
    }

    public void SetIsCorrect(bool newBool)
    {
        _isCorrect = newBool;
    }

    public void OnClick()
    {
        if (_isCorrect && !QuestionSetup.CurrentQuestion.IsDone)
        {
            Statistics.Instance.UpdateRightAnswers();
        }
        else
        {
            StartCoroutine(PostSelectionTimer());
        }

        QuestionSetup.Instance.MarkDone();
    }

    private IEnumerator PostSelectionTimer()
    {
        yield return new WaitForSeconds(0);
        QuestionSetup.Instance.ActivateGamePanel();
        QuestionSetup.NextQuestion();
    }
}
