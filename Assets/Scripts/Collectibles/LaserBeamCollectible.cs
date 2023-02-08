using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCollectible : Collectible
{
    public override void PickUp()
    {
        //PlayerController.Instance.HasLaserBeam = true;
        DeInit();
    }
}