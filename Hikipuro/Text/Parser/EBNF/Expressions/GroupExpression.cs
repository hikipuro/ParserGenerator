using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Parser.Generator.Expressions;
using Hikipuro.Text.Tokenizer;
using System.Text;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF のグループ処理用.
	/// </summary>
	class GroupExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": GroupExpression.Interpret()");

			// 戻り値の準備
			StringBuilder pattern = new StringBuilder();
			GeneratedExpression exp = ExpressionFactory.CreateGroup();

			// 最初のトークンをチェック
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckFirstToken(token);
			pattern.Append(token.Text);

			// 2 番目のトークンをチェック
			token = context.Next();
			CheckTokenExists(token);

			bool loop = true;
			while (loop) {
				DebugLog(": GroupExpression: (" + token.Type + ")");

				switch (token.Type) {
				case TokenType.String:
				case TokenType.Name:
					Token<TokenType> nextToken = token.Next;
					if (nextToken != null && nextToken.Type == TokenType.Or) {
						GeneratedExpression orExp = ParseOr(context);
						exp.AddExpression(orExp);
						pattern.Append(orExp.Name);
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
				case TokenType.CloseParen:
					loop = false;
					pattern.Append(token.Text);
					token = context.Next();
					break;
				case TokenType.String:
					exp.AddExpression(
						ParseTerminal(context)
					);
					pattern.Append(token.Text);
					token = context.Next();
					break;
				case TokenType.Name:
					exp.AddExpression(
						ParseNonterminal(context, token.Text)
					);
					pattern.Append(token.Text);
					token = context.Next();
					break;
				case TokenType.OpenBrace:
					GeneratedExpression loopExp = ParseLoop(context);
					exp.AddExpression(loopExp);
					pattern.Append(loopExp.Name);
					token = context.Current;
					break;
				case TokenType.Comma:
					CheckComma(token);
					pattern.Append(token.Text);
					token = context.Next();
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

			// 戻り値
			exp.Name = pattern.ToString();
			context.PushExpression(exp);
			//GeneratedExpression.Name = pattern;
			DebugLog(": GroupExpression.Pattern: " + pattern);
		}

		/// <summary>
		/// 最初のトークンをチェックする.
		/// </summary>
		/// <param name="token">トークン.</param>
		private void CheckFirstToken(Token<TokenType> token) {
			CheckTokenType(token, new TokenType[] {
				TokenType.OpenParen
			});
		}
	}
}
