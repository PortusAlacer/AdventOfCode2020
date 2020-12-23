using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day17 : MonoBehaviour
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
            Debug.Log("Finished asset " + i + " it took " + (Time.time - startTime)  + " seconds");
        }
    }

    private void Run(int dataID, TextAsset inputAsset)
    {
        string input = inputAsset.text;
        string[] inputLines = input.Split('\n');

        Dictionary<Vector3Int, bool> pocketDimension = new Dictionary<Vector3Int, bool>();
        HashSet<Vector3Int> neighboors = new HashSet<Vector3Int>();
        for (int y = 0; y < inputLines.Length; y++)
        {
            string line = inputLines[y];

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            char[] splitLine = line.ToCharArray();

            for (int x = 0; x < splitLine.Length; x++)
            {
                bool active = splitLine[x] == '#';

                Vector3Int position = new Vector3Int(x, y, 0);

                pocketDimension[position] = active;

                if (active)
                {
                    AddNeighbors(position, ref neighboors);
                }
            }
        }

        int maxIterations = 6;

        for (int i = 0; i < maxIterations; i++)
        {
            Dictionary<Vector3Int, bool> newPocketDimension = new Dictionary<Vector3Int, bool>();
            HashSet<Vector3Int> newNeighboors = new HashSet<Vector3Int>();
            
            foreach (Vector3Int cell in neighboors)
            {
                Vector3Int minPosition = cell + new Vector3Int(-1, -1, -1);
                Vector3Int maxPosition = cell + new Vector3Int( 1,  1,  1);

                int countActiveNeighbors = 0;

                for (int x = minPosition.x; x <= maxPosition.x; x++)
                {
                    for (int y = minPosition.y; y <= maxPosition.y; y++)
                    {
                        for (int z = minPosition.z; z <= maxPosition.z; z++)
                        {
                            Vector3Int neighboor = new Vector3Int(x, y, z);
                            if (Vector3Int.Distance(cell, neighboor) == 0)
                            {
                                continue;
                            }

                            if (pocketDimension.TryGetValue(neighboor, out bool active))
                            {
                                countActiveNeighbors += active ? 1 : 0;
                            }
                        }
                    }
                }

                bool activeState = false;
                if (pocketDimension.ContainsKey(cell) && pocketDimension[cell])
                {
                    activeState = countActiveNeighbors == 2 || countActiveNeighbors == 3;
                }
                else
                {
                    activeState = countActiveNeighbors == 3;
                }

                newPocketDimension[cell] = activeState;

                if (newPocketDimension[cell])
                {
                    AddNeighbors(cell, ref newNeighboors);
                }
            }

            pocketDimension = newPocketDimension;
            neighboors = newNeighboors;
        }

        Debug.LogWarning("Number of active cells: " + pocketDimension.Values.Count(cell => cell));
    }

    private void AddNeighbors(Vector3Int position, ref HashSet<Vector3Int> neighboors)
    {
        Vector3Int minPosition = position + new Vector3Int(-1, -1, -1);
        Vector3Int maxPosition = position + new Vector3Int(1, 1, 1);

        for (int xn = minPosition.x; xn <= maxPosition.x; xn++)
        {
            for (int yn = minPosition.y; yn <= maxPosition.y; yn++)
            {
                for (int zn = minPosition.z; zn <= maxPosition.z; zn++)
                {
                    Vector3Int cell = new Vector3Int(xn, yn, zn);
                    //if (Vector3Int.Distance(cell, position) == 1)
                    //{
                        neighboors.Add(cell);
                    //}
                }
            }
        }
    }
}
