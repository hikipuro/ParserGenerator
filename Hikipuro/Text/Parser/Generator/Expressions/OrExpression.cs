using Result = Hikipuro.Text.Parser.Generator.GeneratorContext.Result;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// Or の処理用.
	/// </summary>
	class OrExpression : GeneratedExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			DebugLog("OrExpression.Interpret(): " + Name + ", " + Expressions.Count);

			// 戻り値の準備
			Result result = new Result(Name);
			//Matches = new TokenMatches(Name);
			//IsMatch = false;

			foreach (GeneratedExpression exp in Expressions) {
				//Console.WriteLine("*** OrExpression.Interpret(): " + exp.Name);
				exp.Interpret(context);
				//Console.WriteLine("*** OrExpression.Interpret(): " + exp.IsMatch);
				Result itemResult = context.PopResult();

				if (itemResult.IsMatch) {
					result.IsMatch = true;
					result.Matches.ConcatTokens(itemResult.Matches);
					break;
				}
			}

			context.PushResult(result);
		}
	}
}
