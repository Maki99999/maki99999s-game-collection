using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Things
{
    public static void CopyTransform(Transform copyTo, Transform copyFrom)
    {
        copyTo.position = copyFrom.position;
        copyTo.rotation = copyFrom.rotation;
        copyTo.localScale = copyFrom.localScale;
    }

    public static void CopyLocalTransform(Transform copyTo, Transform copyFrom)
    {
        copyTo.localPosition = copyFrom.localPosition;
        copyTo.localRotation = copyFrom.localRotation;
        copyTo.localScale = copyFrom.localScale;
    }

    public static IEnumerator PosSLerp(Transform obj, Vector3 to, int lengthInFrames)
    {
        Vector3 oldPos = obj.position;

        for (float f = 0; f < 1f; f += 1f / lengthInFrames)
        {
            obj.position = Vector3.Lerp(oldPos, to, Mathf.SmoothStep(0, 1, f));
            yield return new WaitForSeconds(1f / 60f);
        }

        obj.position = to;
    }

    public static IEnumerator PosRotLerp(Transform obj, Transform to, int lengthInFrames)
    {
        Vector3 oldPos = obj.position;
        Quaternion oldRot = obj.rotation;

        for (float f = 0; f < 1f; f += 1f / lengthInFrames)
        {
            obj.position = Vector3.Lerp(oldPos, to.position, f);
            obj.rotation = Quaternion.Lerp(oldRot, to.rotation, f);
            yield return new WaitForSeconds(1f / 60f);
        }

        obj.position = to.position;
        obj.rotation = to.rotation;
    }

    public static IEnumerator PosRotSLerp(Transform obj, Transform to, int lengthInFrames)
    {
        Vector3 oldPos = obj.position;
        Quaternion oldRot = obj.rotation;

        float fS;
        for (float f = 0; f < 1f; f += 1f / lengthInFrames)
        {
            fS = Mathf.SmoothStep(0, 1, f);
            obj.position = Vector3.Lerp(oldPos, to.position, fS);
            obj.rotation = Quaternion.Lerp(oldRot, to.rotation, fS);
            yield return new WaitForSeconds(1f / 60f);
        }

        obj.position = to.position;
        obj.rotation = to.rotation;
    }

    public static IEnumerator PosRotScaleLerp(Transform obj, Transform to, int lengthInFrames)
    {
        Vector3 oldPos = obj.position;
        Vector3 oldScale = obj.localScale;
        Quaternion oldRot = obj.rotation;

        for (float f = 0; f < 1f; f += 1f / lengthInFrames)
        {
            obj.position = Vector3.Lerp(oldPos, to.position, f);
            obj.localScale = Vector3.Lerp(oldScale, to.localScale, f);
            obj.rotation = Quaternion.Lerp(oldRot, to.rotation, f);
            yield return new WaitForSeconds(1f / 60f);
        }

        obj.position = to.position;
        obj.localScale = to.localScale;
        obj.rotation = to.rotation;
    }


    public static IEnumerator ScaleLerp(Transform obj, Vector3 newScale, int lengthInFrames)
    {
        Vector3 oldScale = obj.localScale;

        for (float f = 0; f < 1f; f += 1f / lengthInFrames)
        {
            obj.localScale = Vector3.Lerp(oldScale, newScale, f);
            yield return new WaitForSeconds(1f / 60f);
        }

        obj.localScale = newScale;
    }

    public static IEnumerator DisableAfter(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
