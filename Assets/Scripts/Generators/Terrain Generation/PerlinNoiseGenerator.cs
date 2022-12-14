using UnityEngine;

/// <summary>
/// 
/// </summary>
public static class PerlinNoiseGenerator {

	/// <summary>
	/// 
	/// </summary>
	/// <param name="a_octaves"></param>
	/// <param name="a_persistance"></param>
	/// <param name="a_lacunarity"></param>
	/// <param name="a_meshSize"></param>
	/// <param name="a_scale"></param>
	/// <param name="a_offsets"></param>
	/// <param name="a_chunkX"></param>
	/// <param name="a_chunkZ"></param>
	/// <returns></returns>
	public static float[,] GenerateNoise(int a_octaves, float a_persistance, float a_lacunarity, int a_meshSize, float a_scale, Vector2 a_offsets, float a_chunkX, float a_chunkZ) {
		
		float[,] noiseMap = new float[a_meshSize + 1, a_meshSize + 1];
		
		float xCoord;
		float zCoord;
		for (int z = 0; z <= a_meshSize; z++) {
			for (int x = 0; x <= a_meshSize; x++) {

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;
				float totalAmplitude = 0;
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

				noiseHeight = Mathf.InverseLerp(0.0f, totalAmplitude, noiseHeight);

				noiseMap[x, z] = noiseHeight;
			}
		}

		return noiseMap;
	}
}
