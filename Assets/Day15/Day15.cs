using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day15 : MonoBehaviour
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

        foreach (string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string[] numbersString = line.Split(',');
            List<int> numbers = new List<int>();

            foreach(string number in numbersString)
            {
                numbers.Add(int.Parse(number));
            }

            Debug.LogWarning("Final Iteration Number: " + RunGame(numbers));
        }
    }

    private int RunGame(List<int> numbers)
    {
        Dictionary<int, List<int>> occurences = new Dictionary<int, List<int>>();

        int iteration = 1;

        foreach(int numberSpoken in numbers)
        {
            List<int> positions = new List<int>();

            if (!occurences.TryGetValue(numberSpoken, out positions))
            {
                positions = new List<int>();
                occurences[numberSpoken] = positions;
            }

            occurences[numberSpoken].Add(iteration);

            iteration++;
        }

        for (; iteration <= 2020; iteration++)
        {
            int previousNumber = numbers[iteration - 1 - 1];
            int newNumber = 0;

            if (occurences[previousNumber].Count > 1)
            {
                List<int> positions = occurences[previousNumber];
                newNumber = positions[positions.Count - 1] - positions[positions.Count - 2];
            }

            numbers.Add(newNumber);

            if (!occurences.ContainsKey(newNumber))
            {
                occurences[newNumber] = new List<int>();
            }

            occurences[newNumber].Add(iteration);
        }

        return numbers[iteration - 1 - 1];
    }
}
