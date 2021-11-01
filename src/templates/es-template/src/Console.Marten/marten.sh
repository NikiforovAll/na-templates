#!/bin/bash
SCRIPTPATH="$( cd "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"

echo $SCRIPTPATH

dotnet run --project Console.Marten.csproj $@
