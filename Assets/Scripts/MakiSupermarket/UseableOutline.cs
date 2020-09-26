using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public abstract class UseableOutline : MonoBehaviour, Useable
    {
        public List<cakeslice.Outline> outlines;

        public void LookingAt()
        {
            foreach (cakeslice.Outline outline in outlines)
                outline.enabled = true;
        }

        public abstract void Use();

        void Start()
        {
            HashSet<GameObject> outlineObjects = new HashSet<GameObject>();

            foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>(true))
                outlineObjects.Add(meshRenderer.gameObject);

            if (GetComponent<MeshRenderer>() != null)
                outlineObjects.Add(gameObject);

            outlines = new List<cakeslice.Outline>();
            foreach (GameObject outlineObject in outlineObjects)
                outlines.Add(outlineObject.AddComponent(typeof(cakeslice.Outline)) as cakeslice.Outline);
        }

        void Update()
        {
            foreach (cakeslice.Outline outline in outlines)
                outline.enabled = false;
        }
    }
}
