#!/bin/bash

./scripts/cleanup-templates.sh

./scripts/git-clone-all.sh

./scripts/cleanup-templates.sh -g

git restore 'src/templates/bu-template/*.sln'

