using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF {
	/// <summary>
	/// EBNF ファイルのパース時に, 生成されたパーサ.
	/// </summary>
	public class GeneratedParser {
		public event GeneratorContext.MatchFieldEventHandler MatchField;

		/// <summary>
		/// 生成された Tokenizer.
		/// </summary>
		Tokenizer<GeneratorTokenType> tokenizer;

		/// <summary>
		/// 生成された Expression のリスト.
		/// </summary>
		Dictionary<string, GeneratorExpression> fields;

		/// <summary>
		/// 生成されたパーサのルート.
		/// </summary>
		GeneratorExpression expession;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="context">EBNF のパース時に使用したコンテキストオブジェクト.</param>
		public GeneratedParser(EBNFContext context) {
			tokenizer = context.Tokenizer;
			fields = context.Fields;
			expession = context.Root;
		}

		/// <summary>
		/// 生成されたパーサでパースする.
		/// </summary>
		/// <param name="text">処理対象のテキスト.</param>
		public void Parse(string text) {
			if (tokenizer == null || fields == null || expession == null) {
				return;
			}
			Debug.WriteLine(expession.ToString());

			// トークンに分解する
			TokenList<GeneratorTokenType> tokens = tokenizer.Tokenize(text);

			// パース
			GeneratorContext context = new GeneratorContext(tokens.GetEnumerator());
			context.Fields = fields;
			context.MatchField += MatchField;
			expession.Interpret(context);

			/*
			foreach (string key in context.Fields.Keys) {
				Debug.WriteLine(context.Fields[key].ToString());
			}
			//*/
		}
	}
}
