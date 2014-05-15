using UnityEngine;
using System;
using System.IO;

using UnityGB;

public class DefaultSaveMemory : ISaveMemory
{
	public void Save(string name, byte[] data)
	{
		if (data == null)
			return;

		string path = null;
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_WIN
		path = Environment.CurrentDirectory + "\\Assets\\StreamingAssets\\";
#else
		Debug.Log("I don't know where to save data on this platform.");
#endif

		if (path != null)
		{
			try
			{
				File.WriteAllBytes(path + name + ".sav", data);
			} catch (System.Exception e)
			{
				Debug.Log("Couldn't save data file.");
				Debug.Log(e.Message);
			}
		}
	}

	public byte[] Load(string name)
	{
		string path = null;
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_WIN
		path = Environment.CurrentDirectory + "\\Assets\\StreamingAssets\\";
#else
		Debug.Log("I don't know where to load data on this platform.");
#endif
		
		byte[] loadedData = null;

		if (path != null)
		{
			try
			{
				loadedData = File.ReadAllBytes(path + name + ".sav");
			} catch (System.Exception e)
			{
				Debug.Log("Couldn't load data file.");
				Debug.Log(e.Message);
			}
		}
		return loadedData;
	}
}
