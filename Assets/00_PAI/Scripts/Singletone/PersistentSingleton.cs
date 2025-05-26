using UnityEngine;
using System.Collections;


/// <summary>
/// Singleton que persiste a traves de las escenas
/// </summary>
public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
{
	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}
}
