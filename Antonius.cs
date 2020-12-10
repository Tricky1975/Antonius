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
???using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyUnits;

namespace Antonius {
    class GoedeVrind {
        static void Main(string[] args) {
            MKL.Version("Antonius - Antonius.cs","20.12.10");
            MKL.Lic    ("Antonius - Antonius.cs","GNU General Public License 3");
            Dirry.InitAltDrives();
            QCol.Yellow("Antonius ");
            QCol.Cyan(MKL.Newest);
            QCol.Green("\t Coded by: Tricky");
            QCol.Magenta($"(c) Jeroen P. Broks {MKL.CYear(2020)}\n\n");

            TrickyDebug.AttachWait();
        }
    }
}