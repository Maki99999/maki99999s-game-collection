using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpinningObject : MonoBehaviour
{
    public int movingFrameCount;
    public AnimationCurve moveSpeedCurve;
    Vector3 movingOffset;
    Vector3 position0 = Vector3.zero;
    public Vector3 position1;
    public Vector3 position2;
    public bool movingBackAndForth;
    bool movingBack = false;

    [Space(20)]

    public int rotatingFrameCount;
    public AnimationCurve rotateSpeedCurve;
    Vector3 rotatingOffset;
    Vector3 rotation0 = Vector3.zero;
    public Vector3 rotation1;
    public Vector3 rotation2;
    public bool rotatingBackAndForth;
    bool rotatingBack = false;

    private void Awake()
    {
        movingOffset = transform.localPosition;
        rotatingOffset = transform.localEulerAngles;
    }

    private void OnEnable()
    {
        if (movingFrameCount > 0)
            StartCoroutine(Move());

        if (rotatingFrameCount > 0)
            StartCoroutine(Spin());
    }

    private IEnumerator Move()
    {
        while (enabled)
        {
            for (int i = 1; i <= movingFrameCount / 2; i++)
            {
                transform.localPosition = Vector3.Lerp(movingBack ? position2 : position0, position1, moveSpeedCurve.Evaluate(2f * i / movingFrameCount)) + movingOffset;
                yield return new WaitForSeconds(1f / 60f);
            }

            for (int i = 1; i <= movingFrameCount / 2; i++)
            {
                transform.localPosition = Vector3.Lerp(position1, movingBack ? position0 : position2, moveSpeedCurve.Evaluate(2f * i / movingFrameCount)) + movingOffset;
                yield return new WaitForSeconds(1f / 60f);
            }

            if (movingBackAndForth)
            {
                movingBack = !movingBack;
            }
        }
    }

    private IEnumerator Spin()
    {
        while (enabled)
        {
            for (int i = 1; i <= rotatingFrameCount / 2; i++)
            {
                transform.localEulerAngles = Vector3.Lerp(rotatingBack ? rotation2 : rotation0, rotation1, rotateSpeedCurve.Evaluate(2f * i / rotatingFrameCount)) + rotatingOffset;
                yield return new WaitForSeconds(1f / 60f);
            }

            for (int i = 1; i <= rotatingFrameCount / 2; i++)
            {
                transform.localEulerAngles = Vector3.Lerp(rotation1, rotatingBack ? rotation0 : rotation2, rotateSpeedCurve.Evaluate(2f * i / rotatingFrameCount)) + rotatingOffset;
                yield return new WaitForSeconds(1f / 60f);
            }

            if (rotatingBackAndForth)
            {
                rotatingBack = !rotatingBack;
            }
        }
    }
}
