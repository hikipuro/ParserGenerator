using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.Generator.GeneratedParser.TokenType;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// ループの処理用.
	/// </summary>
	class LoopExpression : GeneratedExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			DebugLog("LoopExpression.Interpret(): " + Name + ", " + Expressions.Count);

			Matches = new TokenMatches(Name);
			IsMatch = false;

			bool loop = true;
			while (loop) {
				int count = 0;
				GeneratedExpression exp2 = null;
				foreach (GeneratedExpression exp in Expressions) {
					exp2 = exp;
					DebugLog("LoopExpression.Expressions : (" + exp.Name + ")");
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

				Token<TokenType> token = context.Current;
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
