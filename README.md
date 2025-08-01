# Cryptalk Server

Cryptalk Server is a self-hosted message relay for the Cryptalk communication system.  
It securely stores and delivers encrypted messages between trusted devices connected to the same server.

It is intended to run on a personal Linux server and requires no cloud services.

## Overview

Cryptalk Server is a lightweight backend service designed for users who want to communicate securely over the internet using their Cryptalk devices.  
All messages are encrypted end-to-end before reaching the server. The server does not perform any decryption or metadata processing.

## Pairing

Devices connect to the server only after being securely paired, either:
 
- over the internet using server address

Pairing is done once. The server itself only facilitates encrypted message delivery.

## Features

- Storage of encrypted messages
- PostgreSQL database
- Minimal and auditable codebase
- Docker deployment
- No personal data collection or tracking

## Setup

The server is distributed as a prebuilt Docker image.  
Installation instructions will be provided on the official Cryptalk website.

## Security

- API access requires a secure key per device
- Messages are never decrypted

## Developer Notes

The project is fully open source.  
You are free to inspect the code, explore the API structure, and extend the functionality under the terms of the license.

If you're a developer and want to integrate or contribute:
- The project uses ASP.NET Core Web API with a controller-service-repository pattern
- Dockerfile and configuration templates are included
- Contributions are welcome

## License

This software is licensed under a custom Non-Commercial License.

You may:
- Use it for personal and educational purposes
- Modify and self-host it freely

You may not:
- Use it or modified versions for commercial purposes

See the `LICENSE` file for full terms.  
© 2025 Orest Pokotylenko