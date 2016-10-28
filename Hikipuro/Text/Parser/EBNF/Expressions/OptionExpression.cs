using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Parser.Generator.Expressions;
using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF のオプション.
	/// </summary>
	class OptionExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": OptionExpression.Interpret()");

			// 戻り値の準備
			GeneratedExpression = ExpressionFactory.CreateOption();

			// 最初のトークンをチェック
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckFirstToken(token);

			// 2 番目のトークンをチェック
			token = context.Next();
			CheckTokenExists(token);

			bool loop = true;
			while (loop) {
				DebugLog(": OptionExpression: (" + token.Text + ")");

				switch (token.Type) {
				case TokenType.String:
				case TokenType.Name:
					Token<TokenType> nextToken = token.Next;
					if (nextToken != null && nextToken.Type == TokenType.Or) {
						GeneratedExpression exp = ParseOr(context);
						token = context.Current;
						CheckTokenExists(token);
					}
					break;
				}
				if (token.IsLast) {
					ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}

				switch (token.Type) {
				case TokenType.CloseBracket:
					loop = false;
					token = context.Next();
					break;
				case TokenType.String:
					ParseTerminal(context);
					token = context.Next();
					break;
				case TokenType.Name:
					ParseNonterminal(context, token.Text);
					token = context.Next();
					break;
				case TokenType.OpenBrace:
					ParseLoop(context);
					token = context.Current;
					break;
				default:
					ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}

				CheckTokenExists(token);
				if (token.IsLast) {
					ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}
			}
		}

		/// <summary>
		/// 最初のトークンをチェックする.
		/// </summary>
		/// <param name="token">トークン.</param>
		private void CheckFirstToken(Token<TokenType> token) {
			CheckTokenType(token, new TokenType[] {
				TokenType.OpenBracket
			});
		}
	}
}
