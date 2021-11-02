# Developer Setup

This repository is intended for bootstrapping developer environment. The general idea is to have an option to install microservice solution and have a developer support for infrastructure components (e.g: database, message broker, monitoring tools, etc.).

## Getting Started

Download downstream projects:

`./scripts/git-clone-all.sh`

Run test inside Docker:

`./scripts/execute-tests.sh`

Generate report:

`./scripts/generate-report.sh`

Find report under `./artifacts/report`.

**Run locally**

`./build/run-services start`

Open in browser:

`./scripts/open-in-browser.sh`

Check build-project state:

`./scripts/git-summary/git-summary.sh`
