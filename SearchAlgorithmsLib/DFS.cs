
using System.Collections.Generic;


namespace SearchAlgorithmsLib
{
    public class DFS<T> : ISearcher<T>
    {
        private int evaluatedNodes;
        private Stack<State<T>> stack;
        private HashSet<State<T>> marked;
        public DFS()
        {
            stack = new Stack<State<T>>();
            marked = new HashSet<State<T>>();
        }
        public int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }

        public Solution<T> Search(ISearchable<T> searchable)
        {
            evaluatedNodes = 0;
            stack.Push(searchable.GetInitialState());
            State<T> goal = searchable.GetGoalState();
            State<T> tamp;
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
            throw new NotSolvableException();

        }
        private State<T> StackPop()
        {
            this.evaluatedNodes++;
            return stack.Pop();
        }

    }

    
}
