using App.Core.States;
using UnityEngine;

namespace App.Features.Run.Views
{
    public class RunSessionsAmountPanelView : RunPanelView
    {
        [SerializeField] private string additionalLeftText;
        [SerializeField] private string additionalRightText;
        [Space]
        [SerializeField] private bool setsColor;
        [SerializeField] private Color timerSessionsAmountColorLess = Color.red;
        [SerializeField] private Color timerSessionsAmountColorMore = Color.green;

        protected override void SetValue(RunState run)
        {
            text.text = $"{additionalLeftText} {run.PlannedSessionsAmountCompletedSessions} / {run.PlannedSessionsAmount} {additionalRightText}";
            if (setsColor) text.color = run.PlannedSessionsAmountCompletedSessions < run.PlannedSessionsAmount ? timerSessionsAmountColorLess : timerSessionsAmountColorMore;
        }
    }
}