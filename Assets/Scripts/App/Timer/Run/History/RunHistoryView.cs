using PT.Tools.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Timer.Run.History
{
    public class RunHistoryView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI description;
        [Space]
        [SerializeField] private TextMeshProUGUI dateRange;
        [Space]
        [SerializeField] private TextMeshProUGUI duration;
        [SerializeField] private TextMeshProUGUI plannedSessions;
        [SerializeField] private TextMeshProUGUI completedSessions;
        [Space]
        [SerializeField] private Image completenessImage;
        [SerializeField] private TextMeshProUGUI completenessTextNumber;
        [SerializeField] private TextMeshProUGUI completenessText;
        [Space]
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite[] iconSprites;
        [Space]
        [SerializeField] private Color amazingColor = Color.green;
        [SerializeField] private Color goodColor = Color.yellow;
        [SerializeField] private Color badColor = Color.red;

        public void SetView(RunHistoryData runHistoryData)
        {
            description.text = runHistoryData.Description;
            
            var localStartTime = runHistoryData.RunStartTime.ToLocalTime();
            var localEndTime = runHistoryData.RunEndTime.ToLocalTime();

            dateRange.text = $"{localStartTime:MMM dd, HH:mm} - {localEndTime:MMM dd, HH:mm}";
            
            duration.text = Utils.ConvertSecondsToTime(runHistoryData.SessionDuration);
            plannedSessions.text = runHistoryData.PlannedSessionsAmount.ToString();
            completedSessions.text = runHistoryData.CompletedSessions.ToString();

            float completenessRatio = (float)runHistoryData.CompletedSessions / runHistoryData.PlannedSessionsAmount;
            completenessImage.fillAmount = completenessRatio;
            
            completenessTextNumber.text = $"{completenessRatio * 100}%";
            SetCompletenessStyle(completenessRatio);
            SetRandomIcon();
        }
        
        private void SetCompletenessStyle(float ratio)
        {
            if (ratio >= 0.8f)
            {
                completenessText.text = "Amazing";
                completenessImage.color = completenessText.color = completenessTextNumber.color = amazingColor;
            }
            else if (ratio >= 0.5f)
            {
                completenessText.text = "Good";
                completenessImage.color = completenessText.color = completenessTextNumber.color = goodColor;
            }
            else
            {
                completenessText.text = "Bad";
                completenessImage.color = completenessText.color = completenessTextNumber.color = badColor;
            }
        }
        
        private void SetRandomIcon()
        {
            if (iconSprites != null && iconSprites.Length > 0)
            {
                int randomIndex = Random.Range(0, iconSprites.Length);
                iconImage.sprite = iconSprites[randomIndex];
            }
        }
    }
}