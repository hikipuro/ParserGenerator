namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF のパース時に発生するエラーのメッセージ.
	/// </summary>
	class ErrorMessages {
		public const string TokenNotFound = "次のトークンが見つかりません。";
		public const string UnexpectedToken = "予期しないトークンが見つかりました。";
		public const string NameNotFound = "式に名前が付いていません。";
	}
}
