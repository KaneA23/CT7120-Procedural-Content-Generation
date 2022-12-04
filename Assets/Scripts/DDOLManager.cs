using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the properties for the new scene's terrain
/// Created by: Kane Adams
/// </summary>
public class DDOLManager : MonoBehaviour {
	DDOLManager instance;

	private string seed;
	private bool isRandomSeed;

	private Color snowColour;
	private Color stoneColour;
	private Color grassColour;
	private Color seaColour;

	public string Seed {
		get {
			return seed;
		}
		set {
			seed = value;
		}
	}

	public bool IsRandomSeed {
		get {
			return isRandomSeed;
		}
		set {
			isRandomSeed = value;
		}
	}

	public Color SnowColour {
		get {
			return snowColour;
		}
		set {
			snowColour = value;
		}
	}

	public Color StoneColour {
		get {
			return stoneColour;
		}
		set {
			stoneColour = value;
		}
	}

	public Color GrassColour {
		get {
			return grassColour;
		}
		set {
			grassColour = value;
		}
	}

	public Color SeaColour {
		get {
			return seaColour;
		}
		set {
			seaColour = value;
		}
	}

	private void Awake() {
		if (instance != null) {
			Destroy(this);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this);
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Changes the seed to be sent to next scene
	/// </summary>
	/// <param name="a_userSeed">User's input</param>
	public void SetSeed(string a_userSeed) {
		seed = a_userSeed;
		Debug.Log(seed);
	}
}