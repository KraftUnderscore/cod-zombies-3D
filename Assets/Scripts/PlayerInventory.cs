using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform weaponsContainer;

    private GameObject[] guns;

    private GameObject firstSlot;
    private GameObject secondSlot;

    private Gun selected;

    private bool isFirstSelected = true;

    public void Shoot()
    {
        selected.Shoot();
    }

    private void Awake()
    {
        guns = new GameObject[weaponsContainer.childCount];
        for(int i = 0; i < weaponsContainer.childCount; i++)
            guns[i] = weaponsContainer.GetChild(i).gameObject;
        firstSlot = guns[0];
        selected = firstSlot.GetComponent<Gun>();
        firstSlot.SetActive(true);
        secondSlot = null;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && !isFirstSelected)
        {
            SwitchGun(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isFirstSelected && secondSlot != null)
        {
            SwitchGun(false);
        }
    }

    private void SwitchGun(bool switchToFirst)
    {
        GameObject prevGunObj = (isFirstSelected ? firstSlot : secondSlot);
        prevGunObj.SetActive(false);
        GameObject gunObj = (switchToFirst ? firstSlot : secondSlot);
        selected = gunObj.GetComponent<Gun>();
        isFirstSelected = switchToFirst;
        gunObj.SetActive(true);
    }
}
