using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1 : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_Input = null;

    void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');
        int[] inputNumbers = new int[inputLines.Length - 1];
        for (int i = 0; i < inputLines.Length - 1; i++)
        {
            inputNumbers[i] = int.Parse(inputLines[i]);
        }

        Check2Sum(inputNumbers);
        Check3Sum(inputNumbers);
    }

    private void Check2Sum(int[] inputNumbers)
    {
        for (int i = 0; i < inputNumbers.Length - 1; i++)
        {
            for (int j = i + 1; j < inputNumbers.Length; j++)
            {
                if (inputNumbers[i] + inputNumbers[j] == 2020)
                {
                    Debug.Log("Result for 2: " + inputNumbers[i] * inputNumbers[j]);
                    Debug.Log(i + " : " + inputNumbers[i]);
                    Debug.Log(j + " : " + inputNumbers[j]);
                    break;
                }
            }
        }
    }

    private void Check3Sum(int[] inputNumbers)
    {
        for (int i = 0; i < inputNumbers.Length - 2; i++)
        {
            for (int j = i + 1; j < inputNumbers.Length - 1; j++)
            {
                for (int k = j + 1; k < inputNumbers.Length; k++)
                {
                    if (inputNumbers[i] + inputNumbers[j] + inputNumbers[k] == 2020)
                    {
                        Debug.Log("Result for 3: " + inputNumbers[i] * inputNumbers[j] * inputNumbers[k]);
                        Debug.Log(i + " : " + inputNumbers[i]);
                        Debug.Log(j + " : " + inputNumbers[j]);
                        Debug.Log(k + " : " + inputNumbers[k]);
                        return;
                    }
                }
            }
        }
    }
}
