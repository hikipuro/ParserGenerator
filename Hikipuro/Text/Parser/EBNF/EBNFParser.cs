using Hikipuro.Text.Parser.EBNF.Expressions;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF {
	/// <summary>
	/// EBNF ファイルのパーサ.
	/// </summary>
	public class EBNFParser {
		/// <summary>
		/// EBNF ファイルで使用するトークンの種類.
		/// </summary>
		public enum TokenType {
			None,
			NewLine,
			Comma,
			Semicolon,
			Equal,
			Minus,
			Or,
			Question,
			OpenComment,
			CloseComment,
			OpenParen,
			CloseParen,
			OpenBrace,
			CloseBrace,
			OpenBracket,
			CloseBracket,
			String,
			Name,
			Space
		}

		/// <summary>
		/// EBNF ファイルをパース.
		/// </summary>
		/// <param name="text">JSON ファイル.</param>
		/// <returns>JSON オブジェクト.</returns>
		public static GeneratedParser Parse(string text) {
			// トークンに分解する
			TokenList<TokenType> tokens = Tokenize(text);

			// パース
			EBNFContext context = new EBNFContext(tokens.GetEnumerator());
			EBNFExpression exp = new EBNFExpression();
			exp.Interpret(context);

			// 生成されたパーサを返す
			GeneratedParser parser = new GeneratedParser(context);
			return parser;
		}

		/// <summary>
		/// 渡された JSON ファイルを分解する.
		/// </summary>
		/// <param name="text">JSON ファイル.</param>
		/// <returns>トークンのリスト.</returns>
		public static TokenList<TokenType> Tokenize(string text) {
			// Tokenizer オブジェクトを準備する
			Tokenizer<TokenType> tokenizer = new Tokenizer<TokenType>();

			// トークンの分解規則を追加する
			tokenizer.AddPattern(TokenType.NewLine, "\r\n|\r|\n");
			tokenizer.AddPattern(TokenType.Comma, ",");
			tokenizer.AddPattern(TokenType.Semicolon, ";");
			tokenizer.AddPattern(TokenType.Equal, "=");
			tokenizer.AddPattern(TokenType.Minus, "-");
			tokenizer.AddPattern(TokenType.Or, @"\|");
			tokenizer.AddPattern(TokenType.Question, @"\?");
			tokenizer.AddPattern(TokenType.OpenComment, @"\(\*");
			tokenizer.AddPattern(TokenType.CloseComment, @"\*\)");
			tokenizer.AddPattern(TokenType.OpenParen, @"\(");
			tokenizer.AddPattern(TokenType.CloseParen, @"\)");
			tokenizer.AddPattern(TokenType.OpenBrace, "{");
			tokenizer.AddPattern(TokenType.CloseBrace, "}");
			tokenizer.AddPattern(TokenType.OpenBracket, @"\[");
			tokenizer.AddPattern(TokenType.CloseBracket, @"\]");
			tokenizer.AddPattern(TokenType.String, @"(""((?<=\\)""|[^\r\n""])*"")|('([^'])*')");
			tokenizer.AddPattern(TokenType.Space, @" +");
			tokenizer.AddPattern(TokenType.Name, @"([^\s=""{}\[\]()\|*,;?-]|[ ])+");

			// リストにトークンを追加する直前に発生するイベント
			// - e.Cancel = true; で追加しない
			bool commentStarted = false;
			tokenizer.BeforeAddToken += (object sender, BeforeAddTokenEventArgs<TokenType> e) => {
				// コメントを Tokenizer で取り除いておく
				if (e.TokenMatch.Type == TokenType.OpenComment) {
					commentStarted = true;
					e.Cancel = true;
					return;
				}
				if (e.TokenMatch.Type == TokenType.CloseComment) {
					commentStarted = false;
					e.Cancel = true;
					return;
				}
				if (commentStarted) {
					e.Cancel = true;
					return;
				}

				// 改行
				if (e.TokenMatch.Type == TokenType.NewLine) {
					e.Cancel = true;
					return;
				}
				// スペース
				if (e.TokenMatch.Type == TokenType.Space) {
					e.Cancel = true;
					return;
				}
				// 名前
				if (e.TokenMatch.Type == TokenType.Name) {
					e.TokenMatch.Text = e.TokenMatch.Text.Trim(' ');
					return;
				}
			};

			// トークンに分解する
			TokenList<TokenType> tokens = tokenizer.Tokenize(text);

			///*
			// 分解した内容を表示する (デバッグ用)
			foreach (Token<TokenType> token in tokens) {
				Debug.WriteLine(string.Format(
					"token: ({0},{1}): {2}: {3}",
					token.LineNumber, token.LineIndex,
					token.Type, token.Text
				));
			}
			//*/

			return tokens;
		}
	}
}
