using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day8 : MonoBehaviour
{
    [Serializable]
    private class Instruction : ICloneable
    {
        [Serializable]
        public enum InstructionType
        {
            nop,
            acc,
            jmp
        }

        public InstructionType Type;
        public int Value;
        public bool Executed = false;

        public Instruction(string type, string value)
        {
            Type = (InstructionType)Enum.Parse(typeof(InstructionType), type);
            Value = int.Parse(value);
        }

        public Instruction(InstructionType type, int value)
        {
            Type = type;
            Value = value;
        }

        public bool Execute(ref int accumulator, out int jumpValue)
        {
            jumpValue = 1;

            if (Executed)
            {
                return false;
            }

            switch (Type)
            {
                case InstructionType.nop:
                    break;
                case InstructionType.acc:
                    accumulator += Value;
                    break;
                case InstructionType.jmp:
                    jumpValue = Value;
                    break;
            }

            Executed = true;
            return true;
        }

        public object Clone()
        {
            return new Instruction(Type, Value);
        }
    }

    [SerializeField]
    private TextAsset m_Input = null;

    private List<Instruction> m_Instructions = new List<Instruction>();

    private void Start()
    {
        string input = m_Input.text;
        string[] inputLines = input.Split('\n');

        foreach(string line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string[] lineSplitted = line.Split(' ');

            m_Instructions.Add(new Instruction(lineSplitted[0], lineSplitted[1]));
        }

        RunInstructions(m_Instructions);

        FixInstructions();
    }

    private int RunInstructions(List<Instruction> instructions)
    {
        int instructionID = 0;

        int accumulator = 0;

        while (instructionID < instructions.Count && instructions[instructionID].Execute(ref accumulator, out int jumpValue))
        {
            instructionID += jumpValue;
        }

        Debug.LogError("Final instruction " + instructionID + ". Accumulator value = " + accumulator);

        return instructionID;
    }

    private void FixInstructions()
    {
        for (int i = 0; i < m_Instructions.Count; i++)
        {
            List<Instruction> alteredInstructions = new List<Instruction>();
            m_Instructions.ForEach(inst => alteredInstructions.Add((Instruction)inst.Clone()));

            switch (alteredInstructions[i].Type)
            {
                case Instruction.InstructionType.jmp:
                    alteredInstructions[i].Type = Instruction.InstructionType.nop;
                    break;
                case Instruction.InstructionType.nop:
                    alteredInstructions[i].Type = Instruction.InstructionType.jmp;
                    break;
            }

            if (RunInstructions(alteredInstructions) >= m_Instructions.Count)
            {
                Debug.LogError("Modified instruction " + i);
                return;
            }
        }
    }
}
