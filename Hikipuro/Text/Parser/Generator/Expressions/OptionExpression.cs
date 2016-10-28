using Result = Hikipuro.Text.Parser.Generator.GeneratorContext.Result;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// Option の処理用.
	/// </summary>
	class OptionExpression : GeneratedExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			DebugLog("OptionExpression.Interpret(): " + Name);

			// 戻り値の準備
			Result result = new Result(Name);
			//Matches = new TokenMatches(Name);
			//IsMatch = false;

			foreach (GeneratedExpression exp in Expressions) {
				exp.Interpret(context);
				Result itemResult = context.PopResult();

				if (itemResult.IsMatch) {
					result.IsMatch = true;
					result.Matches.ConcatTokens(itemResult.Matches);
					//Matches.ConcatTokens(exp.Matches);
					//break;
				} else {
					result.Matches.ConcatTokens(itemResult.Matches);
					//Matches.ConcatTokens(exp.Matches);
					break;
				}
			}

			context.PushResult(result);
		}
	}
}
