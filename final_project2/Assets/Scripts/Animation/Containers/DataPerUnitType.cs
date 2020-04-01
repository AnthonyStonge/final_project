using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class DataPerUnitType
{
    public UnitType UnitType;
    public int TotalCount;
    public KeyframeTextureBaker.BakedData BakedData;

    public InstancedSkinningDrawer Drawer;
    public InstancedSkinningDrawer Lod1Drawer;
    public InstancedSkinningDrawer Lod2Drawer;
    public InstancedSkinningDrawer Lod3Drawer;
    public NativeArray<IntPtr> BufferPointers;

    public Material Material;
    public int Count;

    public void Dispose()
    {
        if (Drawer != null) Drawer.Dispose();
        if (Lod1Drawer != null) Lod1Drawer.Dispose();
        if (Lod2Drawer != null) Lod2Drawer.Dispose();
        if (Lod3Drawer != null) Lod3Drawer.Dispose();

        if (BufferPointers.IsCreated) BufferPointers.Dispose();
    }
}