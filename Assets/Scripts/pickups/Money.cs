using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Pickup
{
    protected override void GetBonus()
    {
        levelManager.GetMoney();
    }
}
