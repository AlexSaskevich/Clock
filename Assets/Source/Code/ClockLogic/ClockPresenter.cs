using System;
using System.Collections;
using Source.Code.ClockLogic.Views;
using UnityEngine;
using UnityEngine.Networking;

namespace Source.Code.ClockLogic
{
    public class ClockPresenter
    {
        private readonly Clock _clock;
        private readonly IClockView[] _views;
        private float _elapsedTime;
        private DateTime _currentTime;

        public ClockPresenter(Clock clock, params IClockView[] views)
        {
            _clock = clock;
            _views = views;
        }

        public IEnumerator GetTimeFromServer()
        {
            const string uri = "https://yandex.com/time/sync.json";
            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ServerTimeResponse jsonData = JsonUtility.FromJson<ServerTimeResponse>(webRequest.downloadHandler.text);
                _clock.SetTime(DateTimeOffset.FromUnixTimeMilliseconds((long)jsonData.time).LocalDateTime);
                Debug.LogWarning(_clock.GetTime());
            }
            else
            {
                Debug.LogError("Error fetching time from server: " + webRequest.error);
                _clock.SetTime(DateTime.Now);
            }
        }

        public void CalculateTime()
        {
            _elapsedTime += Time.deltaTime;
            _currentTime = _clock.GetTime().AddSeconds(_elapsedTime);

            foreach (IClockView clockView in _views)
            {
                clockView.SetTime(_currentTime);
            }
        }
    }
}