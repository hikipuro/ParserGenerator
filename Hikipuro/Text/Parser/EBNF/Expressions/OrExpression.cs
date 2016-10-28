using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Parser.Generator.Expressions;
using Hikipuro.Text.Tokenizer;
using System.Text;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF の OR.
	/// </summary>
	class OrExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": OrExpression.Interpret()");

			// 戻り値の準備
			StringBuilder pattern = new StringBuilder();
			GeneratedExpression exp = ExpressionFactory.CreateOr();

			// 最初のトークンをチェック
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckFirstToken(token);

			bool loop = true;
			while (loop) {
				DebugLog(": OrExpression: (" + token.Type + ")");

				switch (token.Type) {
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
				case TokenType.Or:
					pattern.Append(token.Text);
					token = context.Next();
					break;
				default:
					loop = false;
					//ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}

				CheckTokenExists(token);
				if (token.IsLast) {
					break;
				}
			}

			// 戻り値
			exp.Name = pattern.ToString();
			context.PushExpression(exp);
			//GeneratedExpression.Name = pattern;
			DebugLog(": OrExpression.Pattern: " + pattern);
		}

		/// <summary>
		/// 最初のトークンをチェックする.
		/// </summary>
		/// <param name="token">トークン.</param>
		private void CheckFirstToken(Token<TokenType> token) {
			CheckTokenType(token, new TokenType[] {
				TokenType.String,
				TokenType.Name
			});
		}
	}
}
