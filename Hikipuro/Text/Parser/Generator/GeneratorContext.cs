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

		public class Result {
			public bool IsMatch = false;
			public TokenMatches Matches;

			public Result(string name = "") {
				IsMatch = false;
				Matches = new TokenMatches(name);
			}
		}

		/// <summary>
		/// フィールドにマッチした時のイベントの型.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="matches"></param>
		public delegate void MatchFieldEventHandler(object sender, TokenMatches matches);

		/// <summary>
		/// フィールドにマッチした時のイベント.
		/// </summary>
		public event MatchFieldEventHandler MatchField;

		/// <summary>
		/// フィールドのリスト.
		/// </summary>
		public Dictionary<string, GeneratedExpression> Fields;

		/// <summary>
		/// それぞれの Expression の戻り値として使用する.
		/// </summary>
		public Stack<Result> ResultStack;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="source"></param>
		public GeneratorContext(IEnumerator source) : base(source) {
			Fields = new Dictionary<string, GeneratedExpression>();
			ResultStack = new Stack<Result>();
		}

		/// <summary>
		/// フィールドにマッチした時のイベントを発生させる.
		/// </summary>
		/// <param name="name">フィールドの名前.</param>
		/// <param name="matches"></param>
		public void OnMatchField(string name, TokenMatches matches) {
			if (MatchField == null) {
				return;
			}
			matches.Name = name;
			MatchField(this, matches);
		}

		public void PushResult(Result result) {
			ResultStack.Push(result);
		}

		public Result PopResult() {
			return ResultStack.Pop();
		}
	}
}
