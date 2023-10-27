using System;

using Phidgets;
using Phidgets.Events;

namespace VNC.Phidget
{
    public class PhidgetEx : Phidgets.Phidget
    {
        #region Constructors, Initialization, and Load

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public PhidgetEx(string ipAddress, int port, int serialNumber, bool enable, bool embedded)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            _hostIPAddress = ipAddress;
            _hostPort = port;
            _hostSerialNumber = serialNumber;
            _Embedded = embedded;
            _Enable = enable;

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties
      
        private bool _Embedded;

        public bool Embedded
        {
            get { return _Embedded; }
            set
            {
                _Embedded = value;
            }
        }

        private bool _Enable;

        public bool Enable
        {
            get { return _Enable; }
            set
            {
                _Enable = value;
            }
        }

        private int _hostSerialNumber;

        public int HostSerialNumber
        {
            get { return _hostSerialNumber; }
            set
            {
                _hostSerialNumber = value;
            }
        }

        private string _hostIPAddress;

        public string HostIPAddress
        {
            get { return _hostIPAddress; }
            set
            {
                _hostIPAddress = value;
            }
        }

        private int _hostPort;

        public int HostPort
        {
            get { return _hostPort; }
            set
            {
                _hostPort = value;
            }
        }

        #endregion

        #region Event Handlers

        public void Phidget_ServerDisconnect(object sender, Phidgets.Events.ServerDisconnectEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)e.Device;

                Log.Trace("Phidget_ServerDisconnect {device.Address}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        public void Phidget_ServerConnect(object sender, ServerConnectEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)e.Device;

                Log.Trace($"Phidget_ServerConnect {device.Address},{device.Port}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        public void Phidget_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)e.Device;

                Log.Trace($"Phidget_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        public void Phidget_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)e.Device;

                Log.Trace($"Phidget_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        public void Phidget_Error(object sender, Phidgets.Events.ErrorEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;

                Log.Trace($"Phidget_Error {device.Address},{device.Attached} - {e.Type} {e.Code} {e.Description}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods (None)

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)


        #endregion
    }
}
