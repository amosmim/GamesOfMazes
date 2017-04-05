using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    public class Solution<T> 
    {
        private Stack<State<T>> states;
       
        public Solution ()
        {
            this.states = new Stack<State<T>>();
            
        }
        protected void PutOneStep(State<T> state)
        {
            this.states.Push(state);
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
        public Stack<State<T>> StackCopy()
        {
            return new Stack<State<T>>(this.states);
        }

       

       



    }
}