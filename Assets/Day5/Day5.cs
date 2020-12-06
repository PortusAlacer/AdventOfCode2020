using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day5 : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_Input = null;

    private const int rows = 128;
    private const int cols = 8;

    private void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');

        //rows by cols
        bool[] seats = Enumerable.Repeat(false, rows * cols).ToArray();

        foreach(string line in inputLines)
        {
            MakeInstructions(line, out Queue<bool> rowsInstructions, out Queue<bool> colsInstructions);

            int row = BinarySearch(rowsInstructions, 0, rows - 1);
            int col = BinarySearch(colsInstructions, 0, cols - 1);

            seats[row * cols + col] = true;

            Debug.Log(line + " - row " + row + " - col " + col + " - id " + (row * cols + col).ToString());
        }

        Debug.LogError(seats.ToList().FindLastIndex(s => s));
    }

    private void MakeInstructions(string line, out Queue<bool> rowsInstructions, out Queue<bool> colsInstructions)
    {
        char[] characters = line.ToCharArray();

        rowsInstructions = new Queue<bool>();
        colsInstructions = new Queue<bool>();

        for (int i = 0; i < 7; ++i)
        {
            rowsInstructions.Enqueue((characters[i] == 'F'));
        }

        for (int j = 7; j < 10; ++j)
        {
            colsInstructions.Enqueue((characters[j] == 'L'));
        }
    }

    private int BinarySearch(Queue<bool> instructions, int min, int max)
    {
        bool instruction = instructions.Dequeue();

        if (instructions.Count == 0)
        {
            return instruction ? min : max;
        }

        int middle = (max - min) / 2 + min;

        if (instruction)
        {
            return BinarySearch(instructions, min, middle);
        }
        else
        {
            return BinarySearch(instructions, middle + 1, max);
        }
    }
}
