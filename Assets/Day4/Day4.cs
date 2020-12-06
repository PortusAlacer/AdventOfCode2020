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
        private int m_cid = -1;

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
                    m_byr = info;
                    break;
                case "iyr":
                    m_iyr = info;
                    break;
                case "eyr":
                    m_eyr = info;
                    break;
                case "hgt":
                    m_hgt = info;
                    //int value = int.Parse(string.Join("", info.Take(info.Length - 2)));
                    //string units = string.Join("", info.Skip(info.Length - 2).Take(2));
                    //if (units == "cm")
                    //{
                    //    hgt = (float)value / 100f;
                    //}
                    //else
                    //{
                    //    hgt = (float)value / 39.3701f;
                    //}
                    break;
                case "hcl":
                    m_hcl = info;
                    break;
                case "ecl":
                    m_ecl = info;
                    break;
                case "pid":
                    m_pid = info;
                    break;
                case "cid":
                    m_cid = int.Parse(info);
                    break;
            }
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
