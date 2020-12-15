using System;
using System.Collections.Generic;
using UnityEngine;

public class Day13 : MonoBehaviour
{
    [SerializeField]
    private List<TextAsset> m_Inputs = null;

    private void Start()
    {
        for(int i = 0; i < m_Inputs.Count; i++)
        {
            Debug.LogError("Running asset " + m_Inputs[i].name);
            float startTime = Time.time;
            Run(i, m_Inputs[i]);
            Debug.Log("Finished asset " + i + " it took " + (Time.time - startTime)  + " seconds");
        }
    }

    private void Run(int dataID, TextAsset inputAsset)
    {
        string input = inputAsset.text;
        string[] inputLines = input.Split('\n');

        int minimumDeparture = int.Parse(inputLines[0]);

        string[] buses = inputLines[1].Split(',');

        int busDeparture = int.MaxValue;
        int busID = 0;

        foreach(string bus in buses)
        {
            if (bus == "x")
            {
                continue;
            }

            int interval = int.Parse(bus);

            int diffBefore = minimumDeparture % interval;
            int departure = minimumDeparture - diffBefore + interval;

            if (departure < busDeparture)
            {
                busDeparture = departure;
                busID = interval;
            }
        }

        Debug.LogWarning("Departure at " + busDeparture + " on bus " + busID);
        Debug.LogWarning("Results: " + (busID * (busDeparture - minimumDeparture)));

        List<ulong> busesInterval = new List<ulong>();
        List<ulong> busesOffSets = new List<ulong>();
        List<ulong> busesNextDeparture = new List<ulong>();

        for (ulong i = 0; i < (ulong)buses.Length; i++)
        {
            if (buses[i] == "x")
            {
                continue;
            }

            ulong interval = ulong.Parse(buses[i]);

            busesInterval.Add(interval);
            busesOffSets.Add(i);
        }

        ulong time = 1;
        
        while (!CheckIntervals(time, busesInterval, busesOffSets))
        {
            time++;
        }

        Debug.LogWarning("Minimum time is: " + time);
    }

    private bool CheckIntervals(ulong time, List<ulong> intervals, List<ulong> offsets)
    {
        bool aligned = true;

        for (int i = 0; i < intervals.Count; i++)
        {
            aligned &= ((time + offsets[i]) % intervals[i] == 0);
        }

        return aligned;
    }

    // worthless
    // https://www.geeksforgeeks.org/lcm-of-given-array-elements/
    // Returns LCM of array elements 
    private ulong FindLCM(ulong[] arr)
    {
        // Initialize result 
        ulong ans = arr[0];

        // ans contains LCM of arr[0], ..arr[i] 
        // after i'th iteration, 
        for (int i = 1; i < arr.Length; i++)
        {
            ans = ((arr[i] * ans)) / (GBC(arr[i], ans));
        }

        return ans;
    }

    // Utility function to find 
    // GCD of 'a' and 'b' 
    private ulong GBC(ulong a, ulong b)
    {
        if (b == 0)
        {
            return a;
        }
        return GBC(b, a % b);
    }
}
