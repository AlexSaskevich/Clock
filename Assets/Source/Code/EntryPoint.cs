using Source.Code.ClockLogic;
using UnityEngine;

namespace Source.Code
{
    public class EntryPoint : MonoBehaviour
    {
        private void Start()
        {
            Clock clock = new();
            ClockPresenter clockPresenter = new(clock);
            StartCoroutine(clockPresenter.GetTimeFromServer());
        }
    }
}