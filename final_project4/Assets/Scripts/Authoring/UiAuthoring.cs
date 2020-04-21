using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class UiAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public TextMeshPro NbBulletInMagazine;
    public TextMeshPro NbBulletOnPlayer;
    public TextMeshPro GunName;
    public TextMeshPro Life;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        GameVariables.Ui.NbBulletInMagazine = NbBulletInMagazine;
        GameVariables.Ui.NbBulletOnPlayer = NbBulletOnPlayer;
        GameVariables.Ui.GunName = GunName;
        GameVariables.Ui.Life = Life;
    }
}
