using UnityEngine;
using System.Collections;

public class SwitchMaterial : MonoBehaviour {
	public Renderer Renderer;
	public Shader DefaultShader, ReliefShader;
	
	private bool _useRelief = false;
	public bool UseRelief
	{
		get
		{
			return _useRelief;
		}
		
		set
		{
			if(value != _useRelief) {
				_useRelief = value;
				SetShader();
			}
		}
	}

	void OnGUI() {
		UseRelief = GUI.Toggle (new Rect (Screen.width - 120f, Screen.height - 40f, 120f, 30f), UseRelief, "Enable Relief");
	}

	private void SetShader() {
		Renderer.material.shader = (_useRelief) ? ReliefShader : DefaultShader;
	}
}
