using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day12 : MonoBehaviour
{
    private readonly Vector3 NORTH = new Vector3(0f, 0f, 1f);
    private readonly Vector3 SOUTH = new Vector3(0f, 0f, -1f);
    private readonly Vector3 EAST = new Vector3(1f, 0f, 0f);
    private readonly Vector3 WEST = new Vector3(-1f, 0f, 0f);

    [SerializeField]
    private List<TextAsset> m_Inputs = null;

    private void Start()
    {
        for (int i = 0; i < m_Inputs.Count; i++)
        {
            Debug.LogError("Running asset " + i);
            float startTime = Time.time;
            Run(i, m_Inputs[i]);
            Run2(i, m_Inputs[i]);
            Debug.LogError("Finished asset " + i + " it took " + (Time.time - startTime) + " seconds");
        }
    }

    private void Run(int dataID, TextAsset inputAsset)
    {
        string input = inputAsset.text;
        string[] inputLines = input.Split('\n');

        GameObject ferry = new GameObject(inputAsset.name);
        Transform ferryTransform = ferry.transform;
        ferryTransform.rotation = Quaternion.Euler(0f, 90f, 0f);

        foreach (string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            char[] linesChars = line.ToCharArray();

            char direction = linesChars[0];

            float ammount = int.Parse(new string(linesChars.Skip(1).ToArray()));

            switch (direction)
            {
                case 'N':
                    ferryTransform.Translate(NORTH * ammount, Space.World);
                    break;
                case 'S':
                    ferryTransform.Translate(SOUTH * ammount, Space.World);
                    break;
                case 'E':
                    ferryTransform.Translate(EAST * ammount, Space.World);
                    break;
                case 'W':
                    ferryTransform.Translate(WEST * ammount, Space.World);
                    break;
                case 'L':
                    ferryTransform.Rotate(Vector3.up, -ammount);
                    break;
                case 'R':
                    ferryTransform.Rotate(Vector3.up, ammount);
                    break;
                case 'F':
                    ferryTransform.Translate(Vector3.forward * ammount, Space.Self);
                    break;
            }
        }

        Debug.LogWarning("Final position is " + ferryTransform.position.x + ":" + ferryTransform.position.z);
        Debug.LogWarning("Result: " + (Mathf.Abs(ferryTransform.position.x) + Mathf.Abs(ferryTransform.position.z)));
    }

    private void Run2(int dataID, TextAsset inputAsset)
    {
        string input = inputAsset.text;
        string[] inputLines = input.Split('\n');

        GameObject ferry = new GameObject(inputAsset.name + "_2");
        Transform ferryTransform = ferry.transform;
        ferryTransform.rotation = Quaternion.Euler(0f, 90f, 0f);

        GameObject waypoint = new GameObject("waypoint");
        Transform waypointTransform = waypoint.transform;
        waypointTransform.SetParent(ferryTransform);
        waypointTransform.position = 10 * EAST + NORTH;

        foreach (string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            char[] linesChars = line.ToCharArray();

            char direction = linesChars[0];

            float ammount = int.Parse(new string(linesChars.Skip(1).ToArray()));

            switch (direction)
            {
                case 'N':
                    waypointTransform.Translate(NORTH * ammount, Space.World);
                    break;
                case 'S':
                    waypointTransform.Translate(SOUTH * ammount, Space.World);
                    break;
                case 'E':
                    waypointTransform.Translate(EAST * ammount, Space.World);
                    break;
                case 'W':
                    waypointTransform.Translate(WEST * ammount, Space.World);
                    break;
                case 'L':
                    waypointTransform.RotateAround(ferryTransform.position, Vector3.up, -ammount);
                    break;
                case 'R':
                    waypointTransform.RotateAround(ferryTransform.position, Vector3.up, ammount);
                    break;
                case 'F':
                    ferryTransform.Translate((waypointTransform.position - ferryTransform.position) * ammount, Space.World);
                    break;
            }
        }

        Debug.LogWarning("Final position is " + ferryTransform.position.x + ":" + ferryTransform.position.z);
        Debug.LogWarning("Result: " + (Mathf.Abs(ferryTransform.position.x) + Mathf.Abs(ferryTransform.position.z)));
    }
}
