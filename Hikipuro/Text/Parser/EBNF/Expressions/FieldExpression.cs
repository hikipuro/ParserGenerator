using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Parser.Generator.Expressions;
using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF の 1 つの項目を表すクラス.
	/// </summary>
	class FieldExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": FieldExpression.Interpret()");

			// 名前
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckTokenType(token, TokenType.Name);
			if (token.Text == string.Empty) {
				ThrowParseException(
					ErrorMessages.UnexpectedToken, token
				);
			}
			string name = token.Text;

			// イコール
			token = context.Next();
			CheckTokenExists(token);
			CheckTokenType(token, TokenType.Equal);

			// 式の右辺
			token = context.Next();
			CheckTokenExists(token);
			GeneratedExpression exp = ParseRight(context);
			exp.Name = name;

			// セミコロン
			token = context.Current;
			CheckTokenExists(token);
			CheckTokenType(token, TokenType.Semicolon);

			// 次に進めておく
			token = context.Next();
			//CheckTokenExists(token);

			// 戻り値
			context.PushExpression(exp);
			//GeneratedExpression = exp.GeneratedExpression;
		}
	}
}
