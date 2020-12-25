using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Day18 : MonoBehaviour
{
    [SerializeField]
    private List<TextAsset> m_Inputs = null;

    private void Start()
    {
        for(int i = 0; i < m_Inputs.Count; i++)
        {
            Debug.LogError("Running asset " + i);
            float startTime = Time.time;
            Run(i, m_Inputs[i]);
            Debug.Log("Finished asset " + i + " it took " + (Time.time - startTime)  + " seconds");
        }
    }

    private void Run(int dataID, TextAsset inputAsset)
    {
        string input = inputAsset.text;
        string[] inputLines = input.Split('\n');

        long result = 0;

        foreach (string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string cleanedLine = new string(line.ToCharArray()
                                            .Where(c => !Char.IsWhiteSpace(c))
                                            .ToArray());

            long parcialResult = PerformCalculation(cleanedLine);
            Debug.Log("parcial result for: '" + line + "' is " + parcialResult);

            result += parcialResult;
        }

        Debug.LogWarning("Result: " + result);
    }

    private long PerformCalculation(string line)
    {
        Regex rx = new Regex(@"\([^\(\)]+\)");

        Match match =  rx.Match(line);

        while (match.Success)
        {
            string matchString = match.ToString();
            string insideParentisis = new string(matchString.Skip(1).Take(match.Length - 2).ToArray());
            string calculationOutput = PerformCalculation(insideParentisis).ToString();

            line = line.Replace(matchString, calculationOutput);

            match = rx.Match(line);
        }

        char[] characters = line.ToCharArray();

        string[] splitted = line.Split(new char[2] { '+', '*' });

        int currentOperatorIndex = splitted[0].Length;
        char oper = characters[currentOperatorIndex];
        long result = 0;
        switch (oper)
        {
            case '*':
                result = long.Parse(splitted[0]) * long.Parse(splitted[1]);
                break;
            case '+':
                result = long.Parse(splitted[0]) + long.Parse(splitted[1]);
                break;
        }

        for (int i = 1; i < splitted.Length - 1; i++)
        {
            currentOperatorIndex += splitted[i].Length + 1;
            oper = characters[currentOperatorIndex];

            switch(oper)
            {
                case '*':
                    result *= long.Parse(splitted[i + 1]);
                    break;
                case '+':
                    result += long.Parse(splitted[i + 1]);
                    break;
            }
        }

        return result;
    }
}
