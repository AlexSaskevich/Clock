using Source.Code.ClockLogic;
using Source.Code.ClockLogic.Views;
using UnityEngine;

namespace Source.Code
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private AnalogClockView _analogClockView;
        [SerializeField] private DigitalClockView _digitalClockView;

        private ClockPresenter _clockPresenter;

        private void Start()
        {
            Clock clock = new();
            _clockPresenter = new ClockPresenter(clock, _analogClockView, _digitalClockView);
            StartCoroutine(_clockPresenter.GetTimeFromServer());
        }

        private void Update()
        {
            _clockPresenter.CalculateTime();
        }
    }
}