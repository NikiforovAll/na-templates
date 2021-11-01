#!/bin/bash

cd "$(dirname "$0")"/..

dotnet ef "$@" \
    --project src/Infrastructure \
    --startup-project src/Console \
    --context NikiforovAll.ES.Template.Infrastructure.Persistence.ApplicationDbContext
