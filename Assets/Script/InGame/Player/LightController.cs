using Input;
using R3;
using UnityEngine;

namespace InGame.Player
{

    public class LightController : MonoBehaviour
    {
        [SerializeField] private Renderer _render;
        [SerializeField] private Material _activeMaterial;
        [SerializeField] private Material _inactiveMaterial;
        [SerializeField] private GameObject _spotLight;

        private bool _isLightOn = true;

        private void Start()
        {
            InputManager.HeadLight.Subscribe(_ => SwitchLight(!_isLightOn)).RegisterTo(destroyCancellationToken);
            SwitchLight(false);
        }

        private void SwitchLight(bool isOn)
        {
            if (isOn)
            {
                _render.material = _activeMaterial;
            }
            else
            {
                _render.material = _inactiveMaterial;
            }

            _spotLight.SetActive(isOn);
            _isLightOn = isOn;
        }
    }
}