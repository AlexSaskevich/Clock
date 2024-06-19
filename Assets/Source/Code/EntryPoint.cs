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

        private void Update()
        {
            _clockPresenter.CalculateTime();

            if (_clockPresenter.IsNeedGetServerTime)
            {
                StartCoroutine(_clockPresenter.UpdateServerTime());
            }
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }


        private void OnButtonClick()
        {
            _clockPresenter.SetRandomTime();
        }
    }
}