using App.Timer.States;
using PT.Tools.Helper;

namespace App.Timer.Windows
{
    public class RunDurationPanelView : RunPanelView
    {
        protected override void SetValue(RunState runState)
        {
            text.text = Utils.ConvertSecondsToTime(runState.SessionDuration);
        }
    }
}