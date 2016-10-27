using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using System.Collections;
using System.Collections.Generic;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// ジェネレータで使用するコンテキスト.
	/// </summary>
	public class GeneratorContext : Context<Token<GeneratorTokenType>> {
		public delegate void MatchFieldEventHandler(object sender, GeneratorTokenMatches matches);

		public event MatchFieldEventHandler MatchField;
		public Dictionary<string, GeneratorExpression> Fields;


		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="source"></param>
		public GeneratorContext(IEnumerator source) : base(source) {
			Fields = new Dictionary<string, GeneratorExpression>();
		}

		public void OnMatchField(string name, GeneratorTokenMatches matches) {
			if (MatchField == null) {
				return;
			}
			matches.Name = name;
			MatchField(this, matches);
		}
	}
}
