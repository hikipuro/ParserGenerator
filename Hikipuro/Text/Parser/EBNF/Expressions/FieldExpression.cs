using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	class FieldExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			Debug.WriteLine(": FieldExpression.Interpret()");

			// 名前
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckTokenType(token, TokenType.Name);
			if (token.Text == string.Empty) {
				ThrowInterpretException(
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

			RightExpression exp = new RightExpression();
			exp.Generator = GeneratorExpression.CreateField();
			exp.Generator.Name = name;
			exp.Interpret(context);

			// セミコロン
			token = context.Current;
			CheckTokenExists(token);
			CheckTokenType(token, TokenType.Semicolon);

			// 次に進めておく
			token = context.Next();
			//CheckTokenExists(token);

			// 戻り値
			Generator = exp.Generator;
		}
	}
}
