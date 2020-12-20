using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyBitArray
{
    public bool[] Array;

    public MyBitArray(int capacity, bool defaultValue = false)
    {
        Array = new bool[capacity];

        for (int i = 0; i < Array.Length; i++)
        {
            Array[i] = defaultValue;
        }
    }


    public MyBitArray(int capacity, Int64 decimalValue)
    {
        Array = new bool[capacity];

        for (int i = 0; i < Array.Length; i++)
        {
            Array[i] = false;
        }

        bool[] decimalInBinary = Convert.ToString(decimalValue, 2).Select(s => s.Equals('1')).ToArray();

        int idArray = Array.Length - 1;
        for (int i = decimalInBinary.Length - 1; i >= 0; i--)
        {
            Array[idArray] = decimalInBinary[i];
            idArray--;
        }
    }

    public MyBitArray(MyBitArray bitArray)
    {
        Array = new bool[bitArray.Array.Length];

        for(int i = 0; i < Array.Length; i++)
        {
            Array[i] = bitArray.Array[i];
        }
    }

    public bool this[int id]
    {
        get => Array[id];
        set { Array[id] = value; }
    }
}

public static class MyBitArrayExtensions
{
    public static MyBitArray And(this MyBitArray me, MyBitArray other)
    {
        MyBitArray result = new MyBitArray(me.Array.Length);
        for (int i = 0; i < me.Array.Length; i++)
        {
            result[i] = me[i] && other[i];
        }
        return result;
    }

    public static MyBitArray Or(this MyBitArray me, MyBitArray other)
    {
        MyBitArray result = new MyBitArray(me.Array.Length);
        for (int i = 0; i < me.Array.Length; i++)
        {
            result[i] = me[i] || other[i];
        }
        return result;
    }

    public static Int64 ToInt64(this MyBitArray me)
    {
        Int64 res = 0;
        for (int i = 0; i < me.Array.Length; i++)
        {
            Int64 val = me.Array[me.Array.Length - 1 - i] ? 1 : 0;
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
                public Int64 Bit;
                public Int64 Value;
            }

            private const char EQUAL_SEPARATOR = ']';

            private MyBitArray m_AndMask = new MyBitArray(36, true);
            private MyBitArray m_OrMask = new MyBitArray(36, false);

            private char[] m_FullMask = new char[36];

            private List<Override> m_Overrides = new List<Override>();

            public Instruction(string mask, List<string> overrides)
            {
                List<char> maskList = mask.Skip(7).ToList();

                for (int i = 0; i < 36; i++)
                {
                    m_FullMask[i] = '0';
                }

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

                    m_FullMask[i] = maskList[i];
                }

                foreach (string over in overrides)
                {
                    string[] splitted = over.Split(EQUAL_SEPARATOR);
                    Int64 value = Int64.Parse(new string(splitted[1].Skip(3).ToArray()));
                    Int64 bit = Int64.Parse(new string(splitted[0].Skip(4).ToArray()));

                    m_Overrides.Add(new Override()
                    {
                        Bit = bit,
                        Value = value
                    });
                }
            }

            internal void Apply(ref Dictionary<Int64, MyBitArray> memory)
            {
                foreach(Override over in m_Overrides)
                {
                    MyBitArray valueBit = new MyBitArray(36, over.Value);
                    memory[over.Bit] = valueBit.And(m_AndMask).Or(m_OrMask);
                }
            }

            internal void ApplyOnAddress(ref Dictionary<Int64, MyBitArray> memory)
            {
                foreach(Override over in m_Overrides)
                {
                    MyBitArray bit = new MyBitArray(36, over.Bit);

                    char[] copyMask = new char[36];
                    m_FullMask.CopyTo(copyMask, 0);

                    List<MyBitArray> possibleBits = GetPossibleAddresses(bit, copyMask);

                    foreach(MyBitArray possibleBit in possibleBits)
                    {
                        Int64 bitId = possibleBit.ToInt64();
                        memory[bitId] = new MyBitArray(36, over.Value);
                    }
                }
            }

            private List<MyBitArray> GetPossibleAddresses(MyBitArray address, char[] mask, int statrIndex = 0)
            {
                List<MyBitArray> addressesResult = new List<MyBitArray>();
                MyBitArray newAddress = new MyBitArray(address);
                for (int i = statrIndex; i < 36; i++)
                {
                    switch (mask[36 - 1- i])
                    {
                        case 'X':
                            char[] copyMask = new char[36];
                            mask.CopyTo(copyMask, 0);
                            copyMask[36 - 1 - i] = '0';

                            newAddress[36 - 1 - i] = true;
                            addressesResult.AddRange(GetPossibleAddresses(newAddress, copyMask, i + 1));

                            newAddress[36 - 1 - i] = false;
                            addressesResult.AddRange(GetPossibleAddresses(newAddress, copyMask, i + 1));
                            return addressesResult;
                        case '1':
                            newAddress[36 - 1 - i] = true;
                            break;
                        case '0':
                            break;
                    }
                }
                //Debug.Log(newAddress.ToInt64());
                addressesResult.Add(newAddress);
                return addressesResult;
            }
        }

        private Dictionary<Int64, MyBitArray> m_Memory = new Dictionary<Int64, MyBitArray>();

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

        internal void ExecuteOnAddress()
        {
            foreach (Instruction instruction in m_Instructions)
            {
                instruction.ApplyOnAddress(ref m_Memory);
            }
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
        MemoryInfo memory2 = new MemoryInfo();
        m_Memories.Add(memory2);

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
                    memory2.AddInstruction(currentMask, overrides);

                    currentMask = line;
                    overrides = new List<string>();

                    break;
            }
        }

        if (overrides.Count > 0)
        {
            memory.AddInstruction(currentMask, overrides);
            memory2.AddInstruction(currentMask, overrides);
        }

        memory.Execute();
        Debug.LogWarning("Result: " + memory.GetSum());

        memory2.ExecuteOnAddress();
        Debug.LogWarning("Result 2: " + memory2.GetSum());
    }
}
