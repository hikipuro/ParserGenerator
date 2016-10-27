namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// ジェネレータのトークンの種類.
	/// </summary>
	public struct GeneratorTokenType {
		/// <summary>
		/// 名前.
		/// </summary>
		public string Name;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="name"></param>
		public GeneratorTokenType(string name) {
			Name = name;
		}
	}
}
