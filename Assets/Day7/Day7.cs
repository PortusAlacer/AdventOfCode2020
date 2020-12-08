using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day7 : MonoBehaviour
{
    [Serializable]
    private class Bag
    {
        public string ID;

        public Dictionary<string, int> Contents = new Dictionary<string, int>();

        public Bag(string id)
        {
            ID = id;
        }

        public bool CanHoldBag(string bagID)
        {
            foreach(string key in Contents.Keys)
            {
                if (m_PossibleBags.Contains(key))
                {
                    return true;
                }

                if (key == bagID)
                {
                    return true;
                }

                if (m_BagRules[key].CanHoldBag(bagID))
                {
                    return true;
                }
            }

            return false;
        }

        public int GetCountInsideBags()
        {
            int count = 1;

            foreach (KeyValuePair<string, int> bag in Contents)
            {
                count += bag.Value * m_BagRules[bag.Key].GetCountInsideBags();
            }

            return count;
        }
    }


    [SerializeField]
    private TextAsset m_Input = null;

    [SerializeField]
    private static Dictionary<string, Bag> m_BagRules = new Dictionary<string, Bag>();

    private static HashSet<string> m_PossibleBags = new HashSet<string>();

    private void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');

        foreach(string line in inputLines)
        {
            ProcessRule(line);
        }

        CountBags("shiny gold");

        CountInsideBags("shiny gold");
    }

    private void ProcessRule(string line)
    {
        if (string.IsNullOrEmpty(line))
        {
            return;
        }

        string[] bagsContainSeparator = new string[1] { " bags contain " };
        string[] enumeratorSepartor = new string[1] { ", " };

        string[] firstSplit = line.Split(bagsContainSeparator, System.StringSplitOptions.RemoveEmptyEntries);
        string[] secondSplit = firstSplit[1].Replace(".","").Split(enumeratorSepartor, System.StringSplitOptions.RemoveEmptyEntries);

        string id = firstSplit[0];

        Bag bag = null;
        if (!m_BagRules.TryGetValue(id, out bag))
        {
            bag = new Bag(id);
            m_BagRules[id] = bag;
        }

        string noBags = "no other bags";

        if (secondSplit[0] == noBags)
        {
            return;
        }

        foreach (string bagString in secondSplit)
        {
            string[] splitted = bagString.Split(' ');

            int quatity = int.Parse(splitted[0]);
            string contentID = string.Join(" ", splitted[1], splitted[2]);

            Bag insideBag = null;
            if (!m_BagRules.TryGetValue(contentID, out insideBag))
            {
                insideBag = new Bag(contentID);
                m_BagRules[contentID] = insideBag;
            }

            bag.Contents[contentID] = quatity;
        }
    }

    private void CountBags(string target)
    {
        foreach (KeyValuePair<string, Bag> bag in m_BagRules)
        {
            if (bag.Value.CanHoldBag(target))
            {
                m_PossibleBags.Add(bag.Key);
            }
        }

        Debug.LogError("Possible bags " + m_PossibleBags.Count);
    }

    private void CountInsideBags(string target)
    {
        Debug.LogError(target + "bag has " + (m_BagRules[target].GetCountInsideBags() - 1) + " bags inside");
    }
}
