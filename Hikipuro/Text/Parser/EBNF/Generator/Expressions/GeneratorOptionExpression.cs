using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// Option の処理用.
	/// </summary>
	class GeneratorOptionExpression : GeneratorExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			DebugLog("OptionExpression.Interpret(): " + Name);

			Matches = new GeneratorTokenMatches(Name);
			IsMatch = false;

			foreach (GeneratorExpression exp in Expressions) {
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
