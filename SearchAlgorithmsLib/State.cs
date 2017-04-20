using Priority_Queue;
namespace SearchAlgorithmsLib
{
    public class State<T> : FastPriorityQueueNode
    {
        private T state; // the state represented by a string
        private float cost; // cost to reach this state (set by a setter)
        private State<T> cameFrom; // the state we came from to this state (setter)



        /// <summary>
        /// Initializes a new instance of the <see cref="State{T}"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public State(T state) // CTOR
        {
            this.state = state;
            this.cameFrom = null;
        }


        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>
        /// The cost.
        /// </value>
        public float Cost { get { return cost; } set { cost = value; }}

        /// <summary>
        /// Gets or sets the came from.
        /// </summary>
        /// <value>
        /// The came from.
        /// </value>
        public State<T> CameFrom { get { return cameFrom; } set { cameFrom = value; } }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.state.GetHashCode();
        }


        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="s">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object s) // we overload Object's Equals method
        {
            return state.Equals((s as State<T>).state);
        }


        /// <summary>
        /// Gets the value of the state.
        /// </summary>
        /// <returns>T value</returns>
        public T GetTypeValue()
        {
            return state;
        }
       
    }

}