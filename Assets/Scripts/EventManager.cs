using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void DeathEvent(GameObject gameObject);
    public static event DeathEvent OnDeath;

    public static void RegisterDeath(GameObject gameObject)
    {
        OnDeath(gameObject);
    }

    public delegate void SpawnEvent(GameObject gameObject);
    public static event SpawnEvent OnSpawn;

    public static void RegisterSpawn(GameObject gameObject)
    {
        OnSpawn(gameObject);
    }


}

