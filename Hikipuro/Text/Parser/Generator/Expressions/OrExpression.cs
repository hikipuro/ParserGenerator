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

			Matches = new TokenMatches(Name);
			IsMatch = false;

			foreach (GeneratedExpression exp in Expressions) {
				//Console.WriteLine("*** OrExpression.Interpret(): " + exp.Name);
				exp.Interpret(context);
				//Console.WriteLine("*** OrExpression.Interpret(): " + exp.IsMatch);
				if (exp.IsMatch) {
					IsMatch = true;
					Matches.ConcatTokens(exp.Matches);
					break;
				}
			}
		}
	}
}
