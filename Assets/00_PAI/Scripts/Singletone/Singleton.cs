using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Clase Singleton
/// </summary>
/// <typeparam name="T">Type of the singleton</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T _singleton;

	/// <summary>
	/// La referencia estatica de la instancia
	/// </summary>
	public static T singleton
	{
		get
		{
			return _singleton;
		}
		protected set
		{
			_singleton = value;
		}
	}

	/// <summary>
	/// Regresa un bool que representa la existencia de la instancia
	/// </summary>
	public static bool _singletonExists { get { return singleton != null; } }

	public static event Action InstanceSet = delegate { };

	/// <summary>
	/// Se asigna la instancia a la referencia estatica o se destruye si ya existe
	/// </summary>
	protected virtual void Awake()
	{
		if (_singleton != null)
		{
			Destroy(gameObject);
		}
		else
		{
			_singleton = (T)this;
    		InstanceSet();			
		}
	}

	/// <summary>
	/// OnDestroy limpia la referencia estatica
	/// </summary>
	protected virtual void OnDestroy()
	{
		if (_singleton == this)
		{
			_singleton = null;
		}
	}
}

