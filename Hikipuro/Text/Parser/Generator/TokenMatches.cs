using Hikipuro.Text.Tokenizer;
using System.Collections.Generic;
using TokenType = Hikipuro.Text.Parser.Generator.GeneratedParser.TokenType;

namespace Hikipuro.Text.Parser.Generator {
	/// <summary>
	/// ジェネレータのトークンマッチ処理用.
	/// </summary>
	public class TokenMatches {
		public string Name = string.Empty;
		public List<Token<TokenType>> Tokens;
		public List<TokenMatches> Matches;
		public TokenMatches(string name = "") {
			Name = name;
			Tokens = new List<Token<TokenType>>();
			Matches = new List<TokenMatches>();
		}
		public void Clear() {
			Tokens.Clear();
		}
		public void AddToken(Token<TokenType> token) {
			Tokens.Add(token);
		}
		public void AddMatches(TokenMatches matches) {
			if (matches.Matches.Count == 0 && matches.Tokens.Count == 0) {
				return;
			}
			Matches.Add(matches);
		}
		public void ConcatTokens(TokenMatches matches) {
			Tokens.AddRange(matches.Tokens);
		}
	}
}
