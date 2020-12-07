using System;
using System.Collections.Generic;
using UnityEngine;

public class Day6 : MonoBehaviour
{
    [Serializable]
    private class PassengerAnswers
    {
        public HashSet<char> Answers = new HashSet<char>();

        public PassengerAnswers(string answers)
        {
            char[] thisPersonAnswers = answers.ToCharArray();

            foreach (char answer in answers)
            {
                Answers.Add(answer);
            }
        }

        public bool Answered(char answer)
        {
            return Answers.Contains(answer);
        }

        public void ParseUniqueAnwswers(ref HashSet<char> answered)
        {
            foreach(char answer in Answers)
            {
                answered.Add(answer);
            }
        }
    }

    [Serializable]
    private class GroupAnswers
    {
        public List<PassengerAnswers> Passengers = new List<PassengerAnswers>();

        public void AddPassenger(string answers)
        {
            Passengers.Add(new PassengerAnswers(answers));
        }

        public int GetValidAnswers()
        {
            HashSet<char> firstPassengerAnswers = Passengers[0].Answers;
            int totalCount = 0;

            foreach (char answer in firstPassengerAnswers)
            {
                int count = 0;
                Passengers.ForEach(p => count += p.Answered(answer) ? 1 : 0);

                if (count == Passengers.Count)
                {
                    totalCount++;
                }
            }

            return totalCount;
        }

        public int GetUnitqueAnsweres()
        {
            HashSet<char> uniques = new HashSet<char>();

            Passengers.ForEach(p => p.ParseUniqueAnwswers(ref uniques));

            return uniques.Count;
        }
    }

    [SerializeField]
    private TextAsset m_Input = null;

    private List<GroupAnswers> m_GroupAnswers = new List<GroupAnswers>();

    void Start()
    {
        string input = m_Input.text;
        Queue<string> inputLines = new Queue<string>(input.Split('\n'));

        int countValidAnswers = 0;
        int countUniqueAnswers = 0;

        while (inputLines.Count > 0)
        {
            GroupAnswers groupAnswers = new GroupAnswers();         

            string newLine = inputLines.Dequeue().Replace("/n", "");
            while (!string.IsNullOrEmpty(newLine))
            {
                groupAnswers.AddPassenger(newLine);

                if (inputLines.Count > 0)
                {
                    newLine = inputLines.Dequeue().Replace("/n", "");
                }
                else
                {
                    newLine = string.Empty;
                }
            }

            countUniqueAnswers += groupAnswers.GetUnitqueAnsweres();
            Debug.Log("Number unique of answers: " + groupAnswers.GetUnitqueAnsweres());
            countValidAnswers += groupAnswers.GetValidAnswers();
            Debug.Log("Number valid of answers: " + groupAnswers.GetValidAnswers());
            m_GroupAnswers.Add(groupAnswers);
        }

        Debug.LogError("Total unique answers: " + countUniqueAnswers);
        Debug.LogError("Total valid answers: " + countValidAnswers);
    }
}
