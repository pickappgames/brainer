﻿using Core.Domain.Game;
using Core.Domain.Result;
using Core.Domain.Score;
using Core.Result;

namespace Core.Action {
    public class Guess {
        private readonly GameRepository gameRepository;
        private readonly ResultGenerator resultGenerator;
        private readonly IResultRepository resultRepository;
        private readonly ScoreService scoreService;

        public Guess(GameRepository gameRepository, 
            IResultRepository resultRepository,
            ResultGenerator resultGenerator,
            ScoreService scoreService) {
            this.gameRepository = gameRepository;
            this.resultRepository = resultRepository;
            this.resultGenerator = resultGenerator;
            this.scoreService = scoreService;
        }

        public GuessResult Invoke(int guessedNumber) {
            var lastResult = resultRepository.Find();

            if (lastResult.IsCorrect(guessedNumber)) {
                resultRepository.Clear();
                var newResult = GenerateNewResult(guessedNumber);
                var brainerGame = UpdateGame(guessedNumber);
                scoreService.IncrementScore();
                return new GuessResult(true, newResult, brainerGame);
            }

            return new GuessResult(false, null, null);
        }

        private GameResults GenerateNewResult(int guessedNumber) {
            var newResults = resultGenerator.Generate(guessedNumber);
            resultRepository.Put(newResults);
            return newResults;
        }

        private BrainerGame UpdateGame(int guessedNumber) {
            var game = gameRepository.Find();
            game.UpdateCurrentNumber(guessedNumber);
            gameRepository.Put(game);
            return game;
        }
    }
}