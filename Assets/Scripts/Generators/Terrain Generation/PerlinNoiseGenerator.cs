using UnityEngine;

/// <summary>
/// Controls how the perlin noise is created dependent on how many iteractions the noise goes through.
/// </summary>
public static class PerlinNoiseGenerator {

	/// <summary>
	/// Creates the noisemap that the current chunk uses dependent on how many times the noise is iterated
	/// </summary>
	/// <param name="a_octaves">How many times the perlin noise will go through for loop</param>
	/// <param name="a_persistance">How much each octave contributes to noisemap</param>
	/// <param name="a_lacunarity">How much detail an octave adds</param>
	/// <param name="a_meshSize">Size of one side of current chunk</param>
	/// <param name="a_scale">How zoomed in onto the perlin noise</param>
	/// <param name="a_offsets">Where the perlin noise should start</param>
	/// <param name="a_chunkX">Chunk offset in X axis (origin is 0)</param>
	/// <param name="a_chunkZ">Chunk offset in Z axis (origin is 0)</param>
	/// <returns>Noisemap to generate mountains and valleys</returns>
	public static float[,] GenerateNoise(int a_octaves, float a_persistance, float a_lacunarity, int a_meshSize, float a_scale, Vector2 a_offsets, float a_chunkX = 0, float a_chunkZ = 0) {

		float[,] noiseMap = new float[a_meshSize + 1, a_meshSize + 1];

		float xCoord;
		float zCoord;
		for (int z = 0; z <= a_meshSize; z++) {
			for (int x = 0; x <= a_meshSize; x++) {

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;
				float totalAmplitude = 0;
				float minTerrainHeight = 0;

				// Runs coordinates sample into the perlin noise multiple times with different frequency and amplitudes to create detail
				for (int i = 0; i < a_octaves; i++) {
					xCoord = (float)x / a_meshSize * a_scale * frequency;
					zCoord = (float)z / a_meshSize * a_scale * frequency;

					xCoord += a_offsets.x * a_scale * frequency;
					zCoord += a_offsets.y * a_scale * frequency;
					float perlinValue = Mathf.PerlinNoise(xCoord + (a_chunkX * a_scale * frequency), zCoord + (a_chunkZ * a_scale * frequency));
					noiseHeight += perlinValue * amplitude;
					totalAmplitude += amplitude;

					amplitude *= a_persistance;
					frequency *= a_lacunarity;
				}

				// Updates maximum and minimum bounds for inverse loop if noise height to out of bounds
				if (noiseHeight > totalAmplitude) {
					totalAmplitude = noiseHeight;
				}
				if (noiseHeight < minTerrainHeight) {
					minTerrainHeight = noiseHeight;
				}

				// Normalises noise height so that it is being 0 and 1
				noiseHeight = Mathf.InverseLerp(minTerrainHeight, totalAmplitude, noiseHeight);

				noiseMap[x, z] = noiseHeight;
			}
		}

		return noiseMap;
	}
}
