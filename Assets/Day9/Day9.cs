using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day9 : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_Input = null;

    private List<long> m_Numbers = new List<long>();

    [SerializeField]
    private int m_BufferSize = 5;

    private void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');

        foreach(string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            m_Numbers.Add(long.Parse(line));
        }

        long invalidNumber = 0;

        for (int i = m_BufferSize; i < m_Numbers.Count; i++)
        {
            if (!IsNumberValid(m_Numbers[i], m_Numbers.GetRange(i - m_BufferSize, m_BufferSize)))
            {
                invalidNumber = m_Numbers[i];
                Debug.LogError("Invalid position " + i + " with value " + m_Numbers[i]);
            }
        }

        GetEncryptionLimits(invalidNumber, out long min, out long max);

        Debug.LogError("Max:" + max + " Min:" + min + " Sum: " + (max + min));
    }

    private bool IsNumberValid(long number, List<long> preamble)
    {
        for(int i = 0; i < preamble.Count - 1; i++)
        {
            for (int j = i + 1; j < preamble.Count; j++)
            {
                if (preamble[i] + preamble[j] == number)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void GetEncryptionLimits(long target, out long min, out long max)
    {
        min = 0;
        max = 0;

        for (int i = 0; i < m_Numbers.Count - 1; i++)
        {
            min = m_Numbers[i];
            max = long.MinValue;
            long count = min;

            for (int j = i + 1; j < m_Numbers.Count; j++)
            {
                count += m_Numbers[j];

                if (min > m_Numbers[j])
                {
                    min = m_Numbers[j];
                }
                if (max < m_Numbers[j])
                {
                    max = m_Numbers[j];
                }

                if (count >= target)
                {
                    break;
                }
            }

            if (count == target)
            {
                return;
            }
        }
    }
}
