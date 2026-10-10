using UnityEngine;

public class ItemData : MonoBehaviour, IItemData
{
    [SerializeField] private string _name = "defaultItemName";

    public string Name { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Name = _name;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PullItem()
    {
        Debug.Log($"{Name}Çà¯Ç¡Ç±î≤Ç¢ÇΩÅI");
    }
}

interface IItemData
{
    public string Name { get; set; }

    void PullItem();
}
