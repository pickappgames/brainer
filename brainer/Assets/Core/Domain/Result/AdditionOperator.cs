﻿using Core.Result;

namespace Core.Domain.Result {
    public class AdditionOperator : IOperator {
        public string GetSymbol() {
            return "+";
        }

        public int Apply(int first, int second) {
            return first + second;
        }
    }
}