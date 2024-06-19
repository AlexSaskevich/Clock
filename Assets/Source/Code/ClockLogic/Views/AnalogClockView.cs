using System;
using UnityEngine;

namespace Source.Code.ClockLogic.Views
{
    public class AnalogClockView : MonoBehaviour, IClockView
    {
        [SerializeField] private RectTransform _hourHand;
        [SerializeField] private RectTransform _minuteHand;
        [SerializeField] private RectTransform _secondHand;

        public void SetTime(DateTime dateTime)
        {
            _hourHand.localRotation = Quaternion.Euler(0f, 0f,
                -(dateTime.Hour % (int)Constants.HoursInClock) * Constants.DegreesPerHour -
                dateTime.Minute * Constants.DegreesPerHour / 60f);
            _minuteHand.localRotation = Quaternion.Euler(0f, 0f, -dateTime.Minute * Constants.DegreesPerMinute);
            _secondHand.localRotation = Quaternion.Euler(0f, 0f, -dateTime.Second * Constants.DegreesPerSecond);
        }
    }
}