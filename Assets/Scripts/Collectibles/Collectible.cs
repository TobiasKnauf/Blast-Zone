using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public abstract void PickUp();

    public void DeInit()
    {
        CollectibleSpawner.Instance.UnSubscribe(this);
        Destroy(this.gameObject);
    }
}
