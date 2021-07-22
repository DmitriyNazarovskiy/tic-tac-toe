using System.Collections;
using System.Linq;
using Configs;
using Core;
using Game;
using Game.Cell;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
	/// <summary>
	/// As I told you before, these are my first Unit-tests. I believe the look not well and implement not "right way".
	/// In general, I understand what they do and how they work.
	/// Also, I believe that the architecture I've built is not the best for Unit tests.
	/// This is why they look weird (at least for me) :)
	/// </summary>
	public class TestSuite
	{
		private TestConfig _testConfig;
		private ICommonFactory _commonFactory;
		private GameControllerBase _gameController;
		private ITimerController _timerController;

		[SetUp]
		public void Setup()
		{
			_testConfig = Resources.Load<TestConfig>(Constants.TestConfigNameString);

			_commonFactory = new CommonFactory();
			_timerController = new TimerController();
			_gameController = new GameControllerPvPC(_commonFactory, _testConfig.Config, _timerController, null, GameMode.PlayerVsPc);
			_gameController.CreateGameView(Object.FindObjectOfType<Canvas>().transform, _testConfig.Config.GameViewPrefab);
		}

		[UnityTest]
		public IEnumerator AreHintsShown()
		{
			var cells = _gameController.View.GetCells().ToList();

			foreach (var cellView in cells)
				cellView.SetHint(true, new Material(Shader.Find("Diffuse")));

			var images = cells.Select(c => c.transform.GetComponent<Image>()).ToList();
			var delayTimer = 0.0f;

			while (delayTimer < 0.1f)
			{
				delayTimer += Time.deltaTime;

				yield return null;
			}

			Assert.IsTrue(images.All(im => im.material != null));

			cells.Clear();
			cells = null;

			images.Clear();
			images = null;
		}

		[UnityTest]
		public IEnumerator DidUndoDiscardTwoPreviousMoves()
		{
			((GameControllerPvPC)_gameController).SetRandomMark();
			((GameControllerPvPC)_gameController).SetRandomMark();
			((GameControllerPvPC)_gameController).UndoButtonPressed();

			var delayTimer = 0.0f;

			while (delayTimer < 1.1f)
			{
				delayTimer += Time.deltaTime;

				yield return null;
			}

			Assert.AreEqual(_gameController.Model.MarkedCells.Count, 0);
		}

		[UnityTest]
		public IEnumerator IsPlayer1Won()
		{
			var winCombination = _testConfig.Config.WinCombinations[0];

			_gameController.Model.MarkedCells.Add(new CellModel(winCombination.Value1) {CurrentState = CellState.X});
			_gameController.Model.MarkedCells.Add(new CellModel(winCombination.Value2) {CurrentState = CellState.X});
			_gameController.Model.MarkedCells.Add(new CellModel(winCombination.Value3) {CurrentState = CellState.X});

			yield return null;

			Assert.AreEqual(_gameController.Model.CheckGameResult(), GameResult.Player1Win);
		}

		[UnityTest]
		public IEnumerator IsPlayer1Lost()
		{
			var winCombination = _testConfig.Config.WinCombinations[0];

			_gameController.Model.MarkedCells.Add(new CellModel(winCombination.Value1) { CurrentState = CellState.O });
			_gameController.Model.MarkedCells.Add(new CellModel(winCombination.Value2) { CurrentState = CellState.O });
			_gameController.Model.MarkedCells.Add(new CellModel(winCombination.Value3) { CurrentState = CellState.O });

			yield return null;

			Assert.AreNotEqual(_gameController.Model.CheckGameResult(), GameResult.Player1Win);
		}

		[TearDown]
		public void Teardown()
		{
			_commonFactory = null;
			Object.DestroyImmediate(_gameController?.View);
			_gameController = null;
			_gameController = null;
			_timerController = null;
		}
	}
}
