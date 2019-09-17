#!/bin/bash

dotnet pack -c Release -p:PackageVersion=$TRAVIS_TAG
dotnet nuget push src/LiveDelegate.ILSpy/bin/Release/LiveDelegate.ILSpy.$TRAVIS_TAG.nupkg -k $NUGET_KEY -s https://api.nuget.org/v3/index.json