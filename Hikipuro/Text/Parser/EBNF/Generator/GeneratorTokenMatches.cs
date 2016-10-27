using Hikipuro.Text.Tokenizer;
using System.Collections.Generic;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// ジェネレータのトークンマッチ処理用.
	/// </summary>
	public class GeneratorTokenMatches {
		public string Name = string.Empty;
		public List<Token<GeneratorTokenType>> Tokens;
		public List<GeneratorTokenMatches> Matches;
		public GeneratorTokenMatches(string name = "") {
			Name = name;
			Tokens = new List<Token<GeneratorTokenType>>();
			Matches = new List<GeneratorTokenMatches>();
		}
		public void Clear() {
			Tokens.Clear();
		}
		public void AddToken(Token<GeneratorTokenType> token) {
			Tokens.Add(token);
		}
		public void AddMatches(GeneratorTokenMatches matches) {
			if (matches.Matches.Count == 0 && matches.Tokens.Count == 0) {
				return;
			}
			Matches.Add(matches);
		}
		public void ConcatTokens(GeneratorTokenMatches matches) {
			Tokens.AddRange(matches.Tokens);
		}
	}
}
