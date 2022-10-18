using System;

/// <summary>
/// Uses the Linear Congruential alogithm to generate a pseudorandom number
/// Created by: Kane Adams
/// </summary>
public class LehmerPRNG {
	private const long m = 8388608;   // Modulus value, 0 < m, currently 2^23 
	private const long a = 2572421;   // Multiplier, 0 < a < m
	private const long c = 4278205;   // Increment amount 0 <= c < m

	/// <summary>
	/// Uses the Linear Congruental method to generate a pseudorandom value
	/// </summary>
	/// <param name="a_minValue">Smallest value wanted, inclusive with default of 0</param>
	/// <param name="a_maxValue">Largest value, exclusive</param>
	/// <returns>Pseudoranom value</returns>
	public float GenerateNumber(int a_maxValue, int a_minValue = 0) {
		long x = DateTime.Now.Ticks % m;    // Creates a seed based on current date and time
		long result = 0;
		long value;

		// Applies formular Xn+1 = (aXn + c) % m until result within user's desired range
		do {
			for (int n = 0; n < 10; n++) {
				value = (a * x + c) % m;
				result = value;

				x = value;
			}
			result %= a_maxValue;
		} while (result < a_minValue);

		return result;
	}
}

