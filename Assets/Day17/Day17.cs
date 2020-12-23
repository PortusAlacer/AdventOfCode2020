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

        Run3D(inputLines);
        Run4D(inputLines);
    }

    private void AddNeighbors3D(Vector3Int position, ref HashSet<Vector3Int> neighboors)
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
                    neighboors.Add(cell);
                }
            }
        }
    }

    private void Run3D(string[] inputLines)
    {
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
                    AddNeighbors3D(position, ref neighboors);
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
                Vector3Int maxPosition = cell + new Vector3Int(1, 1, 1);

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
                    AddNeighbors3D(cell, ref newNeighboors);
                }
            }

            pocketDimension = newPocketDimension;
            neighboors = newNeighboors;
        }

        Debug.LogWarning("Number of 3D active cells: " + pocketDimension.Values.Count(cell => cell));
    }

    private void AddNeighbors4D(Vector4 position, ref HashSet<Vector4> neighboors)
    {
        Vector4 minPosition = position + new Vector4(-1f, -1f, -1f, -1f);
        Vector4 maxPosition = position + new Vector4(1f, 1f, 1f, 1f);

        for (int xn = (int)minPosition.x; xn <= (int)maxPosition.x; xn++)
        {
            for (int yn = (int)minPosition.y; yn <= (int)maxPosition.y; yn++)
            {
                for (int zn = (int)minPosition.z; zn <= (int)maxPosition.z; zn++)
                {
                    for (int wn = (int)minPosition.w;  wn <= (int)maxPosition.w; wn++)
                    {
                        Vector4 cell = new Vector4(xn, yn, zn, wn);
                        neighboors.Add(cell);
                    }
                }
            }
        }
    }

    private void Run4D(string[] inputLines)
    {
        Dictionary<Vector4, bool> pocketDimension = new Dictionary<Vector4, bool>();
        HashSet<Vector4> neighboors = new HashSet<Vector4>();
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

                Vector4 position = new Vector4(x, y, 0, 0);

                pocketDimension[position] = active;

                if (active)
                {
                    AddNeighbors4D(position, ref neighboors);
                }
            }
        }

        int maxIterations = 6;

        for (int i = 0; i < maxIterations; i++)
        {
            Dictionary<Vector4, bool> newPocketDimension = new Dictionary<Vector4, bool>();
            HashSet<Vector4> newNeighboors = new HashSet<Vector4>();

            foreach (Vector4 cell in neighboors)
            {
                Vector4 minPosition = cell + new Vector4(-1, -1, -1, -1);
                Vector4 maxPosition = cell + new Vector4(1, 1, 1, 1);

                int countActiveNeighbors = 0;

                for (int x = (int)minPosition.x; x <= (int)maxPosition.x; x++)
                {
                    for (int y = (int)minPosition.y; y <= (int)maxPosition.y; y++)
                    {
                        for (int z = (int)minPosition.z; z <= (int)maxPosition.z; z++)
                        {
                            for (int w = (int)minPosition.w; w <= (int)maxPosition.w; w++)
                            {
                                Vector4 neighboor = new Vector4(x, y, z, w);
                                if (Vector4.Distance(cell, neighboor) == 0)
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
                    AddNeighbors4D(cell, ref newNeighboors);
                }
            }

            pocketDimension = newPocketDimension;
            neighboors = newNeighboors;
        }

        Debug.LogWarning("Number of 4D active cells: " + pocketDimension.Values.Count(cell => cell));
    }
}
