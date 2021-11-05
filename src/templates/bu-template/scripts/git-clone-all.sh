#!/bin/bash
REPOSITORIES=(
  "na-ca"
  "na-es"
  "na-ga"
)

declare -A FOLDERS=(
  ["na-es"]="na-es"
  ["na-ca"]="na-ca"
  ["na-ga"]="na-ga"
) 

for REPOSITORY in ${REPOSITORIES[*]}
do
  echo ========================================================
  echo Cloning repository: $REPOSITORY
  echo ========================================================
  REPO_URL=https://github.com/NikiforovAll/$REPOSITORY.git
  git clone $REPO_URL "${FOLDERS[$REPOSITORY]}"
done
