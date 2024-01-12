using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Animations;

namespace NinjutsuGames.Photon.Runtime.Components
{
    public class LookAtCamera : MonoBehaviour
    {
        private void Update()
        {
            if (ShortcutMainCamera.Instance && enabled) Setup();
        }

        private void Setup()
        {
            Camera worldCamera = ShortcutMainCamera.Instance ? ShortcutMainCamera.Instance.Get<Camera>() : null;
            if (!worldCamera) worldCamera = Camera.current;
            if (!worldCamera) worldCamera = FindObjectOfType<Camera>();
            
            LookAtConstraint constraint = GetComponent<LookAtConstraint>();
            if (!constraint) constraint = gameObject.AddComponent<LookAtConstraint>();
            constraint.rotationOffset = new Vector3(0, 180, 0);
            constraint.SetSources(new List<ConstraintSource>
            {
                new()
                {
                    sourceTransform = worldCamera.transform,
                    weight = 1.0f
                }
            });
            
            Canvas canvas = GetComponent<Canvas>();
            if (canvas) canvas.worldCamera = worldCamera;

            constraint.constraintActive = true;

            enabled = false;
            Destroy(this);
        }
    }
}