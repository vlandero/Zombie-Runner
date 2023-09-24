using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDamageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private Camera mainCamera;
    private float damage;

    public void SetDamage(float d)
    {
        damage = d;
        text.text = damage.ToString();
    }

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("PlayerMainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
