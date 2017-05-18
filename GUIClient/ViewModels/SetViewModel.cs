﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GUIClient
{
    class SetViewModel : INotifyPropertyChanged
    {
        private SetModel setModel;
        public event PropertyChangedEventHandler PropertyChanged;

        private string vmIP;
        private int vmPort;
        private int vmRows;
        private int vmCols;
        private int vmAlgo;

        public string VMIP 
        {
            get { return this.vmIP; }
            set
            {
                this.vmIP = value;
                this.OnPropertyChanged("VMIP");
            }
        }
        public int VMPort
        {
            get { return this.vmPort; }
            set
            {
                this.vmPort = value;
                this.OnPropertyChanged("VMPort");
            }
        }
        public int VMAlgo
        {
            get { return this.vmAlgo; }
            set
            {
                this.vmAlgo = value;
                this.OnPropertyChanged("VMAlgo");
            }
        }
        public int VMRows
        {
            get { return this.vmRows; }
            set
            {
                this.vmRows = value;
                this.OnPropertyChanged("VMRows");
            }
        }
        public int VMCols
        {
            get { return this.vmCols; }
            set
            {
                this.vmCols = value;
                this.OnPropertyChanged("VMCols");
            }
        }

        public SetViewModel(SetModel setModel)
        {
            this.setModel = setModel;
        }

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

        public void SetData(SettingsData setData)
        {
            this.setModel.SetData(setData);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
    