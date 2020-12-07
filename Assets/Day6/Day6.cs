using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day6 : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_Input = null;

    private List<HashSet<char>> m_Answers = new List<HashSet<char>>();

    void Start()
    {
        string input = m_Input.text;
        Queue<string> inputLines = new Queue<string>(input.Split('\n'));

        while (inputLines.Count > 0)
        {
            HashSet<char> thisGroupAnswers = new HashSet<char>();

            string newLine = inputLines.Dequeue().Replace("/n", "");
            while (!string.IsNullOrEmpty(newLine))
            {
                char[] thisPersonAnswers = newLine.ToCharArray();

                foreach(char answer in thisPersonAnswers)
                {
                    thisGroupAnswers.Add(answer);
                }

                if (inputLines.Count > 0)
                {
                    newLine = inputLines.Dequeue().Replace("/n", "");
                }
                else
                {
                    newLine = string.Empty;
                }
            }

            Debug.Log("Number of answers: " + thisGroupAnswers.Count);
            m_Answers.Add(thisGroupAnswers);
        }

        int count = 0;
        m_Answers.ForEach(a => count += a.Count);

        Debug.LogError("Total answers: " + count);
    }
}
