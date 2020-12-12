using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day10 : MonoBehaviour
{
    private class TreeNode
    {
        public int Value;
        public List<TreeNode> Childreen = new List<TreeNode>();

        private long m_CountBranches = -1;

        public TreeNode(int value, List<bool> nextNodes, ref Dictionary<int, TreeNode> allNodes)
        {
            Value = value;

            allNodes[value] = this;

            if (nextNodes == null || nextNodes.Count() == 0)
            {
                return;
            }
            if (nextNodes[0])
            {
                if (allNodes.TryGetValue(value + 1, out TreeNode node))
                {
                    Childreen.Add(node);
                }
                else
                {
                    List<bool> subNodes = nextNodes.Skip(1).ToList();
                    Childreen.Add(new TreeNode(value + 1, subNodes, ref allNodes));
                }
            }

            if (nextNodes.Count() <= 1)
            {
                return;
            }
            if (nextNodes[1])
            {
                if (allNodes.TryGetValue(value + 2, out TreeNode node))
                {
                    Childreen.Add(node);
                }
                else
                {
                    List<bool> subNodes = nextNodes.Skip(2).ToList();
                    Childreen.Add(new TreeNode(value + 2, subNodes, ref allNodes));
                }
            }

            if (nextNodes.Count() <= 2)
            {
                return;
            }
            if (nextNodes[2])
            {
                if (allNodes.TryGetValue(value + 3, out TreeNode node))
                {
                    Childreen.Add(node);
                }
                else
                {
                    List<bool> subNodes = nextNodes.Skip(3).ToList();
                    Childreen.Add(new TreeNode(value + 3, subNodes, ref allNodes));
                }
            }
        }

        public bool IsLeaf
        {
            get
            {
                return Childreen == null || Childreen.Count == 0;
            }
        }

        public long CountBranches()
        {
            if (m_CountBranches != -1)
            {
                return m_CountBranches;
            }

            if (IsLeaf)
            {
                m_CountBranches = 1;
                return m_CountBranches;
            }

            m_CountBranches = 0;
            Childreen.ForEach(node => m_CountBranches += node.CountBranches());
            return m_CountBranches;
        }
    }

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

            switch (diff)
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

        //second part
        bool[] availableChargers = new bool[chargers.Max() + 3];
        availableChargers.Fill<bool>(false);

        for (int i = 0; i < chargers.Count; i++)
        {
            availableChargers[chargers[i] - 1] = true;
        }
        // the extra charger
        availableChargers[availableChargers.Length - 1] = true;

        Dictionary<int, TreeNode> allNodes = new Dictionary<int, TreeNode>();

        TreeNode chargersTree = new TreeNode(0, availableChargers.ToList(), ref allNodes);

        Debug.LogWarning("Number of possibilities: " + chargersTree.CountBranches());
    }
}

public static class ArrayExtensions
{
    public static void Fill<T>(this T[] originalArray, T with)
    {
        for (int i = 0; i < originalArray.Length; i++)
        {
            originalArray[i] = with;
        }
    }
}
