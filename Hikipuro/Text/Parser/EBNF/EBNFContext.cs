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
		/// 生成された Tokenizer.
		/// </summary>
		public Tokenizer<GeneratorTokenType> Tokenizer;

		/// <summary>
		/// 生成されたパーサのルート.
		/// </summary>
		public GeneratorRootExpression Root;

		/// <summary>
		/// 生成された Expression のリスト.
		/// </summary>
		public Dictionary<string, GeneratorExpression> Fields;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="source"></param>
		public EBNFContext(IEnumerator source) : base(source) {
			Tokenizer = new Tokenizer<GeneratorTokenType>();
			Fields = new Dictionary<string, GeneratorExpression>();
		}

		/// <summary>
		/// Tokenizer に分解パターンを追加する.
		/// </summary>
		/// <param name="name">パターンの名前.</param>
		/// <param name="patternText">正規表現のパターン.</param>
		public void AddTokenizerPattern(string name, string patternText) {
			Tokenizer.AddPattern(new GeneratorTokenType(name), patternText);
		}
	}
}
