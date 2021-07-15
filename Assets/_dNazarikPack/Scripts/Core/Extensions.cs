using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Core
{
	public static class Extensions
	{
		public static Vector2Int NumberToMatrixPos(int num, int extent) => new Vector2Int(num % extent, num / extent);
		public static int MatrixPosToNumber(Vector2Int vector, int extent) => vector.y * extent + vector.x;
		public static T RandomElement<T>(this IEnumerable<T> enumerable) => enumerable.RandomElementUsing<T>(new System.Random());

		public static Vector2 ScreenPointToUiSpace(RectTransform parentRect, Camera camera, Vector3 screenPos)
		{
			//Convert the screen point to ui rectangle local point
			RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPos, camera, out var localPoint);
			//Convert the local point to world point
			return localPoint;
		}

		public static Vector2 WorldPointToUiSpace(RectTransform parentRect, Camera camera, Vector3 worldPos)
		{
			var screenPos = camera.WorldToScreenPoint(worldPos);
			//Convert the screen point to ui rectangle local point
			RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPos, camera, out var localPoint);
			//Convert the local point to world point
			return localPoint;
		}

		public static Vector3 WorldPointToWorldPointOnRect(RectTransform parentRect, Camera camera, Vector3 worldPos)
		{
			var localPoint = WorldPointToUiSpace(parentRect, camera, worldPos);
			return parentRect.TransformPoint(localPoint);
		}

		public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, System.Random rand)
		{
			var iEnumerable = enumerable.ToArray();
			int index = rand.Next(0, iEnumerable.Length);
			return iEnumerable.ElementAt(index);
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			System.Random rng = new System.Random();
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		/// <summary>
		/// Splits an array into several smaller arrays.
		/// </summary>
		/// <typeparam name="T">The type of the array.</typeparam>
		/// <param name="array">The array to split.</param>
		/// <param name="size">The size of the smaller arrays.</param>
		/// <returns>An array containing smaller arrays.</returns>
		public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
		{
			for (var i = 0; i < (float) array.Length / size; i++)
			{
				yield return array.Skip(i * size).Take(size);
			}
		}

		public static TKey GetNextRandomItem<TKey>(Dictionary<TKey, float> probabilities) where TKey : class
		{
			var random = new System.Random();
			var sum = probabilities.Values.Sum();

			float from = 0;
			var rnd = (float) random.NextDouble();

			foreach (var kvp in probabilities)
			{
				var to = from + kvp.Value / sum;

				if (from <= rnd && rnd < to)
					return kvp.Key;

				from = to;
			}

			return null;
		}
	}
}