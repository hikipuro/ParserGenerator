using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Parser.Generator.Expressions;
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
		public Tokenizer<GeneratedParser.TokenType> Tokenizer;

		/// <summary>
		/// 生成されたパーサのルート.
		/// </summary>
		public GeneratedExpression Root;

		/// <summary>
		/// 生成された Expression のリスト.
		/// </summary>
		public Dictionary<string, GeneratedExpression> Fields;

		/// <summary>
		/// 生成された Expression のスタック.
		/// それぞれの EBNFExpression の戻り値として使用する.
		/// </summary>
		public Stack<GeneratedExpression> ExpressionStack;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="source"></param>
		public EBNFContext(IEnumerator source) : base(source) {
			Tokenizer = new Tokenizer<GeneratedParser.TokenType>();
			Fields = new Dictionary<string, GeneratedExpression>();
			ExpressionStack = new Stack<GeneratedExpression>();
		}

		/// <summary>
		/// Tokenizer に分解パターンを追加する.
		/// </summary>
		/// <param name="name">パターンの名前.</param>
		/// <param name="patternText">正規表現のパターン.</param>
		public void AddTokenizerPattern(string name, string patternText) {
			Tokenizer.AddPattern(new GeneratedParser.TokenType(name), patternText);
		}

		/// <summary>
		/// Expression の処理内で戻り値をプッシュする.
		/// </summary>
		/// <param name="expression"></param>
		public void PushExpression(GeneratedExpression expression) {
			ExpressionStack.Push(expression);
		}

		/// <summary>
		/// Expression の処理内で戻り値をポップする.
		/// </summary>
		/// <returns></returns>
		public GeneratedExpression PopExpression() {
			return ExpressionStack.Pop();
		}
	}
}
