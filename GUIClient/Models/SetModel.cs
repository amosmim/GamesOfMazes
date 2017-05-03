using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GUIClient
{
    class SetModel
    {
        public SetModel()
        {

        }

        public SettingsData GetData()
        {
            SettingsData setData = new SettingsData();
            setData.IP = Properties.Settings.Default.ServerIP;
            setData.port = Properties.Settings.Default.ServerPort;
            setData.cols = Properties.Settings.Default.cols;
            setData.rows = Properties.Settings.Default.rows;
            setData.algo = Properties.Settings.Default.algo;

            return setData;
        }

        public void SetData(SettingsData setData)
        {
            Properties.Settings.Default.algo = setData.algo;
            Properties.Settings.Default.ServerIP = setData.IP;
            Properties.Settings.Default.rows = setData.rows;
            Properties.Settings.Default.cols = setData.cols;
            Properties.Settings.Default.ServerPort = setData.port;
            Properties.Settings.Default.Save();
        }
    }
    
}
