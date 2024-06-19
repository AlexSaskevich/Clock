using Source.Code.ClockLogic;
using Source.Code.ClockLogic.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private AnalogClockView _analogClockView;
        [SerializeField] private DigitalClockView _digitalClockView;
        [SerializeField] private Button _button;

        private ClockPresenter _clockPresenter;

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
            Clock clock = new();
            _clockPresenter = new ClockPresenter(clock, _analogClockView, _digitalClockView);
            StartCoroutine(_clockPresenter.GetTimeFromServer());
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void Update()
        {
            _clockPresenter.CalculateTime();
        }

        private void OnButtonClick()
        {
            _clockPresenter.SetRandomTime();
        }
    }
}