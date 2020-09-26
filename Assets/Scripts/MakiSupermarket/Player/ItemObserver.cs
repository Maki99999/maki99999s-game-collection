using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public interface ItemObserver
    {
        void UpdateItemStatus(string itemName, bool status);
    }
}