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

	private void GetMenuValues() {
		if (DDOL.Seed == null) {
			useRandomSeed = true;
		} else {
			if (DDOL.Seed != null) {
				gameSeed = DDOL.Seed;
				useRandomSeed = DDOL.IsRandomSeed;
			}
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
