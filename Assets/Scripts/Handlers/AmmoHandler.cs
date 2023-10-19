using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHandler : MonoBehaviour
{
    [SerializeField] private int availableAmmo = 0;

    private bool opened = false;
    private Weapon weapon;
    private AmmoCrateText ammoCrateText;
    private bool initialized = false;
    private AmmoBoxBullets ammoBoxBullets;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if(initialized) return;
        weapon = PlayerManager.instance.playerObject.GetComponentInChildren<Weapon>();
        ammoCrateText = GetComponentInChildren<AmmoCrateText>();
        ammoBoxBullets = GetComponentInChildren<AmmoBoxBullets>();
        initialized = true;
    }

    public void InitializeStart()
    {
        Init();
        opened = false;
        transform.GetComponent<Animator>().SetBool("opened", false);
        ammoBoxBullets.gameObject.SetActive(true);
        ammoCrateText.gameObject.SetActive(true);
        GetComponentInParent<Rigidbody>().isKinematic = false;
        GetComponentInChildren<AmmoBoxTrigger>().InitializeStart();
    }

    public int GetAvailableAmmo()
    {
        return availableAmmo;
    }

    public void SetAvailableAmmo(int ammo)
    {
        availableAmmo = ammo;
        ammoCrateText.UpdateAmmo(availableAmmo);
    }

    public void Open()
    {
        if(opened) return;
        opened = true;
        weapon.GetAmmoComponent().AddAmmo(availableAmmo);
        transform.GetComponent<Animator>().SetBool("opened", true);
        ammoCrateText.gameObject.SetActive(false);
        StartCoroutine(DestroyBullets());
        StartCoroutine(ResetAnimation());
        ObjectPooler.instance.Destroy(gameObject, 10f);
    }

    private IEnumerator DestroyBullets()
    {
        yield return new WaitForSeconds(.8f);
        ammoBoxBullets.gameObject.SetActive(false);
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(9f);
        transform.GetComponent<Animator>().SetBool("opened", false);
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
