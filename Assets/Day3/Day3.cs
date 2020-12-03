using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day3 : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_Input;

    [SerializeField]
    private bool[,] m_MapTrees;

    void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');

        // Remove last empty line
        int rows = inputLines.Length - 1;
        // Remove return character
        int cols = inputLines[0].ToCharArray().Length;

        //rows by cols
        m_MapTrees = new bool[rows, cols];

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

        Vector2Int position = Vector2Int.zero;
        Vector2Int step = new Vector2Int(1, 3);

        int treeCounter = 0;

        while (position.x < rows)
        {
            treeCounter += m_MapTrees[position.x, position.y] ? 1 : 0;

            position += step;
            position.y = position.y % cols;
        }

        Debug.Log("Number of trees encountered: " + treeCounter);
    }
}
