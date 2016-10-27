using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	class StringExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			Debug.WriteLine(": StringExpression.Interpret()");

			// 現在のトークンをチェック
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);

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
			Generator = GeneratorExpression.CreateTerminal(pattern);
		}
	}
}
