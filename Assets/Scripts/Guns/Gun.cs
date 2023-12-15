using UnityEngine;

public interface Gun
{
    string name { get; }
    //how much ammo one magazine holds
    int maxMagazineAmmo { get; }
    //how much ammo the gun can hold in total
    int maxAmmo { get; }
    float fireRatePerSecond { get; }
    float falloffDistance { get; }
    float recoilResetTimeSeconds { get; }
    Vector3[] recoilPattern { get; }
}
