using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// Or の処理用.
	/// </summary>
	class GeneratorOrExpression : GeneratorExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			if (DebugFlag) {
				Debug.WriteLine("OrExpression.Interpret(): " + Name + ", " + Expressions.Count);
			}
			Matches = new GeneratorTokenMatches(Name);
			IsMatch = false;

			foreach (GeneratorExpression exp in Expressions) {
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
