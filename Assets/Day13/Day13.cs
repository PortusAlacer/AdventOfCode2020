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
    }
}
