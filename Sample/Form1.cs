using Hikipuro.Text.Parser.EBNF;
using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Tokenizer;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ParserGenerator {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		/// <summary>
		/// 指定されたパスのテキストファイルを読み込む.
		/// </summary>
		/// <param name="path">テキストファイルのパス.</param>
		/// <returns>テキストファイルの内容.</returns>
		private string LoadText(string path) {
			if (File.Exists(path) == false) {
				return string.Empty;
			}
			StreamReader reader = new StreamReader(
				path //, Encoding.GetEncoding("Shift_JIS")
			);
			string text = reader.ReadToEnd();
			reader.Close();
			return text;
		}

		/// <summary>
		/// デバッグ用.
		/// </summary>
		/// <param name="matches"></param>
		/// <param name="depth"></param>
		/// <returns></returns>
		private string PrintTokenMatches(TokenMatches matches, int depth) {
			StringBuilder stringBuilder = new StringBuilder();
			string tab = new string('\t', depth + 1);
			foreach (Token<GeneratedParser.TokenType> token in matches.Tokens) {
				Debug.WriteLine("t " + tab + token.Text);
			}
			if (matches.Matches.Count > 0) {
				foreach (TokenMatches m in matches.Matches) {
					Debug.WriteLine(string.Format(
						"m {0}: {1} {2}",
						tab,
						m.Name,
						PrintTokenMatches(m, depth + 1)
					));
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// テストボタンが押された時.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonTest_Click(object sender, EventArgs e) {
			// パーサを生成する
			//string text = LoadText("Sample/EBNF/Test.ebnf");
			//string text = LoadText("Sample/EBNF/Test2.ebnf");
			//string text = LoadText("Sample/EBNF/Test3.ebnf");
			string text = LoadText("Sample/EBNF/json.ebnf");
			GeneratedParser parser = EBNFParser.Parse(text);

			// 生成されたパーサのテスト
			string test = "123132";
			//string test = "123+223";
			test = LoadText("Sample/JSON/Test1.json");

			parser.MatchToken += (object sender2, BeforeAddTokenEventArgs<GeneratedParser.TokenType> e2) => {
				string name = e2.TokenMatch.Type.Name;
				if (name == "\\s" || name == "\\r\\n|\\r|\\n") {
					e2.Cancel = true;
				}
				Debug.WriteLine("+++ e2.TokenMatch.Type.Name: " + e2.TokenMatch.Type.Name);
			};

			parser.MatchField += (object sender2, TokenMatches matches) => {
				//return;
				/*
				if (matches.Name != "record") {
					return;
				}
				//*/
				Debug.WriteLine("### Context.MatchField: " + matches.Name);
				PrintTokenMatches(matches, 0);
				//foreach (TokenMatches mateches in matches.Matches) {
				//	Debug.WriteLine("\t" + token.Text);
				//}
			};

			Debug.WriteLine("### " + test);
			parser.Parse(test);
		}
	}
}
