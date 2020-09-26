using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public interface Rideable
    {
        //Moves the Rideable, returns true if player can still move his head
        bool Move(MoveData inputs);
    }
}
