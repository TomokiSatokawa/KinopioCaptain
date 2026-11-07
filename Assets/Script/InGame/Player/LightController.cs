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
            SwitchLight(false);
        }

        private void Update()
        {
            if (InputService.Player.HeadLight.Down)
                SwitchLight(!_isLightOn);
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