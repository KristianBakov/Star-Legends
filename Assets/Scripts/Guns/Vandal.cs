using UnityEngine;

class Vandal : Gun
{
    public string name => "Vandal";

    public int maxMagazineAmmo => 25;

    public int maxAmmo => 100;

    public float fireRatePerSecond => 8f;

    public float falloffDistance => 150f;

    public float recoilResetTimeSeconds => 0.5f;

    public Vector3[] recoilPattern => new Vector3[25]
    {
        new Vector3(-0.5f, 0.0f, 0.0f),
        new Vector3(-0.5f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(-0.8f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.8f, 0.0f),
        new Vector3(0.0f, 0.8f, 0.0f),
        new Vector3(0.0f, 0.8f, 0.0f),
        new Vector3(0.0f, 0.8f, 0.0f),
        new Vector3(0.0f, -0.8f, 0.0f),
        new Vector3(0.0f, -0.8f, 0.0f),
        new Vector3(0.0f, -0.8f, 0.0f),
        new Vector3(0.0f, -0.8f, 0.0f),
        new Vector3(0.0f, -0.8f, 0.0f),
        new Vector3(0.0f, -0.8f, 0.0f),
        new Vector3(0.0f, -0.8f, 0.0f),
        new Vector3(0.0f, -0.8f, 0.0f),
        new Vector3(0.0f, -0.8f, 0.0f),
    };
}

