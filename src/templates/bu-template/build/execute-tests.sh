#!/bin/bash

SCRIPTPATH="$( cd "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"

DOCKER_COMPOSE_FILE="$SCRIPTPATH/../docker-compose-tests.yml"
DOCKER_COMPOSE_FILE_OVERRIDE="$SCRIPTPATH/../docker-compose-tests.override.yml "

# ./build/test-all.sh start --exit-code-from <tests>

if [ -z "$1" ]
then
    echo -e "Usage: \n${0##*/} start [services...] \n${0##*/} stop \n${0##*/} info"
    # ${@: 2} get scritpt params from 2 to end
    exit 1
fi

if [ $1 == start ]
then

    # echo "Cleaning ./artifacts/coverage"
    # rm -rf $SCRIPTPATH/../artifacts/coverage

    docker-compose --env-file $SCRIPTPATH/../.env -f $DOCKER_COMPOSE_FILE \
        -f $DOCKER_COMPOSE_FILE_OVERRIDE \
        up --build ${@: 2}
    STATUS=$?
    if [ $STATUS -eq 0 ]
    then
        echo -e "\nContainers starting in background \nFor log info: ${0##*/} info"
    else
        echo -e "\nFailed starting containers"
    fi
elif [ $1 == stop ]
then
    docker-compose -f $DOCKER_COMPOSE_FILE \
        -f $DOCKER_COMPOSE_FILE_OVERRIDE \
        down
    STATUS=$?
    if [ $STATUS -eq 0 ]
    then
        echo -e "\nContainers successfully stopped"
    else
        echo -e "\nFailed stopping containers"
    fi
elif [ $1 == info ]
then
    docker-compose -f $DOCKER_COMPOSE_FILE  \
        -f $DOCKER_COMPOSE_FILE_OVERRIDE \
        logs --follow ${@: 2}
else
    echo -e "Usage: \n${0##*/} start [services...] \n${0##*/} stop \n${0##*/} info"
    exit 1
fi
