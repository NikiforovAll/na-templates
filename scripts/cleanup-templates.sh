#!/bin/bash

REPOSITORIES=(
  na-ca
  na-es
  na-bu
  na-ga
)
declare -A FOLDERS=(
  ["na-ca"]="src/templates/ca-template"
  ["na-es"]="src/templates/es-template"
  ["na-bu"]="src/templates/bu-template"
  ["na-ga"]="src/templates/ga-template"
)

for REPOSITORY in ${REPOSITORIES[*]}
do
    case "$1" in
    -g)
        rm -rf "${FOLDERS[$REPOSITORY]}"/.git
        git restore "${FOLDERS[$REPOSITORY]}/.template.config"
        ;;
    *) rm -rf "${FOLDERS[$REPOSITORY]}" ;;
    esac
done
