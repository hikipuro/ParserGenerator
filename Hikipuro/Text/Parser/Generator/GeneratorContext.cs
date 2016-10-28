using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Parser.Generator.Expressions;
using Hikipuro.Text.Tokenizer;
using System.Collections;
using System.Collections.Generic;
using TokenType = Hikipuro.Text.Parser.Generator.GeneratedParser.TokenType;

namespace Hikipuro.Text.Parser.Generator {
	/// <summary>
	/// ジェネレータで使用するコンテキスト.
	/// </summary>
	public class GeneratorContext : Context<Token<TokenType>> {
		public delegate void MatchFieldEventHandler(object sender, TokenMatches matches);

		public event MatchFieldEventHandler MatchField;
		public Dictionary<string, GeneratedExpression> Fields;


		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="source"></param>
		public GeneratorContext(IEnumerator source) : base(source) {
			Fields = new Dictionary<string, GeneratedExpression>();
		}

		public void OnMatchField(string name, TokenMatches matches) {
			if (MatchField == null) {
				return;
			}
			matches.Name = name;
			MatchField(this, matches);
		}
	}
}
