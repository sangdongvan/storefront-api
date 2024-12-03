#!/usr/bin/env bash
docker build . --provenance=false -t 012689708946.dkr.ecr.ap-southeast-1.amazonaws.com/storefront/storefront-api:v1.10
docker push 012689708946.dkr.ecr.ap-southeast-1.amazonaws.com/storefront/storefront-api:v1.10
