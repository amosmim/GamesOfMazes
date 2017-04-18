using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// Isearcher that work on BFS algoritm.
    /// </summary>
    /// <typeparam name="T">type of state</typeparam>
    public class BFS<T> : Searcher<T>
    {
        /// <summary>
        /// solve the searchable graph.
        /// </summary>
        /// <param name="searchable">graph to solved</param>
        /// <returns>solution object</returns>
        public override Solution<T> Search(ISearchable<T> searchable)
        {
            // restart the counter of nodes.
            StartOver();
            State<T> start = searchable.GetInitialState();
            start.Cost = 0;
            AddToOpenList(start); // inherited from Searcher
            HashSet<State<T>> closed = new HashSet<State<T>>();
            while (OpenListSize > 0)
            {
                State<T> n = PopOpenList(); // inherited from Searcher, removes the best state
                closed.Add(n);
                if (n.Equals(searchable.GetGoalState()))
                    return Solution<T>.BackTrace(n); // private method, back traces through the parents
                                                                             // calling the delegated method, returns a list of states with n as a parent
                List<State<T>> succerssors = searchable.GetAllPossibleStates(n);
                foreach (State<T> s in succerssors)
                {
                    if (!closed.Contains(s))
                    {
                        
                        if (!OpenListContains(s))
                        {
                           
                            //s.Cost += n.Cost;

                            AddToOpenList(s);
                            
                        }
                    }
                    else
                    {
                        if (OpenListContains(s) && (s.Cost < GetStateFromOpenList(s.GetTypeValue()).Cost))
                        {
                            UpdateCost(s);
                            System.Console.WriteLine("update in open: " + s.ToString());

                        }
                        else
                        {
                            if (!OpenListContains(s))
                            {
                                //AddToOpenList(s);
                            }


                        }

                    }
                }
            }
            // whan the states are ended, but never found the goal state...
            throw new NotSolvableException();
        }
            
       
    }
}

