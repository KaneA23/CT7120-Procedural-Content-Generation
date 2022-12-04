using UnityEngine;

/// <summary>
/// Creates a seed that allows chosen terrains to be used again.
/// Created by: Kane Adams
/// </summary>
public class SeedGenerator : MonoBehaviour {
	private LehmerPRNG PRNG;
	private DDOLManager DDOL;

	[SerializeField] private string gameSeed = "Default";
	[SerializeField] private bool useStringSeed;

	[Space(5)]
	[SerializeField] private bool useRandomSeed;
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
			gameSeed = DDOL.Seed;
			useRandomSeed = DDOL.IsRandomSeed;
		}
	}

	/// <summary>
	/// Gives user a seed dependent on input given or by random
	/// </summary>
	private void GenerateSeed() {
		//// Allows the user to write words and numbers as a seed
		//if (useStringSeed) {
		//	seed = gameSeed.GetHashCode();
		//}

		//// If chosen, the Lehmer generator returns a value to be a seed at random
		//if (useRandomSeed) {
		//	seed = (int)PRNG.GenerateNumber();
		//	gameSeed = seed.ToString();
		//}

		if (useRandomSeed) {
			seed = (int)PRNG.GenerateNumber();
			gameSeed = seed.ToString();
		} else {
			seed = gameSeed.GetHashCode();
		}

		Random.InitState(seed);
	}
}
