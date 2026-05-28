using App.Core.States;
using PT.Tools.Helper;

namespace App.Features.Run.Views
{
    public class RunDurationPanelView : RunPanelView
    {
        protected override void SetValue(RunState runState)
        {
            text.text = Utils.ConvertSecondsToTime(runState.SessionDuration);
        }
    }
}