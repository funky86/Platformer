#if UNITY_EDITOR

using UnityEngine;

namespace Platformer {

    [ExecuteInEditMode]
    public partial class WorldItem {

        [Header("Editor only fields")]
        [SerializeField] float BoundsStart;
        [SerializeField] float BoundsEnd;
        [SerializeField] float BoundsHeight;

        Mesh Mesh;

        void OnValidate() {
            RefreshBounds();
        }

        void Update() {
            RefreshBounds();
        }

        void RefreshBounds() {
            if (!Mesh) {
                Mesh = GetComponent<MeshFilter>().sharedMesh;
            }
            var positionX = transform.position.x;
            var positionY = transform.position.y;
            var scaleX = transform.localScale.x;
            var scaleY = transform.localScale.y;

            BoundsStart = positionX + Mesh.bounds.min.x * scaleX;
            BoundsEnd = positionX + Mesh.bounds.max.x * scaleX;
            BoundsHeight = positionY + 0.5f * (Mesh.bounds.max.y - Mesh.bounds.min.y) * scaleY;
        }
    }
}

#endif
