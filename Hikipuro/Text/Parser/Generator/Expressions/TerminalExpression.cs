using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.Generator.GeneratedParser.TokenType;
using Result = Hikipuro.Text.Parser.Generator.GeneratorContext.Result;

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
			// 戻り値の準備
			Result result = new Result(Name);

			Token<TokenType> token = context.Current;
			if (token == null) {
				context.PushResult(result);
				return;
			}

			result.IsMatch = token.Type.Name == Name;
			if (result.IsMatch) {
				result.Matches.AddToken(token);
				context.Next();
			}
			DebugLog(string.Format(
				"TerminalExpression.Interpret(): {0} = {1}, {2}",
				token.Type.Name,
				Name,
				result.IsMatch
			));

			// 戻り値
			context.PushResult(result);
		}
	}
}
