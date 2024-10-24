
// Please note, this code has been translated (manually) from C# to C++

/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TrickyUnits;
*/   // Nice to keep as a memory :)

#include <SlyvArgParse.hpp>
#include <SlyvQCol.hpp>
#include <SlyvDir.hpp>
#include <SlyvStream.hpp>
#include <SlyvString.hpp>

#define var auto

using namespace std;
using namespace Slyvina;
using namespace Units;

namespace Antonius {
	namespace GoedeVrind {
		static ParsedArg Config{};
		static string Phrase{ "" };
		static bool large(){ return Config.bool_flags["large"]; }
		static bool allowbin(){ return Config.bool_flags["binary"]; }
		static bool ignorecase(){ return Config.bool_flags["ingorecase"]; }

		class Found {
		public:
			string File;
			bool IsBinary = false;
			vector<size_t>Lines{};//List<long> Lines = new List<long>();
			Found(string F) { File = F; }
			Found() {};
		};
		static map<string, Found> HayStack{};// = new SortedDictionary<string, Found>();

		static void InitConfig(int c,char**args) {
			FlagConfig ICFG{};
			AddFlag_Bool(ICFG,"binary", false); //Config.CrBool("binary", false);
			AddFlag_Bool(ICFG,"large", false);  //Config.CrBool("large", false);
			AddFlag_Bool(ICFG, "ignorecase", false); //Config.CrBool("ignorecase", false);
			Config = ParseArg(c, args, ICFG);//Config.Parse();
		}

		static void Zoek(string dir) {
			QCol->Doing("Analysing Dir", dir);
			var d{ GetTree(dir) };
			for (int fc = 0; fc < d->size(); fc++) {
				var ffile{ dir + "/" + (*d)[fc] }; //= $"{dir}/{d[fc]}";
				QCol->Magenta(to_string((int)floor(((double)fc / (double)d->size()) * 100))+"% \r");
				Found Hay{ ffile };
				if ((!large()) && FileSize(ffile) > 2000000) {
					QCol->Red("Skipped: "); QCol->Cyan(ffile); QCol->Yellow(" File bigger than 2 megabytes\n");
					continue;
				}
				char* lastfound{ nullptr };
				try {
					var isbinary = false;
					var line = 1;
					//char[] lastfound = new char[Phrase.Length];
					if (lastfound) delete[] lastfound; lastfound = new char[Phrase.size()];
					var BT = ReadFile(ffile);
					for (int leeg = 0; leeg < Phrase.size(); leeg++) lastfound[leeg] = '\0';
					for (int p = 0; p < BT->Size(); p++) {
						char C = BT->ReadChar();
						if (C == 10) line++;
						if (C == 26) {
							isbinary = true;
							if (!allowbin()) {
								QCol->Red("Skipped: "); QCol->Cyan(ffile); QCol->Yellow(" File appears to be a binary file\n");
								break;
							}
						}
						Hay.IsBinary = isbinary;
						//for (int lfi = 0; lfi < lastfound.Length - 1; lfi++) lastfound[lfi] = lastfound[lfi + 1];
						for (int lfi = 0; lfi < Phrase.size() - 1; lfi++) lastfound[lfi] = lastfound[lfi + 1];
						lastfound[Phrase.size() - 1] = C;//lastfound[lastfound.Length - 1] = C;
						var Ok = true;
						//for (int lfi = 0; lfi < lastfound.Length - 1; lfi++) {
						for (int lfi = 0; lfi < Phrase.size() - 1; lfi++) {
							Ok = Ok && (
								(lastfound[lfi] == Phrase[lfi]) ||
								(lastfound[lfi] >= 'A' && lastfound[lfi] <= 'Z' && lastfound[lfi] + 32 == Phrase[lfi]) ||
								(Phrase[lfi] >= 'A' && Phrase[lfi] <= 'Z' && lastfound[lfi] == Phrase[lfi] + 32)
								);
						}
						if (Ok) { Hay.Lines.push_back(line); }
					}
				} catch (runtime_error E) {
					QCol->Error(E.what());
				} 
				if (Hay.Lines.size() > 0) HayStack[ffile] = Hay;
				if (lastfound) delete[] lastfound;
			}
		}

		static void Results() {
			long count = 0;
			//foreach(var file in HayStack.Keys) {
			for (var __file : HayStack) {
				var file{ __file.first };
				var& HS = HayStack[file];
				count++;
				std::cout << "\n\n";
				QCol->Doing("Result", "#" + to_string(count) + ",","\t"); QCol->Magenta(file + "\n");
				QCol->Doing("= Results", HS.Lines.size());
				if (HS.IsBinary) {
					QCol->Doing("= Type", "Binary");
				} else {
					var i = 0;
					QCol->Doing("= Type", "Text");
					for(var l : HS.Lines) {
						QCol->Doing("  * Line", l);
					}
				}
			}
			if (count == 0) QCol->Red("Sorry! No results!");
		}

		static void Main(int argscount,char** args) {
			//MKL.Version("Antonius - Antonius.cs", "20.12.10");
			//MKL.Lic("Antonius - Antonius.cs", "GNU General Public License 3");
			//Config = new FlagParse(args);
			InitConfig(argscount,args);
			//Dirry.InitAltDrives();
			QCol->Yellow("Antonius ");
			QCol->Cyan(__DATE__); //QCol->Cyan(MKL.Newest);
			QCol->Green("\t Coded by: Tricky\n");
			QCol->Magenta("(c) Jeroen P. Broks " + Right(__DATE__, 4)+"\n\n"); //{MKL.CYear(2020)}\n\n");
			if (Config.arguments.size() == 0) {
				QCol->White("Usage: ");
				QCol->Yellow(StripAll(Config.myexe));
				QCol->Magenta(" [<switches>] ");
				QCol->Cyan(" <Search String> ");
				QCol->Magenta(" [<Directory/Directories>] ");
			} else if (Config.arguments.size() == 1) {
				Phrase = Config.arguments[0];
				Zoek(ChReplace(CurrentDir(), '\\', '/'));
				Results();
			} else {
				Phrase = Config.arguments[0];
				for (int i = 1; i < Config.arguments.size(); i++) // Start at 1, not 0, or the search term itself will be seen as a directory! We do no want that!
					Zoek(ChReplace(Config.arguments[i],'\\', '/'));
				Results();
			}
			QCol->Reset(); //Console.ResetColor();
			//TrickyDebug.AttachWait();
		}
	}
}

int main(int c, char** a) { 
	Antonius::GoedeVrind::Main(c, a); 
}