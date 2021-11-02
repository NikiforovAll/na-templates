#!/bin/bash

SCRIPTPATH="$( cd "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"

reportgenerator -reports:"artifacts/**/*coverage.xml" "-reporttypes:Cobertura;TextSummary;Html" -targetdir:artifacts/report
