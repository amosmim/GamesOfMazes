using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace GUIClient
{
    class SPViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SPModel model;

        public SPViewModel(SPModel model)
        {
            this.model = model;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                this.OnPropertyChanged("VM" + e.PropertyName);
            };
        }
       
        public string VMSerializedGame {
            get { return this.model.SerializedGame; }
        }

        public string VMInitialPos {
            get { return this.model.InitialPos; }
        }

        public string VMGoalPos {
            get { return this.model.GoalPos; }
        }
        public int VMRows {
            get { return this.model.Rows; }
        }

        public int VMCols{
            get { return this.model.Cols; }
        }

        public string VMPlayerPosition {
            get { return this.model.PlayerPosition; }
        }

        public bool CheckMove(string direction)
        {
            return true;
           //return this.model.CheckMove(direction);
        }

        public void StartGame(string name, int rows, int cols)
        {
            string command = "generate " + name + " " + rows + " " + cols;
            this.model.StartGame(command);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
