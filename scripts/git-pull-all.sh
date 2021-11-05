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
  echo ========================================================
  echo Updating repository: $REPOSITORY ./"${FOLDERS[$REPOSITORY]}"
  echo ========================================================
  cd "${FOLDERS[$REPOSITORY]}"
  git checkout main
  git pull
  cd ..
done
