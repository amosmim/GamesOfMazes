using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace GUIClient
{
    /// <summary>
    /// View model for single player.
    /// </summary>
    public class SPViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SPModel model;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model">model</param>
        public SPViewModel(SPModel model)
        {
            this.model = model;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                this.OnPropertyChanged("VM" + e.PropertyName);
            };
        }

        /// <summary>
        /// VMSerializedGame Property.
        /// </summary>
        public string VMSerializedGame {
            get { return this.model.SerializedGame; }
        }

        /// <summary>
        /// VMName Property.
        /// </summary>
        public string VMName
        {
            get { return this.model.Name; }
        }

        /// <summary>
        /// VMInitialPos Property.
        /// </summary>
        public string VMInitialPos {
            get { return this.model.InitialPos; }
        }

        /// <summary>
        /// VMGoalPos Property.
        /// </summary>
        public string VMGoalPos {
            get { return this.model.GoalPos; }
        }

        /// <summary>
        /// VMRows Property.
        /// </summary>
        public int VMRows {
            get { return this.model.Rows; }
        }

        /// <summary>
        /// VMCols Property.
        /// </summary>
        public int VMCols{
            get { return this.model.Cols; }
        }

        /// <summary>
        /// VMPlayerPosition Property.
        /// </summary>
        public string VMPlayerPosition {
            get { return this.model.PlayerPosition; }
        }

        /// <summary>
        /// Initiate move checking algorithm in the model.
        /// </summary>
        /// <param name="direction">direction</param>
        /// <returns>0 - not valid move
        ///          1 - valid move
        ///          2 - win</returns>
        public int CheckMove(string direction)
        { 
            return this.model.CheckMove(direction); 
        }

        /// <summary>
        /// Initiate solving algorithm and animation.
        /// </summary>
        /// <param name="name">maze name</param>
        /// <param name="algo">searcher</param>
        /// <returns>true if success else false</returns>
        public bool InitiateSolution(string name, int algo)
        {
            string command = "solve " + name + " " + algo;
            return this.model.InitiateSolution(command);
        }

        /// <summary>
        /// Starting the single player game.
        /// </summary>
        /// <param name="name">maze name</param>
        /// <param name="rows">rows</param>
        /// <param name="cols">cols</param>
        /// <returns>true if success else false</returns>
        public bool StartGame(string name, int rows, int cols)
        {
            string command = "generate " + name + " " + rows + " " + cols;
            return this.model.StartGame(command);
        }

        /// <summary>
        /// Restart the game.
        /// </summary>
        public void Restart()
        {
            this.model.Restart();
        }

        /// <summary>
        /// Notify on property changed.
        /// </summary>
        /// <param name="propertyName">property name</param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
