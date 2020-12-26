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

        ulong result = 0;
        ulong result2 = 0;
        foreach (string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string cleanedLine = new string(line.ToCharArray()
                                            .Where(c => !Char.IsWhiteSpace(c))
                                            .ToArray());

            ulong parcialResult = PerformCalculation(cleanedLine);
            ulong parcialResult2 = PerformCalculation(cleanedLine, true);

            Debug.Log("parcial result for: '" + line + "' is " + parcialResult);
            Debug.Log("parcial result 2 for: '" + line + "' is " + parcialResult2);

            //result += parcialResult;
            result2 += parcialResult2;
        }

        Debug.LogWarning("Result: " + result);
        Debug.LogWarning("Result 2: " + result2);
    }

    private ulong PerformCalculation(string line, bool additionFirst = false)
    {
        Regex rx = new Regex(@"\([^\(\)]+\)");

        Match match =  rx.Match(line);

        while (match.Success)
        {
            string matchString = match.ToString();
            string insideParentisis = new string(matchString.Skip(1).Take(match.Length - 2).ToArray());

            line = ReplaceMatch(matchString, insideParentisis, line, additionFirst);

            match = rx.Match(line);
        }


        if (additionFirst)
        {
            rx = new Regex(@"\d+\+\d+");

            match = rx.Match(line);

            while (match.Success && match.ToString() != line)
            {
                string matchString = match.ToString();

                line = ReplaceMatch(matchString, matchString, line, additionFirst);

                match = rx.Match(line);
            }
        }

        rx = new Regex(@"\d+(\+|\*)\d+");

        match = rx.Match(line);

        while (match.Success && match.ToString() != line)
        {
            string matchString = match.ToString();

            line = ReplaceMatch(matchString, matchString, line, additionFirst);

            match = rx.Match(line);
        }

        string[] parcels = line.Split(new char[2] { '+', '*' });

        if (parcels.Length == 1)
        {
            return ulong.Parse(parcels[0]);
        }

        char[] characters = line.ToCharArray();
        int currentOperatorIndex = parcels[0].Length;
        char oper = characters[currentOperatorIndex];

        switch (oper)
        {
            case '*':
                return ulong.Parse(parcels[0]) * ulong.Parse(parcels[1]);
            case '+':
                return ulong.Parse(parcels[0]) + ulong.Parse(parcels[1]);
        }

        Debug.LogError("something went wrong");
        return 0;
    }

    private string ReplaceMatch(string matchString, string calculationString, string line, bool additionFirst)
    {
        string calculationOutput = PerformCalculation(calculationString, additionFirst).ToString();

        return line.Replace(matchString, calculationOutput);
    }
}
