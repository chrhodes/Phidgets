using System;

using Phidgets;
using Phidgets.Events;

namespace VNC.Phidget
{
    // TODO(crhodes)
    // Decide if this should be VNCInterfaceKit to distinguish from Phidgets.InterfaceKit
    // For now do InterfaceKitEx

    public class InterfaceKitEx : PhidgetEx // InterfaceKit
    {
        #region Constructors, Initialization, and Load

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public InterfaceKitEx(string ipAddress, int port, int serialNumber, bool enable, bool embedded) 
            : base(ipAddress, port, serialNumber, enable, embedded)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            IntitalizePhidgetInterfaceKit();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void IntitalizePhidgetInterfaceKit()
        {
            //this.Attach += Ifk_Attach;
            //this.Detach += Ifk_Detach;
            //this.Error += Ifk_Error;
            //this.ServerConnect += Ifk_ServerConnect;
            //this.ServerDisconnect += Ifk_ServerDisconnect;

            //this.InputChange += Ifk_InputChange;
            //this.OutputChange += Ifk_OutputChange;
            //this.SensorChange += Ifk_SensorChange;

            InterfaceKit = new Phidgets.InterfaceKit();

            this.InterfaceKit.Attach += Phidget_Attach;
            this.InterfaceKit.Detach += Phidget_Detach;
            this.InterfaceKit.Error += Phidget_Error;
            this.InterfaceKit.ServerConnect += Phidget_ServerConnect;
            this.InterfaceKit.ServerDisconnect += Phidget_ServerDisconnect;

            this.InterfaceKit.InputChange += Ifk_InputChange;
            this.InterfaceKit.OutputChange += Ifk_OutputChange;
            this.InterfaceKit.SensorChange += Ifk_SensorChange;
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

        private void Ifk_SensorChange(object sender, SensorChangeEventArgs e)
        {
            if (LogSensorChangeEvents)
            {
                try
                {
                    InterfaceKit ifk = (InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"Ifk_SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void Ifk_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        {
            if (LogOutputChangeEvents)
            {
                try
                {
                    InterfaceKit ifk = (InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"Ifk_OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void Ifk_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        {
            if (LogInputChangeEvents)
            {
                try
                {
                    InterfaceKit ifk = (InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"Ifk_InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
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
