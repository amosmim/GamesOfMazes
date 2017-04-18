using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// solution object.
    /// </summary>
    /// <typeparam name="T">type of state</typeparam>
    public class Solution<T> 
    {
        private Stack<State<T>> states;
       
        /// <summary>
        /// constractor.
        /// </summary>
        public Solution ()
        {
            this.states = new Stack<State<T>>();
        }

        /// <summary>
        /// get one step to the goal.
        /// </summary>
        /// <param name="state">state to add to solution</param>
        protected void PutOneStep(State<T> state)
        {
            this.states.Push(state);
        }
        
        /// <summary>
        /// create a solution by back trace from some state.
        /// using to CameFrom field.
        /// </summary>
        /// <param name="goalState">state to back from.</param>
        /// <returns>solution object</returns>
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

        /// <summary>
        /// return a copy of the solution's stack.
        /// </summary>
        /// <returns>stack of state, 
        /// when the upper state is the start state,  
        /// and the last is the goal state.</returns>
        public Stack<State<T>> StackCopy()
        {
            return new Stack<State<T>>(this.states);
        }

       

       



    }
}