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

		Tokenizer<GeneratorTokenType> tokenizer;
		Dictionary<string, GeneratorExpression> fields;
		GeneratorExpression expession;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="context"></param>
		public GeneratedParser(EBNFContext context) {
			tokenizer = context.Tokenizer;
			fields = context.Fields;
			expession = context.Root;
		}

		public void Parse(string text) {
			if (tokenizer == null || fields == null || expession == null) {
				return;
			}
			Debug.WriteLine(expession.ToString());

			TokenList<GeneratorTokenType> tokens = tokenizer.Tokenize(text);
			GeneratorContext context = new GeneratorContext(tokens.GetEnumerator());
			context.Fields = fields;
			context.MatchField += MatchField;
			expession.Interpret(context);

			//foreach (string key in context.Fields.Keys) {
			//	Debug.WriteLine(context.Fields[key].ToString());
			//}
		}
	}
}
