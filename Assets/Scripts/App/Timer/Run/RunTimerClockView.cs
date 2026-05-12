using PT.Tools.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Timer.Run
{
    public class RunTimerClockView : MonoBehaviour
    {
        [SerializeField] private Image clockFillImage;
        [SerializeField] private TextMeshProUGUI timerText;

        public void SetTimer(int time, int totalTime, bool playing = false)
        {
            timerText.text = Utils.ConvertSecondsToTime(time);

            clockFillImage.fillAmount = time / (float)totalTime;
        }
    }
}