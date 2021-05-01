using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public new string name;
    public int price;
    public GameObject item;
    public bool isBarrier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.instance.ShowShopItem(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.instance.HideShopItem();
    }
}
