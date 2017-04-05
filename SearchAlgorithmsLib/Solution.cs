using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    public class Solution<T>
    {
        private Stack<State<T>> states;
        public Solution ()
        {
            states = new Stack<State<T>>();
        }
        public void PutOneStep(State<T> state)
        {
            states.Push(state);
        }
        public State<T> GetOneStep( )
        {
            return states.Pop();
        }

        public static Solution<T> BackTrace(State<T> goalState)
        {
            Solution<T> solution = new Solution<T>();
            State<T> next = goalState;
            while (next.CameFrom != null)
            {
                solution.PutOneStep(next);
                next = next.CameFrom;

            }
            return solution;
        }
    }
}