using UnityEngine;

/// <summary>
/// Creates the colour gradient to be used for a terrain.
/// </summary>
public static class TerrainColourManager {
	/// <summary>
	/// Creates a list of colours and when each colour is used dependent on a value from 0-1
	/// </summary>
	/// <param name="a_seaColour">Lowest colour</param>
	/// <param name="a_grassColour">Low-mid colour</param>
	/// <param name="a_rockColour">Mid-high colour</param>
	/// <param name="a_snowColour">Highest colour</param>
	/// <returns>Recalculated colours for terrain</returns>
	public static Gradient CreateColourGradient(Color a_seaColour, Color a_grassColour, Color a_rockColour, Color a_snowColour) {
		Gradient colourGradient = new Gradient();

		// Terrain is split into four regions
		GradientColorKey[] colourKey = new GradientColorKey[4];
		colourKey[0].color = a_seaColour;
		colourKey[0].time = 0f;
		colourKey[1].color = a_grassColour;
		colourKey[1].time = 0.32f;
		colourKey[2].color = a_rockColour;
		colourKey[2].time = 0.55f;
		colourKey[3].color = a_snowColour;
		colourKey[3].time = 0.85f;

		// All colours will have no transparency
		GradientAlphaKey[] alphaKey = new GradientAlphaKey[1];
		alphaKey[0].alpha = 1f;
		alphaKey[0].time = 0f;

		colourGradient.SetKeys(colourKey, alphaKey);

		return colourGradient;
	}
}
