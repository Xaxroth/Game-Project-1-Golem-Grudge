using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 5f);
    }
}
