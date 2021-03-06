﻿using Hikipuro.Text.Tokenizer;
using System.Collections.Generic;
using TokenType = Hikipuro.Text.Parser.Generator.GeneratedParser.TokenType;
using ExpressionType = Hikipuro.Text.Parser.Generator.GeneratedParser.ExpressionType;
using Result = Hikipuro.Text.Parser.Generator.GeneratorContext.Result;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// 開始位置.
	/// </summary>
	public class RootExpression : GeneratedExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			// 一番深い要素を探す
			GeneratedExpression exp = GetDeepestExpression(context, this);
			if (exp == null) {
				ThrowParseException(
					ErrorMessages.ExpressionNotFound
				);
			}

			// 見つかった要素を実行する
			DebugLog("RootExpression: (" + exp.Name + ")");
			exp.Interpret(context);
			Result itemResult = context.PopResult();

			// 最後のトークンをチェックする
			Token<TokenType> token = context.Current;
			if (itemResult.IsMatch == false || token != null) {
				ThrowParseException(
					ErrorMessages.UnexpectedToken, token
				);
			}
		}

		/// <summary>
		/// 一番深い Expression を探す.
		/// </summary>
		/// <param name="context">コンテキスト.</param>
		/// <param name="root">ルート要素.</param>
		/// <returns>一番深い要素.</returns>
		private GeneratedExpression GetDeepestExpression(GeneratorContext context, RootExpression root) {
			int maxDepth = 0;
			GeneratedExpression target = null;
			foreach (GeneratedExpression exp in root.Expressions) {
				int depth = GetDepth(context, exp);
				if (depth > maxDepth) {
					maxDepth = depth;
					target = exp;
				}
				DebugLog("RootExpression.Expressions : (" + exp.Name + ", " + depth + ")");
			}
			return target;
		}

		/// <summary>
		/// 指定された Expression の深さを取得する.
		/// </summary>
		/// <param name="context">コンテキスト.</param>
		/// <param name="expression">Expression.</param>
		/// <param name="names">無限ループ抑止用.</param>
		/// <returns>深さ.</returns>
		private int GetDepth(GeneratorContext context, GeneratedExpression expression, List<string> names = null) {
			if (names == null) {
				names = new List<string>();
			}

			int max = 0;
			foreach (GeneratedExpression exp in expression.Expressions) {
				if (exp.Type == ExpressionType.Terminal) {
					continue;
				}
				int depth = 0;
				if (exp.Type == ExpressionType.Nonterminal) {
					if (names.Contains(exp.Name) == false) {
						names.Add(exp.Name);
						GeneratedExpression field = context.Fields[exp.Name];
						depth = GetDepth(context, field, names) + 1;
					}
				} else {
					depth = GetDepth(context, exp, names) + 1;
				}
				if (depth > max) {
					max = depth;
				}
			}
			return max;
		}
	}
}
