using System;

using Phidgets;
using Phidgets.Events;

namespace VNC.Phidget
{
    public class InterfaceKitEx : PhidgetEx // InterfaceKit
    {
        #region Constructors, Initialization, and Load

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public InterfaceKitEx(string ipAddress, int port, int serialNumber, bool embedded) 
            : base(ipAddress, port, serialNumber, true, embedded)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InitializePhidget();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePhidget()
        {
            //this.Attach += InterfaceKitk_Attach;
            //this.Detach += InterfaceKitk_Detach;
            //this.Error += InterfaceKitk_Error;
            //this.ServerConnect += InterfaceKitk_ServerConnect;
            //this.ServerDisconnect += InterfaceKitk_ServerDisconnect;

            //this.InputChange += InterfaceKitk_InputChange;
            //this.OutputChange += InterfaceKitk_OutputChange;
            //this.SensorChange += InterfaceKitk_SensorChange;

            InterfaceKit = new Phidgets.InterfaceKit();

            this.InterfaceKit.Attach += Phidget_Attach;
            this.InterfaceKit.Detach += Phidget_Detach;
            this.InterfaceKit.Error += Phidget_Error;
            this.InterfaceKit.ServerConnect += Phidget_ServerConnect;
            this.InterfaceKit.ServerDisconnect += Phidget_ServerDisconnect;

            this.InterfaceKit.InputChange += InterfaceKitk_InputChange;
            this.InterfaceKit.OutputChange += InterfaceKitk_OutputChange;
            this.InterfaceKit.SensorChange += InterfaceKitk_SensorChange;
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Phidgets.InterfaceKit InterfaceKit = null;
            
        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }

        #endregion

        #region Event Handlers

        private void InterfaceKitk_SensorChange(object sender, SensorChangeEventArgs e)
        {
            if (LogSensorChangeEvents)
            {
                try
                {
                    InterfaceKit ifk = (InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"InterfaceKitk_SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void InterfaceKitk_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        {
            if (LogOutputChangeEvents)
            {
                try
                {
                    InterfaceKit ifk = (InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"InterfaceKitk_OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void InterfaceKitk_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        {
            if (LogInputChangeEvents)
            {
                try
                {
                    InterfaceKit ifk = (InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"InterfaceKitk_InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        public void Open()
        {
            try
            {
                this.InterfaceKit.open(HostSerialNumber, HostIPAddress, HostPort);

                this.InterfaceKit.waitForAttachment();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        public void Close()
        {
            try
            {
                this.InterfaceKit.close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

        }

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)


        #endregion
    }
}
