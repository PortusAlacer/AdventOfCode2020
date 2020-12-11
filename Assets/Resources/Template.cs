using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_Input = null;

    private void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');

        foreach (string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
        }
    }
}
