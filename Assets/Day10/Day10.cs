using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day10 : MonoBehaviour
{
    [SerializeField]
    private List<TextAsset> m_Inputs = null;

    private void Start()
    {
        for(int i = 0; i < m_Inputs.Count; i++)
        {
            Debug.LogError("Running asset " + i);
            float startTime = Time.time;
            Run(i, m_Inputs[i]);
            Debug.LogError("Finished asset " + i + " it took " + (Time.time - startTime)  + " seconds");
        }
    }

    private void Run(int dataID, TextAsset inputAsset)
    {
        string input = inputAsset.text;
        string[] inputLines = input.Split('\n');

        List<int> chargers = new List<int>();

        foreach (string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            chargers.Add(int.Parse(line));
        }

        chargers.Sort();

        int count1 = 0;
        int count2 = 0;
        int count3 = 1;

        switch (chargers[0])
        {
            case 1:
                count1++;
                break;
            case 2:
                count2++;
                break;
            case 3:
                count3++;
                break;
        }

        for (int i = 0; i < chargers.Count - 1; i++)
        {
            int diff = chargers[i + 1] - chargers[i];

            switch(diff)
            {
                case 1:
                    count1++;
                    break;
                case 2:
                    count2++;
                    break;
                case 3:
                    count3++;
                    break;
            }
        }

        Debug.Log("Count 1 diff " + count1);
        Debug.Log("Count 2 diff " + count2);
        Debug.Log("Count 3 diff " + count3);
        Debug.LogWarning("Result " + (count1 * count3));
    }
}
