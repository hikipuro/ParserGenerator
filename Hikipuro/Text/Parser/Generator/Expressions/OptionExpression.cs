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

			Matches = new TokenMatches(Name);
			IsMatch = false;

			foreach (GeneratedExpression exp in Expressions) {
				exp.Interpret(context);
				if (exp.IsMatch) {
					IsMatch = true;
					Matches.ConcatTokens(exp.Matches);
					//Matches.ConcatTokens(exp.Matches);
					//break;
				} else {
					Matches.ConcatTokens(exp.Matches);
					//Matches.ConcatTokens(exp.Matches);
					break;
				}
			}
		}
	}
}
