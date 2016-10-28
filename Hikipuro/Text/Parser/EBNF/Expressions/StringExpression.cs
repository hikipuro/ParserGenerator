using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF の文字列.
	/// </summary>
	class StringExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": StringExpression.Interpret()");

			// 現在のトークンをチェック
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckTokenType(token, TokenType.String);

			// ", ' を取り除く
			string pattern = token.Text;
			if (pattern[0] == '"') {
				pattern = pattern.Trim('"');
			} else if (pattern[0] == '\'') {
				pattern = pattern.Trim('\'');
			}

			// トークンのバターンを登録する
			context.AddTokenizerPattern(pattern, pattern);

			// 戻り値
			GeneratedExpression = ExpressionFactory.CreateTerminal(pattern);
		}
	}
}
