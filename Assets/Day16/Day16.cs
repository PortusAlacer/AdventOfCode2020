using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Range
{
    public int Min;
    public int Max;

    public bool IsValid(int testValue)
    {
        return testValue >= Min && testValue <= Max;
    }
}

public class Rule
{
    public string RuleName;
    public List<Range> Ranges;

    public bool IsValid(int testValue)
    {
        return Ranges.Any(range => range.IsValid(testValue));
    }

    public bool AreValidFields(List<int> fields)
    {
        bool valid = true;
        foreach(int field in fields)
        {
            valid = Ranges.Any(range => range.IsValid(field));
        }
        return valid;
    }
}

public class Rules
{
    public List<Rule> RulesList = new List<Rule>();

    public void AddRule(string name, int min, int max)
    {
        Rule rule = RulesList.FirstOrDefault(r => r.RuleName == name);

        if (rule != null)
        {
            rule.Ranges.Add(new Range() { Min = min, Max = max });
        }
        else
        {
            rule = new Rule()
            {
                RuleName = name,
                Ranges = new List<Range>() { new Range() { Min = min, Max = max } }
            };
            RulesList.Add(rule);
        }
    }

    public bool IsValid(int testValue)
    {
        return RulesList.Any(rule => rule.IsValid(testValue));
    }

    internal int GetValidRule(List<int> fields, List<int> takenFields)
    {
        for(int i = 0; i < RulesList.Count; i++)
        {
            if (takenFields.Contains(i))
            {
                continue;
            }

            bool valid = true;

            foreach(int field in fields)
            {
                if (!RulesList[i].IsValid(field))
                {
                    valid = false;
                }
            }

            if (valid)
            {
                return i;
            }
        }

        return -1;
    }
}

public class Ticket
{
    public List<int> Fields = new List<int>();
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
            string[] splitted = currentLine.Split(':');
            string[] ranges = splitted[1].Split(new char[4] { ' ', 'o', 'r', '-' }, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < ranges.Length; i += 2)
            {
                rules.AddRule(splitted[0], int.Parse(ranges[i]), int.Parse(ranges[i + 1]));
            }
        }

        Ticket myTicket = new Ticket();

        for (string currentLine = inputLinesQueue.Dequeue();
            !string.IsNullOrEmpty(currentLine);
            currentLine = inputLinesQueue.Dequeue())
        {
            if (currentLine == "your ticket:")
            {
                continue;
            }

            string[] fields = currentLine.Split(',');
            foreach (string field in fields)
            {
                myTicket.Fields.Add(int.Parse(field));
            }
        }

        int countInvalid = 0;
        List<Ticket> validTickets = new List<Ticket>();

        for (string currentLine = inputLinesQueue.Dequeue();
            !string.IsNullOrEmpty(currentLine);
            currentLine = inputLinesQueue.Dequeue())
        {
            if (currentLine == "nearby tickets:")
            {
                continue;
            }

            string[] fields = currentLine.Split(',');
            bool valid = true;
            Ticket newTicket = new Ticket();
            foreach(string field in fields)
            {
                int fieldValue = int.Parse(field);
                newTicket.Fields.Add(fieldValue);
                if (!rules.IsValid(fieldValue))
                {
                    countInvalid += fieldValue;
                    valid = false;
                }
            }

            if (valid)
            {
                validTickets.Add(newTicket);
            }
        }

        Debug.LogWarning("Invalid sum: " + countInvalid);

        int fieldsCount = myTicket.Fields.Count;

        List<List<int>> fieldsAllTickets = new List<List<int>>();

        for (int i = 0; i < fieldsCount; i++)
        {
            List<int> theseFields = new List<int>();

            foreach(Ticket ticket in validTickets)
            {
                theseFields.Add(ticket.Fields[i]);
            }

            fieldsAllTickets.Add(theseFields);

            
        }

        Dictionary<int, int> fieldsMap = new Dictionary<int, int>();

        for (int i = 0; i < rules.RulesList.Count; i++)
        {
            for (int f = 0; f < fieldsAllTickets.Count; f++)
            {
                if (rules.RulesList[i].AreValidFields(fieldsAllTickets[f]))
                {
                    fieldsMap[i] = f;
                }
            }
        }

        int result = 1;

        foreach(KeyValuePair<int, int> fieldsPair in fieldsMap)
        {
            if (rules.RulesList[fieldsPair.Key].RuleName.StartsWith("departure"))
            {
                result *= myTicket.Fields[fieldsPair.Value];
            }
        }

        Debug.LogWarning("Departure multiplcation: " + result);
    }
}
