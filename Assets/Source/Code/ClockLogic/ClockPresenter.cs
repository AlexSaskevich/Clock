using System;
using System.Collections;
using Source.Code.ClockLogic.Views;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Source.Code.ClockLogic
{
    public class ClockPresenter
    {
        private const double MaxTimeDifference = 1d;
        private const double ServerUpdateInterval = 30d;

        private readonly Clock _clock;
        private readonly IClockView[] _views;
        private float _elapsedTime;
        private float _updaterServerTime;
        private DateTime _currentTime;

        public ClockPresenter(Clock clock, params IClockView[] views)
        {
            _clock = clock;
            _views = views;
        }

        public bool IsNeedGetServerTime { get; private set; }

        public IEnumerator GetTimeFromServer()
        {
            string uri = GetServerTimeUri();
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

        public IEnumerator UpdateServerTime()
        {
            yield return GetTimeFromServer();

            DateTime currentTime = _currentTime;
            DateTime timeFromServer = _clock.GetTime();

            TimeSpan timeDifference = timeFromServer - currentTime;

            if (Math.Abs(timeDifference.TotalSeconds) > MaxTimeDifference)
            {
                _currentTime = _clock.GetTime();
                Debug.LogWarning($"Correcting time from server. Difference: {timeDifference:hh\\:mm\\:ss}");
            }
        }

        public void CalculateTime()
        {
            IsNeedGetServerTime = false;

            _elapsedTime += Time.deltaTime;
            _updaterServerTime += Time.deltaTime;
            _currentTime = _clock.GetTime().AddSeconds(_elapsedTime);

            if (_updaterServerTime >= ServerUpdateInterval)
            {
                _updaterServerTime = 0f;
                IsNeedGetServerTime = true;
            }

            foreach (IClockView clockView in _views)
            {
                clockView.SetTime(_currentTime);
            }
        }

        public void SetRandomTime()
        {
            DateTime currentTime = _clock.GetTime();
            DateTime newTime = new(currentTime.Year, currentTime.Month, currentTime.Day,
                Random.Range(0, 23), Random.Range(0, 59), Random.Range(0, 59));

            _elapsedTime = 0;
            _clock.SetTime(newTime);
            _currentTime = _clock.GetTime();
            Debug.LogWarning(_currentTime);
        }

        private string GetServerTimeUri()
        {
            return "https://yandex.com/time/sync.json";
        }
    }
}