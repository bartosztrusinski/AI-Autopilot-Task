using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Target")
    {
      Destroy(gameObject);
    }
  }
}
