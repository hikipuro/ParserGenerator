using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.Generator.GeneratedParser.TokenType;
using ExpressionType = Hikipuro.Text.Parser.Generator.GeneratedParser.ExpressionType;
using Result = Hikipuro.Text.Parser.Generator.GeneratorContext.Result;

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

			// 戻り値の準備
			Result result = new Result(Name);

			int maxCount = 0;
			foreach (GeneratedExpression exp in Expressions) {
				if (exp.Type != ExpressionType.Option) {
					maxCount++;
				}
			}

			bool loop = true;
			while (loop) {
				int count = 0;
				GeneratedExpression exp2 = null;
				foreach (GeneratedExpression exp in Expressions) {
					exp2 = exp;
					DebugLog("LoopExpression.Expressions : (" + exp.Name + ")");
					exp.Interpret(context);
					Result itemResult = context.PopResult();

					result.IsMatch = itemResult.IsMatch;
					if (itemResult.IsMatch == false) {
						if (exp.Type == ExpressionType.Option) {
							continue;
						}
						DebugLog("-\texp.IsMatch == false : (" + exp.Name + ")");
						break;
					}
					result.Matches.ConcatTokens(itemResult.Matches);
					//Matches.ConcatTokens(exp.Matches);
					count++;
				}
				if (count == 0) {
					//Console.WriteLine("LoopExpression: count == 0: " + exp2.Type + ", " + exp2.Name);
					break;
				}
				if (count == maxCount) {

				}
				if (count != maxCount) {
					ThrowParseException(
						ErrorMessages.UnexpectedToken, context.Current
					);
				}

				Token<TokenType> token = context.Current;
				if (token == null) {
					loop = false;
					break;
				}
			}

			context.PushResult(result);
			//Matches = matches;
		}
	}
}
