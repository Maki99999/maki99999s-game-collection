using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public abstract class Rideable : Useable
    {
        public abstract void Move(float xRot, float yRot, float axisSneak, float axisHorizontal, float axisVertical);
    }
}
