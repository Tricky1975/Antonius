// Lic:
// Antonius
// Searches for strings inside files
// 
// 
// 
// (c) Jeroen P. Broks, 2020
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Please note that some references to data like pictures or audio, do not automatically
// fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 20.12.10
// EndLic

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TrickyUnits;

namespace Antonius {
	class GoedeVrind {
		static FlagParse Config;
		static string Phrase;
		static bool large { get => Config.GetBool("large"); }
		static bool allowbin { get => Config.GetBool("binary"); }
		static bool ignorecase { get => Config.GetBool("ingorecase"); }

		class Found {
			public string File;
			public bool IsBinary = false;
			public List<long> Lines = new List<long>();
			public Found(string F) { File = F; }
        }
		static SortedDictionary<string, Found> HayStack = new SortedDictionary<string, Found>();

		static void InitConfig() {
			Config.CrBool("binary", false);
			Config.CrBool("large", false);
			Config.CrBool("ignorecase", false);
			Config.Parse();
		}

		static void Zoek(string dir) {
			QCol.Doing("Analysing Dir", dir);
			var d = FileList.GetTree(dir);
			for(int fc = 0; fc < d.Length; fc++) {
				var ffile = $"{dir}/{d[fc]}";
				QCol.Magenta($"{(int)Math.Floor(((double)fc / d.Length) * 100)}%\r");
				var Hay = new Found(ffile);
				if ((!large) && QuickStream.FileSize(ffile)>2000000) {
					QCol.Red("Skipped: "); QCol.Cyan(ffile); QCol.Yellow(" File bigger than 2 megabytes\n");
					continue;
                }
				try {
					var isbinary = false;
					var line = 1;
					char[] lastfound = new char[Phrase.Length];
					var BT = QuickStream.ReadFile(ffile);
					for (int leeg = 0; leeg < Phrase.Length; leeg++) lastfound[leeg] ='\0';
					for (int p = 0; p < BT.Size; p++) {
						char C = BT.ReadChar();
						if (C == 13) line++;
						if (C == 26) {
							isbinary = true;
							if (!allowbin) {
								QCol.Red("Skipped: "); QCol.Cyan(ffile); QCol.Yellow(" File appears to be a binary file\n");
								break;
							}
                        }
						Hay.IsBinary = isbinary;
						for (int lfi = 0; lfi < lastfound.Length - 1; lfi++) lastfound[lfi] = lastfound[lfi + 1];
						lastfound[lastfound.Length - 1] = C;
							var Ok = true;
						for (int lfi = 0; lfi < lastfound.Length - 1; lfi++) {
							Ok = Ok && (
								(lastfound[lfi] == Phrase[lfi]) ||
								(lastfound[lfi]>='A' && lastfound[lfi]<='Z' && lastfound[lfi]+32==Phrase[lfi]) ||
								(Phrase[lfi]>='A' && Phrase[lfi]<='Z' && lastfound[lfi]==Phrase[lfi]+32)
								);
                        }
						if (Ok) { Hay.Lines.Add(line); }
					}
				}catch(Exception E) {
					QCol.QuickError(E.Message);
                }
				if (Hay.Lines.Count > 0) HayStack[ffile] = Hay;
            }
		}

		static void Results() {
			long count = 0;
			foreach(var file in HayStack.Keys) {
				var HS = HayStack[file];
				count++;
				QCol.Doing("\n\nResult", $"#{count}","\t"); QCol.Magenta($"{file}\n");
				QCol.Doing($"= Results", HS.Lines.Count.ToString());
				if (HS.IsBinary) {
					QCol.Doing("= Type", "Binary");
                } else {
					var i = 0;
						QCol.Doing("= Type", "Text");
					foreach(var l in HS.Lines) {
						QCol.Doing("  * Line", l.ToString());
                    }
                }
            }
			if (count == 0) QCol.Red("Sorry! No results!");
        }

		static void Main(string[] args) {
			MKL.Version("Antonius - Antonius.cs","20.12.10");
			MKL.Lic    ("Antonius - Antonius.cs","GNU General Public License 3");
			Config = new FlagParse(args);
			InitConfig();
			Dirry.InitAltDrives();
			QCol.Yellow("Antonius ");
			QCol.Cyan(MKL.Newest);
			QCol.Green("\t Coded by: Tricky\n");
			QCol.Magenta($"(c) Jeroen P. Broks {MKL.CYear(2020)}\n\n");
			if (Config.Args.Length==0) {
				QCol.White("Usage: ");
				QCol.Yellow(qstr.StripAll(MKL.MyExe));
				QCol.Magenta(" [<switches>] ");
				QCol.Cyan(" <Search String> ");
				QCol.Magenta(" [<Directory/Directories>] ");
			} else if (Config.Args.Length == 1) {
				Phrase = Config.Args[0];
				Zoek(Directory.GetCurrentDirectory().Replace('\\','/'));
				Results();
			} else {
				Phrase = Config.Args[0];
				for (int i = 1; i < Config.Args.Length; i++) // Start at 1, not 0, or the search term itself will be seen as a directory! We do no want that!
					Zoek(Config.Args[i].Replace('\\', '/'));
				Results();
			}
			Console.ResetColor();
			TrickyDebug.AttachWait();
		}
	}
}