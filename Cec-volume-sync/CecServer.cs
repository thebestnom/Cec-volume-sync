using System;
using System.Text;
using CecSharp;

namespace Cec_volume_sync
{
    internal class CecSharpClient : CecCallbackMethods
    {
        private readonly LibCecSharp _lib;

        public CecSharpClient()
        {
            var config = new LibCECConfiguration();
            config.DeviceTypes.Types[0] = CecDeviceType.RecordingDevice;
            config.DeviceName = "PC";
            config.ClientVersion = LibCECConfiguration.CurrentVersion;

            _lib = new LibCecSharp(this, config);
            _lib.InitVideoStandalone();

            Console.WriteLine("CEC Parser created - libCEC version " + _lib.VersionToString(config.ServerVersion));
        }

        public bool Connect(int timeout)
        {
            var adapters = _lib.FindAdapters(string.Empty);
            if (adapters.Length > 0)
                return Connect(adapters[0].ComPort, timeout);
            else
            {
                Console.WriteLine("Did not find any CEC adapters");
                return false;
            }
        }

        private bool Connect(string port, int timeout)
        {
            return _lib.Open(port, timeout);
        }

        public void SendCommand(string command)
        {
            var bytes = new CecCommand();
            var commands = command.Split(":");
            foreach (var c in commands)
            {
                bytes.PushBack(byte.Parse(c, System.Globalization.NumberStyles.HexNumber));
            }

            _lib.Transmit(bytes);
        }

        public void Close()
        {
            _lib.Close();
        }
    }
}