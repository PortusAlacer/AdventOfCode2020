using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day3 : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_Input = null;

    [SerializeField]
    private List<Vector2Int> m_Steps = new List<Vector2Int>();

    void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');

        // Remove last empty line
        int rows = inputLines.Length - 1;
        // Remove return character
        int cols = inputLines[0].ToCharArray().Length;

        //rows by cols
        bool[,]  m_MapTrees = new bool[rows, cols];

        char treeSpace = '#';

        for (int r = 0; r < rows; r++)
        {
            char[] entries = inputLines[r].ToCharArray();

            Debug.Assert(entries.Length == cols, "Number of cols is not correct for line " + r);

            for (int c = 0; c < cols; c++)
            {
                m_MapTrees[r, c] = treeSpace == entries[c];
            }
        }

        List<int> treeCounters = new List<int>();
        int treeCountersMultiplied = 1;

        foreach (Vector2Int step in m_Steps)
        {
            int treeCounter = 0;
            Vector2Int position = Vector2Int.zero;
            while (position.x < rows)
            {
                treeCounter += m_MapTrees[position.x, position.y] ? 1 : 0;

                position += step;
                position.y = position.y % cols;
            }

            treeCounters.Add(treeCounter);
            treeCountersMultiplied *= treeCounter;
            Debug.Log("Number of trees encountered for step " + step.x + "," + step.y + " is : " + treeCounter);
        }

        Debug.Log("Result: " + treeCountersMultiplied);
    }
}
