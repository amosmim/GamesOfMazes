using Priority_Queue;


namespace SearchAlgorithmsLib
{
    /// <summary>
    /// abstract class that implement ISearcher interface with built-in priory queue.
    /// </summary>
    /// <typeparam name="T">type of state</typeparam>
    public abstract class Searcher<T> : ISearcher<T>
    {
        private FastPriorityQueue<State<T>> openList;
        // needed for management of the FastPriorityQueue
        private int maxNudes;
        // counter.
        private int evaluatedNodes;

        /// <summary>
        /// Counstrctor
        /// </summary>
        public Searcher()
        {
            //this priority need max node size.
            maxNudes = 100;
            openList = new FastPriorityQueue<State<T>>(maxNudes);         

            evaluatedNodes = 0;
        }

        /// <summary>
        /// get state from the priory queue.
        /// </summary>
        /// <returns>state object</returns>
        protected State<T> PopOpenList()
        {
            // check bounds
            if(openList.Count+1 >= this.maxNudes)
            {
                maxNudes *= 2;
                openList.Resize(this.maxNudes);
            }
            evaluatedNodes++;
            return openList.Dequeue();
        }

        /// <summary>
        /// add state to priory queue.
        /// </summary>
        /// <param name="state">state to add</param>
        protected void AddToOpenList(State<T> state)
        {
              openList.Enqueue(state, state.Cost);
            
        }


        /// <summary>
        /// update the priory of state in the priory queue.
        /// </summary>
        /// <param name="state">state that is priory need to be update.</param>
        protected void UpdateCost(State<T> state)
        {
            openList.UpdatePriority(state, state.Cost);
        }

        /// <summary>
        /// start the counter of the evaluated Nodes.
        /// </summary>
        protected void StartOver()
        {
            this.evaluatedNodes = 0;
        }
       
        /// <summary>
        /// find the state in the queue that equals to the given one.
        /// </summary>
        /// <param name="state">state from the queue the equals or null</param>
        /// <returns></returns>
        protected State<T> GetStateFromOpenList(T state)
        {
            foreach(State<T> s in openList)
            {
                if (s.GetTypeValue().Equals(state))
                {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// check if the queue contains a state.
        /// </summary>
        /// <param name="state">state to find</param>
        /// <returns>true if found or false if not.</returns>
        protected bool OpenListContains (State<T> state)
        {
            return openList.Contains(state);
        }

        /// <summary>
        /// the count of the state in the queue. 
        /// </summary>
        public int OpenListSize
        { 
            get { return openList.Count; }
        }

        // ISearcher’s methods:

        /// <summary>
        /// get how many nodes were evaluated by the algorithm in the last Search.
        /// </summary>
        /// <returns>
        /// number of the nodes that evaluated by the algorithm
        /// </returns>
        public virtual int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }
        /// <summary>
        /// the search method.
        /// should restart the NumberOfNodesEvaluated counter!
        /// </summary>
        /// <param name="searchable">searchable graph to solve</param>
        /// <returns>
        /// Solution object
        /// </returns>
        public abstract Solution<T> Search(ISearchable<T> searchable);

    }
}
