using UnityEngine;

public class ItemDetection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            Debug.Log("ÉAÉCÉeÉÄÇ∆ê⁄êG");
            if(other.gameObject.TryGetComponent(out IItemData itemData))
            {
                itemData.PullItem();
            }
        }
    }
}
