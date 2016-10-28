using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Parser.Generator.Expressions;
using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// 例外の処理用.
	/// </summary>
	class ExceptionExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": ExceptionExpression.Interpret()");

			// 戻り値の準備
			string pattern = string.Empty;
			GeneratedExpression = ExpressionFactory.CreateException();

			// 最初のトークンをチェック
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckFirstToken(token);
			pattern += token.Text;

			// 2 番目のトークンをチェック
			token = context.Next();
			CheckTokenExists(token);
			DebugLog(": ExceptionExpression: (" + token.Type + ")");

			switch (token.Type) {
			case TokenType.String:
				pattern += token.Text;
				ParseTerminal(context);
				token = context.Next();
				break;
			case TokenType.Name:
				pattern += token.Text;
				ParseNonterminal(context, token.Text);
				token = context.Next();
				break;
			default:
				ThrowParseException(ErrorMessages.UnexpectedToken, token);
				break;
			}

			CheckTokenExists(token);
			if (token.IsLast) {
				//ThrowParseException(ErrorMessages.UnexpectedToken, token);
			}

			// 戻り値
			GeneratedExpression.Name = pattern;
		}

		/// <summary>
		/// 最初のトークンをチェックする.
		/// </summary>
		/// <param name="token">トークン.</param>
		private void CheckFirstToken(Token<TokenType> token) {
			CheckTokenType(token, new TokenType[] {
				TokenType.Minus
			});
		}
	}
}
