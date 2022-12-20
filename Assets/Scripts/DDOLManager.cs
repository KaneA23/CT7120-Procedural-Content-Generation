using UnityEngine;

/// <summary>
/// Contains the properties for the new scene's terrain.
/// </summary>
public class DDOLManager : MonoBehaviour {
	DDOLManager instance;

	// Seed variables
	private string seed;
	private bool isRandomSeed;

	// Terrain colour variables
	private Color snowColour;
	private Color rockColour;
	private Color grassColour;
	private Color seaColour;

	#region Getter Setters

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

	public Color RockColour {
		get {
			return rockColour;
		}
		set {
			rockColour = value;
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

	#endregion Getter Setters

	private void Awake() {
		if (instance != null) {
			Destroy(this);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this);
	}

	/// <summary>
	/// Changes the seed to be sent to next scene
	/// </summary>
	/// <param name="a_userSeed">User's input</param>
	public void SetSeed(string a_userSeed) {
		seed = a_userSeed;
	}
}
