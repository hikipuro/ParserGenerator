using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using System.Collections;
using System.Collections.Generic;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF {
	/// <summary>
	/// EBNF のパース時に使用するコンテキスト.
	/// </summary>
	public class EBNFContext : Context<Token<TokenType>> {
		/// <summary>
		/// 
		/// </summary>
		public Tokenizer<GeneratorTokenType> Tokenizer = new Tokenizer<GeneratorTokenType>();
		public GeneratorRootExpression Root;
		public Dictionary<string, GeneratorExpression> Fields;
		//public List<string> Tokens;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="source"></param>
		public EBNFContext(IEnumerator source) : base(source) {
			Fields = new Dictionary<string, GeneratorExpression>();
			//Tokens = new List<string>();
		}

		public void AddTokenizerPattern(string name, string patternText) {
			Tokenizer.AddPattern(new GeneratorTokenType(name), patternText);
		}
	}
}
