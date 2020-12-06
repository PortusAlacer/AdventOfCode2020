using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day4 : MonoBehaviour
{
    [Serializable]
    private class Passport
    {
        public int byr = -1;
        public int iyr = -1;
        public int eyr = -1;
        //in m
        public string hgt = string.Empty;
        public string hcl = string.Empty;
        public string ecl = string.Empty;
        public string pid = string.Empty;
        //optional
        public int cid = -1;

        public bool IsValid
        {
            get
            {
                return (byr != -1 &&
                        iyr != -1 &&
                        eyr != -1 &&
                        !string.IsNullOrEmpty(hgt) &&
                        !string.IsNullOrEmpty(hcl) &&
                        !string.IsNullOrEmpty(ecl) &&
                        !string.IsNullOrEmpty(pid));
            }
        }

        public void AddEntry(string id, string info)
        {
            switch (id)
            {
                case "byr":
                    byr = int.Parse(info);
                    break;
                case "iyr":
                    iyr = int.Parse(info);
                    break;
                case "eyr":
                    eyr = int.Parse(info);
                    break;
                case "hgt":
                    hgt = info;
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
                    hcl = info;
                    break;
                case "ecl":
                    ecl = info;
                    break;
                case "pid":
                    pid = info;
                    break;
                case "cid":
                    cid = int.Parse(info);
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
