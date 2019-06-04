using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Briefing.Droid
{
    [BroadcastReceiver]
    [Android.App.IntentFilter(new[] { Intent.ActionMediaButton })]
    public class MediaButtonReciever : BroadcastReceiver
    {
        public string ComponentName { get { return this.Class.Name; } }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action != Intent.ActionMediaButton)
                return;

            //The event will fire twice, up and down.
            // we only want to handle the down event though.
            var key = (KeyEvent)intent.GetParcelableExtra(Intent.ExtraKeyEvent);
            if (key.Action != KeyEventActions.Down)
                return;
            switch (key.KeyCode)
            {
                case Keycode.Headsethook:
                case Keycode.MediaPlayPause: if (MainActivity.player.IsPlaying) { MainActivity.player.Pause(); } else { MainActivity.player.Start(); } break;
                case Keycode.MediaPlay: MainActivity.player.Start(); break;
                case Keycode.MediaPause: MainActivity.player.Pause(); break;
                case Keycode.MediaStop: MainActivity.player.Stop(); break;
                default: return;
            }
        }
    }
}