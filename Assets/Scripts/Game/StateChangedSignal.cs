#nullable enable
using Zenject;

namespace Game
{
    public class StateChangedSignal { }

    public static class StateChangedSignalUtils
    {
        public static void StateChanged(this SignalBus signalBus)
        {
            signalBus.Fire<StateChangedSignal>();
        }
    }
}