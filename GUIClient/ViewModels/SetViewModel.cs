using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GUIClient
{
    /// <summary>
    /// View model for settings.
    /// </summary>
    class SetViewModel : INotifyPropertyChanged
    {
        private SetModel setModel;
        public event PropertyChangedEventHandler PropertyChanged;

        private string vmIP;
        private int vmPort;
        private int vmRows;
        private int vmCols;
        private int vmAlgo;

        /// <summary>
        /// VMIP Property.
        /// </summary>
        public string VMIP 
        {
            get { return this.vmIP; }
            set
            {
                this.vmIP = value;
                this.OnPropertyChanged("VMIP");
            }
        }

        /// <summary>
        /// VMPort Property.
        /// </summary>
        public int VMPort
        {
            get { return this.vmPort; }
            set
            {
                this.vmPort = value;
                this.OnPropertyChanged("VMPort");
            }
        }

        /// <summary>
        /// VMAlgo Property.
        /// </summary>
        public int VMAlgo
        {
            get { return this.vmAlgo; }
            set
            {
                this.vmAlgo = value;
                this.OnPropertyChanged("VMAlgo");
            }
        }

        /// <summary>
        /// VMRows Property.
        /// </summary>
        public int VMRows
        {
            get { return this.vmRows; }
            set
            {
                this.vmRows = value;
                this.OnPropertyChanged("VMRows");
            }
        }

        /// <summary>
        /// VMCols Property.
        /// </summary>
        public int VMCols
        {
            get { return this.vmCols; }
            set
            {
                this.vmCols = value;
                this.OnPropertyChanged("VMCols");
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetViewModel(SetModel setModel)
        {
            this.setModel = setModel;
        }

        /// <summary>
        /// Get the data from the model.
        /// </summary>
        public void GetData()
        {
            SettingsData setData = new SettingsData();

            setData = this.setModel.GetData();

            this.VMIP = setData.IP;
            this.VMPort = setData.port;
            this.VMRows = setData.rows;
            this.VMCols = setData.cols;
            this.VMAlgo = setData.algo;
        }

        /// <summary>
        /// Set the changes in settings.
        /// </summary>
        /// <param name="setData">struct</param>
        public void SetData(SettingsData setData)
        {
            this.setModel.SetData(setData);
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
    