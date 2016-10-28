using Hikipuro.Text.Parser.Generator.Expressions;
using Hikipuro.Text.Tokenizer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hikipuro.Text.Parser.Generator {
	/// <summary>
	/// EBNF ファイルのパース時に, 生成されたパーサ.
	/// </summary>
	public class GeneratedParser {
		/// <summary>
		/// ジェネレータのトークンの種類.
		/// </summary>
		public struct TokenType {
			/// <summary>
			/// 名前.
			/// </summary>
			public string Name;

			/// <summary>
			/// コンストラクタ.
			/// </summary>
			/// <param name="name"></param>
			public TokenType(string name) {
				Name = name;
			}
		}

		/// <summary>
		/// ジェネレータの Expression の種類.
		/// </summary>
		public enum ExpressionType {
			Nonterminal,
			Terminal,
			Or,
			Loop,
			Option,
			Group,
			Exception,
			Field,
			Root,
		}

		public event BeforeAddTokenEventHandler<TokenType> MatchToken;

		/// <summary>
		/// フィールドにマッチした時に発生させるイベント.
		/// </summary>
		public event GeneratorContext.MatchFieldEventHandler MatchField;

		/// <summary>
		/// 生成された Tokenizer.
		/// </summary>
		Tokenizer<TokenType> tokenizer;

		/// <summary>
		/// 生成された Expression のリスト.
		/// </summary>
		Dictionary<string, GeneratedExpression> fields;

		/// <summary>
		/// 生成されたパーサのルート.
		/// </summary>
		GeneratedExpression expession;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="tokenizer">生成された Tokenizer.</param>
		/// <param name="fields">生成された Expression のリスト.</param>
		/// <param name="expession">生成されたパーサのルート.</param>
		public GeneratedParser(Tokenizer<TokenType> tokenizer, Dictionary<string, GeneratedExpression> fields, GeneratedExpression expession) {
			this.tokenizer = tokenizer;
			this.fields = fields;
			this.expession = expession;
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

			tokenizer.BeforeAddToken += MatchToken;

			// トークンに分解する
			TokenList<TokenType> tokens = tokenizer.Tokenize(text);

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
