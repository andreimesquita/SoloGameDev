using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Search.Adversarial
{
	public enum EPieceType
	{
		Player,
		Computer,
		Cross,
		Circle
	}

	public class Table
	{
		public EPieceType[,] Tiles = new EPieceType[3, 3];

		public GameStateEnum GetGameState()
		{
			throw new NotImplementedException();
			return GameStateEnum.None;
		}
	}

	public enum GameStateEnum
	{
		None,

		Playing
	}

	public class TableController
	{
		/*
            Default heuristic
                +1 = CrossWins
                0  = Draw/Nothing
                -1 = CircleWins
        */

		//TODO
		public Table[] GetNextMoves(Table table, EPieceType piece)
		{
			throw new NotImplementedException();
		}

		public int GetScore(Table table)
		{
			throw new NotImplementedException();
		}
	}

	public class TicTacToeModel
	{
		public EPieceType Computer;
		public EPieceType Player;
	}

	public class MinimaxSearch
	{
		private TableController _controller;

		public Table FindBestMove(Table table, TicTacToeModel model, TableController controller)
		{
			_controller = controller;

			Table[] nextMoves = controller.GetNextMoves(table, model.Computer);

			int bestMoveIndex = -1;
			int bestMoveScore;

			if (model.Computer == EPieceType.Cross)
			{
				bestMoveScore = int.MinValue;

				for (int i = 0; i < nextMoves.Length; i++)
				{
					if (nextMoves[i] == null)
					{
						break;
					}

					int moveScore = GetMaximumTreeCost(nextMoves[i], int.MaxValue);
					if (moveScore > bestMoveScore)
					{
						bestMoveScore = moveScore;
						bestMoveIndex = i;
					}
				}
			}
			else
			{
				bestMoveScore = int.MaxValue;

				for (int i = 0; i < nextMoves.Length; i++)
				{
					if (nextMoves[i] == null)
					{
						break;
					}

					int moveScore = GetMinimumTreeCost(nextMoves[i], int.MaxValue);
					if (moveScore < bestMoveScore)
					{
						bestMoveScore = moveScore;
						bestMoveIndex = i;
					}
				}
			}

			return nextMoves[bestMoveIndex];
		}

		private int GetMinimumTreeCost(Table table, int depth)
		{
			if (depth <= 0 || table.GetGameState() != GameStateEnum.Playing)
			{
				return _controller.GetScore(table);
			}

			int score = int.MaxValue;

			Table[] nextMoves = _controller.GetNextMoves(table, EPieceType.Circle);

			for (int i = 0; i < nextMoves.Length; i++)
			{
				if (nextMoves[i] == null)
				{
					break;
				}
				score = Math.Min(score, GetMaximumTreeCost(nextMoves[i], depth - 1));
			}

			return score;
		}

		private int GetMaximumTreeCost(Table table, int depth)
		{
			if (depth <= 0 || table.GetGameState() != GameStateEnum.Playing)
			{
				return _controller.GetScore(table);
			}

			int score = int.MinValue;

			Table[] nextMoves = _controller.GetNextMoves(table, EPieceType.Cross);

			for (int i = 0; i < nextMoves.Length; i++)
			{
				if (nextMoves[i] == null)
				{
					break;
				}
				score = Math.Max(score, GetMinimumTreeCost(nextMoves[i], depth - 1));
			}

			return score;
		}
	}
}