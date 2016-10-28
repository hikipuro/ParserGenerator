using Hikipuro.Text.Parser.Generator.Expressions;
using ExpressionType = Hikipuro.Text.Parser.Generator.GeneratedParser.ExpressionType;

namespace Hikipuro.Text.Parser.Generator {
	/// <summary>
	/// 生成された Expression を作成するためのクラス.
	/// </summary>
	public class ExpressionFactory {
		/// <summary>
		/// RootExpression を作成する.
		/// </summary>
		/// <returns></returns>
		public static GeneratedExpression CreateRoot() {
			RootExpression exp = new RootExpression();
			exp.Type = ExpressionType.Root;
			exp.Name = "Root";
			return exp;
		}

		/// <summary>
		/// NonterminalExpression を作成する.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static GeneratedExpression CreateNonterminal(string name) {
			NonterminalExpression exp = new NonterminalExpression();
			exp.Type = ExpressionType.Nonterminal;
			exp.Name = name;
			return exp;
		}

		/// <summary>
		/// TerminalExpression を作成する.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static GeneratedExpression CreateTerminal(string name) {
			TerminalExpression exp = new TerminalExpression();
			exp.Type = ExpressionType.Terminal;
			exp.Name = name;
			return exp;
		}

		/// <summary>
		/// FieldExpression を作成する.
		/// </summary>
		/// <returns></returns>
		public static GeneratedExpression CreateField() {
			FieldExpression exp = new FieldExpression();
			exp.Type = ExpressionType.Field;
			return exp;
		}

		/// <summary>
		/// OptionExpression を作成する.
		/// </summary>
		/// <returns></returns>
		public static GeneratedExpression CreateOption() {
			OptionExpression exp = new OptionExpression();
			exp.Type = ExpressionType.Option;
			return exp;
		}

		/// <summary>
		/// OrExpression を作成する.
		/// </summary>
		/// <returns></returns>
		public static GeneratedExpression CreateOr() {
			OrExpression exp = new OrExpression();
			exp.Type = ExpressionType.Or;
			return exp;
		}

		/// <summary>
		/// LoopExpression を作成する.
		/// </summary>
		/// <returns></returns>
		public static GeneratedExpression CreateLoop() {
			LoopExpression exp = new LoopExpression();
			exp.Type = ExpressionType.Loop;
			return exp;
		}

		/// <summary>
		/// GroupExpression を作成する.
		/// </summary>
		/// <returns></returns>
		public static GeneratedExpression CreateGroup() {
			GroupExpression exp = new GroupExpression();
			exp.Type = ExpressionType.Group;
			return exp;
		}

		/// <summary>
		/// ExceptionExpression を作成する.
		/// </summary>
		/// <returns></returns>
		public static GeneratedExpression CreateException() {
			ExceptionExpression exp = new ExceptionExpression();
			exp.Type = ExpressionType.Exception;
			return exp;
		}
	}
}
