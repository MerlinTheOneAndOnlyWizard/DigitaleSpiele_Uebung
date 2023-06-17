using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoempelForceApplier : MonoBehaviour
{
    [SerializeField] private PlaneController plane;

    [SerializeField] private float _force = 10;

    private void OnTriggerStay(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player") || !plane.IsJumping()) return;

        Vector3 dir = (collision.transform.position - transform.position).normalized;
        if (dir == Vector3.zero)
        {
            dir = Vector3.up;
        }

        collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * _force);

        collision.gameObject.GetComponent<MaterialUpdate>().DoWhiteFlash(); 
    }
}
