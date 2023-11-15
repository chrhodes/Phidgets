using System;
using System.Windows.Documents;

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
        /// <param name="ipAddress">IP Address of Host</param>
        /// <param name="port">Port number of Host</param>
        public PhidgetEx(string ipAddress, int port, int serialNumber)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            Host = new Host { IPAddress = ipAddress, Port = port };
            
            //_hostIPAddress = ipAddress;
            //_hostPort = port;
            _serialNumber = serialNumber;
            //_Embedded = embedded;
            //_Enable = enable;

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Host Host { get; set; }

        //private bool _Embedded;

        //public bool Embedded
        //{
        //    get { return _Embedded; }
        //    set
        //    {
        //        _Embedded = value;
        //    }
        //}

        //private bool _Enable;

        //public bool Enable
        //{
        //    get { return _Enable; }
        //    set
        //    {
        //        _Enable = value;
        //    }
        //}

        private int _serialNumber;

        public int SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                _serialNumber = value;
            }
        }

        //private string _hostIPAddress;

        //public string HostIPAddress
        //{
        //    get { return _hostIPAddress; }
        //    set
        //    {
        //        _hostIPAddress = value;
        //    }
        //}

        //private int _hostPort;

        //public int HostPort
        //{
        //    get { return _hostPort; }
        //    set
        //    {
        //        _hostPort = value;
        //    }
        //}

        public bool LogEvents { get; set; }

        #endregion

        #region Event Handlers

        public void Phidget_ServerDisconnect(object sender, Phidgets.Events.ServerDisconnectEventArgs e)
        {
            if (LogEvents)
            {
                try
                {
                    Phidgets.Phidget device = (Phidgets.Phidget)e.Device;

                    Log.Trace($"Phidget_ServerDisconnect {device.Address}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        public void Phidget_ServerConnect(object sender, ServerConnectEventArgs e)
        {
            if (LogEvents)
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
        }

        public void Phidget_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            if (LogEvents)
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
        }

        public void Phidget_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            if (LogEvents)
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

        // NOTE(crhodes)
        // This did not work.  Had to go back to opening type'd Phidget.  See AdvancedServoEx, InterfaceKitEx, StepperEx
        ///// <summary>
        ///// Open Phidget and waitForAttachment
        ///// </summary>
        ///// <param name="timeOut">Optionally time out after timeOut(ms)</param>
        //public void Open(Int32? timeOut = null)
        //{
        //    Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

        //    try
        //    {
        //        open(SerialNumber, Host.IPAddress, Host.Port);

        //        if (timeOut is not null) { waitForAttachment((Int32)timeOut); }
        //        else { waitForAttachment(); }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //public void Close()
        //{
        //    Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

        //    try
        //    {
        //        close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)


        #endregion
    }
}
