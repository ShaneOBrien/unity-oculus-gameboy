using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityGB;

public class DefaultEmulatorManager : MonoBehaviour
{
	public string Filename;
	public Renderer ScreenRenderer;

	public EmulatorBase Emulator
	{
		get;
		private set;
	}

	private Dictionary<KeyCode, EmulatorBase.Button> _keyMapping;

	// Use this for initialization
	void Start()
	{
		// Init Keyboard mapping
		_keyMapping = new Dictionary<KeyCode, EmulatorBase.Button>();
		_keyMapping.Add(KeyCode.UpArrow, EmulatorBase.Button.Up);
		_keyMapping.Add(KeyCode.DownArrow, EmulatorBase.Button.Down);
		_keyMapping.Add(KeyCode.LeftArrow, EmulatorBase.Button.Left);
		_keyMapping.Add(KeyCode.RightArrow, EmulatorBase.Button.Right);
		_keyMapping.Add(KeyCode.Z, EmulatorBase.Button.A);
		_keyMapping.Add(KeyCode.X, EmulatorBase.Button.B);
		_keyMapping.Add(KeyCode.Space, EmulatorBase.Button.Start);
		_keyMapping.Add(KeyCode.LeftShift, EmulatorBase.Button.Select);


		// Load emulator
		IVideoOutput drawable = new DefaultVideoOutput();
		IAudioOutput audio = GetComponent<DefaultAudioOutput>();
		ISaveMemory saveMemory = new DefaultSaveMemory();
		Emulator = new Emulator(drawable, audio, saveMemory);
		ScreenRenderer.material.mainTexture = ((DefaultVideoOutput) Emulator.Video).Texture;

		gameObject.audio.enabled = false;
		StartCoroutine(prepareToStart());
	}

	void Update()
	{
		// Input
		foreach (KeyValuePair<KeyCode, EmulatorBase.Button> entry in _keyMapping)
		{
			if (Input.GetKeyDown(entry.Key))
				Emulator.SetInput(entry.Value, true);
			else if (Input.GetKeyUp(entry.Key))
				Emulator.SetInput(entry.Value, false);
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			byte[] screenshot = ((DefaultVideoOutput) Emulator.Video).Texture.EncodeToPNG();
			File.WriteAllBytes("./screenshot.png", screenshot);
			Debug.Log("Screenshot saved.");
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			
		}
	}

	public IEnumerator prepareToStart()
	{
		yield return new WaitForSeconds(2);
		StartCoroutine(LoadRom(Filename));
	}

	private IEnumerator LoadRom(string filename)
	{
		string path;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		path = "file://" + Environment.CurrentDirectory + "\\" + filename;
#elif UNITY_WEBPLAYER
		path = "./StreamingAssets/" + filename;
#else
		Debug.LogError("I don't know where to find the rom on this platform.");
		yield break;
#endif
		Debug.Log("Loading rom at " + path);
		WWW www = new WWW(path);
		yield return www;

		if (www.error == null)
		{
			Emulator.LoadRom(www.bytes);
			StartCoroutine(Run());
		} else
			Debug.LogError("Error during loading the rom.\n" + www.error);
	}

	private IEnumerator Run()
	{
		gameObject.audio.enabled = true;
		while (true)
		{			
			// Run
			Emulator.RunNextStep();

			yield return null;
		}
	}
}
