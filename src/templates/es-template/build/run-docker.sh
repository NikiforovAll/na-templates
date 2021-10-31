#!/bin/bash

SCRIPTPATH="$( cd "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"

DOCKER_COMPOSE_FILE="$SCRIPTPATH/../docker-compose.yml"
DOCKER_COMPOSE_FILE_OVERRIDE="$SCRIPTPATH/../docker-compose.override.yml"
DOCKER_COMPOSE_FILE_INFRA="$SCRIPTPATH/../docker-compose-infrastructure.yml"

if [ -z "$1" ]
then
    echo -e "Usage: \n${0##*/} start [services...] \n${0##*/} stop  \n${0##*/} down \n${0##*/} info"
    # ${@: 2} get scritpt params from 2 to end
    exit 1
fi

if [ $1 == start ]
then
    docker-compose -f $DOCKER_COMPOSE_FILE \
        -f $DOCKER_COMPOSE_FILE_INFRA \
        -f $DOCKER_COMPOSE_FILE_OVERRIDE \
        up -d --build ${@: 2}
    STATUS=$?
    if [ $STATUS -eq 0 ]
    then
        echo -e "\nContainers starting in background \nFor log info: ${0##*/} info"
    else
        echo -e "\nFailed starting containers"
    fi
elif [ $1 == down ]
then
    docker-compose -f $DOCKER_COMPOSE_FILE \
        -f $DOCKER_COMPOSE_FILE_INFRA \
        -f $DOCKER_COMPOSE_FILE_OVERRIDE \
        down ${@: 2}
    STATUS=$?
    if [ $STATUS -eq 0 ]
    then
        echo -e "\nContainers successfully stopped"
    else
        echo -e "\nFailed stopping containers"
    fi
elif [ $1 == stop ]
then
    docker-compose -f $DOCKER_COMPOSE_FILE \
        -f $DOCKER_COMPOSE_FILE_INFRA \
        -f $DOCKER_COMPOSE_FILE_OVERRIDE \
        stop ${@: 2}
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
        -f $DOCKER_COMPOSE_FILE_INFRA \
        -f $DOCKER_COMPOSE_FILE_OVERRIDE \
        logs --follow ${@: 2}
else
    echo -e "Usage: \n${0##*/} start [services...] \n${0##*/} stop \n{0##*/} down \n${0##*/} info"
    exit 1
fi
