using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day8 : MonoBehaviour
{
    [Serializable]
    private class Instruction
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

        RunInstructions();
    }

    private void RunInstructions()
    {
        int instructionID = 0;

        int accumulator = 0;

        while (m_Instructions[instructionID].Execute(ref accumulator, out int jumpValue))
        {
            Debug.Log("Instruction executed: " + instructionID);
            instructionID += jumpValue;
        }

        Debug.LogError("Final instruction " + instructionID + ". Accumulator value = " + accumulator);
    }
}
