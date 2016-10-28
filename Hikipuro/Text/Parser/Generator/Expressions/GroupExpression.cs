using ExpressionType = Hikipuro.Text.Parser.Generator.GeneratedParser.ExpressionType;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// グループの処理用.
	/// </summary>
	class GroupExpression : GeneratedExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			DebugLog("GroupExpression.Interpret(): " + Name + ", " + Expressions.Count);

			Matches = new TokenMatches(Name);
			IsMatch = false;

			foreach (GeneratedExpression exp in Expressions) {
				exp.Interpret(context);
				Matches.ConcatTokens(exp.Matches);
				if (exp.IsMatch == false) {
					if (exp.Type == ExpressionType.Option) {
						continue;
					}
					IsMatch = false;
					//throw new InterpreterException("FieldExpression.Interpret() Error");
					break;
				} else {
					IsMatch = true;
				}
			}

			if (IsMatch) {
				context.OnMatchField(Name, Matches);
			}
		}
	}
}
