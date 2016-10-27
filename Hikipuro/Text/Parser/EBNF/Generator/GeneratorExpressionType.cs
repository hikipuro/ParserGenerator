namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// ジェネレータの Expression の種類.
	/// </summary>
	public enum GeneratorExpressionType {
		Nonterminal,
		Terminal,
		Or,
		Loop,
		Option,
		Field,
		Root,
	}
}
