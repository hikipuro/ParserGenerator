using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.Generator.GeneratedParser.TokenType;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// 終端記号.
	/// </summary>
	class TerminalExpression : GeneratedExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			Matches = new TokenMatches(Name);
			//Matches.Clear();
			IsMatch = false;

			Token<TokenType> token = context.Current;
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
