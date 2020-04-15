using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Guns/Pistol", fileName = "new Pistol")]
public class PistolScriptableObject : ScriptableObject
{
    [Header("Variables")]
    public short DefaultMagazineSize = 20;
    public short AmountAmmoShotEachShot = 1;

    [Header("Internal Variables")]
    public AudioClip ShootSound;    //TODO IMPLEMENT MULTIPLE SOUNDS SO IT DOESNT GET BORING
}
