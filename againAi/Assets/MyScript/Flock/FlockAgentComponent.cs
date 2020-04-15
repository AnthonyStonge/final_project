using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct FlockAgentComponent : IBufferElementData
{
    public float driveFactor;
    public float maxSpeed;
    public float neighborRadius;
    public float avoidanceRadius;
    public float avoidanceRadiusMultiplier;
    public float squareMaxSpeed;
    public float squareNeighborRadius;
    public float squareAvoidanceRadius;
}
