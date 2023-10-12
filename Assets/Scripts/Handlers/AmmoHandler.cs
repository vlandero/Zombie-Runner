using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHandler : MonoBehaviour
{
    [SerializeField] private int availableAmmo = 7;

    private bool opened = false;
    private Weapon weapon;

    private void Start()
    {
        weapon = PlayerManager.instance.playerObject.GetComponentInChildren<Weapon>();
    }

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
        weapon.GetAmmoComponent().AddAmmo(availableAmmo);
        transform.GetComponent<Animator>().SetBool("opened", true);
        GetComponentInChildren<AmmoCrateText>().gameObject.SetActive(false);
        Destroy(transform.GetComponentInChildren<AmmoBoxBullets>().gameObject, .8f);
        Destroy(gameObject, 10f);
    }

    public bool GetOpened()
    {
        return opened;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 10)
        {
            GetComponentInParent<Rigidbody>().isKinematic = true;
        }
    }
}
