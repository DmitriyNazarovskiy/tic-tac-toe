using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
	public interface ITimerController
	{
		int CreateTimeEntity();
		float ElapsedTime(int entityId);
		float ElapsedTimeReverse(int entityId, float countTime);
		bool RemoveEntity(int entityId);
		void ResetTimeEntity(int entityId);
	}

	public class TimeEntity
	{
		public int Id;
		public float StartTime;
	}

	public class TimerController : ITimerController
	{
		private readonly List<TimeEntity> _timeEntities;

		public TimerController() => _timeEntities = new List<TimeEntity>();

		int ITimerController.CreateTimeEntity()
		{
			_timeEntities.Add(new TimeEntity
			{
				Id = _timeEntities.Count,
				StartTime = Time.time
			});

			return _timeEntities.Count - 1;
		}

		float ITimerController.ElapsedTime(int entityId)
		{
			float elapsedTime = -1;

			var timeEntity = _timeEntities.FirstOrDefault(e => e.Id == entityId);

			if (timeEntity != null)
				elapsedTime = Math.Abs(timeEntity.StartTime - Time.time);

			return elapsedTime;
		}

		float ITimerController.ElapsedTimeReverse(int entityId, float countTime)
		{
			float elapsedTime = -1;

			var timeEntity = _timeEntities.FirstOrDefault(e => e.Id == entityId);

			if (timeEntity != null)
				elapsedTime = Math.Abs(timeEntity.StartTime - Time.time);

			return countTime - elapsedTime;
		}

		bool ITimerController.RemoveEntity(int entityId)
		{
			var timeEntity = _timeEntities.FirstOrDefault(e => e.Id == entityId);

			if (timeEntity == null)
			{
				return false;
			}

			_timeEntities.Remove(timeEntity);

			return true;
		}

		void ITimerController.ResetTimeEntity(int entityId)
		{
			if (_timeEntities.All(e => e.Id != entityId))
			{
				return;
			}

			var timeEntity = _timeEntities.First(e => e.Id == entityId);

			if (timeEntity != null)
				timeEntity.StartTime = Time.time;
		}
	}
}
