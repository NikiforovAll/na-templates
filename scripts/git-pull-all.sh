#!/bin/bash

REPOSITORIES=(
  ca-template
)
declare -A FOLDERS=(
  ["ca-template"]="src/templates/ca-template"
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
