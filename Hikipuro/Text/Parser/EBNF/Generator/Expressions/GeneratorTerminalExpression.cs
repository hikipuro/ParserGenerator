using Hikipuro.Text.Tokenizer;
using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// 終端記号.
	/// </summary>
	class GeneratorTerminalExpression : GeneratorExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			Matches = new GeneratorTokenMatches(Name);
			//Matches.Clear();
			IsMatch = false;

			Token<GeneratorTokenType> token = context.Current;
			IsMatch = token.Type.Name == Name;
			if (IsMatch) {
				Matches.AddToken(token);
				context.Next();
			}
			DebugLog(string.Format(
				"TerminalExpression.Interpret(): {0} = {1}, {2}",
				token.Type.Name,
				Name,
				IsMatch
			));

			//Matches = matches;
		}
	}
}
