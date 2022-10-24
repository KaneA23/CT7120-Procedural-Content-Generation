using UnityEngine;

/// <summary>
/// Creates a seed that allows chosen terrains to be used again.
/// Created by: Kane Adams
/// </summary>
public class SeedGenerator : MonoBehaviour {
	private LehmerPRNG PRNG;

	[SerializeField] private string gameSeed = "Default";
	[SerializeField] private bool useStringSeed;

	[Space(5)]
	[SerializeField] private bool useRandomSeed;
	private int seed;

	private void Awake() {
		PRNG = new LehmerPRNG();

		GenerateSeed();
	}

	/// <summary>
	/// Gives user a seed dependent on input given or by random
	/// </summary>
	void GenerateSeed() {
		// Allows the user to write words and numbers as a seed
		if (useStringSeed) {
			seed = gameSeed.GetHashCode();
		}

		// If chosen, the Lehmer generator returns a value to be a seed at random
		if (useRandomSeed) {
			seed = (int)PRNG.GenerateNumber(999999999);
			gameSeed = seed.ToString();
		}

		Random.InitState(seed);
	}
}
