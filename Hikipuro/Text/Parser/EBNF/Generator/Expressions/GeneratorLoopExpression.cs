using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// ループの処理用.
	/// </summary>
	class GeneratorLoopExpression : GeneratorExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			if (DebugFlag) {
				Debug.WriteLine("LoopExpression.Interpret(): " + Name + ", " + Expressions.Count);
			}
			Matches = new GeneratorTokenMatches(Name);
			IsMatch = false;

			bool loop = true;
			while (loop) {
				int count = 0;
				GeneratorExpression exp2 = null;
				foreach (GeneratorExpression exp in Expressions) {
					exp2 = exp;
					if (DebugFlag) {
						Debug.WriteLine("LoopExpression.Expressions : (" + exp.Name + ")");
					}
					exp.Interpret(context);
					IsMatch = exp.IsMatch;
					if (exp.IsMatch == false) {
						break;
					}
					Matches.ConcatTokens(exp.Matches);
					//Matches.ConcatTokens(exp.Matches);
					count++;
				}
				if (count == 0) {
					//Console.WriteLine("LoopExpression: count == 0: " + exp2.Type + ", " + exp2.Name);
					break;
				}
				if (count != Expressions.Count) {
					throw new InterpreterException("LoopExpression.Interpret() Error");
				}

				Token<GeneratorTokenType> token = context.Current;
				if (token != null) {
					//Console.WriteLine("*** Test: " + token.Next.Type.Name);
				}
				if (token == null) {
					loop = false;
					break;
				}
			}

			//Matches = matches;
		}
	}
}
