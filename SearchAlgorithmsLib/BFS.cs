using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    public class BFS<T> : Searcher<T>
    {
        public override Solution<T> Search(ISearchable<T> searchable)
        {
            // update max nodes size in the FastPriorityQueue
            UpdateMaxNodes(searchable.getMaxNodes());

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
                        if (OpenListContains(s) && (s.Cost < GetStateFromOpenList(s.getTypeValue()).Cost))
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
            throw new NotSolvableException();
        }
            
     


       
    }
}

