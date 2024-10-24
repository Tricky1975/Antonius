# License:
# 
# Antonius
# Linux build script
# 
# 
# 
# 	(c) Jeroen P. Broks, 2024
# 
# 		This program is free software: you can redistribute it and/or modify
# 		it under the terms of the GNU General Public License as published by
# 		the Free Software Foundation, either version 3 of the License, or
# 		(at your option) any later version.
# 
# 		This program is distributed in the hope that it will be useful,
# 		but WITHOUT ANY WARRANTY; without even the implied warranty of
# 		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# 		GNU General Public License for more details.
# 		You should have received a copy of the GNU General Public License
# 		along with this program.  If not, see <http://www.gnu.org/licenses/>.
# 
# 	Please note that some references to data like pictures or audio, do not automatically
# 	fall under this licenses. Mostly this is noted in the respective files.
# 
# Version: 24.10.25 I
# End License
mkdir LinExe
gcc -o LinExe/antonius "../../../Libs/Units/Source/SlyvArgParse.cpp" "../../../Libs/Units/Source/SlyvDir.cpp" "../../../Libs/Units/Source/SlyvDirry.cpp" "../../../Libs/Units/Source/SlyvQCol.cpp" "../../../Libs/Units/Source/SlyvSTOI.cpp" "../../../Libs/Units/Source/SlyvStream.cpp" "../../../Libs/Units/Source/SlyvString.cpp" "../../../Libs/Units/Source/SlyvTime.cpp" "../../../Libs/Units/Source/SlyvVolumes.cpp" "Antonius.cpp" -I../../../Libs/Units/Headers
