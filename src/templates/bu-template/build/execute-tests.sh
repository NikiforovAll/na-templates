#!/bin/bash

SCRIPTPATH="$( cd "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"

docker compose \
    -f docker-compose-tests.yml \
    -f docker-compose-tests.override.yml up
