using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day4 : MonoBehaviour
{
    [Serializable]
    private class Passport
    {
        private string m_byr = string.Empty;
        private string m_iyr = string.Empty;
        private string m_eyr = string.Empty;
        private string m_hgt = string.Empty;
        private string m_hcl = string.Empty;
        private string m_ecl = string.Empty;
        private string m_pid = string.Empty;
        //optional
        private string m_cid = string.Empty;

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(m_byr) &&
                        !string.IsNullOrEmpty(m_iyr) &&
                        !string.IsNullOrEmpty(m_eyr) &&
                        !string.IsNullOrEmpty(m_hgt) &&
                        !string.IsNullOrEmpty(m_hcl) &&
                        !string.IsNullOrEmpty(m_ecl) &&
                        !string.IsNullOrEmpty(m_pid));
            }
        }

        public void AddEntry(string id, string info)
        {
            switch (id)
            {
                case "byr":
                    ParseBYR(info);
                    break;
                case "iyr":
                    ParseIYR(info);
                    break;
                case "eyr":
                    ParseEYR(info);
                    break;
                case "hgt":
                    ParseHGT(info);
                    break;
                case "hcl":
                    ParseHCL(info);
                    break;
                case "ecl":
                    ParseECL(info);
                    break;
                case "pid":
                    ParsePID(info);
                    break;
                case "cid":
                    ParseCID(info);
                    break;
            }
        }

        private void ParseBYR(string info)
        {
            int year = int.Parse(info);
            if (year >= 1920 && year <= 2002)
            {
                m_byr = info;
            }
        }

        private void ParseIYR(string info)
        {
            int year = int.Parse(info);
            if (year >= 2010 && year <= 2020)
            {
                m_iyr = info;
            }
        }

        private void ParseEYR(string info)
        {
            int year = int.Parse(info);
            if (year >= 2020 && year <= 2030)
            {
                m_eyr = info;
            }
        }

        private void ParseHGT(string info)
        {
            int.TryParse(string.Join("", info.Take(info.Length - 2)), out int value);
            string units = string.Join("", info.Skip(info.Length - 2).Take(2));
            if (units == "cm")
            {
                if (value >= 150 && value <= 193)
                {
                    m_hgt = info;
                }
            }
            else
            {
                if (value >= 59 && value <= 76)
                {
                    m_hgt = info;
                }
            }
        }

        private void ParseHCL(string info)
        {
            char[] availableColorCharacters = new char[17] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'a', 'b', 'c', 'd', 'e', 'f' };

            char[] characters = info.ToCharArray();

            if (characters[0] != '#')
            {
                return;
            }

            if (characters.Length != 7)
            {
                return;
            }

            for (int i = 1; i < 7; i++)
            {
                if (!availableColorCharacters.Contains(characters[i]))
                {
                    return;
                }
            }

            m_hcl = info;
        }

        private void ParseECL(string info)
        {
            string[] availableColors = new string[7] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

            if (availableColors.Contains(info))
            {
                m_ecl = info;
            }
        }

        private void ParsePID(string info)
        {
            if (info.Length == 9)
            {
                m_pid = info;
            }
        }

        private void ParseCID(string info)
        {
            m_cid = info;
        }
    }

    [SerializeField]
    private TextAsset m_Input = null;

    private List<Passport> m_Passports = new List<Passport>();

    private void Start()
    {
        string input = m_Input.text;
        Queue<string> inputLines = new Queue<string>(input.Split('\n'));

        while (inputLines.Count > 0)
        {
            Passport newPassport = new Passport();

            string newLine = inputLines.Dequeue().Replace("/n", "");
            while (!string.IsNullOrEmpty(newLine))
            {
                string[] entries = newLine.Split(' ');

                foreach(string entry in entries)
                {
                    string[] entrySplitted = entry.Split(':');

                    newPassport.AddEntry(entrySplitted[0], entrySplitted[1]);
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

            Debug.Log(newPassport.IsValid);

            m_Passports.Add(newPassport);
        }

        Debug.LogError("Number of valid passports: " + m_Passports.Count(p => p.IsValid));
    }
}
