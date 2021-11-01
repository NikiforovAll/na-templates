#!/bin/bash
REPOSITORIES=(
)

declare -A FOLDERS=(
  # [""]=""
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
