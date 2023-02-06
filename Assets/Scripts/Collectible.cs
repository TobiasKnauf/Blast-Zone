using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    private void Start()
    {
        CollectibleSpawner.Instance.Subscribe(this);
    }

    public virtual void PickUp()
    {
        CollectibleSpawner.Instance.UnSubscribe(this);
    }

    public void Destroy()
    {
        CollectibleSpawner.Instance.UnSubscribe(this);
        Destroy(this.gameObject);
    }
}
