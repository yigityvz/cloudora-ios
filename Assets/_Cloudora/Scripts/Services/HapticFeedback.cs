using UnityEngine;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Cloudora.Services
{
    public interface IHapticFeedback { void InvalidMove(); }
    public sealed class HapticFeedback : IHapticFeedback
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void CloudoraInvalidMoveHaptic();
#endif
        public void InvalidMove()
        {
#if UNITY_IOS && !UNITY_EDITOR
            CloudoraInvalidMoveHaptic();
#elif UNITY_ANDROID && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }
}
