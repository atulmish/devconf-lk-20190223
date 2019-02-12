#!/usr/bin/env bash
dotnet new console --name AkkaActorSystem
dotnet new xunit --name AkkaActorSystem.Tests

dotnet new sln --name AkkaInANutshell
dotnet sln add AkkaActorSystem.Tests/AkkaActorSystem.Tests.csproj
dotnet sln add AkkaActorSystem/AkkaActorSystem.csproj


cd AkkaActorSystem
dotnet add package Akka --version 1.3.11
dotnet add package Akka.Remote --version 1.3.11
dotnet add package Akka.Cluster --version 1.3.11

cd ..
cd AkkaActorSystem.Tests

dotnet add reference ../AkkaActorSystem/AkkaActorSystem.csproj

dotnet add package Akka --version 1.3.11
dotnet add package Akka.Remote --version 1.3.11
dotnet add package Akka.Cluster --version 1.3.11
dotnet add package Akka.TestKit.Xunit --version 1.3.11
dotnet add package Akka.TestKit --version 1.3.11

