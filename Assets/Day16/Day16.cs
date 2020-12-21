using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rule
{
    public int Min;
    public int Max;

    public bool IsValid(int testValue)
    {
        return testValue >= Min && testValue <= Max;
    }
}

public class Rules
{
    public List<Rule> RulesList = new List<Rule>();

    public void AddRule(int min, int max)
    {
        RulesList.Add(new Rule() { Min = min, Max = max });
    }

    public bool IsValid(int testValue)
    {
        return RulesList.Any(rule => rule.IsValid(testValue));
    }
}

public class Day16 : MonoBehaviour
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
        Queue<string> inputLinesQueue = new Queue<string>(inputLines);

        Rules rules = new Rules();

        for (string currentLine = inputLinesQueue.Dequeue();
            !string.IsNullOrEmpty(currentLine);
            currentLine = inputLinesQueue.Dequeue())
        {
            string[] ranges = currentLine.Split(':')[1].Split(new char[4] { ' ', 'o', 'r', '-' }, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < ranges.Length; i += 2)
            {
                rules.AddRule(int.Parse(ranges[i]), int.Parse(ranges[i + 1]));
            }
        }

        for (string currentLine = inputLinesQueue.Dequeue();
            !string.IsNullOrEmpty(currentLine);
            currentLine = inputLinesQueue.Dequeue())
        {
            if (currentLine == "your ticket:")
            {
                continue;
            }
        }

        int countInvalid = 0;

        for (string currentLine = inputLinesQueue.Dequeue();
            !string.IsNullOrEmpty(currentLine);
            currentLine = inputLinesQueue.Dequeue())
        {
            if (currentLine == "nearby tickets:")
            {
                continue;
            }

            string[] fields = currentLine.Split(',');
            foreach(string field in fields)
            {
                int fieldValue = int.Parse(field);
                if (!rules.IsValid(fieldValue))
                {
                    countInvalid += fieldValue;
                }
            }
        }

        Debug.LogWarning("Invalid sum: " + countInvalid);
    }
}
