using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Question", menuName = "ScriptableObjects/Question",order =1)]
public class QuestionData : ScriptableObject
{
    public string Question;
    public Category Category;
    [Tooltip("Correct is the first")]
    public string[] Answers;
    public bool IsDone = false;
}

public enum Category
{
    Selectable,
    Input
}
