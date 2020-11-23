#!/bin/bash

# "${BASH_SOURCE%/*}" runs command relative to file location: https://stackoverflow.com/a/6659698/10557332
#  " & " makes it so the command run in the background and does not block the command line
dotnet run -p ${BASH_SOURCE%/*}/../Bumbo.Web/ &