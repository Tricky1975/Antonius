# Antonius
Quick in file string finder

This really isn't much, but it can be handy if you need this!

Antonius is a CLI tool, so you need to use it from the command prompt.

# Compile
Antonius has been written in C++ with the Slyvina framework.
You need a dedicated directory to compile Slyvina projects and from that dir go this way
```powershell
md Tools
md Libs
git clone "https://github.com/Tricky1975/Antonius" "Tools/Antonius"
git clone "https://github.com/Slyvina/Libs/Units" "Libs/Units"
```

For Windows you can best open Antonius.sln and compile Antonius in Visual Studio. Make sure it's configured for Release and not for Debug!
For Linux make sure you are in the Tools/Antonius/Antonius folder and run the shellscript LinBuild.sh from there. You may need to type "sh LinBuild.sh" as the file was set up in Windows so I could not add the 'x' attribute.

# Name
Antonius was named after Antony of Padua (In Dutch: Antonius van Padua), as a kind of memento to my grandmother who always prayed to him when she lost something.
In Dutch the rhyme "Heilige Antonius, beste vrind, maak dat ik m'n ... vind" (Holy Antony, dear friend, make that I find my ....) is very common when searching for lost items.
So the basic belief that Anthony would come to your help when looking for something, the name was actually obvious choice for this little search tool.
