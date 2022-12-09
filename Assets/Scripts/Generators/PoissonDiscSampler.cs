using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public static class PoissonDiscSampler {
	private static float cellSize;

	public static List<Vector2> GeneratePoints(float a_radius, int a_meshSize, int a_attemptAmount = 10) {
		cellSize = a_radius / Mathf.Sqrt(2);

		int gridSize = Mathf.CeilToInt(a_meshSize / cellSize);
		int[,] grid = new int[gridSize, gridSize];

		List<Vector2> pointsCreated = new List<Vector2>();
		List<Vector2> activePoints = new List<Vector2>() { new Vector2(gridSize / 2, gridSize / 2) };

		int pointIndex;
		Vector2 currentPoint;
		bool isPointValid;

		// Placement of next point from current
		float angle;
		Vector2 dir;
		Vector2 potentialPoint;

		while (activePoints.Count > 0) {
			pointIndex = Random.Range(0, activePoints.Count);
			currentPoint = activePoints[pointIndex];
			isPointValid = false;

			for (int i = 0; i < a_attemptAmount; i++) {
				angle = Random.value * Mathf.PI * 2;
				dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

				potentialPoint = currentPoint + dir * Random.Range(a_radius, a_radius * 2);

				if (IsValidPoint(potentialPoint, a_meshSize, a_radius, pointsCreated, grid)) {
					pointsCreated.Add(potentialPoint);
					activePoints.Add(potentialPoint);
					grid[(int)(potentialPoint.x / cellSize), (int)(potentialPoint.y / cellSize)] = pointsCreated.Count;

					isPointValid = true;

					break;
				}
			}

			if (!isPointValid) {
				activePoints.RemoveAt(pointIndex);
			}
		}

		return pointsCreated;
	}

	private static bool IsValidPoint(Vector2 a_point, int a_meshSize, float a_radius, List<Vector2> a_createdPoints, int[,] a_grid) {
		if (a_point.x > 0 && a_point.x < a_meshSize && a_point.y > 0 && a_point.y < a_meshSize) {
			int cellX = (int)(a_point.x / cellSize);
			int cellY = (int)(a_point.y / cellSize);

			int startX = Mathf.Max(0, cellX - 2);
			int endX = Mathf.Min(cellX + 2, a_grid.GetLength(0) - 1);
			int startY = Mathf.Max(0, cellY - 2);
			int endY = Mathf.Min(cellY + 2, a_grid.GetLength(1) - 1);

			int pointIndex;
			for (int x = startX; x <= endX; x++) {
				for (int y = startY; y <= endY; y++) {
					pointIndex = a_grid[x, y] - 1;
					if (pointIndex != -1) {
						float distance = (a_point - a_createdPoints[pointIndex]).magnitude;

						if (distance < a_radius) {
							return false;
						}
					}
				}
			}

			return true;
		}

		return false;
	}
}
