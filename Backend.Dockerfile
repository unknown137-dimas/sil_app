# Use the official .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution file and restore the dependencies
COPY LaboratoriumInformationSystem.sln ./
COPY Backend/Backend.csproj ./Backend/
COPY Database/Database.csproj ./Database/

RUN dotnet restore

# Copy the remaining project files
COPY ./Backend ./Backend
COPY ./Database ./Database

# Build the project
RUN dotnet publish Backend/Backend.csproj -c Release -o out

# Use the official .NET runtime image for the runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /app/out .

# Install fonts
RUN sed -i 's/^Components: main$/& contrib/' /etc/apt/sources.list.d/debian.sources
RUN apt-get update && apt-get install -y ttf-mscorefonts-installer fontconfig && fc-cache -f -v

# Expose the port the app runs on
EXPOSE 8080

# Set the entry point for the Docker container
ENTRYPOINT ["dotnet", "Backend.dll"]
