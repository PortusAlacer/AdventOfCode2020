using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day11 : MonoBehaviour
{
    private class Ferry
    {
        private class Seat
        {
            public enum State
            {
                Floor,
                Empty,
                Occupied
            }

            public State CurrentState { private set; get; } = State.Floor;
            private State m_ProposesNewState = State.Floor;
            private Vector2Int m_Coordinates;

            private Seat[,] m_AllSeats;

            public Seat(Vector2Int coordinates, State state, ref Seat[,] allSeats)
            {
                CurrentState = state;
                m_ProposesNewState = CurrentState;
                m_Coordinates = coordinates;
                m_AllSeats = allSeats;
            }

            public bool CheckChange()
            {
                if (CurrentState == State.Floor)
                {
                    return false;
                }

                Vector2Int minPosition = new Vector2Int(Mathf.Max(0, m_Coordinates.x - 1), Mathf.Max(0, m_Coordinates.y - 1));
                Vector2Int maxPosition = new Vector2Int(Mathf.Min(m_AllSeats.GetLength(0) - 1, m_Coordinates.x + 1), Mathf.Min(m_AllSeats.GetLength(1) - 1, m_Coordinates.y + 1));

                int countOccupied = 0;

                for (int x = minPosition.x; x <= maxPosition.x; x++)
                {
                    for (int y = minPosition.y; y <= maxPosition.y; y++)
                    {
                        if (m_Coordinates == new Vector2Int(x, y))
                        {
                            continue;
                        }

                        if (m_AllSeats[x, y].CurrentState == State.Occupied)
                        {
                            countOccupied++;
                        }
                    }
                }

                switch (CurrentState)
                {
                    case State.Empty:
                        if (countOccupied == 0)
                        {
                            m_ProposesNewState = State.Occupied;
                            return true;
                        }
                        else
                        {
                            m_ProposesNewState = CurrentState;
                            return false;
                        }
                    case State.Occupied:
                        if (countOccupied >= 4)
                        {
                            m_ProposesNewState = State.Empty;
                            return true;
                        }
                        else
                        {
                            m_ProposesNewState = CurrentState;
                            return false;
                        }
                }

                return false;
            }

            internal void ApplyChange()
            {
                CurrentState = m_ProposesNewState;
            }
        }

        private Seat[,] m_Seats;

        public Ferry(string[] rows)
        {
            m_Seats = new Seat[rows[0].Length, rows.Length - 1];

            for (int r = 0; r < rows.Length - 1; r++)
            {
                char[] seats = rows[r].ToCharArray();

                for (int c = 0; c < seats.Length; c++)
                {
                    Vector2Int coords = new Vector2Int(c, r);
                    Seat.State state = Seat.State.Floor;
                    switch (seats[c])
                    {
                        case 'L':
                            state = Seat.State.Empty;
                            break;
                    }

                    m_Seats[c, r] = new Seat(coords, state, ref m_Seats);
                }
            }
        }

        public int Iterate()
        {
            int iteration = 1;

            while (CheckAllChanges())
            {
                ApplyChanges();
                iteration++;
            }

            return iteration;
        }

        public int CountOccupied()
        {
            int countOccupied = 0;

            foreach (Seat seat in m_Seats)
            {
                countOccupied += seat.CurrentState == Seat.State.Occupied ? 1 : 0;
            }

            return countOccupied;
        }

        private bool CheckAllChanges()
        {
            bool needsChange = false;

            foreach (Seat seat in  m_Seats)
            {
                needsChange |= seat.CheckChange();
            }

            return needsChange;
        }

        private void ApplyChanges()
        {
            foreach (Seat seat in m_Seats)
            {
                seat.ApplyChange();
            }
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

        Ferry ferry = new Ferry(inputLines);

        Debug.LogWarning("Stable with " + ferry.Iterate() + " iterations");
        Debug.LogWarning(ferry.CountOccupied() + " occupied seats");
    }
}
