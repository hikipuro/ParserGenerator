using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// 式の右辺.
	/// </summary>
	class RightExpression : BaseExpression {

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": RightExpression.Interpret()");

			// 最初のトークンをチェック
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckFirstToken(token);

			bool loop = true;
			while (loop) {
				DebugLog(": RightExpression: (" + token.Type + ")");

				switch (token.Type) {
				case TokenType.String:
				case TokenType.Name:
					Token<TokenType> nextToken = token.Next;
					if (nextToken != null && nextToken.Type == TokenType.Or) {
						ParseOr(context);
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
				case TokenType.OpenBracket:
					ParseOption(context);
					token = context.Current;
					break;
				case TokenType.Comma:
					CheckComma(token);
					token = context.Next();
					break;
				case TokenType.Semicolon:
					loop = false;
					break;
				default:
					ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}

				CheckTokenExists(token);
				if (token.IsLast) {
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
				TokenType.String,
				TokenType.Name,
				TokenType.OpenBrace,
				TokenType.OpenBracket
			});
		}
	}
}
