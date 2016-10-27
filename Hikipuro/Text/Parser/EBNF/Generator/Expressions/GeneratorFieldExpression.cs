using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// フィールド.
	/// </summary>
	class GeneratorFieldExpression : GeneratorExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			DebugLog("FieldExpression.Interpret(): " + Name);

			Matches = new GeneratorTokenMatches(Name);
			IsMatch = false;

			foreach (GeneratorExpression exp in Expressions) {
				exp.Interpret(context);
				Matches.ConcatTokens(exp.Matches);
				if (exp.IsMatch == false) {
					if (exp.Type == GeneratorExpressionType.Option) {
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
