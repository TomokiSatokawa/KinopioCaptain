using Input;
using UnityEngine;
using R3;

public class LightController : MonoBehaviour
{
    [SerializeField] private Material _LightOn;
    [SerializeField] private Material _LightOff;

    public GameObject _LightActive;

    public MeshRenderer _LightRender;

    private bool _isLightOn = true;

    void Start()
    {
        InputManager.HeadLight.Subscribe(_ => SwitchLight(!_isLightOn));
    }

    private void SwitchLight(bool isOn)
    {
        if (isOn)
        {
            _LightRender.material = _LightOn;
            _LightActive.SetActive(true);
        }
        else
        {
            _LightRender.material = _LightOff;
            _LightActive.SetActive(false);
        }
        _isLightOn = isOn;
    }
}
