using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMan : MonoBehaviour
{
    public EmitterReference[] ParticleSystemReferences;
    static Dictionary<string, ParticleSystem> Emitters;

    // Start is called before the first frame update
    void Awake()
    {
        Emitters = new Dictionary<string, ParticleSystem>();
        for (int i = 0; i < ParticleSystemReferences.Length; i++)
        {
            if (ParticleSystemReferences[i].Emitter == null)
            {
                Debug.Log($"{ParticleSystemReferences[i].Name} null");
            }
            Emitters.Add(ParticleSystemReferences[i].Name.ToLower(), ParticleSystemReferences[i].Emitter);
        }
    }

    public static void Emit(string name, int amount, Vector3 position, Vector3 normal)
    {
        if (Emitters.ContainsKey(name))
        {
            ParticleSystem p = Emitters[name];
            p.transform.position = position;
            p.transform.forward = normal;
            p.Emit(amount);
        }
    }

    public static void Emit(string name, int amount, Vector3 position)
    {
        if (Emitters.ContainsKey(name))
        {
            ParticleSystem p = Emitters[name];
            p.transform.position = position;
            p.Emit(amount);
        }
    }

    public static void Emit(string name, int amount)
    {
        if (Emitters.ContainsKey(name))
        {
            ParticleSystem p = Emitters[name];
            p.Emit(amount);
        }
    }
}

[Serializable]
public struct EmitterReference
{
    public string Name;
    public ParticleSystem Emitter;
}