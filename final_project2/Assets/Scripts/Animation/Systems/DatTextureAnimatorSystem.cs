using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Profiling;

public enum UnitType
{
    Melee = 0,
    Skeleton = 1
}

public struct AnimationName
{
    public const int Attack1 = 0;
    public const int Attack2 = 1;
    public const int AttackRanged = 2;
    public const int Death = 3;
    public const int Falling = 4;
    public const int Idle = 5;
    public const int Walk = 6;
}

public class DatTextureAnimatorSystem : SystemBase
{
    public bool initialized = false;
    
    private static NativeArray<AnimationClipBakedData> animationClipData;
    public static Dictionary<UnitType, DataPerUnitType> perUnitTypeDataHolder;

    public int lod0Count;
    public int lod1Count;
    public int lod2Count;
    public int lod3Count;

    protected override void OnUpdate()
    {
        if (!initialized)
        {
            animationClipData =
                new NativeArray<AnimationClipBakedData>(100, Allocator.Persistent);

            perUnitTypeDataHolder = new Dictionary<UnitType, DataPerUnitType>();
            InstantiatePerUnitTypeData(UnitType.Melee);
            InstantiatePerUnitTypeData(UnitType.Skeleton);
        }

       
        float dt = Time.DeltaTime;
        if (perUnitTypeDataHolder != null)
        {
	        //previousFrameFence.Complete();
	       // previousFrameFence = inputDeps;

	       // lod0Count = lod1Count = lod2Count = lod3Count = 0;

	        /*foreach (var data in perUnitTypeDataHolder)
	        {
		        data.Value.Drawer.Draw();
		        data.Value.Lod1Drawer.Draw();
		        data.Value.Lod2Drawer.Draw();
		        data.Value.Lod3Drawer.Draw();

		        lod0Count += data.Value.Drawer.UnitToDrawCount;
		        lod1Count += data.Value.Lod1Drawer.UnitToDrawCount;
		        lod2Count += data.Value.Lod2Drawer.UnitToDrawCount;
		        lod3Count += data.Value.Lod3Drawer.UnitToDrawCount;
		        data.Value.Count = lod0Count + lod1Count + lod2Count + lod3Count;
	        }*/
	        
	        Entities.ForEach((ref TextureAnimatorData textureAnimatorData) =>
	        {
		        
		        if (textureAnimatorData.CurrentAnimationId != textureAnimatorData.NewAnimationId)
		        {
			        textureAnimatorData.CurrentAnimationId = textureAnimatorData.NewAnimationId;
			        textureAnimatorData.AnimationNormalizedTime = 0f;
		        }
		        
		        AnimationClipBakedData clip =
			        animationClipData[(int) textureAnimatorData.UnitType * 25 + textureAnimatorData.CurrentAnimationId];
		        float normalizedTime = textureAnimatorData.AnimationNormalizedTime + dt / clip.AnimationLength;
		        
		        if (normalizedTime > 1.0f)
		        {
			        if (clip.Looping) normalizedTime = normalizedTime % 1.0f;
			        else normalizedTime = 1f;
		        }

		        textureAnimatorData.AnimationNormalizedTime = normalizedTime;

	        }).Schedule();
	    
	        //var prepareAnimatorFence =
	        //   prepareAnimatorJob.Schedule(units.Length, SimulationState.BigBatchSize, previousFrameFence);

	       //NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(4, Allocator.Temp);
	       //jobHandles[0] = prepareAnimatorFence;

	       /* foreach (var data in perUnitTypeDataHolder)
	        {
		        switch (data.Key)
		        {
			        case UnitType.Melee:
				        ComputeFences(meleeUnits.animationData, dt, meleeUnits.transforms, data);
				        data.Value.Count = meleeUnits.Length;
				        break;
			        case UnitType.Skeleton:
				        ComputeFences(skeletonUnits.animationData, dt, skeletonUnits.transforms, data);
				        data.Value.Count = skeletonUnits.Length;
				        break;
		        }
	        }*/

	        //Profiler.BeginSample("Combine all dependencies");
	        //previousFrameFence = JobHandle.CombineDependencies(jobHandles);
	        //Profiler.EndSample();

	       // jobHandles.Dispose();
	        //return previousFrameFence;
        }

       // return inputDeps;

    }
    
    private void InstantiatePerUnitTypeData(UnitType type)
	{
		//var minionPrefab = PrefabSpawner.GetMinionPrefab(type);
		//var renderingData = minionPrefab.GetComponentInChildren<RenderingDataProxy>().Value;
		var renderingData = PrefabSpawner.GetRenderingData(type);
		var bakingObject = GameObject.Instantiate(renderingData.bakingPrefab);
		SkinnedMeshRenderer renderer = bakingObject.GetComponentInChildren<SkinnedMeshRenderer>();
		Material material = renderingData.material;
		LodData lodData = renderingData.lodData;

		var dataPerUnitType = new DataPerUnitType
		{
			UnitType = type,
			BakedData = KeyframeTextureBaker.BakeClips(renderer,
														GetAllAnimationClips(renderer.GetComponentInParent<Animation>()), lodData),
			Material = material,
		};
		dataPerUnitType.Drawer = new InstancedSkinningDrawer(dataPerUnitType, dataPerUnitType.BakedData.NewMesh);
		dataPerUnitType.Lod1Drawer = new InstancedSkinningDrawer(dataPerUnitType, dataPerUnitType.BakedData.lods.Lod1Mesh);
		dataPerUnitType.Lod2Drawer = new InstancedSkinningDrawer(dataPerUnitType, dataPerUnitType.BakedData.lods.Lod2Mesh);
		dataPerUnitType.Lod3Drawer = new InstancedSkinningDrawer(dataPerUnitType, dataPerUnitType.BakedData.lods.Lod3Mesh);

		perUnitTypeDataHolder.Add(type, dataPerUnitType);
		TransferAnimationData(type);
		GameObject.Destroy(bakingObject);
	}
    
