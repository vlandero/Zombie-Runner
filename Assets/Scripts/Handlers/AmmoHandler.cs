using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHandler : MonoBehaviour
{
    [SerializeField] private int availableAmmo = 7;

    private bool opened = false;
    private Weapon weapon = null;

    public int GetAvailableAmmo()
    {
        return availableAmmo;
    }

    public void SetAvailableAmmo(int ammo)
    {
        availableAmmo = ammo;
    }

    public void Open()
    {
        opened = true;
        transform.parent.GetComponent<Animator>().SetBool("opened", true);
        Debug.Log(transform.parent.GetComponentInChildren<AmmoBoxBullets>());
        Destroy(transform.parent.GetComponentInChildren<AmmoBoxBullets>().gameObject, .8f);
    }

    public void Update()
    {
        if (weapon != null && Input.GetKeyDown(KeyCode.E) && !opened)
        {
            weapon.GetAmmoComponent().AddAmmo(availableAmmo);
            Open();

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform playerWrapper = collision.gameObject.transform.root;
            weapon = playerWrapper.GetComponentInChildren<Weapon>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            weapon = null;
        }
    }
}
