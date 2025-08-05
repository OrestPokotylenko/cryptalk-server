#!/bin/bash

echo "Running EF Core migrations..."

dotnet tool restore

echo "Applying migrations..."
dotnet tool run dotnet-ef database update --project Cryptalk/Cryptalk.csproj