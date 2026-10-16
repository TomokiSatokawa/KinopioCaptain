using Input;
using UnityEngine;
using R3;

public class LightController : MonoBehaviour
{
    [SerializeField] private Material _LightOn;
    [SerializeField] private Material _LightOff;

    public MeshRenderer LightRender;

    private bool _isLightOn = true;

    void Start()
    {
        InputManager.HeadLight.Subscribe(_ => SwitchLight(!_isLightOn));
    }

    private void SwitchLight(bool isOn)
    {
        if (isOn)
        {
            LightRender.material = _LightOn;
        }
        else
        {
            LightRender.material = _LightOff;
        }
        _isLightOn = isOn;

        Debug.Log("Light is " + (_isLightOn ? "On" : "Off"));
    }
}
