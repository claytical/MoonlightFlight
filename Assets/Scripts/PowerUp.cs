using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [System.Serializable]
    public enum Reward
    {
        Shield,
        Boundary,
        PassThrough,
        Part,
        Consciousness,
        Stop,
        Nuke,
        Warp
    };

    public Reward reward;

    void Update()
    {
    }

}
