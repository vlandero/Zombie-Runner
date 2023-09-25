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
        transform.GetComponent<Animator>().SetBool("opened", true);
        GetComponentInChildren<AmmoCrateText>().gameObject.SetActive(false);
        Destroy(transform.GetComponentInChildren<AmmoBoxBullets>().gameObject, .8f);
        Destroy(gameObject, 10f);
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
        if(collision.gameObject.layer == 10)
        {
            GetComponentInParent<Rigidbody>().isKinematic = true;
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
