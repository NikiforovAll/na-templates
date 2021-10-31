#!/bin/bash

cd "$(dirname "$0")"/..

dotnet ef "$@" \
    --project src/Infrastructure \
    --startup-project src/Console \
    --context Nikiforovall.CA.Template.Infrastructure.Persistence.ApplicationDbContext