    private AnimationClip[] GetAllAnimationClips(Animation animation)
    {
	    List<AnimationClip> animationClips = new List<AnimationClip>();
	    foreach (AnimationState state in animation)
	    {
		    animationClips.Add(state.clip);
	    }

	    animationClips.Sort((x, y) => String.Compare(x.name, y.name, StringComparison.Ordinal));

	    return animationClips.ToArray();
    }
    
    private void TransferAnimationData(UnitType type)
    {
	    var bakedData = perUnitTypeDataHolder[type].BakedData;
	    for (int i = 0; i < bakedData.Animations.Count; i++)
	    {
		    AnimationClipBakedData data = new AnimationClipBakedData();
		    data.AnimationLength = bakedData.Animations[i].Clip.length;
		    GetTextureRangeAndOffset(bakedData, bakedData.Animations[i], out data.TextureRange,
			    out data.TextureOffset, out data.OnePixelOffset, out data.TextureWidth);
		    data.Looping = bakedData.Animations[i].Clip.wrapMode == WrapMode.Loop;
		    animationClipData[(int) type * 25 + i] = data;
	    }
    }
    private void GetTextureRangeAndOffset(KeyframeTextureBaker.BakedData bakedData,
	    KeyframeTextureBaker.AnimationClipData clipData, out float range, out float offset,
	    out float onePixelOffset, out int textureWidth)
    {
	    float onePixel = 1f / bakedData.Texture0.width;
	    float start = (float) clipData.PixelStart / bakedData.Texture0.width + onePixel * 0.5f;
	    float end = (float) clipData.PixelEnd / bakedData.Texture0.width + onePixel * 0.5f;
	    onePixelOffset = onePixel;
	    textureWidth = bakedData.Texture0.width;
	    range = end - start;
	    offset = start;
    }
    
    /*private void ComputeFences(ComponentDataArray<TextureAnimatorData> textureAnimatorDataForUnitType, float dt,
			ComponentDataArray<UnitTransformData> unitTransformDataForUnitType,
			KeyValuePair<UnitType, DataPerUnitType> data)
		{
			Profiler.BeginSample("Scheduling");
			// TODO: Replace this with more efficient search.
			Profiler.BeginSample("Create filtering jobs");
			var cameraPosition = Camera.main.transform.position;


			data.Value.Drawer.ObjectPositions.Clear();
			data.Value.Lod1Drawer.ObjectPositions.Clear();
			data.Value.Lod2Drawer.ObjectPositions.Clear();
			data.Value.Lod3Drawer.ObjectPositions.Clear();

			data.Value.Drawer.ObjectRotations.Clear();
			data.Value.Lod1Drawer.ObjectRotations.Clear();
			data.Value.Lod2Drawer.ObjectRotations.Clear();
			data.Value.Lod3Drawer.ObjectRotations.Clear();

			data.Value.Drawer.TextureCoordinates.Clear();
			data.Value.Lod1Drawer.TextureCoordinates.Clear();
			data.Value.Lod2Drawer.TextureCoordinates.Clear();
			data.Value.Lod3Drawer.TextureCoordinates.Clear();

			var cullAndComputeJob = new CullAndComputeParametersSafe()
			{
				unitTransformData = unitTransformDataForUnitType,
				textureAnimatorData = textureAnimatorDataForUnitType,
				animationClips = animationClipData,
				dt = dt,
				CameraPosition = cameraPosition,
				DistanceMaxLod0 = data.Value.BakedData.lods.Lod1Distance,
				DistanceMaxLod1 = data.Value.BakedData.lods.Lod2Distance,
				DistanceMaxLod2 = data.Value.BakedData.lods.Lod3Distance,
				Lod0Positions = data.Value.Drawer.ObjectPositions,
				Lod0Rotations = data.Value.Drawer.ObjectRotations,
				Lod0TexturePositions = data.Value.Drawer.TextureCoordinates,
				Lod1Positions = data.Value.Lod1Drawer.ObjectPositions,
				Lod1Rotations = data.Value.Lod1Drawer.ObjectRotations,
				Lod1TexturePositions = data.Value.Lod1Drawer.TextureCoordinates,
				Lod2Positions = data.Value.Lod2Drawer.ObjectPositions,
				Lod2Rotations = data.Value.Lod2Drawer.ObjectRotations,
				Lod2TexturePositions = data.Value.Lod2Drawer.TextureCoordinates,
				Lod3Positions = data.Value.Lod3Drawer.ObjectPositions,
				Lod3Rotations = data.Value.Lod3Drawer.ObjectRotations,
				Lod3TexturePositions = data.Value.Lod3Drawer.TextureCoordinates,
			};

			Profiler.EndSample();

			Profiler.EndSample();
		}
	}*/
}
