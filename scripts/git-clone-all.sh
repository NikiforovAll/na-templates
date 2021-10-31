#!/bin/bash
REPOSITORIES=(
  na-ca
  na-es
)
declare -A FOLDERS=(
  ["ca-ca"]="src/templates/ca-template"
  ["na-es"]="src/templates/es-template"
)

for REPOSITORY in ${REPOSITORIES[*]}
do
  echo ========================================================
  echo Cloning repository: $REPOSITORY

  REPO_URL=https://github.com/NikiforovAll/$REPOSITORY.git

  echo Repository URL: $REPO_URL
  echo ========================================================
  git clone $REPO_URL "${FOLDERS[$REPOSITORY]}"
done
