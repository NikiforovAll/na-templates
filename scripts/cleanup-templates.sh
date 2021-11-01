#!/bin/bash
REPOSITORIES=(
  na-ca
  na-es
  na-bu
)
declare -A FOLDERS=(
  ["na-ca"]="src/templates/ca-template"
  ["na-es"]="src/templates/es-template"
  ["na-bu"]="src/templates/bu-template"
)

for REPOSITORY in ${REPOSITORIES[*]}
do
  rm -rf "${FOLDERS[$REPOSITORY]}"
done
