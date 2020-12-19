using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyBitArray
{
    internal bool[] m_Array;

    public MyBitArray(int capacity, bool defaultValue = false)
    {
        m_Array = new bool[capacity];

        for (int i = 0; i < m_Array.Length; i++)
        {
            m_Array[i] = defaultValue;
        }
    }


    public MyBitArray(int capacity, Int64 decimalValue)
    {
        m_Array = new bool[capacity];

        for (int i = 0; i < m_Array.Length; i++)
        {
            m_Array[i] = false;
        }

        bool[] decimalInBinary = Convert.ToString(decimalValue, 2).Select(s => s.Equals('1')).ToArray();

        int idArray = m_Array.Length - 1;
        for (int i = decimalInBinary.Length - 1; i >= 0; i--)
        {
            m_Array[idArray] = decimalInBinary[i];
            idArray--;
        }
    }

    public bool this[int id]
    {
        get => m_Array[id];
        set { m_Array[id] = value; }
    }
}

public static class MyBitArrayExtensions
{
    public static MyBitArray And(this MyBitArray me, MyBitArray other)
    {
        MyBitArray result = new MyBitArray(me.m_Array.Length);
        for (int i = 0; i < me.m_Array.Length; i++)
        {
            result[i] = me[i] && other[i];
        }
        return result;
    }

    public static MyBitArray Or(this MyBitArray me, MyBitArray other)
    {
        MyBitArray result = new MyBitArray(me.m_Array.Length);
        for (int i = 0; i < me.m_Array.Length; i++)
        {
            result[i] = me[i] || other[i];
        }
        return result;
    }

    public static Int64 ToInt64(this MyBitArray me)
    {
        Int64 res = 0;
        for (int i = 0; i < me.m_Array.Length; i++)
        {
            Int64 val = me.m_Array[me.m_Array.Length - 1 - i] ? 1 : 0;
            Int64 valShifted = val << i;
            res += valShifted;
        }
        return res;
    }
}

public class Day14 : MonoBehaviour
{
    [Serializable]
    private class MemoryInfo
    {
        [Serializable]
        private class Instruction
        {
            [Serializable]
            private class Override
            {
                public int Bit;
                public Int64 Value;
            }

            private const char EQUAL_SEPARATOR = ']';

            private MyBitArray m_AndMask = new MyBitArray(36, true);
            private MyBitArray m_OrMask = new MyBitArray(36, false);

            private List<Override> m_Overrides = new List<Override>();

            public Instruction(string mask, List<string> overrides)
            {
                List<char> maskList = mask.Skip(7).ToList();

                for (int i = maskList.Count - 1; i >= 0; i--)
                {
                    switch (maskList[i])
                    {
                        case 'X':
                            break;
                        case '1':
                            m_OrMask[i] = true;
                            break;
                        case '0':
                            m_AndMask[i] = false;
                            break;
                    }
                }

                foreach (string over in overrides)
                {
                    string[] splitted = over.Split(EQUAL_SEPARATOR);
                    Int64 value = Int64.Parse(new string(splitted[1].Skip(3).ToArray()));
                    int bit = int.Parse(new string(splitted[0].Skip(4).ToArray()));

                    m_Overrides.Add(new Override()
                    {
                        Bit = bit,
                        Value = value
                    });
                }
            }

            internal void Apply(ref Dictionary<int, MyBitArray> memory)
            {
                foreach(Override over in m_Overrides)
                {
                    MyBitArray valueBit = new MyBitArray(36, over.Value);
                    memory[over.Bit] = valueBit.And(m_AndMask).Or(m_OrMask);
                }
            }
        }

        private Dictionary<int, MyBitArray> m_Memory = new Dictionary<int, MyBitArray>();

        private List<Instruction> m_Instructions = new List<Instruction>();

        public void AddInstruction(string mask, List<string> overrides)
        {
            m_Instructions.Add(new Instruction(mask, overrides));
        }

        public void Execute()
        {
            foreach(Instruction instruction in m_Instructions)
            {
                instruction.Apply(ref m_Memory);
            }
        }

        public Int64 GetSum()
        {
            Int64 sum = 0;
            foreach(MyBitArray memoryUnits in m_Memory.Values)
            {
                sum += memoryUnits.ToInt64();
            }

            return sum;
        }
    }

    [SerializeField]
    private List<TextAsset> m_Inputs = null;

    private List<MemoryInfo> m_Memories = new List<MemoryInfo>();

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

        string currentMask = inputLines[0];
        List<string> overrides = new List<string>();

        MemoryInfo memory = new MemoryInfo();
        m_Memories.Add(memory);

        for (int i = 1; i < inputLines.Length; i++)
        {
            string line = inputLines[i];

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string start = new string(line.Take(3).ToArray());

            switch(start)
            {
                case "mem":
                    overrides.Add(line);
                    break;
                case "mas":
                    memory.AddInstruction(currentMask, overrides);

                    currentMask = line;
                    overrides = new List<string>();

                    break;
            }
        }

        if (overrides.Count > 0)
        {
            memory.AddInstruction(currentMask, overrides);
        }

        memory.Execute();

        Debug.LogWarning("Result: " + memory.GetSum());
    }
}
