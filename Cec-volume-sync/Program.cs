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
            var main = new MainRunner();
            return await main.Run(args);
        }
    }
}