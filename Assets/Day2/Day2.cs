using System;
using System.Collections.Generic;
using UnityEngine;

public class Day2 : MonoBehaviour
{
    [Serializable]
    private class PasswordEntry
    {
        public string Password;
        public string LetterCheck;
        public int MinAppearences;
        public int MaxAppearences;

        public int FirstPositionIndex;
        public int SecondPositionIndex;

        public bool IsValid
        {
            get
            {
                int counter = Password.Length - Password.Replace(LetterCheck, "").Length;

                return (counter >= MinAppearences && counter <= MaxAppearences);
            }
        }

        public bool IsValidByPosition
        {
            get
            {
                char[] chars = Password.ToCharArray();
                string firstPosition = chars[FirstPositionIndex].ToString();
                string secondPosition = chars[SecondPositionIndex].ToString();

                return (firstPosition == LetterCheck && secondPosition != LetterCheck ||
                        firstPosition != LetterCheck && secondPosition == LetterCheck);
            }
        }
    }

    [SerializeField]
    private TextAsset m_Input = null;

    private List<PasswordEntry> m_Passwords = new List<PasswordEntry>();

    void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');

        foreach (string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string[] firstSplit = line.Split(':');
            string password = firstSplit[1].Replace(" ", "").Replace("/r", "");

            string[] secondSplit = firstSplit[0].Split(' ');
            string letterCheck = secondSplit[1];

            string[] thirdSplit = secondSplit[0].Split('-');
            int min = int.Parse(thirdSplit[0]);
            int max = int.Parse(thirdSplit[1]);

            m_Passwords.Add(new PasswordEntry()
            {
                Password = password,
                LetterCheck = letterCheck,
                MinAppearences = min,
                MaxAppearences = max,
                FirstPositionIndex = min - 1,
                SecondPositionIndex = max - 1,
            });
        }

        int counterValid = 0;
        int counterValidByPosition = 0;
        foreach (PasswordEntry password in m_Passwords)
        {
            counterValid += password.IsValid ? 1 : 0;
            counterValidByPosition += password.IsValidByPosition ? 1 : 0;
        }

        Debug.Log(counterValid + " valid passwords");
        Debug.Log(counterValidByPosition + " valid passwords by position");
    }
}
