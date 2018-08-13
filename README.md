# ndid-examples-dotnet
This project is an example of IdP application on NDID platform with ASP.Net Core 2. This project is based on NDID version as follow:
- [smart-contract](https://github.com/ndidplatform/smart-contract/tree/v0.5.0) version 0.5.0
- [api](https://github.com/ndidplatform/api/tree/v0.5.2) version 0.5.2
- [examples](https://github.com/ndidplatform/examples/tree/d022247bf154be6baffd4457a490a8d3086df94e) commit version d022247bf154be6baffd4457a490a8d3086df94e

The latest version of NDID (v0.6) require that hash message for sign will need no padding, which standard crypto library of .Net cannot support.

## Run in Docker

Required

- Docker CE 17.06+ [Install docker](https://docs.docker.com/install/)
- docker-compose 1.14.0+ [Install docker-compose](https://docs.docker.com/compose/install/)

## Run
Start NDID smart-contract
```
### Clone smart-contract repo
git clone https://github.com/ndidplatform/smart-contract.git```

### Build the `smart-contract` docker image (use v0.5.0 version)
cd smart-contract
git checkout tags/v0.5.0 -b v0.5.0
cd docker
docker-compose -f docker-compose.build.yml build --no-cache

### Run the smart contract components
docker-compose up -d
```

Start NDID api
```
### Clone api repo
git clone https://github.com/ndidplatform/api.git```

### Build the `api` docker image (use v0.5.2 version)
cd api
git checkout tags/v0.5.2 -b v0.5.2
cd docker
docker-compose -f docker-compose.build.yml build --no-cache

### Run the api components
docker-compose up -d
```

Start NDID examples
```
### Clone examples repo
git clone https://github.com/ndidplatform/examples.git

### Build the `examples` docker image
cd examples
git checkout d022247bf154be6baffd4457a490a8d3086df94e -b 0.5.0
cd docker
docker-compose -f docker-compose.build.yml build --no-cache

### Run the examples components
docker-compose up -d
```

Start this project (finally)
```
### Clone examples repo
git clone https://github.com/ndidplatform/examples.git

### Build the `examples` docker image
cd examples
cd docker
docker-compose -f docker-compose.build.yml build --no-cache

### Run the examples components
docker-compose up -d
```

Now, you can go to port 8000-8002 for idp and port 9000 for rp respectively.