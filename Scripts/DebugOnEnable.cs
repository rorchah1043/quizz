using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        print($"{gameObject.name} enabled");
    }
}
