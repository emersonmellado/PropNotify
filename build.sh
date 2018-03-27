#!/bin/bash

# A (very and overly) simple script for calling MSBuild on a prject. A path to
# the actual .sln file is required as the first paramter and the second
# parameter is optional. Passing in '/?' as a parameter instead of a .sln
# file path will break up the help menu.
#
# Examples:
#
# ./build.sh ./SchedulePro.sln                      # Builds SchedulePro
# ./build.sh ./SchedulePro.sln /t:clean             # Cleans the solution build
# ./build.sh /?                                     # Brings up the HELP menu
#/c/Program\ Files\ \(x86\)/MSBuild/14.0/Bin/MSBuild.exe /verbosity:quiet ./SchedulePro.sln 
#/c/Program\ Files\ \(x86\)/MSBuild/14.0/Bin/MSBuild.exe $1 /t:Clean

/c/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe ./PropNotify.sln
