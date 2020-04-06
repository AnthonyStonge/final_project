
    using System;
    using Unity.Entities;
    using UnityEngine;

    [Serializable]
    public struct MeshAnimationComponent : IComponentData
    {
        public short currentActiveMesh;
    }
