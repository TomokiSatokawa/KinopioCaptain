using UnityEngine;

namespace InGame.Gimmick
{

    public class PlantBase : MonoBehaviour
    {

        [SerializeField] private GameObject _leavesObject;
        [SerializeField] private Renderer _plantRenderer;
        [SerializeField] private Material _availableMaterial;
        [SerializeField] private Material _usedMaterial;



        private void Awake()
        {
            if (_leavesObject == null || _plantRenderer == null || _availableMaterial == null || _usedMaterial == null)
            {
                Debug.LogError("[CoinPlant] •K{‚ÌInspectorQÆ‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñB", this);
                enabled = false;
            }
        }

        protected void SetState(bool b)
        {
            _plantRenderer.material = b ? _availableMaterial : _usedMaterial;
            _leavesObject.SetActive(b);
        }
    }
}