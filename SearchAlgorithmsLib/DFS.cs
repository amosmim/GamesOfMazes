
using System.Collections.Generic;


namespace SearchAlgorithmsLib
{
    /// <summary>
    /// Isearcher that work on DFS algoritm.
    /// </summary>
    /// <typeparam name="T">type of state</typeparam>
    public class DFS<T> : ISearcher<T>
    {
        private int evaluatedNodes;
        private Stack<State<T>> stack;
        private HashSet<State<T>> marked;

        /// <summary>
        /// constractor.
        /// </summary>
        public DFS()
        {
            stack = new Stack<State<T>>();
            marked = new HashSet<State<T>>();
        }

        /// <summary>
        /// gets numbers of evaluated Nodes in the algoritm.
        /// </summary>
        /// <returns>numbers of evaluated Nodes</returns>
        public int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }

        /// <summary>
        /// solve the searchable graph.
        /// </summary>
        /// <param name="searchable">graph to solved</param>
        /// <returns>solution object</returns>
        public Solution<T> Search(ISearchable<T> searchable)
        {
            // restart the databases and counter
            stack.Clear();
            marked.Clear();
            evaluatedNodes = 0;

            // start with start-state.
            stack.Push(searchable.GetInitialState());
            //save instance of goal state for save calls later.
            State<T> goal = searchable.GetGoalState();
            State<T> tamp;

            // DFS algoritm 
            while(stack.Count > 0)
            {
                tamp = StackPop();
                if (tamp.Equals(goal))
                          return Solution<T>.BackTrace(tamp);
                if (!marked.Contains(tamp))
                {
                    marked.Add(tamp);
                    foreach(State<T> s in searchable.GetAllPossibleStates(tamp))
                    {
                        stack.Push(s);
                    }
                }
                

            }
            // whan the states are ended, but never found the goal state...
            throw new NotSolvableException();

        }

        /// <summary>
        /// pop from the stack and count calls.
        /// </summary>
        /// <returns>state from stack</returns>
        private State<T> StackPop()
        {
            this.evaluatedNodes++;
            return stack.Pop();
        }

    }

    
}
