using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_Input = null;

    void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');
    }
}
