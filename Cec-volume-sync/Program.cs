using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;

namespace Cec_volume_sync
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            CoreAudioDevice controlledAudioDevice;
            if (args.Length >= 1)
            {
                controlledAudioDevice = new CoreAudioController().GetDevices()
                    .FirstOrDefault(audioDevice => audioDevice.Name == args[0]);
            }
            else
            {
                controlledAudioDevice = new CoreAudioController().DefaultPlaybackDevice;
            }

            if (controlledAudioDevice == null)
            {
                Console.WriteLine("Could not open a connection to the VSX");
                return 1;
            }
            await controlledAudioDevice.SetVolumeAsync(90);
            
            var p = new CecSharpClient();
            if (p.Connect(10000))
            {
                controlledAudioDevice.VolumeChanged.Subscribe(new AudioObserver(p));
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
    }
}