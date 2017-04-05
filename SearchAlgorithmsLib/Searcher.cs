using Priority_Queue;


namespace SearchAlgorithmsLib
{
    public abstract class Searcher<T> : ISearcher<T>
    {

        const int DEFAULT_MAX_NODES=  100;
       
        private FastPriorityQueue<State<T>> openList;
        private int evaluatedNodes;
        public Searcher()
        {
            //this priority need max node size, its shold be override with UpdateMaxNodes()
            openList = new FastPriorityQueue<State<T>>(DEFAULT_MAX_NODES); 
           

            evaluatedNodes = 0;
        }
        protected State<T> PopOpenList()
        {
            evaluatedNodes++;
            return openList.Dequeue();
        }
        protected void AddToOpenList(State<T> state)
        {
              openList.Enqueue(state, state.Cost);
            
        }

        protected void UpdateCost(State<T> state)
        {
            openList.UpdatePriority(state, state.Cost);
        }

        protected void UpdateMaxNodes(int max)
        {
            openList.Resize(max);
        }
       
        protected State<T> GetStateFromOpenList(T state)
        {
            foreach(State<T> s in openList)
            {
                if (s.getTypeValue().Equals(state))
                {
                    return s;
                }
            }
            return null;
        }

        protected bool OpenListContains (State<T> state)
        {
            return openList.Contains(state);
        }
        // a property of openList
        public int OpenListSize
        { // it is a read-only property :)
            get { return openList.Count; }
        }
        // ISearcher’s methods:
        public virtual int getNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }
        public abstract Solution<T> Search(ISearchable<T> searchable);

    }
}
