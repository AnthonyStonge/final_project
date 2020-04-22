using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
public class UiAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public TextMeshProUGUI NbBulletInMagazine;
    public TextMeshProUGUI NbBulletOnPlayer;
    public TextMeshProUGUI GunName;
    public TextMeshProUGUI Life;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        GameVariables.Ui.NbBulletInMagazine = NbBulletInMagazine;
        GameVariables.Ui.NbBulletOnPlayer = NbBulletOnPlayer;
        GameVariables.Ui.GunName = GunName;
        GameVariables.Ui.Life = Life;
    }
}
