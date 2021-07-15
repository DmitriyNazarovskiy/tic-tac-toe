using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public static class Randomizer
	{
		public static int[] GetNumbers(int length, int min = 0, int max = int.MaxValue, bool sequence = false)
		{
			var numbers = new int[length];
			var range = new List<int>(max - min);

			for (var i = min; i < max; i++)
				range.Add(i);

			if (sequence)
			{
				var startId = Random.Range(0, range.Count);

				if (startId + length > range.Count)
					startId = range.Count - length;

				for (var i = 0; i < numbers.Length; i++)
					numbers[i] = range[startId + i];

				return numbers;
			}

			range.Shuffle();

			for (var i = 0; i < numbers.Length; i++)
				numbers[i] = range[i];

			return numbers;
		}

		public static float GetNumberInRange(float from, float to) => Random.Range(from, to);
		public static int GetNumberInRange(int from, int to) => Random.Range(from, to);

		public static Color GetRandomColor(bool isAlphaRandom)
			=> new Color(Random.value, Random.value, Random.value, isAlphaRandom ? Random.value : 1.0f);
	}
}
