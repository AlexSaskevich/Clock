using System;
using TMPro;
using UnityEngine;

namespace Source.Code.ClockLogic.Views
{
    public class DigitalClockView : MonoBehaviour, IClockView
    {
        [SerializeField] private TextMeshProUGUI _timeText;

        public void SetTime(DateTime dateTime)
        {
            _timeText.text = dateTime.ToString("HH:mm:ss");
        }
    }
}