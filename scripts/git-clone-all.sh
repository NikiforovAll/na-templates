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
  echo Cloning repository: $REPOSITORY

  REPO_URL=https://github.com/NikiforovAll/$REPOSITORY.git

  echo Repository URL: $REPO_URL
  echo ========================================================
  git clone $REPO_URL "${FOLDERS[$REPOSITORY]}"
done
