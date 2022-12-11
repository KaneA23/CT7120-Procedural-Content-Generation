using UnityEngine;

/// <summary>
/// Creates a seed that allows chosen terrains to be used again.
/// </summary>
public class SeedGenerator : MonoBehaviour {
	private LehmerPRNG PRNG;
	private DDOLManager DDOL;

	private string gameSeed = "Default";

	private bool useRandomSeed;
	private int seed;

	private void Awake() {
		PRNG = new LehmerPRNG();
		DDOL = FindObjectOfType<DDOLManager>();

		GetMenuValues();
		GenerateSeed();
	}

	/// <summary>
	/// Assigns local variables to values collected through player choice in Main Menu Scene
	/// </summary>
	private void GetMenuValues() {
		if (DDOL.Seed == null) {
			useRandomSeed = true;
		} else {
			gameSeed = DDOL.Seed;
			useRandomSeed = DDOL.IsRandomSeed;

		}
	}

	/// <summary>
	/// Gives user a seed dependent on input given or by random
	/// </summary>
	private void GenerateSeed() {
		if (useRandomSeed) {
			seed = (int)PRNG.GenerateNumber();
			gameSeed = seed.ToString();
		} else {
			seed = gameSeed.GetHashCode();
		}

		Random.InitState(seed);
	}
}
