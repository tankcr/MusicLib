using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegawMOD.Android;
using System.Management;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace MediaLib
{
    public class Devices
    {

        AndroidController android;
        Device device;

        public event EventHandler TableDataChanged;

        private List<Device> tableData;
        public List<Device> TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                if (TableDataChanged != null)
                {
                    TableDataChanged(this, EventArgs.Empty);
                }
            }
        }
        public Devices()
        {

        }
    }
    public class GetDevices
    {
		public static Devices GetDeviceData()
        {
			Devices table = new Devices ();
			table.TableData = new List<Device> ();
			DeviceState state;
			AndroidController android;
            RegawMOD.Android.Device device;
            android = AndroidController.Instance;
            if (android.HasConnectedDevices)
            {
                int num = android.ConnectedDevices.Count;
				for (int i = 0; i < android.ConnectedDevices.Count; i++) {
					string serial = android.ConnectedDevices [i];
					device = android.GetConnectedDevice (serial);
					string devserial = device.SerialNumber;
					state = device.State;
					string prodcmd = "shell cat /system/build.prop | grep \"product\"";
					AdbCommand adbCmdCard = Adb.FormAdbCommand (device, "shell df /storage/*Card*");
					AdbCommand adbCmdcard = Adb.FormAdbCommand (device, "shell df /storage/*card*");
					AdbCommand adbCmddesc = Adb.FormAdbCommand (device, prodcmd);
					string exp = @"\s+";
					string expdesc = @"\r\n";
					//AdbCommand cmd;
					//cmd = Adb.FormAdbCommand(device, "shell df /storage/*card*", null);
					string Card = Adb.ExecuteAdbCommand (adbCmdCard);
					string card = Adb.ExecuteAdbCommand (adbCmdcard);
					string descript = Adb.ExecuteAdbCommand (adbCmddesc);
					string[] Card1 = Regex.Split (Card, exp);
					string[] Card2 = Regex.Split (card, exp);
					string[] myphone = Regex.Split (descript, expdesc, RegexOptions.ExplicitCapture);
					Device item = new Device ();
					string expmodel = @"^(.*model.*)";
					string expbrand = @"^(.*brand.*)";
					string explanguage = @"^(.*language.*)";
					string expver = @"^(.*version.*)";
					string expregion = @"^(.*region.*)";

					foreach (string line in myphone) {
						try {
							Match mymodel = Regex.Match (line, expmodel);
							if (mymodel.Success) {
								item.model = line;
							}
						} catch {
						}
						;
						try {
							Match mybrand = Regex.Match (line, expbrand);
							if (mybrand.Success) {
								item.Brand = line;
							}
						} catch {
						}
						;
						try {
							Match mylanguage = Regex.Match (line, explanguage);
							if (mylanguage.Success) {
								item.language = line;
							}
						} catch {
						}
						;
						try {
							Match myversion = Regex.Match (line, expver);
							if (myversion.Success) {
								item.version = line;
							}
						} catch {
						}
						;
						try {
							Match myregion = Regex.Match (line, expregion);
							if (myregion.Success) {
								item.country = line;
							}
						} catch {
						}
						;
						item.free = Card1 [8];
						item.SerialNumber = serial;

					}
					item.Brand = myphone [2];
					table.TableData.Add (item);
					item.State = state.ToString();
				}
			}
			return table;
        }
    }
}
