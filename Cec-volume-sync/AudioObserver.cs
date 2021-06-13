using System;
using System.Threading;
using AudioSwitcher.AudioApi;

namespace Cec_volume_sync
{
    internal class AudioObserver : IObserver<DeviceVolumeChangedArgs>
    {
        private readonly CecSharpClient _cec;

        public AudioObserver(CecSharpClient cec)
        {
            _cec = cec;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(DeviceVolumeChangedArgs value)
        {
            switch ((int)Math.Floor(value.Volume))
            {
                case 90:
                    return;
                case > 90:
                    _cec.SendCommand("15:44:41"); //volume up 
                    Thread.Sleep(5);
                    _cec.SendCommand("15:45");
                    Thread.Sleep(5);
                    break;
                default:
                    _cec.SendCommand("15:44:42");
                    Thread.Sleep(5);
                    _cec.SendCommand("15:45");
                    Thread.Sleep(5);
                    break;
            }

            value.Device.SetVolumeAsync(90);
        }
    }
}