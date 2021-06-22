using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Observables;

namespace Cec_volume_sync
{
    public class MainRunner: IObserver<DeviceChangedArgs>
    {
        private IDevice _controlledAudioDevice;
        private AudioObserver _audioObserver;

        public async Task<int> Run(string[] args)
        {
            var audioController = new CoreAudioController();
           ;
            if (args.Length >= 1)
            {
                _controlledAudioDevice = (await audioController.GetDevicesAsync())
                    .FirstOrDefault(audioDevice => audioDevice.Name == args[0]);
            }
            else
            {
                _controlledAudioDevice = audioController.DefaultPlaybackDevice;
                audioController.AudioDeviceChanged.Subscribe(this);
            }

            if (_controlledAudioDevice == null)
            {
                Console.WriteLine("Could not open a connection to the VSX");
                return 1;
            }
            await _controlledAudioDevice.SetVolumeAsync(90);
            
            var p = new CecSharpClient();
            if (p.Connect(10000))
            {
                _audioObserver = new AudioObserver(p);
                _controlledAudioDevice.VolumeChanged.Subscribe(_audioObserver);
                Console.WriteLine("Ready");
                Thread.Sleep(Timeout.Infinite);
            }
            else
            {
                Console.WriteLine("Could not open a connection to the CEC adapter");
                return 1;
            }

            return 0;
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(DeviceChangedArgs value)
        {
            _controlledAudioDevice = value.Device;
            _controlledAudioDevice.VolumeChanged.Subscribe(_audioObserver);
        }
    }
}